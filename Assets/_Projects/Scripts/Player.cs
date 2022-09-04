using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneWeekGamejam.Charge
{
	public class Player : MonoBehaviour
	{
		[SerializeField, Range(0.01f, 10.0f)] float _maxTime = 3.0f;
		[SerializeField] float _moveSpeed = 1.0f;

		float _charge = 0.0f;
		float _chargeMax = 1.0f;
		Vector3 _vec = Vector3.zero;

		private void Update()
		{
			transform.position += _vec * _moveSpeed * Time.deltaTime;
		}


		public void OnMove(InputAction.CallbackContext context)
		{
			_vec = context.ReadValue<Vector2>();
			
		}
	}
}