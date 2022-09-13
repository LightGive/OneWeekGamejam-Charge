using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using LightGive.UIUtil;

namespace OneWeekGamejam.Charge
{
	public class UIOperation : UINode
	{
		[System.Serializable]
		public class OperationPage
		{
			[TextArea(1,3)] 
			public string OperationText = "";
			public GameObject PageObject = null;
		}

		[SerializeField] OperationPage[] _pages = null;
		[SerializeField] Button _buttonPrev = null;
		[SerializeField] Button _buttonNext = null;
		[SerializeField] Button _buttonBackTitle = null;
		[SerializeField] TextMeshProUGUI _text = null;
		int _page = 0;

		protected override void OnInit()
		{
			base.OnInit();
			_buttonPrev.onClick.AddListener(() => SetPage(_page - 1));
			_buttonNext.onClick.AddListener(() => SetPage(_page + 1));
			_buttonBackTitle.onClick.AddListener(() => 
			{
				Hide();
			});
		}

		protected override void OnShowBefore()
		{
			base.OnShowBefore();
			SetPage(0);
		}

		protected override void OnHideAfter()
		{
			base.OnHideAfter();
			foreach (var page in _pages)
			{
				page.PageObject.SetActive(false);
			}
		}

		public void SetPage(int page)
		{
			_page = Mathf.Clamp(page, 0, _pages.Length - 1);
			_buttonPrev.gameObject.SetActive(_page != 0);
			_buttonNext.gameObject.SetActive(_page != _pages.Length - 1);
			for (var i = 0; i < _pages.Length; i++)
			{
				_pages[i].PageObject.SetActive(i == _page);
			}
			var p = _pages[_page];
			_text.text = p.OperationText;
		}
	}
}