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

		protected override void OnInit()
		{
			base.OnInit();
			_buttonStart.onClick.AddListener(OnButtonDownStart);
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
	}
}