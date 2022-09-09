using UnityEngine;

public class ThirdPersonAnimation : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;
    private const float MaxSpeed = 5f;
    private static readonly int Speed = Animator.StringToHash("speed");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _animator.SetFloat(Speed, _rb.velocity.magnitude / MaxSpeed);
    }
}