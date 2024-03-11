using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _sensetivity = 1f;

    private PlayerInput _playerInput;
    private Vector3 _direction;
    private Vector2 _look;
    private PlayerMover _mover;
    private bool _isMenuOpen;

    public float Sensetivity => _sensetivity;

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Look.performed += OnLook;
        _playerInput.Player.Look.canceled += OnLookStop;
        _playerInput.Player.Jump.performed += OnJump;

        _playerInput.Player.Menu.performed += OnMenu;
    }

    private void Start()
    {
        EnableMotion();
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

    public void SetSensetivity(float value)
    {
        _sensetivity = value;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.action.ReadValue<Vector3>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _look = context.action.ReadValue<Vector2>();
        _look *= _sensetivity;
    }

    private void OnLookStop(InputAction.CallbackContext context)
    {
        _look = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _mover.Jump();
    }

    private void OnMenu(InputAction.CallbackContext obj)
    {
        _isMenuOpen = _isMenuOpen ? false : true;

        if (_isMenuOpen)
            DisableMotion();
        else
            EnableMotion();
    }

    private void DisableMotion()
    {
        _mover.enabled = false;
        ShowCursor();
    }

    private void EnableMotion()
    {
        _mover.enabled = true;
        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
