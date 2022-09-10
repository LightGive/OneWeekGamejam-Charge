using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigid = null;
        [SerializeField] SpriteRenderer _spRenderer = null;
        [SerializeField] bool isPenetration = false;

        float _speed = 0.0f;
        public UnityEvent OnHitEvent { get; private set; } = new UnityEvent();
        public UnityEvent OnHitDestroy { get; private set; } = new UnityEvent();
        public UnityEvent OnCleared { get; private set; } = new UnityEvent();


        void Update()
		{
            _rigid.MovePosition((Vector3)_rigid.position + transform.up * _speed * GameSystem.ObjectDeltaTime);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag != TagName.Enemy) { return; }
            if (!col.gameObject.TryGetComponent(out EnemyCollider result))
            {
                Debug.LogError("EnemyのタグでEnemyColliderがアタッチされていない");
            }
            result.Hit(transform.up);
            Hit();
            
        }

        public void Generate(float speed, float angle, Vector3 pos)
        {
            _speed = speed;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            transform.position = pos;
        }

        public void Hit()
		{
            OnHitEvent?.Invoke();
            if (!isPenetration)
			{
                OnHitDestroy?.Invoke();
			}
		}
        public void Clear()
        {
            OnCleared?.Invoke();
        }
    }
}