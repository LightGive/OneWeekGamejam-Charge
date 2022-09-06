using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OneWeekGamejam
{
	public class UIChargeLevel : MonoBehaviour
	{
		[SerializeField] Image[] _gages = null;
		[SerializeField] GameObject _chargeUI = null;

		public void OnChargeChanged(bool isStart)
		{
			_chargeUI.SetActive(isStart);
		}

		public void OnChargeLevelChanged(int value)
		{
			for (var i = 0; i < _gages.Length; i++)
			{
				_gages[i].enabled = i < value;
			}
		}

		public void OnChargeLevelMaxChanged(int value)
		{
			for (var i = 0; i < _gages.Length; i++)
			{
				_gages[i].gameObject.SetActive(i < value);
			}
		}
	}
}