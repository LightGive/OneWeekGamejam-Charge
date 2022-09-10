using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace OneWeekGamejam.Charge
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] Enemy _enemyPrefab = null;

        public ObjectPool<Enemy> Pool { get; private set; } = null;

		void Awake()
		{
            Pool = new ObjectPool<Enemy>(
                OnCreatePooledObject,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject);
        }

        Enemy OnCreatePooledObject()
        {
            return Instantiate(_enemyPrefab);
        }
        void OnGetFromPool(Enemy obj)
        {
            obj.gameObject.SetActive(true);
        }
        void OnReleaseToPool(Enemy obj)
        {
            obj.gameObject.SetActive(false);
        }
        void OnDestroyPooledObject(Enemy obj)
        {
            Destroy(obj);
        }
        public void ReleaseEnemy(Enemy obj)
        {
            Pool.Release(obj);
            obj.OnHitEvent.RemoveAllListeners();
            obj.OnClearEvent.RemoveAllListeners();
        }
    }
}