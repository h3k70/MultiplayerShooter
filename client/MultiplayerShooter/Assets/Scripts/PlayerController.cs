using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Mover))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Vector3 _direction;
    private Mover _mover;

    private void Awake()
    {
        _mover = GetComponent<Mover>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMove;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Update()
    {
        _mover.SetInput(_direction);
        SendMove();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.action.ReadValue<Vector3>();
    }

    private void SendMove()
    {
        _mover.GetMoveInfo(out Vector3 position, out Vector3 velocity);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x},
            { "pY", position.y},
            { "pZ", position.z},
            { "vX", velocity.x},
            { "vY", velocity.y},
            { "vZ", velocity.z}
        };
        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
