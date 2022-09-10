using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace OneWeekGamejam.Charge
{
	public class BulletPool : MonoBehaviour
	{
        [SerializeField] Bullet _bulletPrefab = null;
        public ObjectPool<Bullet> Pool { get; private set; } = null;
        void Awake()
        {
            Pool = new ObjectPool<Bullet>(
                OnCreatePooledObject,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPooledObject);
        }

        Bullet OnCreatePooledObject()
        {
            return Instantiate(_bulletPrefab);
        }
        void OnGetFromPool(Bullet obj)
        {
            obj.gameObject.SetActive(true);
        }
        void OnReleaseToPool(Bullet obj)
        {
            obj.gameObject.SetActive(false);
        }
        void OnDestroyPooledObject(Bullet obj)
        {
            Destroy(obj);
        }
        public void ReleaseBullet(Bullet obj)
        {
            Pool.Release(obj);
            obj.OnHitEvent.RemoveAllListeners();
            obj.OnHitDestroy.RemoveAllListeners();
            obj.OnScreenOutEvent.RemoveAllListeners();
        }
    }
}