using UnityEngine;

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