using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    [CreateAssetMenu(menuName = "LightGive/OneWeekGamejam/CreateWaveData")]
    public class WaveData : ScriptableObject
    {
        public enum EnemyType
        {
            Ufo = 0,
        }

        [SerializeField] public EnemyType GenerateType;
        [SerializeField] public float Speed = 10;
        [SerializeField] public float AngleSmoothTime = 90.0f;
        [SerializeField] public int MaxEnemyNum = 5;
        [SerializeField] public Color EnemyColor = Color.white;
    }
}