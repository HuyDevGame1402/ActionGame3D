using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class CharacterBase : MonoBehaviour
{
    protected const string SPEED_PARAM = "Speed";
    protected const string AIRBORNE_PARAM = "AirBorne";
    protected const string ATTACK_PARAM = "Attack";
    protected const string DEAD_PARAM = "Dead";
    protected const string BEHINGHIT_PARAM = "BeingHit";

    protected CharacterController _cc;
    protected Animator _animator;

    [Header("Base Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float gravity = -20f;

    protected Vector3 _movementDirection;
    protected float _verticalVelocity;

    public GameObject itemToDrop;

    public enum CharacterState
    {
        Normal,
        Attacking,
        Dead,
        BeingHit,
    }
    public CharacterState currentState;

    protected Health health;
    [SerializeField] private DamageCaster damageCaster;

    private MaterialPropertyBlock materialPropertyBlock;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    protected virtual void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(materialPropertyBlock);

        if(health != null)
        {
            health.OnDead += SwitchStateDead;
        }
    }

    protected virtual void FixedUpdate()
    {
        // Các lớp con sẽ ghi đè logic xử lý di chuyển ở đây
    }

    protected virtual void ApplyGravityAndMove()
    {
        if (_cc == null) return;

        // Xử lý trọng lực ghì nhân vật xuống đất hoặc rơi tự do
        if (_cc.isGrounded)
        {
            _verticalVelocity = gravity * 0.2f;
        }
        else
        {
            _verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        // Tạo Vector vận tốc cuối cùng (Di chuyển ngang + Rơi tự do dọc)
        Vector3 finalVelocity = (_movementDirection * moveSpeed) + (Vector3.up * _verticalVelocity);

        // Thực hiện di chuyển vật lý
        _cc.Move(finalVelocity * Time.fixedDeltaTime);

        // Cập nhật trạng thái trên không cho Animator
        _animator.SetBool(AIRBORNE_PARAM, !_cc.isGrounded);
    }
    protected virtual void SwitchStateTo(CharacterState newState)
    {
        
        switch (currentState)
        {
            case CharacterState.Normal:
                break;

            case CharacterState.Attacking:

                //if(damageCaster != null)
                //{
                //    DisableDamageCaster();
                //}
                break;

            case CharacterState.Dead:

                return;

            case CharacterState.BeingHit:

                break;
        }
        switch (newState)
        {
            case CharacterState.Normal:
                break;

            case CharacterState.Attacking:
                _animator.SetTrigger(ATTACK_PARAM);
                break;
            case CharacterState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger(DEAD_PARAM);
                StartCoroutine(MaterialDissolve());
                break;

            case CharacterState.BeingHit:
                _animator.SetTrigger(BEHINGHIT_PARAM);
                break;
        }
        currentState = newState;
    }
    private void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public virtual void ApplyDamage(int damage, Vector3 attackPos = new Vector3())
    {
        if(health != null)
        {
            health.ApplyDamage(damage);
        }
        StartCoroutine(MaterialBlink());
    }

    public void EnableDamageCaster()
    {
        damageCaster.EnableDamageCaster();
    }
    public void DisableDamageCaster()
    {
        damageCaster.DisableDamageCaster();
    }

    protected IEnumerator MaterialBlink()
    {
        materialPropertyBlock.SetFloat("_blink", 0.4f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
        yield return new WaitForSeconds(0.2f);
        materialPropertyBlock.SetFloat("_blink", 0);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
    private void SwitchStateDead(object sender, EventArgs o)
    {
        SwitchStateTo(CharacterState.Dead);
    }
    protected IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);
        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0f;
        float dissolveHightStart = 20f;
        float dissolveHightTarget = -10f;
        float dissolveHight;
        materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
        while(currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHight = Mathf.Lerp(dissolveHightStart, dissolveHightTarget, currentDissolveTime
                / dissolveTimeDuration);
            materialPropertyBlock.SetFloat("_dissolve_height", dissolveHight);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
        DropItem();
    }
    protected void DropItem()
    {
        if(itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }
    public void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public virtual void PickUpItem(PickUp item)
    {

    }
}