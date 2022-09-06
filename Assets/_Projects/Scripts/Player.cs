using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace OneWeekGamejam.Charge
{
	public class Player : MonoBehaviour
	{
		public class HitPoint
		{
			public class OnHitpointChangedEvent : UnityEvent<int, int> { }
			public OnHitpointChangedEvent OnHitpointChanged { get; private set; } = null;
			public int Max { get; private set; }
			public int Current { get; private set; }

			public HitPoint(int current, int max)
			{
				OnHitpointChanged = new OnHitpointChangedEvent();
				Current = current;
				Max = max;
			}

			public void SetMax(int value, bool isCureCurrent)
			{
				if (value < 0)
				{
					Debug.LogError("HPÇÃç≈ëÂílÇÕ0à»è„");
					return;
				}

				var off = value - Max;
				if (!isCureCurrent)
				{
					off = Mathf.Clamp(off, int.MinValue, 0);
				}
				Max = Mathf.Clamp(value, 0, int.MaxValue);
				SetCurrent(Current + off);
			}

			public void SetCurrent(int value)
			{
				if (value > Max)
				{
					Debug.Log("ç≈ëÂílÇè„âÒÇ¡ÇƒâÒïúèoóàÇ»Ç¢");
				}
				Current = Mathf.Clamp(value, 0, Max);
				OnHitpointChanged?.Invoke(Max, Current);
			}
		}

		const string AnimParamDamage = "Damage";
		const string AnimParamNormal = "Normal";
		const int MaxHitpoint = 4;
		const int MaxChageLevel = 4;
		const int StartHitPoint = 3;
		const int StartChageLevel = 2;

		[SerializeField] BulletGenerator _bulletGenerator = null;
		[SerializeField] Animation _anim = null;
		[SerializeField, Range(0.01f, 10.0f)] float _chargeTimeOnLevel = 1.0f;
		[SerializeField] float _moveSpeed = 1.0f;
		[SerializeField] float _stickAngleThresholdStart = 0.5f;
		[SerializeField] float _stickAngleThresholdEnd = 0.2f;
		[SerializeField] float _invisibleTime = 4.0f;
		public HitPoint HP { get; private set; } = new HitPoint(StartHitPoint, StartHitPoint);

		int _chargeLevelMax = StartChageLevel;
		bool _isInvisible = false;
		bool _isCharge = false;
		float _invisibleTimeCnt = 0.0f;
		float _chargeTimeCnt = 0.0f;
		Vector3 _vec = Vector3.zero;
		Vector2 _screenCenter = Vector2.zero;

		private void Awake()
		{
			_screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
		}
		void Start()
		{
			HP.SetMax(StartHitPoint, false);
		}

		private void Update()
		{
			transform.position += _vec * _moveSpeed * Time.deltaTime;
			if (_isCharge)
			{
				_chargeTimeCnt = Mathf.Clamp(_chargeTimeCnt + Time.deltaTime, 0.0f, _chargeLevelMax * _chargeTimeOnLevel);
			}
			CheckInvisible();
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.tag != TagName.Enemy) { return; }
			Damage();
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

		/// <summary>
		/// ìGÇ‚ìGÇÃçUåÇÇ™ìñÇΩÇ¡ÇΩ
		/// </summary>
		void Damage()
		{
			if (_isInvisible) { return; }
			HP.SetCurrent(HP.Current - 1);
			_anim.Play(AnimParamDamage);
			_isInvisible = true;
		}

		/// <summary>
		/// ñ≥ìGâèúÉ`ÉFÉbÉN
		/// </summary>
		void CheckInvisible()
		{
			if (!_isInvisible) { return; }
			_invisibleTimeCnt += Time.deltaTime;
			if (_invisibleTimeCnt > _invisibleTime)
			{
				_invisibleTimeCnt = 0.0f;
				_isInvisible = false;
				_anim.Play(AnimParamNormal);
			}
		}

		void Look(Vector2 vec)
		{
			var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90.0f;
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}

		void StartCharge()
		{
			_isCharge = true;
		}

		void ReleaseCharge()
		{
			_isCharge = false;
			var level = Mathf.FloorToInt(_chargeTimeCnt / _chargeTimeOnLevel);
			_chargeTimeCnt = 0.0f;
			if (level == 0) { return; }

			_bulletGenerator.GeneratePlayerBullet(level, 50.0f, transform.up, transform.position);
		}
	}
}