using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using KanKikuchi.AudioManager;

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

		void Update()
		{

		}

		public void Prepare()
		{
			_enemyGenerator.ClearGenerateEnemy();
			_player.transform.position = Vector3.zero;
		}

		public void GameStart()
		{
			_UIMain.ResetExp();
			_UITitle.Hide();
			_UIMain.Show();

			ScoreData.ResetScore();
			_enemyGenerator.ResetGenerate();
			_enemyGenerator.StartGenerate();
			_player.GameStart();

			BGMSwitcher.FadeOutAndFadeIn(BGMPath.MAIN1);
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