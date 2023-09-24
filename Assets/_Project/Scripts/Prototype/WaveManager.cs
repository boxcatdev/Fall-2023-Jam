using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //references
    private Spawner spawner;

    //properties
    public float initialStartDelay = 5.0f;

    //waves
    [Space]
    public Wave[] waves;
    private Queue<Wave> _waveQueue;

    //private
    private bool _hasStarted = false;
    private bool _canSpawn = false;

    [Header("Debug")]
    [SerializeField]
    private int _enemiesLeft = 0;

    private bool _waveStart = false;
    private bool _spawnBool = true;
    [SerializeField]
    private float _spawnTimer;

    #region Struct
    [Serializable]
    public struct Wave
    {
        [Tooltip("Spawn all the enemies in the wave at once.")]
        public bool spawnAll;
        [Tooltip("Delay before starting this wave.")]
        public float startDelay;
        [Tooltip("Time between each enemy being spawned")]
        public float spawnDelay;
        public EnemySpawn[] enemyTypes;
    }
    [Serializable]
    public struct EnemySpawn
    {
        public Transform prefab;
        public int count;
    }
    #endregion

    #region Monobehavior
    private void Awake()
    {
        spawner = GetComponent<Spawner>();
        if (spawner == null) Debug.LogError("Missing spawner reference.");
    }
    private void Start()
    {
        SetupQueue();
    }
    private void Update()
    {
        if (_canSpawn == false) return;

        #region Timer
        /*if(_spawnBool == true)
        {
            _spawnTimer -= Time.deltaTime;
        }*/
        #endregion

        #region Before Waves
        if (_hasStarted == false)
        {
            initialStartDelay -= Time.deltaTime;
            if (initialStartDelay <= 0)
            {
                _hasStarted = true;
            }
        }
        #endregion

        #region Waves

        if (_hasStarted)
        {
            #region Spawn Enemies at Start of wave
            if(_enemiesLeft <= 0)
            {
                _waveStart = true;
            }

            /*if(_spawnBool == false)
            {
                _spawnTimer = _waveQueue.Peek().startDelay;
                _spawnBool = true;
            }

            if (_spawnTimer > 0) return;*/

            if(_waveQueue.Count > 0)
            {
                if (_waveStart)
                {
                    Debug.Log("New Wave");

                    Wave currentWave = _waveQueue.Dequeue();

                    if (currentWave.spawnAll)
                    {
                        //spawn all the enemies at once
                        foreach (var type in currentWave.enemyTypes)
                        {
                            if (type.prefab != null)
                                Debug.LogFormat("Spawning Enemy Type {0}", type.prefab);

                            //each type of enemy
                            for (int i = 0; i < type.count; i++)
                            {
                                //Vector3 spawnPoint = spawner.GetRandomSpawnPoint();

                                Vector3 spawnPoint = Vector3.zero;

                                if (i < spawner.GetSpawnPointCount())
                                    spawnPoint = spawner.GetSpecificSpawnPoint(i);
                                else
                                    spawnPoint = spawner.GetSpecificSpawnPoint(i - spawner.GetSpawnPointCount());

                                Instantiate(type.prefab, spawnPoint, Quaternion.identity);
                            }

                            _enemiesLeft += type.count;
                        }

                    }
                    else
                    {
                        //spawn enemies one at a time (in order of the array)
                    }

                    Debug.LogFormat("Enemies Left: {0}", _enemiesLeft);
                    _waveStart = false;
                }
            }
            

            #endregion
        }
        #endregion
    }
    #endregion

    internal void RemoveEnemy()
    {
        Debug.Log("RemoveEnemy()");
        _enemiesLeft--;
    }
    private void SetupQueue()
    {
        //put waves into a queue
        if (waves.Length > 0)
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


}
