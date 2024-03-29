using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemyGun))]
public class EnemyController : MonoBehaviour
{
    private EnemyGun _enemyGun;
    private Player _player;
    private EnemyMover _mover;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _position;
    private List<float> _receiveTimeInterval = new List<float> {0, 0, 0, 0, 0};
    private float _lastReceiveTime = 0f;

    public float AverageInterval
    {
        get
        {
            float summ = 0;

            for (int i = 0; i < _receiveTimeInterval.Count; i++)
            {
                summ += _receiveTimeInterval[i];
            }
            return summ / _receiveTimeInterval.Count;
        }
    }

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _enemyGun = GetComponent<EnemyGun>();
    }

    private void Start()
    {
        _position = transform.position;
    }

    public void Init(Player player)
    {
        _player = player;
        _mover.SetSpeed(player.speed);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

        _enemyGun.Shoot(position, velocity);
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveReceiveTime()
    {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeInterval.Add(interval);
        _receiveTimeInterval.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        SaveReceiveTime();

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    _position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    _position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    _position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    _velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    _velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    _velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _mover.SetRotateX((float)dataChange.Value, AverageInterval);
                    break;
                case "rY":
                    _mover.SetRotateY((float)dataChange.Value, AverageInterval);
                    break;
                default:
                    Debug.LogWarning($"�� �������������� ��������� ���� - {dataChange.Field}");
                    break;
            }
        }
        _mover.SetMovement(_position, _velocity, AverageInterval);
    }
}
