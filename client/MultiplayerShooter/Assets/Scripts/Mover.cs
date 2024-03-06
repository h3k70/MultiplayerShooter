using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetInput(Vector3 direction)
    {
        _direction = direction;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
    }

    private void Move()
    {
        Vector3 velocity = transform.TransformVector(_direction);
        _rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z) * _speed;
    }
}
