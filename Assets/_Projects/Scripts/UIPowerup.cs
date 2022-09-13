using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive.UIUtil;
using UnityEngine.UI;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge 
{
	public class UIPowerup : UINodeAnimation
	{
		[SerializeField] Transform _buttonContentParent = null;
		[SerializeField] Player _player = null;
		[SerializeField] PlayerPowerup _playerPowerup = null;
		[SerializeField] Button _buttonNotPowerup = null;
		[SerializeField] UIPowerupButton[] _UIPowerupButtons = null;
		[SerializeField] PowerupButtonInfoData[] _buttonInfos = null;
		int _powerupCnt = 0;

		protected override void OnInit()
		{
			base.OnInit();
			_player.EXP.OnLevelUped.AddListener(OnLevelUped);
			foreach (var button in _UIPowerupButtons)
			{
				button.OnPoweruped.AddListener(OnButtonDownPowerup);
			}
			_buttonNotPowerup.onClick.AddListener(OnButtonDownNotPowerup);
		}

		protected override void OnShowBefore()
		{
			base.OnShowBefore();
			SEManager.Instance.Play(SEPath.LEVELUP);
		}

		protected override void OnHideBefore()
		{
			base.OnHideBefore();
			foreach(var button in _UIPowerupButtons)
			{
				button.ChangeInteractable(false);
			}
		}

		void OnLevelUped(int upLevel)
		{
			GameSystem.Instance.Pause(GameSystem.TimeType.GameSystemTime);
			Show();
			_powerupCnt = upLevel;
			SetButton();
		}

		void SetButton()
		{
			var list = _playerPowerup.GetCanPlayerPowerupList();
			var max = Mathf.Clamp(list.Count, 0, _UIPowerupButtons.Length);
			for (int i = 0; i < _UIPowerupButtons.Length; i++)
			{
				var button = _UIPowerupButtons[i];
				var isActive = i < max;
				button.gameObject.SetActive(isActive);
				button.ChangeInteractable(isActive);
				if (!isActive) { continue; }

				var ran = Random.Range(0, list.Count);
				var t = list[ran];
				list.Remove(t);
				button.SetPowerUp(t, _buttonInfos[(int)t]);
			}
		}

		void OnButtonDownPowerup(PlayerPowerupType powerupType)
		{
			SEManager.Instance.Play(SEPath.POWERUP);
			_playerPowerup.OnPowerup(powerupType);
			_powerupCnt--;
			if (_powerupCnt <= 0)
			{
				Hide(onHideAfter: () =>
				 {
					 GameSystem.Instance.Resume(GameSystem.TimeType.GameSystemTime);
				 });
				return;
			}
			SetButton();
		}

		void OnButtonDownNotPowerup()
		{
			SEManager.Instance.Play(SEPath.CANCEL);
			Hide(onHideAfter: () =>
			{
				GameSystem.Instance.Resume(GameSystem.TimeType.GameSystemTime);
			});
		}
	}
}