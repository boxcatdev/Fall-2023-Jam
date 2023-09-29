using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;

    public Vector3 GetSpecificSpawnPoint(int order)
    {
        if(order < spawnPoints.Length)
        {
            return spawnPoints[order].position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 GetRandomSpawnPoint()
    {
        int rand = UnityEngine.Random.Range(0, spawnPoints.Length);

        return spawnPoints[rand].position;
    }
    public int GetSpawnPointCount()
    {
        return spawnPoints.Length;
    }
}
