using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private readonly List<PlayerInput> _players = new();

    [SerializeField]
    private List<Transform> startingPoints;

    [SerializeField]
    private List<LayerMask> playerLayers;

    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
    }

    private void AddPlayer(PlayerInput player)
    {
        _players.Add(player);

        // Need to use the parent due to the structure of the prefab
        Transform playerParent = player.transform.parent;
        playerParent.position = startingPoints[_players.Count - 1].position;
        playerParent.GetComponentInChildren<CinemachineInputProvider>().PlayerIndex = player.playerIndex;

        // Convert layer mask (bit) to an integer 
        var layerToAdd = (int)Mathf.Log(playerLayers[_players.Count - 1].value, 2);

        // Set the layer
        playerParent.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;

        // Add the layer
        var bitMask = (1 << layerToAdd) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5) | (1 << 8);
        playerParent.GetComponentInChildren<Camera>().cullingMask = bitMask;

        // Set the action in the custom Input Handler
        playerParent.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("Look");
    }
}