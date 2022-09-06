using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField] EnemyPool _enemyPool = null;
        [SerializeField] Player _player = null;
        [SerializeField] int _maxGenerateNum = 3;
        [SerializeField] float _generateInterval = 2.0f;
        [SerializeField] float _generateRange = 10.0f;
        List<Enemy> _activeEnemyList = null;

        float _generateTimeCnt = 0.0f;

        void Awake()
        {
            _activeEnemyList = new List<Enemy>();
        }

		void Update()
        {
            _generateTimeCnt += Time.deltaTime;
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
            var e = _enemyPool.Pool.Get();
            e.SetActivate(p, _player);
            _activeEnemyList.Add(e);
            _generateTimeCnt = 0.0f;
            e.OnHitEvent.AddListener(() =>
            {
                _enemyPool.ReleaseEnemy(e);
                _activeEnemyList.Remove(e);
                e.OnHitEvent.RemoveAllListeners();
            });
        }
    }
}