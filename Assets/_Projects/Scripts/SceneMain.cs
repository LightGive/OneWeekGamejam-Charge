using System.Collections;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
	public class SceneMain : MonoBehaviour
    {
		[SerializeField] UITitle _UITitle = null;
		[SerializeField] UIMain _UIMain = null;
		[SerializeField] UIResult _UIResult = null;
		[SerializeField] EnemyGenerator _enemyGenerator = null;
		[SerializeField] Player _player = null;

		public ScoreData ScoreData { get; private set; } = null;

		void Awake()
		{
			ScoreData = new ScoreData();
		}

		void Start()
		{
			_UITitle.Show();
			_player.HP.OnHitpointChanged.AddListener(OnHitPointChanged);
		}

		public void GameStart()
		{
			_UITitle.Hide();
			_UIMain.Show();

			ScoreData.ResetScore();
			_enemyGenerator.StartGenerate();
		}

		void GameOver()
		{
			_enemyGenerator.StopGenerate();
			_UIResult.SetScore(ScoreData);
			_UIResult.Show();
			
		}

		void OnHitPointChanged(int max, int current)
		{
			if(current > 0) { return; }
			GameOver();
		}
	}
}