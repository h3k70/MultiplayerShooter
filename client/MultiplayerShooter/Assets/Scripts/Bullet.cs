using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private Rigidbody _rigidbody;

    public void Init(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
        StartCoroutine(DeleyDestroy());
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator DeleyDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }
}
