using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Useable _startWeapon;

    private Useable _currentItem;

    private void Awake()
    {
        _currentItem = Instantiate(_startWeapon, transform.position, transform.rotation, transform);
    }

    public ShootInfo GetShootInfo()
    {
        return _currentItem.GetShootInfo();
    }

    public bool TryUse()
    {
        if (enabled == false) return false;

        return _currentItem.TryUse();
    }
}
