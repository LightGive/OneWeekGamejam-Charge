using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge 
{
    public class BulletGenerator : MonoBehaviour
    {
        [SerializeField] BulletPool[] _bulletPools = null;
        List<Bullet> _activeBulletList = null;

		void Awake()
		{
            _activeBulletList = new List<Bullet>();
		}

		public void GeneratePlayerBullet(int level, float speed, Vector3 dir, Vector3 pos)
        {
            dir = dir.normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270.0f;
            var bullet = _bulletPools[level].Pool.Get();
            bullet.Generate(speed, angle, pos);

            _activeBulletList.Add(bullet);
            bullet.OnScreenOutEvent.AddListener(() => 
            {
                EnemyReleaseEvent(bullet, _bulletPools[level]);
            });

            bullet.OnHitDestroy.AddListener(() =>
            {
                EnemyReleaseEvent(bullet, _bulletPools[level]);
            });
        }

        void EnemyReleaseEvent(Bullet b, BulletPool pool)
        {
            pool.ReleaseBullet(b);
            _activeBulletList.Remove(b);
        }
    }
}