using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class EnemyCollider : MonoBehaviour
    {
        [SerializeField] public Enemy _enemy = null;
        public void Hit(Vector2 vec)
        {
            _enemy.Damage(vec);
        }
    }
}