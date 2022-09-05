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
		[SerializeField] float _stickAngleThresholdStart = 0.5f;
		[SerializeField] float _stickAngleThresholdEnd = 0.2f;
		bool _isCharge = false;
		float _charge = 0.0f;
		float _chargeMax = 1.0f;
		Vector3 _vec = Vector3.zero;
		Vector2 _screenCenter = Vector2.zero;

		private void Awake()
		{
			_screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
		}
		private void Update()
		{
			transform.position += _vec * _moveSpeed * Time.deltaTime;


			if (_isCharge)
			{
				_charge += Time.deltaTime;
			}
		}


		public void OnMove(InputAction.CallbackContext context)
		{
			_vec = context.ReadValue<Vector2>();
			
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			var stickVec = context.ReadValue<Vector2>();
			var stickMag = stickVec.magnitude;
			if (_isCharge && stickMag <= _stickAngleThresholdEnd)
			{
				ReleaseCharge();
			}
			else if(!_isCharge && stickVec.magnitude >= _stickAngleThresholdStart)
			{
				StartCharge();
			}

			if (_isCharge)
			{
				Look(stickVec);
			}
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			if(!_isCharge) { return; }
			var screenPoint = context.ReadValue<Vector2>();
			var vec = screenPoint - _screenCenter;
			Look(vec);
		}

		public void OnMouse(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				if (context.action.IsPressed())
				{
					StartCharge();
				}
				else
				{
					ReleaseCharge();
				}
			}
		}
		void Look(Vector2 vec)
		{
			var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg + 90.0f;
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}

		void StartCharge()
		{
			_isCharge = true;
		}

		void ReleaseCharge()
		{
			_isCharge = false;
		}
	}
}