using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive.UIUtil;
using TMPro;

namespace OneWeekGamejam.Charge
{
    public class UIWave : UINodeAnimation
    {
        [SerializeField] TextMeshProUGUI _textWave = null;
        public void SetWave(int waveNo)
        {
			_textWave.text = waveNo.ToString("0");
            Show();
        }

		protected override void OnShowAfter()
		{
			base.OnShowAfter();
            Hide();
		}
	}
}