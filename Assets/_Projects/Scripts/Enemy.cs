using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float _angleSmoothTime = 1.0f;
        [SerializeField] float _angleMaxSpeed = 360.0f;
        [SerializeField] float _baseSpeed = 1.0f;
        Transform _target = null;
        float _angleCurrent = 0.0f;
        float _angleVelocity = 0.0f;
        public void SetActivate(Vector3 generatePos, Player player)
        {
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
            transform.position += transform.up * _baseSpeed * Time.deltaTime;
        }

        float GetTargetAngle()
        {
            var v = (_target.position - transform.position).normalized;
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + 270.0f;
        }
	}
}