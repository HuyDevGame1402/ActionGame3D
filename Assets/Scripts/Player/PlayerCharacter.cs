using UnityEngine;
using System.Collections;
using System;

public class PlayerCharacter : CharacterBase
{
    private PlayerInput _playerInput;
    private Transform _mainCameraTransform;

    [Header("Player Specific")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Attack")]
    private float attackStartTime;
    public float attackSlideDuration = 0.1f;
    public float attackSlideSpeed = 1.5f;
    private float timePassed;
    private float lerpTime;

    private Vector3 impactOnCharacter;
    private bool isInvincible;
    private float invincibleDuration = 2f;

    private int coin;

    private float attackAnimationDuration;
    private float slideSpeed = 9f;

    public event Action<int> AddCoinEventAction;

    protected override void Awake()
    {
        base.Awake(); // Gọi hàm Awake của lớp cha để lấy _cc và _animator

        _playerInput = GetComponent<PlayerInput>();
        if (Camera.main != null)
            _mainCameraTransform = Camera.main.transform;
    }

    protected override void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                CalculatePlayerMovement();
                break;

            case CharacterState.Attacking:

                if (Time.time < attackStartTime + attackSlideDuration)
                {
                    timePassed = Time.time - attackStartTime;
                    lerpTime = timePassed / attackSlideDuration;
                    _movementDirection = Vector3.Lerp(transform.forward * attackSlideSpeed,
                        Vector3.zero, lerpTime);
                }
                if(_playerInput.mouseButtonDown && _cc.isGrounded)
                {
                    string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                
                    if(currentClipName != "LittleAdventurerAndie_ATTACK_03"
                        && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                    {
                        _playerInput.mouseButtonDown = false;
                        SwitchStateTo(CharacterState.Attacking);
                        CalculatePlayerMovement();
                    }
                
                }
                break;

            case CharacterState.Dead:

                return;

            case CharacterState.BeingHit:
                if(impactOnCharacter.magnitude > 0.2f)
                {
                    _movementDirection = impactOnCharacter * Time.deltaTime;
                }
                impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero,
                    Time.deltaTime * 5f);
                break;
            case CharacterState.Slide:
                _movementDirection = transform.forward * slideSpeed * Time.deltaTime;
                break;
        }
        ApplyGravityAndMove(); // Sử dụng lại logic trọng lực và di chuyển từ lớp cha
        _movementDirection = Vector3.zero;
    }

    private void CalculatePlayerMovement()
    {
        if (_playerInput == null) return;

        if(_playerInput.mouseButtonDown && _cc.isGrounded)
        {
            ResetInputMouseDown();
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        else if (_playerInput.spaceKeyDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
            return;
        }

        // 1. Nhận đầu vào và chuẩn hóa tốc độ chéo
        Vector3 inputDrive = new Vector3(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        inputDrive = Vector3.ClampMagnitude(inputDrive, 1f);

        // 2. Tính hướng di chuyển theo Camera phẳng (Bỏ trục Y)
        if (_mainCameraTransform != null)
        {
            Vector3 camForward = _mainCameraTransform.forward;
            Vector3 camRight = _mainCameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            _movementDirection = (camForward * inputDrive.z) + (camRight * inputDrive.x);
        }
        else
        {
            _movementDirection = inputDrive;
        }

        // Cập nhật giá trị Speed cho Animator chuyển cảnh chạy/đi bộ
        _animator.SetFloat(SPEED_PARAM, inputDrive.magnitude);

        // 3. Xoay mượt nhân vật nhìn theo hướng di chuyển
        if (_movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    private void ResetInputMouseDown()
    {
        _playerInput.mouseButtonDown = false;
    }
    protected override void SwitchStateTo(CharacterState newState)
    {

        _playerInput.ClearCache();

        if(currentState == CharacterState.Attacking)
        {
            PlayerVFXManager.Instance.StopBlade();
        }
        if (currentState == CharacterState.Spawn)
        {
            isInvincible = false;
        }
        base.SwitchStateTo(newState);
        if(newState == CharacterState.Attacking)
        {
            attackStartTime = Time.time;
        }

        if(newState == CharacterState.BeingHit)
        {
            isInvincible = true;
            StartCoroutine(DelayCancelInvincible());
        }

        if(newState == CharacterState.Slide)
        {
            _animator.SetTrigger(SLIDE_PARAM);
        }
        if(newState == CharacterState.Spawn)
        {
            isInvincible = true;
            currentSpawnTime = spawnDuration;
            StartCoroutine(MaterialAppear());
        }
    }
    public override void ApplyDamage(int damage, Vector3 attackPos = default)
    {
        if (isInvincible) return;
        base.ApplyDamage(damage, attackPos);
        SwitchStateTo(CharacterState.BeingHit);
        AddImpact(attackPos, 10f);
    }
    private void AddImpact(Vector3 attackerPos, float force)
    {
        Vector3 impactDir = transform.position - attackerPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharacter = impactDir * force;
    }

    private IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    public override void PickUpItem(PickUp item)
    {
        switch (item.type)
        {
            case PickUp.PickUpType.Heal:
                AddHealth(item.value);
                break;

            case PickUp.PickUpType.Coin:
                AddCoin(item.value);
                break;

        }
    }
    private void AddHealth(int health)
    {
        this.health.AddHealth(health);
        PlayerVFXManager.Instance.PlayHealVFX();
    }
    private void AddCoin(int coin)
    {
        this.coin += coin;
        AddCoinEventAction?.Invoke(coin);
    }
    private void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
}