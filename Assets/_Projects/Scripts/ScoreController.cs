using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
	public class ScoreController : MonoBehaviour
	{
		readonly int[] ComboScores = new int[]
		{
			100,
			200,
			400,
			600,
			800,
			1000,
		};

		[SerializeField] UIMain _UIMain = null;
		[SerializeField] ScoreProduction _scoreProduction = null;
		public int CurrentScore { get; private set; } = 0;

		public void Init()
		{
			CurrentScore = 0;
			_UIMain.SetScore(0);
		}

		public void AddScore(int hitCount, Vector3 pos)
		{
			var count = Mathf.Clamp(hitCount - 1, 0, ComboScores.Length - 1);
			var addScore = ComboScores[count];
			CurrentScore += addScore;
			_UIMain.SetScore(CurrentScore);
			_scoreProduction.Popup(addScore, pos);
		}
	}
}