using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;
using TMPro;
using KanKikuchi.AudioManager;

namespace OneWeekGamejam.Charge
{
	public class UIResult : UINodeAnimation
	{
		[SerializeField] TextMeshProUGUI _textScore = null;
		[SerializeField] Button _buttonTitle = null;
		[SerializeField] Button _buttonRetry = null;
		[SerializeField] SceneMain _sceneMain  = null;

		protected override void OnInit()
		{
			base.OnInit();
			_buttonRetry.onClick.AddListener(OnButtonDownRetry);
			_buttonTitle.onClick.AddListener(OnButtonDownTitle);
		}

		protected override void OnShowBefore()
		{
			base.OnShowBefore();
			SEManager.Instance.Play(SEPath.JINGLE_RESULT);
		}

		protected override void OnShowAfter()
		{
			base.OnShowAfter();
			_sceneMain.Prepare();
		}

		public void SetScore(ScoreData scoreData)
		{
			_textScore.text = scoreData.Score.ToString();
		}

		void OnButtonDownTitle()
		{
			_sceneMain.ResultReturnTitle();
		}

		void OnButtonDownRetry()
		{
			_sceneMain.ResultRetry();	
		}

	}
}
