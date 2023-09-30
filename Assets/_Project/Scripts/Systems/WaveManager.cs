using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WaveManager : MonoBehaviour
{
    //references
    private Spawner spawner;

    //properties
    [Header("Wave Properties")]
    //public float initialStartDelay = 5.0f;
    [SerializeField]
    private TextMeshProUGUI _waveCounter;
    [SerializeField]
    private TextMeshProUGUI _countdownTimer;
    [SerializeField]
    private TextMeshProUGUI _enemyCounter;

    //waves
    [Space]
    public Wave[] waves;
    private Queue<Wave> _waveQueue;
    //public int currentWaveNum;
    public int currentWaveNum { get; private set; }
    private Wave _currentWave;

    public UnityEvent OnWinCondition;

    //private
    private bool _updateTimer = false;
    private float _spawnCountdown;
    private int _enemiesLeft = 0;

    private bool _canSpawn = false;
    //private bool _hasStarted = false;

    //[Header("Debug")]
    //[SerializeField]

    //private bool _waveStart = false;
    //private bool _spawnBool = true;
    //[SerializeField]
    //private float _spawnTimer;

    #region Structs
    [Serializable]
    public struct Wave
    {
        /*[Tooltip("Spawn all the enemies in the wave at once.")]
        public bool spawnAll;*/
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

        SetupQueue();
    }
    private void Start()
    {
        currentWaveNum = 0;

        if (_waveCounter != null) _waveCounter.text = "";
        if (_enemyCounter != null) _enemyCounter.text = "";

        StartCoroutine(DelayWaveStart());
    }
    private void Update()
    {
        #region Old
        /*if (_canSpawn == false) return;

        #region Timer
        *//*if(_spawnBool == true)
        {
            _spawnTimer -= Time.deltaTime;
        }*//*
        #endregion

        #region Before Waves
        if (_hasStarted == false)
        {
            initialStartDelay -= Time.deltaTime;
            if (initialStartDelay <= 0)
            {
                _hasStarted = true;
                if (_countdownTimer != null) _countdownTimer.gameObject.SetActive(false);
            }
            else
            {
                if (_countdownTimer != null) _countdownTimer.text = Utility.DisplayTimeSeconds(initialStartDelay);
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

            *//*if(_spawnBool == false)
            {
                _spawnTimer = _waveQueue.Peek().startDelay;
                _spawnBool = true;
            }

            if (_spawnTimer > 0) return;*//*

            if(_waveQueue.Count > 0)
            {
                if (_waveStart)
                {
                    Debug.Log("New Wave");

                    Wave currentWave = _waveQueue.Dequeue();
                    
                    //update wave display
                    currentWaveNum++;
                    if (_waveCounter != null) _waveCounter.text = currentWaveNum.ToString();

                    // loop through wave properties
                    if (currentWave.spawnAll)
                    {
                        int spawnPointCounter = 0;
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

                                if (spawnPointCounter < spawner.GetSpawnPointCount())
                                    spawnPoint = spawner.GetSpecificSpawnPoint(spawnPointCounter);
                                else
                                    spawnPoint = spawner.GetSpecificSpawnPoint(spawnPointCounter - spawner.GetSpawnPointCount());

                                Instantiate(type.prefab, spawnPoint, Quaternion.identity);

                                spawnPointCounter++;
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
        #endregion*/
        #endregion

        #region New Coroutine Method
        //new coroutine method
        if (_updateTimer)
        {
            _spawnCountdown -= Time.deltaTime;

            if (_countdownTimer != null) _countdownTimer.gameObject.SetActive(true);

            if (_spawnCountdown <= 0)
            {
                if (_countdownTimer != null) _countdownTimer.gameObject.SetActive(false);
            }
            else
            {
                if (_countdownTimer != null) _countdownTimer.text = Utility.DisplayTimeSeconds(_spawnCountdown);
            }
        }
        #endregion
    }
    #endregion

    internal void RemoveEnemy()
    {
        Debug.Log("RemoveEnemy()");
        _enemiesLeft--;

        if (_enemyCounter != null) _enemyCounter.text = "Enemies: " + _enemiesLeft.ToString();

        if (_enemiesLeft <= 0)
        {
            if (_waveQueue.Count > 0)
                StartCoroutine(DelayWaveStart());
            else
                WinCondition();
        }
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
            currentWaveNum = -1;
        }
        else
        {
            Debug.LogError("Requires at least 1 enemy wave.");
        }
    }
    private void DoWaveStuff()
    {
        //Debug.Log("DoWaveStuff()");

        //update wave display
        currentWaveNum++;
        if (_waveCounter != null) _waveCounter.text = "Wave: " + currentWaveNum.ToString();

        // loop through wave properties

        int spawnPointCounter = 0;
        //spawn all the enemies at once
        foreach (var type in _currentWave.enemyTypes)
        {
            if (type.prefab != null)
                Debug.LogFormat("Spawning Enemy Type {0}", type.prefab);

            //each type of enemy
            for (int i = 0; i < type.count; i++)
            {
                //Vector3 spawnPoint = spawner.GetRandomSpawnPoint();

                Vector3 spawnPoint = Vector3.zero;

                if (spawnPointCounter < spawner.GetSpawnPointCount())
                    spawnPoint = spawner.GetSpecificSpawnPoint(spawnPointCounter);
                else
                    spawnPoint = spawner.GetSpecificSpawnPoint(spawnPointCounter - spawner.GetSpawnPointCount());

                Instantiate(type.prefab, spawnPoint, Quaternion.identity);

                spawnPointCounter++;
            }

            _enemiesLeft += type.count;

            if (_enemyCounter != null) _enemyCounter.text = "Enemies: " + _enemiesLeft.ToString();

        }

        //Debug.LogFormat("Enemies Left: {0}", _enemiesLeft);
    }
    private void WinCondition()
    {
        OnWinCondition?.Invoke();
    }

    IEnumerator DelayWaveStart()
    {
        _currentWave = _waveQueue.Dequeue();

        _updateTimer = true;

        _spawnCountdown = _currentWave.startDelay;

        yield return new WaitForSeconds(_currentWave.startDelay);

        _updateTimer = false;

        DoWaveStuff();
    }
}
