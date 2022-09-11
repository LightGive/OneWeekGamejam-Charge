using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class PlayerPowerup : MonoBehaviour
    {
		[SerializeField] Player _player = null;
		[SerializeField] int[] _powerupMax = null;

		public int[] PowerupCounts { get; private set; } = null;

		public int SpeedLevel => PowerupCounts[(int)PlayerPowerupType.MoveSpeed];
		public float ChargeSpeedPercent =>
			Mathf.Clamp01(
				PowerupCounts[(int)PlayerPowerupType.ShortenChargeTime] /
				_powerupMax[(int)PlayerPowerupType.ShortenChargeTime]);

		void Awake()
		{
			PowerupCounts = new int[_powerupMax.Length];
			var powerupTypeName = System.Enum.GetNames(typeof(PlayerPowerupType));
			if (_powerupMax.Length != powerupTypeName.Length)
			{
				Debug.LogError("MaxÇÃêîÇ™enumÇÃêîÇ∆çáÇ¡ÇƒÇ¢Ç»Ç¢");
			}
		}

		public void Init()
		{
			for (var i = 0; i < PowerupCounts.Length; i++)
			{
				PowerupCounts[i] = 0;
			}
		}

		public List<PlayerPowerupType> GetCanPlayerPowerupList()
		{
			var li = new List<PlayerPowerupType>();
			for (var i = 0; i < _powerupMax.Length; i++)
			{
				var t = (PlayerPowerupType)i;
				if (PowerupCounts[i] >= _powerupMax[i]) { continue; }
				switch (t)
				{
					case PlayerPowerupType.CureLife:
						if (_player.HP.IsFullHP) { continue; }
						break;
				}

				li.Add((PlayerPowerupType)i);
			}
			return li;
		}

		public void OnPowerup(PlayerPowerupType playerPowerupType)
		{
			PowerupCounts[(int)playerPowerupType]++;
			switch (playerPowerupType) 
			{
				case PlayerPowerupType.AddMaxChargeLevel:
					_player.Charge.SetChargeMaxLevel(_player.Charge.ChargeLevelMax + 1);
					break;
				case PlayerPowerupType.AddMaxLife:
					_player.HP.SetMax(_player.HP.Max + 1, true);
					break;
				case PlayerPowerupType.CureLife:
					_player.HP.SetCurrent(_player.HP.Max);
					break;
				case PlayerPowerupType.MoveSpeed:
					break;
			}
		}
	}
}