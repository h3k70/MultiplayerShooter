using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrounded : MonoBehaviour
{
    [SerializeField] private float _angleForJump = 0.50f;

    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void OnCollisionStay(Collision collision)
    {
        var contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (contactPoints[i].normal.y > _angleForJump) _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }

    public void SetGrounded(bool value)
    {
        _isGrounded = value;
    }
}
