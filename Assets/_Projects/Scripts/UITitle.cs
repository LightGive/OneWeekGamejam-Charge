using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;

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

		void OnButtonDownStart()
		{
			_sceneMain.GameStart();
		}
	}
}