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
		[SerializeField] BulletGenerator _bulletGenerator = null;
		[SerializeField] Player _player = null;
		[SerializeField] ScoreController _scoreController = null;

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
			_UIMain.Show();

			_scoreController.Init();
			_enemyGenerator.ResetGenerate();
			_enemyGenerator.StartGenerate();
			_player.GameStart();

			BGMSwitcher.FadeOutAndFadeIn(BGMPath.MAIN1);
		}

		void GameOver()
		{
			BGMManager.Instance.Stop();
			_enemyGenerator.StopGenerate();
			_bulletGenerator.ClearGenerateBullet();
			_UIResult.SetScore(_scoreController.CurrentScore);
			StartCoroutine(GameOverCoroutine());
		}

		public void ResultReturnTitle()
		{
			_UIResult.Hide(onHideAfter:()=> 
			{
				_UIMain.Hide();
				_UITitle.Show();
			});
		}

		public void ResultRetry()
		{
			_UIResult.Hide(onHideAfter: () =>
			{
				GameStart();
			});
		}

		IEnumerator GameOverCoroutine()
		{
			yield return GameSystem.Instance.WaitForSecoundsForObjectTime(1.0f);
			_UIResult.Show();
		}

		void OnHitPointChanged(int max, int current)
		{
			if(current > 0) { return; }
			GameOver();
		}
	}
}