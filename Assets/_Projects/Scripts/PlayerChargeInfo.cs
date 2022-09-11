using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge
{
	[System.Serializable]
	public class PlayerChargeInfo
	{
		const int MaxChageMaxLevel = 4;
		const string AnimParamIntChargeLevel = "ChargeLevel";

		[System.Serializable]
		public class ChargeInfo
		{
			[field: SerializeField] public float Pitch { get; private set; } = 0.0f;
		}
		public class OnChargeChangeEvent : UnityEvent<bool> { }
		public class OnChargeLevelChangeEvent : UnityEvent<int> { }
		public class OnChargeLevelMaxChangeEvent : UnityEvent<int> { }

		[SerializeField] Player _player = null;
		[SerializeField] Animator _anim = null;
		[SerializeField] AudioSource _source = null;
		[SerializeField] ChargeInfo _currentChargeInfo = null;
		int _chargeLevel = 0;
		int _chargeLevelMax = 0;


		[field: SerializeField] public float ChargeTimeOneLevel { get; private set; } = 0.5f;
		[field: SerializeField] public float ShortenChargeSpeedRateMax { get; private set; } = 2.0f;
		[field: SerializeField] public ChargeInfo[] ChargeInfos { get; private set; } = new ChargeInfo[5];
		public float ChargeTimeCnt { get; private set; } = 0.0f;
		public int ChargeLevelMax
		{
			get => _chargeLevelMax;
			private set
			{
				if (_chargeLevelMax == value) { return; }
				_chargeLevelMax = value;
				OnChargeLevelMaxChanged?.Invoke(value);
			}
		}
		public int ChargeLevel
		{
			get => _chargeLevel;
			private set
			{
				if (_chargeLevel == value) { return; }
				ChangeChargeLevel(value);
			}
		}
		public UnityEvent OnChargeCanceled { get; private set; } = null;
		public OnChargeChangeEvent OnChargeChanged { get; private set; } = null;
		public OnChargeLevelChangeEvent OnChargeLevelChanged { get; private set; } = null;
		public OnChargeLevelMaxChangeEvent OnChargeLevelMaxChanged { get; private set; } = null;
		public PlayerChargeInfo()
		{
			ChargeTimeCnt = 0.0f;
			ChargeLevel = 0;
			ChargeLevelMax = 0;
			OnChargeChanged = new OnChargeChangeEvent();
			OnChargeLevelChanged = new OnChargeLevelChangeEvent();
			OnChargeLevelMaxChanged = new OnChargeLevelMaxChangeEvent();
			OnChargeCanceled = new UnityEvent();
			_currentChargeInfo = ChargeInfos[0];
		}

		public void Init(int chargeLevelMax, int chargeLevel)
		{
			ChargeTimeCnt = 0.0f;
			_chargeLevelMax = chargeLevelMax;
			OnChargeLevelMaxChanged?.Invoke(chargeLevelMax);
			OnChargeLevelChanged?.Invoke(chargeLevel);
			_anim.gameObject.SetActive(false);
		}

		public void ChangeSEVolume(float value)
		{
			_source.volume = value;
		}

		public void SetChargeMaxLevel(int chargeLevelMax)
		{
			ChargeLevelMax = chargeLevelMax;
		}

		public void ChargeStart()
		{
			ChangeChargeLevel(0);
			_anim.gameObject.SetActive(true);
			_source.Play();
			OnChargeChanged.Invoke(true);
		}

		public void ChargeStop()
		{
			ChangeChargeLevel(0);
			_anim.gameObject.SetActive(false);
			_source.Stop();
			ChargeTimeCnt = 0.0f;
			OnChargeChanged.Invoke(false);
		}

		public void ChargeCancel()
		{
			ChangeChargeLevel(0);
			_anim.gameObject.SetActive(false);
			_source.Stop();
			ChargeTimeCnt = 0.0f;
			OnChargeCanceled.Invoke();
		}

		public void ChargeTimeCount()
		{
			var speedMag = Mathf.Lerp(1, ShortenChargeSpeedRateMax, _player.PlayerPowerUp.ChargeSpeedPercent);
			ChargeTimeCnt = Mathf.Clamp(
				ChargeTimeCnt + GameSystem.ObjectDeltaTime * speedMag,
				0.0f,
				ChargeLevelMax * ChargeTimeOneLevel);

			ChargeLevel = Mathf.FloorToInt(ChargeTimeCnt / ChargeTimeOneLevel);
			var scale = Mathf.Clamp01(ChargeTimeCnt / (ChargeTimeOneLevel * MaxChageMaxLevel));
			_anim.transform.localScale = new Vector3(scale, scale, scale);
		}


		void ChangeChargeLevel(int level)
		{
			_chargeLevel = level;
			_anim.SetInteger(AnimParamIntChargeLevel, level);
			_currentChargeInfo = ChargeInfos[level];
			_source.pitch = _currentChargeInfo.Pitch;
			OnChargeLevelChanged?.Invoke(level);
		}
	}
}