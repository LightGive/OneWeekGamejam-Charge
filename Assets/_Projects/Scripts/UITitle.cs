using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge
{
    public class UITitle : UINodeAnimation
    {
        [SerializeField] Button _buttonStart = null;
		[SerializeField] SceneMain _sceneMain = null;
		[SerializeField] Slider _sliderVolumeBGM = null;
		[SerializeField] Slider _sliderVolumeSE = null;
		[SerializeField] Player _player = null;
		bool _canGameStart = false;

		protected override void OnInit()
		{
			base.OnInit();
			_buttonStart.onClick.AddListener(OnButtonDownStart);
			var startVolume = 0.2f;
			_sliderVolumeBGM.value = startVolume;
			_sliderVolumeSE.value = startVolume;
			OnValueChangedBGMVolume(startVolume);
			OnValueChangedSEVolume(startVolume);
			_sliderVolumeBGM.onValueChanged.AddListener(OnValueChangedBGMVolume);
			_sliderVolumeSE.onValueChanged.AddListener(OnValueChangedSEVolume);
		}

		protected override void OnShowBefore()
		{
			base.OnShowBefore();
			BGMManager.Instance.Play(BGMPath.TITLE1);
		}

		protected override void OnShowAfter()
		{
			base.OnShowAfter();
			_canGameStart = true;
		}

		void OnButtonDownStart()
		{
			if (!_canGameStart) { return; }
			_canGameStart = false;
			SEManager.Instance.Play(SEPath.START_GAME); 
			Hide();
			_sceneMain.GameStart();
		}

		void OnValueChangedBGMVolume(float val)
		{
			BGMManager.Instance.ChangeBaseVolume(val);

		}
		void OnValueChangedSEVolume(float val)
		{
			_player.Charge.ChangeSEVolume(val);
			SEManager.Instance.ChangeBaseVolume(val);
		}
	}
}