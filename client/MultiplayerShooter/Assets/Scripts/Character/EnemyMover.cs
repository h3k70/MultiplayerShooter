using UnityEngine;

public class EnemyMover : Mover
{
    [SerializeField] private Transform _rotatePointY;

    private Vector3 _targetPosition = Vector3.zero;
    private float _velocityMagnitude = 0;
    private float _minVelocityMagnitude = 0.1f;

    private Vector3 _targetRotate = Vector3.zero;
    private Vector3 _lastRotate = Vector3.zero;
    private float _rotateSpeedX;
    private float _rotateSpeedY;
    private float _minRotate = 0.01f;
    private float _minRotateSpeed = 0.1f;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void LateUpdate()
    {
        Move();
        RotateY();
        RotateX();
    }

    public void SetSpeed(float value) => Speed = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        _targetPosition = position + velocity * averageInterval;
        Velocity = velocity;
        _velocityMagnitude = velocity.magnitude;
    }

    public void SetRotateX(in float value, in float averageInterval)
    {
        //transform.localEulerAngles = new Vector3(0, value, 0);
        _rotateSpeedY = value - _lastRotate.y;

        if (Mathf.Abs(_rotateSpeedY) > 180)
        {
            if (_rotateSpeedY < 0)
                _rotateSpeedY += 360;
            else
                _rotateSpeedY -= 360;
        }
        _targetRotate.y = value + _rotateSpeedY * averageInterval;
        _lastRotate.y = value;
    }

    public void SetRotateY(float value, in float averageInterval)
    {
        //foreach (var item in _rotatabls)
        //{
        //    item.localEulerAngles = new Vector3(value, 0, 0);
        //}
        _rotateSpeedX = value - _lastRotate.x;

        if (Mathf.Abs(_rotateSpeedX) > 180)
        {
            if (_rotateSpeedX < 0)
                _rotateSpeedX += 360;
            else
                _rotateSpeedX -= 360;
        }
        _targetRotate.x = value + _rotateSpeedX * averageInterval;
        _lastRotate.x = value;
    }

    private void Move()
    {
        if (_velocityMagnitude > _minVelocityMagnitude)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);
        }
    }

    private void RotateX()
    {
        float maxRotate = (Mathf.Abs(_rotateSpeedY) * Time.deltaTime);

        if(maxRotate > _minRotate)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, _targetRotate.y, 0), maxRotate);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, _targetRotate.y, 0), _minRotateSpeed);
        }
    }

    private void RotateY()
    {
        float maxRotate = (Mathf.Abs(_rotateSpeedX) * Time.deltaTime);

        if(maxRotate > _minRotate)
        {
            _rotatePointY.localRotation = Quaternion.Lerp(_rotatePointY.localRotation, Quaternion.Euler(_targetRotate.x, 0, 0), maxRotate);
        }
        else
        {
            _rotatePointY.localRotation = Quaternion.Lerp(_rotatePointY.localRotation, Quaternion.Euler(_targetRotate.x, 0, 0), _minRotateSpeed);
        }
    }
}
