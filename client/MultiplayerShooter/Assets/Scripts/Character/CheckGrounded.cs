using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _radius = 0.30f;

    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    public void SetGrounded(bool value)
    {
        _isGrounded = value;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
