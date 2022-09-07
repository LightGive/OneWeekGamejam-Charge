using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float _angleSmoothTime = 1.0f;
        [SerializeField] float _angleMaxSpeed = 360.0f;
        [SerializeField] float _baseSpeed = 1.0f;
        [SerializeField] float _exp = 1.0f;
        Transform _target = null;
        float _angleCurrent = 0.0f;
        float _angleVelocity = 0.0f;

        public float ExperiencePoint { get; private set; } = 0.0f;
        public UnityEvent OnHitEvent { get; private set; } = new UnityEvent();

        public void SetActivate(Vector3 generatePos, Player player)
        {
            ExperiencePoint = 10.0f;
            _target = player.transform;
            _angleCurrent = GetTargetAngle();
            transform.position = generatePos;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _angleCurrent);
        }

		void Update()
		{
            Move();
		}

        protected virtual void Move()
        {
            var a = GetTargetAngle();
            _angleCurrent = Mathf.SmoothDampAngle(_angleCurrent, a, ref _angleVelocity, _angleSmoothTime, _angleMaxSpeed);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, _angleCurrent);
            transform.position += transform.up * _baseSpeed * GameSystem.ObjectDeltaTime;
        }

        float GetTargetAngle()
        {
            var v = (_target.position - transform.position).normalized;
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + 270.0f;
        }

		void OnTriggerEnter2D(Collider2D col)
		{
            if (col.tag != TagName.PlayerBullet) { return; }
            var bullet = col.transform.GetComponent<Bullet>();
            if (bullet == null) 
            {
                Debug.LogError("Bulletがアタッチされて無い");
                return; 
            }
            var bulletVec = bullet.transform.up;
			if (bullet.gameObject.activeSelf)
			{
                bullet.Hit();
            }
            Damage(bulletVec);
		}

		public void Damage(Vector3 vec)
		{
            var shakePow = 0.2f;

            GameSystem.Instance.ShakeCamera(vec * shakePow, 0.1f);
            GameSystem.Instance.HitStop(0.1f,()=> 
            {
                OnHitEvent?.Invoke();
            });
		}
	}
}