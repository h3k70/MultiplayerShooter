using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    private Vector3 _direction;

    private void Update()
    {
        Move();
    }

    public void SetInput(Vector3 direction)
    {
        _direction = direction;
    }

    public void GetMoveInfo(out Vector3 position)
    {
        position = transform.position;
    }

    private void Move()
    {
        transform.position += _direction * Time.deltaTime * _speed;
    }
}
