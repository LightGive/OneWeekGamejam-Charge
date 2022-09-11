using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace OneWeekGamejam.Charge
{
	public class UIPowerupButton : MonoBehaviour
	{
		[SerializeField] Button _button = null;
		[SerializeField] Image _image = null;
		[SerializeField] TextMeshProUGUI _text = null;

		public class OnPowerupEvent : UnityEvent<PlayerPowerupType> { }
		public OnPowerupEvent OnPoweruped { get; private set; } = new OnPowerupEvent();

		public void SetPowerUp(PlayerPowerupType playerPowerupType, PowerupButtonInfoData buttonInfo)
		{
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(() =>
			{
				OnPoweruped.Invoke(playerPowerupType);
			});

			_image.sprite = buttonInfo.ButtonImage;
			_text.text = buttonInfo.ButtonText;
		}

		public void ChangeInteractable(bool isInteractable)
		{
			_button.interactable = isInteractable;
		}
	}
}