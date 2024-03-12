using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CheckGrounded))]
public class PlayerMover : Mover
{
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _gravityModifier = 0.2f;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private Transform[] _rotateWithCamera;
    [SerializeField] private float _maxHeadAngle = 80;
    [SerializeField] private float _minHeadAngle = -80;

    private Transform _camera;
    private Rigidbody _rigidbody;
    private CheckGrounded _checkGrounded;
    private Vector3 _directionMove;
    private Vector2 _directionLook;
    private float _currentRotateY;
    private float _lastRotateY;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _checkGrounded = GetComponent<CheckGrounded>();
    }

    private void Start()
    {
        CameraCaptor();
    }

    private void FixedUpdate()
    {
        Move();
        RotateX();
        Gravity();
    }

    private void Update()
    {
        RotateY();
    }

    private void OnDisable()
    {
        _directionLook.x = 0;
        RotateX();
    }

    public void SetMoveInput(Vector3 direction)
    {
        _directionMove = direction;
    }

    public void SetRotateInput(Vector2 direction)
    {
        _directionLook.y = direction.y;
        _directionLook.x += direction.x;
    }

    public void Jump()
    {
        if (_checkGrounded.IsGrounded && enabled == true)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
            //_checkGrounded.SetGrounded(false);
        }
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotateX = transform.localEulerAngles.y;
        rotateY = _cameraPoint.localEulerAngles.x;
    }

    private void CameraCaptor()
    {
        _camera = Camera.main.transform;
        _camera.parent = _cameraPoint;
        _camera.localPosition = Vector3.zero;
        _camera.localRotation = Quaternion.identity;
    }

    private void Move()
    {
        Vector3 velocity = transform.TransformVector(_directionMove) * Speed;
        velocity.y = _rigidbody.velocity.y;
        Velocity = velocity;
        _rigidbody.velocity = Velocity;
    }

    private void RotateY()
    {
        _lastRotateY = _currentRotateY;
        _currentRotateY = Mathf.Clamp(_currentRotateY + _directionLook.y, _minHeadAngle, _maxHeadAngle);
        _cameraPoint.localEulerAngles = new Vector3(_currentRotateY, 0, 0);

        foreach (var item in _rotateWithCamera)
        {
            item.RotateAround(_cameraPoint.position, _cameraPoint.right, _currentRotateY - _lastRotateY);
        }
    }

    private void RotateX()
    {
        _rigidbody.angularVelocity = new Vector3(0, _directionLook.x, 0);
        _directionLook.x = 0;
    }

    private void Gravity()
    {
        _rigidbody.AddForce(0, -_gravityModifier, 0, ForceMode.VelocityChange);
    }
}
