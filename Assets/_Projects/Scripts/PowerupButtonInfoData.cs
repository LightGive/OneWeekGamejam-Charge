using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
	[CreateAssetMenu(menuName = "LightGive/OneWeekGamejam/PowerupButtonInfo")]
	public class PowerupButtonInfoData : ScriptableObject
	{
		[SerializeField] public string ButtonText;
		[SerializeField] public Sprite ButtonImage;
	}
}