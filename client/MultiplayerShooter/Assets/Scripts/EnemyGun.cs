using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    public void Shoot(Vector3 position, Vector3 velocity)
    {
        Debug.Log(1);
        Instantiate(_bullet, position, Quaternion.identity).Init(velocity);
    }
}
