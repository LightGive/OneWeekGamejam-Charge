using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;

namespace OneWeekGamejam.Charge
{
    public class UIMain : UINode
    {
        [SerializeField] Image[] _imageHP = new Image[3];
        [SerializeField] Sprite _hpFull = null;
        [SerializeField] Sprite _hpEmpty = null;
		[SerializeField] Slider _expSlider = null;
		[SerializeField] Player _player = null;
		[SerializeField] UIChargeLevel _UIChargeLevel = null;

		protected override void OnInit()
		{
			base.OnInit();
			_player.HP.OnHitpointChanged.AddListener(OnHitpointChanged);
			_player.Charge.OnChargeChanged.AddListener(_UIChargeLevel.OnChargeChanged);
			_player.Charge.OnChargeLevelChanged.AddListener(_UIChargeLevel.OnChargeLevelChanged);
			_player.Charge.OnChargeLevelMaxChanged.AddListener(_UIChargeLevel.OnChargeLevelMaxChanged);
			_player.EXP.OnExperiencePointChanged.AddListener()
		}

		protected override void OnShowBefore()
		{
			base.OnShowAfter();
			_UIChargeLevel.Show();
		}

		void OnHitpointChanged(int max,int current)
		{
			for(var i = 0; i < _imageHP.Length; i++)
			{
				var target = _imageHP[i];
				target.gameObject.SetActive(i < max);
				if (!target.gameObject.activeSelf) { continue; }
				target.sprite = i < current ? _hpFull : _hpEmpty;
			}
		}

		void OnExperiencePointChanged(int max int current)
		{

		}
	}
}