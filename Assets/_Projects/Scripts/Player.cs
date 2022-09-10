using UnityEngine;
using UnityEngine.Events;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge
{
	public class Player : MonoBehaviour
	{
		public class PlayerHitPoint
		{
			public class OnHitpointChangedEvent : UnityEvent<int, int> { }
			public OnHitpointChangedEvent OnHitpointChanged { get; private set; } = null;
			public int Max { get; private set; }
			public int Current { get; private set; }

			public PlayerHitPoint(int current, int max)
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
		public class PlayerExperiencePoint
		{
			const float NextExpMag = 1.04f;
			const float StartMaxExp = 3.0f;

			public class OnLevelUpEvent:UnityEvent<int> { }
			public class OnExperiencePointChangedEvent : UnityEvent<int, float,float> { }
			public float Current { get; private set; }
			public float Max { get; private set; }
			public int Level { get; private set; }
			public OnExperiencePointChangedEvent OnExperiencePointChanged { get; private set; } = null;
			public OnLevelUpEvent OnLevelUped { get; private set; } = null;
			public PlayerExperiencePoint()
			{
				OnExperiencePointChanged = new OnExperiencePointChangedEvent();
				OnLevelUped = new OnLevelUpEvent();
				Current = 0.0f;
				Max = StartMaxExp;
				Level = 1;
			}

			public void AddExperiencePoint(float exp)
			{
				var expCount = exp;
				while (expCount >= Max - Current)
				{
					expCount -= Max - Current;
					OnExperiencePointChanged?.Invoke(
						Level,
						Mathf.Clamp01(Current / Max),
						1.0f);

					//LevelUp
					Level++;
					Current = 0.0f;
					Max *= NextExpMag;

				}
				var preCurrent = Current;
				Current += expCount;
				OnExperiencePointChanged?.Invoke(
					Level,
					Mathf.Clamp01(preCurrent / Max),
					Mathf.Clamp01(Current / Max));
			}

			public void Init()
			{
				Current = 0.0f;
				Level = 1;
			}
		}

		const string AnimParamDamage = "Damage";
		const string AnimParamNormal = "Normal";
		const int MaxHitpoint = 4;
		const int StartHitPoint = 1;
		const int StartExperiencePoint = 3;
		const int StartChageMaxLevel = 3;

		[SerializeField] SceneMain _sceneMain = null;
		[SerializeField] Collider2D _collider = null;
		[SerializeField] SpriteFlusher _spriteFlusher = null;
		[SerializeField] BulletGenerator _bulletGenerator = null;
		[SerializeField] PlayerInput _playerInput = null;
		[SerializeField] Animation _anim = null;
		[SerializeField] float _moveSpeed = 1.0f;
		[SerializeField] float _smoothTimeAngle = 360.0f;
		[SerializeField] float _smoothMaxSpeedAngle = 1.0f;
		[SerializeField] float _stickAngleThresholdStart = 0.5f;
		[SerializeField] float _stickAngleThresholdEnd = 0.2f;
		[SerializeField] float _invisibleTime = 4.0f;

		bool _canMove = false;
		bool _isInvisible = false;
		bool _isCharge = false;
		int _level = 0;
		float _exp = 0.0f;
		float _invisibleTimeCnt = 0.0f;
		(float target, float current, float velocity) _smoothAngle = (0.0f, 0.0f, 0.0f);

		[field: SerializeField] public PlayerChargeInfo Charge { get; private set; } = new PlayerChargeInfo();
		[field: SerializeField] public PlayerHitPoint HP { get; private set; } = new PlayerHitPoint(StartHitPoint, StartHitPoint);
		[field: SerializeField] public PlayerExperiencePoint EXP { get; private set; } = new PlayerExperiencePoint();

		void Awake()
		{
			_playerInput.OnAimStart.AddListener(OnAimStart);
			_playerInput.OnFire.AddListener(OnFire);
		}

		void Start()
		{
			HP.SetMax(StartHitPoint, false);
			Charge.Init(StartChageMaxLevel, 0);
		}

		void Update()
		{
			if (_isCharge)
			{
				Charge.ChargeTimeCount();
			}
			if (!_canMove) { return; }
			Move();
			Aim();
			CheckInvisible();
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.tag != TagName.Enemy) { return; }
			Damage();
		}

		public void GameStart()
		{
			_level = 0;
			HP.SetCurrent(StartHitPoint);
			HP.SetMax(StartHitPoint, false);
			Charge.Init(StartChageMaxLevel, 0);
			EXP.Init();
			_collider.enabled = true;
			_canMove = true;
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
			if (!_canMove) { return; }
			_isCharge = true;
			Charge.ChargeStart();
		}

		void OnFire()
		{
			if (!_canMove) { return; }
			_isCharge = false;
			var level = Charge.ChargeLevel;
			Charge.ChargeStop();
			if (level == 0) { return; }
			_bulletGenerator.GeneratePlayerBullet(
				level - 1,
				1000.0f,
				transform.up,
				transform.position);
		}

		/// <summary>
		/// 敵や敵の攻撃が当たった
		/// </summary>
		void Damage()
		{
			if (_isInvisible) { return; }
			_isInvisible = true;

			HP.SetCurrent(HP.Current - 1);
			var isDead = HP.Current <= 0;
			if (isDead)
			{
				Dead();
				SEManager.Instance.Play(SEPath.PLAYER_DEAD);
			}
			else
			{
				SEManager.Instance.Play(SEPath.PLAYER_HIT);
			}

			// カメラ振動
			var ranRad = Random.value * Mathf.PI * 2.0f;
			var ranVec = new Vector3(Mathf.Cos(ranRad), Mathf.Sin(ranRad), 0.0f);
			var cameraShakeDuration = 0.6f;
			var cameraShakePower = isDead ? 40 : 20;
			GameSystem.Instance.ShakeCamera(
				ranVec,
				cameraShakeDuration,
				cameraShakePower);

			// 点滅
			_spriteFlusher.StartFlush(2);
			
			// ヒットストップ
			GameSystem.Instance.HitStop(0.4f, () =>
			{
				_anim.Play(AnimParamDamage);

			});
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

		void Dead()
		{
			_canMove = false;
			_collider.enabled = false;
			Charge.ChargeCancel();
		}
	}
}