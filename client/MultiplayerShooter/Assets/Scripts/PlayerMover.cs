using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 80;
    [SerializeField] private float _minHeadAngle = -80;

    private Transform _camera;
    private Rigidbody _rigidbody;
    private Vector3 _directionMove;
    private Vector2 _directionLook;
    private float _currentRotateY;
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        CameraCaptor();
        HideCursor();
    }

    private void FixedUpdate()
    {
        Move();
        RotateX();
    }

    private void Update()
    {
        RotateY();
    }

    private void OnCollisionStay(Collision collision)
    {
        _isGrounded = true;
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
        if (_isGrounded)
        {
            _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
            _isGrounded = false;
        }
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
    }

    private void CameraCaptor()
    {
        _camera = Camera.main.transform;
        _camera.parent = _cameraPoint;
        _camera.localPosition = Vector3.zero;
        _camera.localRotation = Quaternion.identity;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Move()
    {
        Vector3 velocity = transform.TransformVector(_directionMove) * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private void RotateY()
    {
        _currentRotateY = Mathf.Clamp(_currentRotateY + _directionLook.y, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateY, 0, 0);
    }

    private void RotateX()
    {
        _rigidbody.angularVelocity = new Vector3(0, _directionLook.x, 0);
        _directionLook.x = 0;
    }
}
