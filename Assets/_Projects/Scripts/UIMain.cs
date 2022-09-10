using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Animations;
using LightGive.UIUtil;
using System.Linq;
using System;
using TMPro;

namespace OneWeekGamejam.Charge
{
	public class UIMain : UINode
	{
		const float LevelUpStopDuration = 0.1f;
		public class OnAddExpEvent : UnityEvent<int> { }

		[SerializeField] Player _player = null;
		[SerializeField] UIChargeLevel _UIChargeLevel = null;

		[SerializeField] TextMeshProUGUI _textLevel = null;
		[SerializeField] Slider _expSlider = null;
		[SerializeField] AnimationCurve _expBarUpCurve = new AnimationCurve();
		[SerializeField] float _expBarUpDucation = 0.1f;

		[SerializeField] Image[] _imageHP = new Image[3];
		[SerializeField] Sprite _hpFull = null;
		[SerializeField] Sprite _hpEmpty = null;

		public OnAddExpEvent OnAddExp { get; private set; } = null;

		Dictionary<int, Tuple<float, float>> _expBarUpDic = null;
		Coroutine _expBarCoroutine = null;

		protected override void OnInit()
		{
			base.OnInit();
			OnAddExp = new OnAddExpEvent();
			_expBarUpDic = new Dictionary<int, Tuple<float, float>>();
			_player.EXP.OnExperiencePointChanged.AddListener(OnExperiencePointChanged);
			_player.HP.OnHitpointChanged.AddListener(OnHitpointChanged);
			_player.Charge.OnChargeChanged.AddListener(_UIChargeLevel.OnChargeChanged);
			_player.Charge.OnChargeLevelChanged.AddListener(_UIChargeLevel.OnChargeLevelChanged);
			_player.Charge.OnChargeLevelMaxChanged.AddListener(_UIChargeLevel.OnChargeLevelMaxChanged);
			_player.Charge.OnChargeCanceled.AddListener(_UIChargeLevel.OnChargeCanceled);
		}

		protected override void OnShowBefore()
		{
			base.OnShowAfter();
			_UIChargeLevel.Show();
		}

		void Update()
		{
			if (_expBarUpDic.Keys.Count == 0 || _expBarCoroutine != null) { return; }
			var minLevel = _expBarUpDic.Keys.Min();
			_expBarUpDic.Remove(minLevel, out var fromTo);
			_expBarCoroutine = StartCoroutine(ExperienceBarUpCoroutine(minLevel, fromTo.Item1, fromTo.Item2, _expBarUpDic.Count + 1));
		}


		public void ResetExp()
		{
			_textLevel.text = "1";
			_expSlider.value = 0.0f;
			_expBarUpDic.Clear();
			if(_expBarCoroutine != null)
			{
				StopCoroutine(_expBarCoroutine);
			}
		}


		void OnHitpointChanged(int max, int current)
		{
			for (var i = 0; i < _imageHP.Length; i++)
			{
				var target = _imageHP[i];
				target.gameObject.SetActive(i < max);
				if (!target.gameObject.activeSelf) { continue; }
				target.sprite = i < current ? _hpFull : _hpEmpty;
			}
		}

		void OnExperiencePointChanged(int level, float from, float to)
		{
			var nextLevel = level + 1;
			var f = _expBarUpDic.ContainsKey(nextLevel) ?
				_expBarUpDic[nextLevel].Item1 :
				from;
			var fromTo = new Tuple<float, float>(f, to);
			_expBarUpDic[nextLevel] = fromTo;
		}

		IEnumerator ExperienceBarUpCoroutine(int nextLevel,float from, float to, float multi)
		{
			var timeCnt = 0.0f;
			while (timeCnt <= _expBarUpDucation)
			{
				timeCnt += GameSystem.GameSystemDeltaTime * multi;
				var lerp = _expBarUpCurve.Evaluate(Mathf.Clamp01(timeCnt / _expBarUpDucation));
				_expSlider.value = Mathf.Lerp(from, to, lerp);
				yield return null;
			}

			timeCnt = 0.0f;
			while (timeCnt <= LevelUpStopDuration)
			{
				timeCnt += GameSystem.GameSystemDeltaTime * multi;
				yield return null;
			}

			if (to >= 1.0f)
			{
				_expSlider.value = 0.0f;
				_textLevel.text = nextLevel.ToString("0");
			}
			_expBarCoroutine = null;
		}
	}
}