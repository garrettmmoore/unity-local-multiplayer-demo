using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // Input fields
    private InputActionAsset _inputAsset;
    private InputActionMap _player;
    private InputAction _move;

    // Movement fields
    private Rigidbody _rb;

    [SerializeField]
    private float movementForce = 1f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float maxSpeed = 5f;

    private Vector3 _forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    // Animation fields
    private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("attack");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _inputAsset = GetComponent<PlayerInput>().actions;
        _player = _inputAsset.FindActionMap("Player");
    }

    private void OnEnable()
    {
        _player.FindAction("Jump").started += DoJump;
        _player.FindAction("Attack").started += DoAttack;
        _move = _player.FindAction("Move");
        _player.Enable();
    }

    private void OnDisable()
    {
        _player.FindAction("Jump").started -= DoJump;
        _player.FindAction("Attack").started -= DoAttack;
        _player.Disable();
    }

    private void FixedUpdate()
    {
        _forceDirection += GetCameraRight(playerCamera) * (_move.ReadValue<Vector2>().x * movementForce);
        _forceDirection += GetCameraForward(playerCamera) * (_move.ReadValue<Vector2>().y * movementForce);

        _rb.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = Vector3.zero;

        if (_rb.velocity.y < 0f)
        {
            _rb.velocity -= Vector3.down * (Physics.gravity.y * Time.fixedDeltaTime);
        }

        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            _rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * _rb.velocity.y;
        }

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = _rb.velocity;
        direction.y = 0f;

        if (_move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private static Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private static Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            _forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        var ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
        return Physics.Raycast(ray, out _, 0.5f);
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        _animator.SetTrigger(Attack);
    }
}