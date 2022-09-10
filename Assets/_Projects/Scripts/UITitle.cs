using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge
{
    public class UITitle : UINode
    {
        [SerializeField] Button _buttonStart = null;
		[SerializeField] SceneMain _sceneMain = null;
		
		[SerializeField] Slider _sliderVolumeBGM = null;
		[SerializeField] Slider _sliderVolumeSE = null;

		protected override void OnInit()
		{
			base.OnInit();
			_buttonStart.onClick.AddListener(OnButtonDownStart);

			_sliderVolumeBGM.value = 0.5f;
			_sliderVolumeSE.value = 0.5f;
			OnValueChangedBGMVolume(0.5f);
			OnValueChangedSEVolume(0.5f);
			_sliderVolumeBGM.onValueChanged.AddListener(OnValueChangedBGMVolume);
			_sliderVolumeSE.onValueChanged.AddListener(OnValueChangedSEVolume);
		}

		protected override void OnShowBefore()
		{
			base.OnShowBefore();
			BGMManager.Instance.Play(BGMPath.TITLE1);
		}

		protected override void OnHideBefore()
		{
			base.OnHideBefore();
		}

		void OnButtonDownStart()
		{
			_sceneMain.GameStart();
		}

		void OnValueChangedBGMVolume(float val)
		{
			BGMManager.Instance.ChangeBaseVolume(val);

		}
		void OnValueChangedSEVolume(float val)
		{
			SEManager.Instance.ChangeBaseVolume(val);
		}
	}
}