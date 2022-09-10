using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KanKikuchi.AudioManager;

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
        [SerializeField] int _hp = 1;
        [field: SerializeField] public float ExperiencePoint { get; private set; } = 1.0f;

        Transform _target = null;
        float _angleCurrent = 0.0f;
        float _angleVelocity = 0.0f;
        int _hpCnt = 1;
        
        Vector2 _moveVec = Vector2.zero;

        public UnityEvent OnHited { get; private set; } = new UnityEvent();
        public UnityEvent OnDeaded { get; private set; } = new UnityEvent();
        public UnityEvent OnCleared { get; private set; } = new UnityEvent();


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
            _moveVec = new Vector2(Mathf.Cos(_angleCurrent), Mathf.Sin(_angleCurrent)).normalized;
            _rigid.MovePosition((Vector3)_rigid.position + (Vector3)_moveVec * _baseSpeed * GameSystem.ObjectDeltaTime);

        }

        public void Generate(Vector3 generatePos, Player player)
        {
            _hpCnt = _hp;
            _target = player.transform;
            _angleCurrent = GetTargetAngleRad();
            transform.position = generatePos;
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            _spriteFlusher.ResetFlush();
        }

        public void Damage(Vector3 vec)
        {
            var shakePow = 0.2f;
            _spriteFlusher.StartFlush(2);
            SEManager.Instance.Play(SEPath.ENEMY_HIT);
            GameSystem.Instance.ShakeCamera(vec * shakePow, 0.1f, 50.0f);
            GameSystem.Instance.HitStop(0.1f, () =>
             {
                 OnHited?.Invoke();
             });

            _hpCnt--;
            if (_hpCnt <= 0)
            {
                Dead();
            }
        }

        public void Clear()
		{
            OnCleared?.Invoke();
		}

        void Dead()
        {
            OnDeaded?.Invoke();
        }
        float GetTargetAngleRad()
        {
            var v = (_target.position - transform.position).normalized;
            return Mathf.Atan2(v.y, v.x);
        }

    }
}