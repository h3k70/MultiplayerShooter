using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _sensetivity = 3f;

    private PlayerInput _playerInput;
    private Vector3 _direction;
    private Vector2 _look;
    private PlayerMover _mover;

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Look.performed += OnLook;
        _playerInput.Player.Look.canceled += OnLookStop;
        _playerInput.Player.Jump.performed += OnJump;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Update()
    {
        _mover.SetMoveInput(_direction);
        _mover.SetRotateInput(_look);
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

    private void OnLook(InputAction.CallbackContext context)
    {
        _look = context.action.ReadValue<Vector2>();
    }

    private void OnLookStop(InputAction.CallbackContext context)
    {
        _look = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _mover.Jump();
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
