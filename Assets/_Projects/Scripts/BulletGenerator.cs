using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge 
{
    public class BulletGenerator : MonoBehaviour
    {
        [SerializeField] BulletPool _pool = null;

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
            _pool.Pool.Get().SetActivate(level, speed, angle, pos);
        }
    }
}