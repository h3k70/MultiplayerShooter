using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckGrounded))]
[RequireComponent(typeof(Mover))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Mover _mover;
    private CheckGrounded _checkGrounded;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _checkGrounded = GetComponent<CheckGrounded>();
    }

    private void Update()
    {
        Vector3 localVelocity = _mover.transform.InverseTransformVector(_mover.Velocity);
        float speed = localVelocity.magnitude / _mover.Speed;
        float sing = Mathf.Sign(localVelocity.z);

        _animator.SetFloat(HashPlayerAnimation.Speed, speed * sing);
        _animator.SetBool(HashPlayerAnimation.Grounded, _checkGrounded.IsGrounded);
    }
}
