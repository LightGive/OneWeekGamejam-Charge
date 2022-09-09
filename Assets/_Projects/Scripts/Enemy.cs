using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigid;
        [SerializeField] SpriteFlusher _spriteFlusher = null;
        [SerializeField] float _angleSmoothTime = 1.0f;
        [SerializeField] float _angleMaxSpeed = 360.0f;
        [SerializeField] float _baseSpeed = 1.0f;
        [SerializeField] float _exp = 1.0f;
        

        Transform _target = null;
        float _angleCurrent = 0.0f;
        float _angleVelocity = 0.0f;
        
        Vector2 _moveVec = Vector2.zero;

        public float ExperiencePoint { get; private set; } = 0.0f;
        public UnityEvent OnHitEvent { get; private set; } = new UnityEvent();

        public void SetActivate(Vector3 generatePos, Player player)
        {
            ExperiencePoint = 10.0f;
            _target = player.transform;
            _angleCurrent = GetTargetAngleRad();
            transform.position = generatePos;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

		void Update()
		{
            Move();
		}

		private void OnDrawGizmos()
		{
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, transform.position + ((Vector3)_moveVec * 10.0f));
		}

		protected virtual void Move()
        {
            var a = GetTargetAngleRad();
            var off = a - _angleCurrent;
            if (Mathf.Abs(off) > Mathf.PI)
            {
                _angleCurrent += Mathf.PI * (off > 0 ? 2 : -2);
            }
            _angleCurrent = Mathf.SmoothDamp(_angleCurrent, a, ref _angleVelocity, _angleSmoothTime, _angleMaxSpeed);
            _moveVec = new Vector2(Mathf.Cos(_angleCurrent), Mathf.Sin(_angleCurrent));
            _rigid.MovePosition((Vector3)_rigid.position + (Vector3)_moveVec * _baseSpeed * GameSystem.ObjectDeltaTime);

        }

        float GetTargetAngleRad()
        {
            var v = (_target.position - transform.position).normalized;
            return Mathf.Atan2(v.y, v.x);
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
            _spriteFlusher.StartFlush(2);
            GameSystem.Instance.ShakeCamera(vec * shakePow, 0.1f, 50.0f);
            GameSystem.Instance.HitStop(0.1f,()=> 
            {
                OnHitEvent?.Invoke();
            });
		}


	}
}