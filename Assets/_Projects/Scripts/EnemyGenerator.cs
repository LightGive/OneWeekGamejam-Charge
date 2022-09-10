using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField] EnemyPool[] _enemyPools = null;
        [SerializeField] MainCamera _mainCamera = null;
        [SerializeField] Player _player = null;
        [SerializeField] int _maxGenerateNum = 3;
        [SerializeField] float _oneWaveTime = 0.0f;
        [SerializeField] float _generateInterval = 2.0f;
        [SerializeField] float _generateRange = 10.0f;
        [SerializeField] WaveData[] _waveData = null;

        List<Enemy> _activeEnemyList = null;
        WaveData _currentWave = null;

        float _generateTimeCnt = 0.0f;
        float _waveTimeCnt  = 0.0f;
        int _waveCnt = 0;

        public bool IsStartGenerate { get; private set; } = false;

        void Awake()
        {
            _activeEnemyList = new List<Enemy>();
        }

        void Update()
        {
            if (!IsStartGenerate) { return; }
            CheckGenerate();
            CheckWave();
        }

		void OnDrawGizmos()
		{
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(Vector3.zero, _generateRange);
		}

        public void ClearGenerateEnemy()
		{
            for (var i = _activeEnemyList.Count - 1; i >= 0; i--)
            {
                _activeEnemyList[i].Clear();
            }
		}

        public void ResetGenerate()
		{
            _currentWave = _waveData[0];
            _waveTimeCnt = 0.0f;
            _waveCnt = 0;
		}

        public void StartGenerate()
		{
            IsStartGenerate = true;
		}

        public void StopGenerate()
		{
            IsStartGenerate = false;
		}

        void CheckGenerate()
		{
            _generateTimeCnt += GameSystem.ObjectDeltaTime;
            if (_generateTimeCnt > _generateInterval)
            {
                Generate();
            }
        }

		void Generate()
        {
            if (_activeEnemyList.Count >= _currentWave.MaxEnemyNum) { return; }
            var enemyType = (int)_currentWave.GenerateType;
            var e = _enemyPools[enemyType].Pool.Get();
            var p = _mainCamera.GetRandomPositionScreenOut(10.0f);
            e.Generate(p, _player,_currentWave.Speed);
            _activeEnemyList.Add(e);
            _generateTimeCnt = 0.0f;

            e.OnDeaded.AddListener(() =>
            {
                EnemyReleaseEvent(e, _enemyPools[enemyType]);
                _player.EXP.AddExperiencePoint(e.ExperiencePoint);
            });

            e.OnCleared.AddListener(() => EnemyReleaseEvent(e, _enemyPools[enemyType]));
        }

        void CheckWave()
        {
            _waveTimeCnt += GameSystem.ObjectDeltaTime;
            if (_waveTimeCnt <= _oneWaveTime) { return; }
            _waveCnt++;
            var idx = Mathf.Clamp(_waveCnt, 0, _waveData.Length - 1);
            _currentWave = _waveData[idx];
        }

        void EnemyReleaseEvent(Enemy e, EnemyPool pool)
        {
            pool.ReleaseEnemy(e);
            _activeEnemyList.Remove(e);
        }
    }
}