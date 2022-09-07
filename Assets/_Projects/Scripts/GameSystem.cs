using System.Collections;
using Cinemachine;
using LightGive.ManagerSystem;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge
{
	public class GameSystem : SingletonMonoBehaviour<GameSystem>
	{
		public static float GameSystemDeltaTime => Instance._timeGameSystem.DeltaTime;
		public static float ObjectDeltaTime => Instance._timeObject.DeltaTime;
		public static float ObjectTimeScale => Instance._timeObject.TimeScale;

		[SerializeField] CinemachineImpulseSource _cinemachineImpulseSource = null;

		TimeGroup _timeGameSystem = null;
		TimeGroup _timeObject = null;

		void Start()
		{
			_timeGameSystem = TimeController.Instance.CreateTimeGroup("GameSystem", TimeController.Instance.GlobalTimeGroup);
			_timeObject = TimeController.Instance.CreateTimeGroup("Object", _timeGameSystem);
		}

		public void ShakeCamera(Vector3 vec,float shakeDuration)
		{
			_cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = shakeDuration;
			_cinemachineImpulseSource.GenerateImpulseAt(Vector3.zero, vec);

		}

		public void HitStop(float stopTime, UnityAction onResumed = null)
		{
			StartCoroutine(HitStopCoroutine(stopTime, onResumed));
		}

		IEnumerator HitStopCoroutine(float stopTime, UnityAction onResumed)
		{
			_timeObject.Pause();
			yield return new WaitForSeconds(stopTime);
			_timeObject.Resume();
			onResumed?.Invoke();
		}
	}
}