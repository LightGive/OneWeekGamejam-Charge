using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge 
{
    public class BulletGenerator : MonoBehaviour
    {
        [SerializeField] BulletPool[] _bulletPools = null;
        [SerializeField] ScoreController _scoreController = null;
        [SerializeField] MainCamera _mainCamera = null;
        List<Bullet> _activeBulletList = null;

        void Awake()
        {
            _activeBulletList = new List<Bullet>();
        }

        void Update()
        {
            for (var i = _activeBulletList.Count - 1; i >= 0; i--)
            {
                var target = _activeBulletList[i];
                if (_mainCamera.IsScreenOut(target.transform.position, 100.0f))
                {
                    target.Clear();
                }
            }
        }

		public void GeneratePlayerBullet(int level, float speed, Vector3 dir, Vector3 pos)
        {
            dir = dir.normalized;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270.0f;
            var bullet = _bulletPools[level].Pool.Get();
            bullet.Generate(speed, angle, pos);
            PlayShotSound(level);

            _activeBulletList.Add(bullet);
            bullet.OnHitDestroy.AddListener(() =>
            {
                EnemyReleaseEvent(bullet, _bulletPools[level]);
            });
            bullet.OnCleared.AddListener(() => 
            {
                EnemyReleaseEvent(bullet, _bulletPools[level]);
            });
            bullet.OnHited.AddListener((hitCount) =>
            {
                _scoreController.AddScore(hitCount, bullet.transform.position);
            });
        }

        public void ClearGenerateBullet()
        {
            for (var i = _activeBulletList.Count - 1; i >= 0; i--)
            {
                _activeBulletList[i].Clear();
            }
        }

        void EnemyReleaseEvent(Bullet b, BulletPool pool)
        {
            pool.ReleaseBullet(b);
            _activeBulletList.Remove(b);
        }

        void PlayShotSound(int level)
        {
            switch (level)
            {
                case 0: SEManager.Instance.Play(SEPath.PLAYER_SHOT1); break;
                case 1: SEManager.Instance.Play(SEPath.PLAYER_SHOT2); break;
                case 2: SEManager.Instance.Play(SEPath.PLAYER_SHOT3); break;
                case 3: SEManager.Instance.Play(SEPath.PLAYER_SHOT4); break;
            }
        }
    }
}