using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCharacter : CharacterBase
{
    private NavMeshAgent _navMeshAgent;
    private Transform _targetPlayer;

    protected override void Awake()
    {
        base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();

        // BẬT 2 dòng này: Bắt NavMeshAgent giao quyền di chuyển lại cho CharacterController
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = true; // Vẫn để agent tự xoay hướng mượt mà

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _targetPlayer = playerObj.transform;

        SwitchStateTo(CharacterState.Spawn);
    }

    protected override void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                CalculateEnemyMovement();
                break;
            case CharacterState.Dead:
                return;

            case CharacterState.Spawn:
                currentSpawnTime -= Time.deltaTime;
                if(currentSpawnTime <= 0)
                {
                    SwitchStateTo(CharacterState.Normal);
                }
                break;
        }
        ApplyGravityAndMove();

        // ĐỒNG BỘ vị trí của NavMeshAgent theo vị trí thực của CharacterController
        _navMeshAgent.nextPosition = transform.position;
    }

    private void CalculateEnemyMovement()
    {
        if (_targetPlayer == null || _navMeshAgent == null) return;

        if (Vector3.Distance(_targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(_targetPlayer.position);

            // Lấy hướng đi mong muốn tiếp theo của Agent để gán vào biến vận tốc lớp cha
            _movementDirection = _navMeshAgent.desiredVelocity.normalized;

            _animator.SetFloat(SPEED_PARAM, 0.2f);
        }
        else
        {
            _movementDirection = Vector3.zero; // Dừng lại
            _animator.SetFloat(SPEED_PARAM, 0f);
            SwitchStateTo(CharacterState.Attacking);
        }

        // Lúc này _cc.isGrounded đã hoạt động chính xác vì có ApplyGravityAndMove() xử lý bên dưới
        _animator.SetBool(AIRBORNE_PARAM, !_cc.isGrounded);
    }
    protected override void SwitchStateTo(CharacterState newState)
    {
        if (newState == CharacterState.Attacking)
        {
            Quaternion newRotation = Quaternion.LookRotation(_targetPlayer.position - transform.position);
            transform.rotation = newRotation;
        }

        base.SwitchStateTo(newState);
    }

    public override void ApplyDamage(int damage, Vector3 attackPos = default)
    {
        base.ApplyDamage(damage, attackPos);
        GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackPos);
    }
    public void RotateToTarget()
    {
        if(currentState != CharacterState.Dead)
        {
            transform.LookAt(_targetPlayer, Vector3.up);
        }
    }
}