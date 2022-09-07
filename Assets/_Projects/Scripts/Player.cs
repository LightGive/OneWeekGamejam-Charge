using UnityEngine;
using UnityEngine.Events;

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
					Debug.LogError("HPの最大値は0以上");
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
					Debug.Log("最大値を上回って回復出来ない");
				}
				Current = Mathf.Clamp(value, 0, Max);
				OnHitpointChanged?.Invoke(Max, Current);
			}
		}
		[System.Serializable]
		public class ChargeInfo
		{
			public class OnChargeChangeEvent : UnityEvent<bool> { }
			public class OnChargeLevelChangeEvent : UnityEvent<int> { }
			public class OnChargeLevelMaxChangeEvent : UnityEvent<int> { }
			int _chargeLevel = 0;
			int _chargeLevelMax = 0;
			[field: SerializeField] public float ChargeTimeOneLevel { get; private set; } = 0.5f;
			public float ChargeTimeCnt { get; private set; } = 0.0f;
			public int ChargeLevelMax 
			{ 
				get => _chargeLevelMax;
				private set 
				{
					if(_chargeLevelMax == value) { return; }
					_chargeLevelMax = value;
					OnChargeLevelMaxChanged?.Invoke(value);
				} 
			}
			public int ChargeLevel 
			{
				get => _chargeLevel; 
				private set
				{
					if(_chargeLevel == value) { return; }
					_chargeLevel = value;
					OnChargeLevelChanged?.Invoke(value);
				}
			}
			public OnChargeChangeEvent OnChargeChanged { get; private set; } = null;
			public OnChargeLevelChangeEvent OnChargeLevelChanged { get; private set; } = null;
			public OnChargeLevelMaxChangeEvent OnChargeLevelMaxChanged { get; private set; } = null;
			public ChargeInfo()
			{
				ChargeTimeCnt = 0.0f;
				ChargeLevel = 0;
				ChargeLevelMax = 0;
				OnChargeChanged = new OnChargeChangeEvent();
				OnChargeLevelChanged = new OnChargeLevelChangeEvent();
				OnChargeLevelMaxChanged = new OnChargeLevelMaxChangeEvent();
			}

			public void SetChargeMaxLevel(int chargeLevelMax)
			{
				ChargeLevelMax = chargeLevelMax;
			}

			public void ChargeTimeCount()
			{
				ChargeTimeCnt = Mathf.Clamp(ChargeTimeCnt + GameSystem.ObjectDeltaTime, 0.0f, ChargeLevelMax * ChargeTimeOneLevel);
				ChargeLevel = Mathf.FloorToInt(ChargeTimeCnt / ChargeTimeOneLevel);
			}

			public void ResetChargeTime()
			{
				ChargeTimeCnt = 0.0f;
			}
		}

		const string AnimParamDamage = "Damage";
		const string AnimParamNormal = "Normal";
		const int MaxHitpoint = 4;
		const int MaxChageMaxLevel = 4;
		const int StartHitPoint = 3;
		const int StartChageMaxLevel = 2;

		[SerializeField] BulletGenerator _bulletGenerator = null;
		[SerializeField] PlayerInput _playerInput = null;
		[SerializeField] Animation _anim = null;
		[SerializeField] float _moveSpeed = 1.0f;
		[SerializeField] float _smoothTimeAngle = 360.0f;
		[SerializeField] float _smoothMaxSpeedAngle = 1.0f;
		[SerializeField] float _stickAngleThresholdStart = 0.5f;
		[SerializeField] float _stickAngleThresholdEnd = 0.2f;
		[SerializeField] float _invisibleTime = 4.0f;

		bool _isInvisible = false;
		bool _isCharge = false;
		float _exp = 0.0f;
		float _invisibleTimeCnt = 0.0f;
		(float target, float current, float velocity) _smoothAngle = (0.0f, 0.0f, 0.0f);

		[field: SerializeField] public ChargeInfo Charge { get; private set; } = new ChargeInfo();
		public HitPoint HP { get; private set; } = new HitPoint(StartHitPoint, StartHitPoint);

		void Awake()
		{
			_playerInput.OnAimStart.AddListener(OnAimStart);
			_playerInput.OnFire.AddListener(OnFire);
		}

		void Start()
		{
			HP.SetMax(StartHitPoint, false);
			Charge.SetChargeMaxLevel(StartChageMaxLevel);
		}

		void Update()
		{
			if (_isCharge)
			{
				Charge.ChargeTimeCount();
			}
			Move();
			Aim();
			CheckInvisible();
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.tag != TagName.Enemy) { return; }
			Damage();
		}

		void Aim()
		{
			// deltaTimeに0を設定してSmoothDampAngleを実行すると以下のアサートを吐くため
			// ポーズしている時は角度を変える処理を通さないように
			// Assertion failed on expression: 'CompareApproximately(SqrMagnitude(result), 1.0F)'
			if (GameSystem.ObjectTimeScale <= 0.0f) { return; }

			// 自キャラの向きを変える
			var vec = _playerInput.VecDir;
			_smoothAngle.target = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90.0f;
			_smoothAngle.current =
				Mathf.SmoothDampAngle(
					_smoothAngle.current,
					_smoothAngle.target,
					ref _smoothAngle.velocity,
					_smoothTimeAngle,
					_smoothMaxSpeedAngle);
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _smoothAngle.current);
		}

		void Move()
		{
			transform.position += 
				_playerInput.VecMove * 
				_moveSpeed * 
				GameSystem.ObjectDeltaTime;

		}

		void OnAimStart()
		{
			_isCharge = true;
			Charge.OnChargeChanged.Invoke(true);
		}

		void OnFire()
		{
			_isCharge = false;
			var level = Charge.ChargeLevel;
			Charge.OnChargeChanged.Invoke(false);
			Charge.ResetChargeTime();
			if (level == 0) { return; }
			_bulletGenerator.GeneratePlayerBullet(level, 30.0f, transform.up, transform.position);
		}

		/// <summary>
		/// 敵や敵の攻撃が当たった
		/// </summary>
		void Damage()
		{
			if (_isInvisible) { return; }
			HP.SetCurrent(HP.Current - 1);
			_anim.Play(AnimParamDamage);
			_isInvisible = true;
		}

		/// <summary>
		/// 無敵解除チェック
		/// </summary>
		void CheckInvisible()
		{
			if (!_isInvisible) { return; }
			_invisibleTimeCnt += GameSystem.ObjectDeltaTime;
			if (_invisibleTimeCnt > _invisibleTime)
			{
				_invisibleTimeCnt = 0.0f;
				_isInvisible = false;
				_anim.Play(AnimParamNormal);
			}
		}
	}
}