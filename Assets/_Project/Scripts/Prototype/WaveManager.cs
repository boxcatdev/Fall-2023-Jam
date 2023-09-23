using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public float initialStartDelay = 5.0f;

    [Space]
    public Wave[] waves;
    
    private Queue<Wave> _waveQueue;

    private bool _hasStarted = false;
    private bool _canSpawn = false;

    private float _spawnTimer;

    #region Struct
    [Serializable]
    public struct Wave
    {
        [Tooltip("Spawn all the enemies in the wave at once.")]
        public bool spawnAll;
        [Tooltip("Time between each enemy being spawned")]
        public float spawnDelay;
        public EnemySpawn[] enemyTypes;
    }
    [Serializable]
    public struct EnemySpawn
    {
        public GameObject prefab;
        public int count;
    }
    #endregion

    #region Monobehavior
    private void Start()
    {
        //put waves into a queue
        if(waves.Length > 0)
        {
            _waveQueue = new Queue<Wave>();

            for (int i = 0; i < waves.Length; i++)
            {
                _waveQueue.Enqueue(waves[i]);
            }
            Debug.Log("Wave count: " + waves.Length);

            _canSpawn = true;
        }
        else
        {
            Debug.LogError("Requires at least 1 enemy wave.");
        }
        

    }
    private void Update()
    {
        if (_canSpawn == false) return;

        #region Before Waves
        if(_hasStarted == false)
        {
            initialStartDelay -= Time.deltaTime;
            if (initialStartDelay <= 0)
            {
                _hasStarted = true;
            }
        }
        #endregion

        #region
        if (_hasStarted)
        {
            Wave currentWave = _waveQueue.Peek();
            //currentWave.
        }
        #endregion
    }
    #endregion

}
