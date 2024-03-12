using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _sensetivity = 1f;
    [SerializeField] Hand _hand;

    private MultiplayerManager _multiplayerManager;
    private PlayerInput _playerInput;
    private Vector3 _direction;
    private Vector2 _look;
    private PlayerMover _mover;
    private bool _isMenuOpen;
    private Coroutine _fireJob;

    public float Sensetivity => _sensetivity;

    private void Awake()
    {
        _multiplayerManager = MultiplayerManager.Instance;

        _mover = GetComponent<PlayerMover>();

        _playerInput = new PlayerInput();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Look.performed += OnLook;
        _playerInput.Player.Look.canceled += OnLookStop;
        _playerInput.Player.Jump.performed += OnJump;

        _playerInput.Player.Fire.performed += OnStartFire;
        _playerInput.Player.Fire.canceled += OnStopFire;

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

    private void OnStartFire(InputAction.CallbackContext obj)
    {
        _fireJob = StartCoroutine(Fire());
    }

    private void OnStopFire(InputAction.CallbackContext obj)
    {
        StopCoroutine(_fireJob);
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
        ShowCursor();
        _hand.enabled = false;
        _mover.enabled = false;
    }

    private void EnableMotion()
    {
        HideCursor();
        _hand.enabled = true;
        _mover.enabled = true;
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
        _mover.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x},
            { "pY", position.y},
            { "pZ", position.z},

            { "vX", velocity.x},
            { "vY", velocity.y},
            { "vZ", velocity.z},

            { "rX", rotateX },
            { "rY", rotateY }
        };
        _multiplayerManager.SendMessage("move", data);
    }

    private void SendShoot(ShootInfo info)
    {
        info.key = _multiplayerManager.GetClientKey();
        string json = JsonUtility.ToJson(info);
        _multiplayerManager.SendMessage("shoot", json);
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            if(_hand.TryUse())
                SendShoot(_hand.GetShootInfo());
            yield return null;
        }
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string key;

    public float pX;
    public float pY;
    public float pZ;

    public float dX;
    public float dY;
    public float dZ;
}
