using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LightGive.UIUtil;
using TMPro;

namespace OneWeekGamejam.Charge
{
	public class UIResult : UINode
	{
		[SerializeField] TextMeshProUGUI _textScore = null;
		[SerializeField] Button _buttonTitle = null;
		[SerializeField] Button _buttonRetry = null;

		protected override void OnInit()
		{
			base.OnInit();
			_buttonRetry.onClick.AddListener(OnButtonDownRetry);
			_buttonTitle.onClick.AddListener(OnButtonDownTitle);
		}

		public void SetScore(ScoreData scoreData)
		{
			_textScore.text = scoreData.Score.ToString();
		}

		void OnButtonDownTitle()
		{
			Hide();
		}

		void OnButtonDownRetry()
		{

		}
	}
}
