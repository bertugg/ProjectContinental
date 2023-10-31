using System;
using System.Collections.Generic;
using UnityEngine;

public class AimController : Singleton<AimController>
{
    private List<Enemy> _enemyList;
    private Enemy _currentEnemy;

    public bool hasEnemy
    {
        get => _currentEnemy != null;
    }

    public Transform CurrentEnemyTransform
    {
        get => _currentEnemy.aimPoint;
    }

    private void Start() => _enemyList = new List<Enemy>();

    public void AddEnemy(Enemy target)
    {
        if(_enemyList.Contains(target))
            return;
        
        _enemyList.Add(target);
        CheckCurrentEnemy();
    }

    public void RemoveEnemy(Enemy target)
    {
        if(!_enemyList.Contains(target))
            return;
        _enemyList.Remove(target);
        CheckCurrentEnemy();
    }

    private void CheckCurrentEnemy()
    {
        if (_enemyList.Count <= 0) // No enemy Scenario
        {
            _currentEnemy = null;
            return;
        }
        
        /*
        float enemyDistance = 100000000;
        _currentEnemy = null;
        for (int i = 0; i < _enemyList.Count; i++)
        {
            if (Vector3.Distance(_enemyList[i].transform.position, transform.position) < enemyDistance)
            {
                _currentEnemy = _enemyList[i];
                enemyDistance = Vector3.Distance(_enemyList[i].transform.position, transform.position);
            }
        }
        */
        
        float dot = -2;

        for(int i  = 0; i < _enemyList.Count; i++) {
            // store the Dot compared to the camera's forward position (or where the object is locally in the camera's space)
            // Very important that the point is normalized.

            Vector3 localPoint = Camera.main.transform.InverseTransformPoint(_enemyList[i].transform.position).normalized;
            Vector3 forward = Vector3.forward;
            float test = Vector3.Dot(localPoint, forward);
            if (test > dot)
            {
                dot = test;
                _currentEnemy = _enemyList[i];
            }
        }
    }
}
