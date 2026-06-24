using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private const string SPEED = "Speed";
    private const string AIRBORNE = "AirBorne";

    private CharacterController _cc;
    private Animator _animator;
    private PlayerInput _playerInput;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f; // Tốc độ xoay mượt (Lerp/Slerp)
    [SerializeField] private float gravity = -20f;
    [SerializeField] private bool isPlayer = true;

    private Vector3 _movementDirection;
    private float _verticalVelocity;
    private Transform targetPlayer;
    private Transform _mainCameraTransform;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        if (!isPlayer)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) targetPlayer = playerObj.transform;

            if (_navMeshAgent != null) _navMeshAgent.speed = moveSpeed;
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
            if (Camera.main != null) _mainCameraTransform = Camera.main.transform;
        }
    }

    private void CalculatePlayerMovement()
    {
        // 1. Lấy đầu vào từ bàn phím WASD
        Vector3 inputDrive = new Vector3(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        inputDrive = Vector3.ClampMagnitude(inputDrive, 1f); // Giữ tốc độ đi chéo không bị nhanh hơn đi thẳng

        // 2. Tính toán hướng di chuyển dựa theo hướng Camera (Nếu không có camera sẽ đi theo hướng thế giới)
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

        // Update Animator tham số Speed dựa trên độ dài vector đầu vào
        _animator.SetFloat(SPEED, inputDrive.magnitude);

        // 3. Xử lý xoay nhân vật mượt mà bằng Quaternion.Slerp
        if (_movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        _animator.SetBool(AIRBORNE, !_cc.isGrounded);
    }

    private void CalculateEnemyMovement()
    {
        if (targetPlayer == null || _navMeshAgent == null) return;

        if (Vector3.Distance(targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(targetPlayer.position);
            _animator.SetFloat(SPEED, 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat(SPEED, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (isPlayer)
        {
            CalculatePlayerMovement();
            ApplyGravityAndMove();
        }
        else
        {
            CalculateEnemyMovement();
        }
    }

    private void ApplyGravityAndMove()
    {
        // Xử lý trọng lực chính xác
        if (_cc.isGrounded)
        {
            _verticalVelocity = gravity * 0.2f; // Giữ một lực nhỏ ghì nhân vật xuống mặt đất
        }
        else
        {
            _verticalVelocity += gravity * Time.fixedDeltaTime; // Rơi tự do tăng dần theo thời gian
        }

        // Kết hợp vận tốc di chuyển ngang và vận tốc rơi dọc
        Vector3 finalVelocity = (_movementDirection * moveSpeed) + (Vector3.up * _verticalVelocity);

        // Di chuyển CharacterController một lần duy nhất trong FixedUpdate sử dụng Time.fixedDeltaTime
        _cc.Move(finalVelocity * Time.fixedDeltaTime);
    }
}