using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Useable
{
    [SerializeField] private float _fireDelay = 0.3f;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _bulletSpeed = 15;

    private ShootInfo _shootInfo;
    private float _lastShotTime;

    public override ShootInfo GetShootInfo()
    {
        return _shootInfo;
    }

    public override bool TryUse()
    {
        if (Time.time - _lastShotTime < _fireDelay) return false;

        Vector3 velocity = _firePoint.forward * _bulletSpeed;

        Instantiate(_bullet, _firePoint.position, _firePoint.rotation).Init(velocity);
        _lastShotTime = Time.time;

        Vector3 position = _firePoint.position;
        _shootInfo.pX = position.x;
        _shootInfo.pY = position.y;
        _shootInfo.pZ = position.z;
        _shootInfo.dX = velocity.x;
        _shootInfo.dY = velocity.y;
        _shootInfo.dZ = velocity.z;

        return true;
    }
}
