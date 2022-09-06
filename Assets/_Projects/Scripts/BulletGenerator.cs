using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge 
{
    public class BulletGenerator : MonoBehaviour
    {
        [SerializeField] BulletPool _pool = null;
        List<Bullet> _activeBulletList = null;

        void Start()
        {

        }

        void Update()
        {
        }

        public void GeneratePlayerBullet(int level, float speed, Vector3 dir, Vector3 pos)
        {
            dir = dir.normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270.0f;
            var bullet = _pool.Pool.Get();
            bullet.SetActivate(level, speed, angle, pos);
            bullet.OnHitEvent.AddListener(() => 
            {
                _pool.ReleaseGameObject(bullet);
                bullet.OnHitEvent.RemoveAllListeners();
            });
        }
    }
}