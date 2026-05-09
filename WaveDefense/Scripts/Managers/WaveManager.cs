using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveDefense.Core;

namespace WaveDefense.Managers
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> enemyPool;
        [SerializeField] private float initialSpawnInterval = 2f;
        [SerializeField] private float difficultyMultiplier = 0.98f;
        [SerializeField] private float minSpawnInterval = 0.5f;

        private float _currentSpawnInterval;
        private bool _isRunning;

        private void Start()
        {
            _currentSpawnInterval = initialSpawnInterval;
            GameEvents.OnGameOver += StopSpawning;
            StartSpawning();
        }

        private void OnDestroy()
        {
            GameEvents.OnGameOver -= StopSpawning;
        }

        public void StartSpawning()
        {
            _isRunning = true;
            StartCoroutine(SpawnRoutine());
        }

        public void StopSpawning()
        {
            _isRunning = false;
        }

        private IEnumerator SpawnRoutine()
        {
            while (_isRunning)
            {
                yield return new WaitForSeconds(_currentSpawnInterval);
                if (_isRunning)
                {
                    SpawnEnemy();
                    _currentSpawnInterval = Mathf.Max(minSpawnInterval, _currentSpawnInterval * difficultyMultiplier);
                }
            }
        }

        private void SpawnEnemy()
        {
            if (enemyPool == null || enemyPool.Count == 0) return;

            EnemyData data = enemyPool[Random.Range(0, enemyPool.Count)];
            if (data == null || data.prefab == null) return;

            float side = Random.value > 0.5f ? 12f : -12f;
            Vector3 spawnPos = new Vector3(side, 0, 0);
            
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.Get(data.prefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(data.prefab, spawnPos, Quaternion.identity);
            }
        }
}
}
