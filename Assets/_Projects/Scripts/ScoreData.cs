using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class ScoreData
    {
        public const int EnemyDefeateScore = 1;
        public int Score { get; private set; } = 0;

        public ScoreData()
		{
            ResetScore();
		}

        public void ResetScore()
		{
            Score = 0;
		}

        public void DefeatEnemy(int level)
        {
            Score += EnemyDefeateScore * level;
        }
    }
}