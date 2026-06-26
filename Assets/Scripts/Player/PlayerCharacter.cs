using UnityEngine;

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
                _movementDirection = Vector3.zero;

                if (Time.time < attackStartTime + attackSlideDuration)
                {
                    timePassed = Time.time - attackStartTime;
                    lerpTime = timePassed / attackSlideDuration;
                    _movementDirection = Vector3.Lerp(transform.forward * attackSlideSpeed,
                        Vector3.zero, lerpTime);
                }

                break;
            case CharacterState.Dead:
                return;
        }
        ApplyGravityAndMove(); // Sử dụng lại logic trọng lực và di chuyển từ lớp cha
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
        base.SwitchStateTo(newState);
        if(newState == CharacterState.Attacking)
        {
            attackStartTime = Time.time;
        }
    }
}