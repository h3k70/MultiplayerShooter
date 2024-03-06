using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private Vector3 _targetPosition = Vector3.zero;
    private float _velocityMagnitude = 0;
    private float _minVelocityMagnitude = 0.1f;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if(_velocityMagnitude > _minVelocityMagnitude)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        }
        else
        {
            transform.position = _targetPosition;
        }
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        _targetPosition = position + velocity * averageInterval;
        _velocityMagnitude = velocity.magnitude;
    }
}
