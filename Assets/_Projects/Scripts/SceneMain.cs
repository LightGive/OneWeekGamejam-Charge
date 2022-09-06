using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive.UIUtil;

namespace OneWeekGamejam.Charge
{
    public class SceneMain : MonoBehaviour
    {
		[SerializeField] UIMain _UIMain = null;

		void Start()
		{
			_UIMain.Show();
		}
	}
}