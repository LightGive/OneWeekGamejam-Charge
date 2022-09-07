using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigid = null;
        [SerializeField] SpriteRenderer _spRenderer = null;
        float _speed = 0.0f;
        public UnityEvent OnHitEvent { get; private set; } = new UnityEvent();

        void Update()
		{
            _rigid.MovePosition((Vector3)_rigid.position + transform.up * _speed * GameSystem.ObjectDeltaTime);
        }

        public void SetActivate(int level, float speed, float angle, Vector3 pos)
        {
            _speed = speed;
            var scale = level * 0.1f;
            _spRenderer.transform.localScale = new Vector3(scale, scale, scale);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
            transform.position = pos;
        }

        public void Hit()
		{
            OnHitEvent?.Invoke();   
		}
    }
}