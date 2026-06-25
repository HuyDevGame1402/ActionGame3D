using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class CharacterBase : MonoBehaviour
{
    protected const string SPEED_PARAM = "Speed";
    protected const string AIRBORNE_PARAM = "AirBorne";
    protected const string ATTACK_PARAM = "Attack";

    protected CharacterController _cc;
    protected Animator _animator;

    [Header("Base Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float gravity = -20f;

    protected Vector3 _movementDirection;
    protected float _verticalVelocity;

    public enum CharacterState
    {
        Normal,
        Attacking,

    }
    public CharacterState currentState;

    private Health health;

    protected virtual void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        health = GetComponent<Health>();
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
                break;
        }
        switch (newState)
        {
            case CharacterState.Normal:
                break;

            case CharacterState.Attacking:
                _animator.SetTrigger(ATTACK_PARAM);
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
    }
}