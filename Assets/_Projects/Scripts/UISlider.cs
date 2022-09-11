using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OneWeekGamejam.Charge
{
    public class UISlider : MonoBehaviour
    {
        [SerializeField] Slider _slider = null;
        [SerializeField] TextMeshProUGUI _textValue = null;
		[SerializeField] GameObject _background = null;

		void Awake()
		{
			_slider.onValueChanged.AddListener(OnValueChanged);
		}

		void OnValueChanged(float _val)
		{
			_textValue.text = $"{_val * 100.0f:0}%";
		}

		public void OnPointerDown()
		{
			_background.SetActive(true);
		}

		public void OnPointerUp()
		{
			_background.SetActive(false);
		}
	}
}