using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleOnPlayerJoin : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += ToggleThis;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= ToggleThis;
    }

    private void ToggleThis(PlayerInput player)
    {
        Debug.Log(player);
        gameObject.SetActive(false);
    }
}