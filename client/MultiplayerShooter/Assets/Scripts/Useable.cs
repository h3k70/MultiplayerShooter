using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Useable : MonoBehaviour
{
    abstract public bool TryUse();
    abstract public ShootInfo GetShootInfo();
}
