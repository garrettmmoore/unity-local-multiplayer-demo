using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class InputHandler : MonoBehaviour, AxisState.IInputAxisProvider
{
    [HideInInspector]
    public InputAction horizontal;
    [HideInInspector]
    public InputAction vertical;

    public float GetAxisValue(int axis)
    {
        return axis switch
        {
            0 => horizontal.ReadValue<Vector2>().x,
            1 => horizontal.ReadValue<Vector2>().y,
            2 => vertical.ReadValue<float>(),
            _ => 0
        };
    }
}