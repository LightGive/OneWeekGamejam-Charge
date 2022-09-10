using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class EnemyGenerator : MonoBehaviour
    {
        public enum EnemyType
        {
            Ufo = 0,
        }
        [System.Serializable]
        public class WaveData
		{
            public EnemyType EnemyType;

		}

        [SerializeField] EnemyPool[] _enemyPools = null;
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

        public bool IsStartGenerate { get; private set; } = false;

        void Awake()
        {
            _activeEnemyList = new List<Enemy>();
        }

        void Update()
        {
            CheckGenerate();
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
			if (!IsStartGenerate) { return; }
            _generateTimeCnt += GameSystem.ObjectDeltaTime;
            if (_generateTimeCnt > _generateInterval)
            {
                Generate();
            }
        }

		void Generate()
        {
            if (_activeEnemyList.Count >= _maxGenerateNum) { return; }
            var ran = Random.value;
            var r = Mathf.PI * 2.0f * ran;
            var v = new Vector3(Mathf.Sin(r), Mathf.Cos(r), 0.0f);
            var p = _player.transform.position + (v * _generateRange);
            var enemyType = (int)_currentWave.EnemyType;
            var e = _enemyPools[enemyType].Pool.Get();
            e.Generate(p, _player);
            _activeEnemyList.Add(e);
            _generateTimeCnt = 0.0f;

            e.OnDeaded.AddListener(() =>
            {
                EnemyReleaseEvent(e, _enemyPools[enemyType]);
                _player.EXP.AddExperiencePoint(e.ExperiencePoint);
            });

            e.OnCleared.AddListener(() => EnemyReleaseEvent(e, _enemyPools[enemyType]));
        }

        void EnemyReleaseEvent(Enemy e, EnemyPool pool)
        {
            pool.ReleaseEnemy(e);
            _activeEnemyList.Remove(e);
        }
    }
}