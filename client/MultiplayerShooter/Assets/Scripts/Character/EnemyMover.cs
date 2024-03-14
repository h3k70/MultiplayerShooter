using UnityEngine;

public class EnemyMover : Mover
{
    [SerializeField] private Transform[] _rotatabls;

    private Vector3 _targetPosition = Vector3.zero;
    private float _velocityMagnitude = 0;
    private float _minVelocityMagnitude = 0.1f;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void LateUpdate()
    {
        if(_velocityMagnitude > _minVelocityMagnitude)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);
        }
    }

    public void SetSpeed(float value) => Speed = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        _targetPosition = position + velocity * averageInterval;
        Velocity = velocity;
        _velocityMagnitude = velocity.magnitude;
    }

    public void SetRotateX(float value)
    {
        transform.localEulerAngles = new Vector3(0, value, 0);
    }

    public void SetRotateY(float value)
    {
        foreach (var item in _rotatabls)
        {
            item.localEulerAngles = new Vector3(value, 0, 0);
        }
    }
}
