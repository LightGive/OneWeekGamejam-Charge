using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;

namespace OneWeekGamejam
{
	public class UIChargeLevel : UINode
	{
		[SerializeField] Image[] _gages = null;
		[SerializeField] GameObject _chargeUI = null;

		protected override void OnShowBefore()
		{
			base.OnShowBefore();
			_chargeUI.SetActive(false);
		}

		public void OnChargeChanged(bool isShow)
		{
			_chargeUI.SetActive(isShow);
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