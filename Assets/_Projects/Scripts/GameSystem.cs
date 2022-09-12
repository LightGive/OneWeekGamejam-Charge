using System.Collections;
using Cinemachine;
using LightGive.ManagerSystem;
using UnityEngine;
using UnityEngine.Events;

namespace OneWeekGamejam.Charge
{
	public class GameSystem : SingletonMonoBehaviour<GameSystem>
	{
		public enum TimeType
		{
			GameSystemTime,
			ObjectTime
		}
		public static float GameSystemDeltaTime => Instance._timeGameSystem.DeltaTime;
		public static float GameSystemTimeScale => Instance._timeGameSystem.TimeScale;
		public static float ObjectDeltaTime => Instance._timeObject.DeltaTime;
		public static float ObjectTimeScale => Instance._timeObject.TimeScale;

		public static bool IsPausingObjectTime => Instance._timeObject.TimeScale <= 0.0f;

		[SerializeField] CinemachineImpulseSource _cinemachineImpulseSource = null;
		TimeGroup _timeGameSystem = null;
		TimeGroup _timeObject = null;
		bool _isGameStart = false;

		void Start()
		{
			Application.targetFrameRate = 60;
			_timeGameSystem = TimeController.Instance.CreateTimeGroup("GameSystem", TimeController.Instance.GlobalTimeGroup);
			_timeObject = TimeController.Instance.CreateTimeGroup("Object", _timeGameSystem);
		}

		public void StartGame()
		{
			_isGameStart = true;
		}

		public void ShakeCamera(Vector3 vec,float shakeDuration,float shakePower)
		{
			_cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = shakeDuration;
			_cinemachineImpulseSource.GenerateImpulseAt(Vector3.zero, vec.normalized * shakePower);
		}

		public void HitStop(float stopTime, UnityAction onResumed = null)
		{
			StartCoroutine(HitStopCoroutine(stopTime, onResumed));
		}

		public void Pause(TimeType timeType)
		{
			switch (timeType)
			{
				case TimeType.GameSystemTime:
					_timeGameSystem.Pause();
					break;
				case TimeType.ObjectTime:
					_timeObject.Pause();
					break;
			}
		}

		public void Resume(TimeType timeType)
		{
			switch (timeType)
			{
				case TimeType.GameSystemTime:
					_timeGameSystem.Resume();
					break;
				case TimeType.ObjectTime:
					_timeObject.Resume();
					break;
			}
		}

		IEnumerator HitStopCoroutine(float stopTime, UnityAction onResumed)
		{
			_timeObject.Pause();
			yield return new WaitForSeconds(stopTime);
			_timeObject.Resume();
			onResumed?.Invoke();
		}

		public Coroutine WaitForSecoundsForObjectTime(float waitDuration)
		{
			return StartCoroutine(WaitForSecoundsForObjectTimeCoroutine(waitDuration));
		}

		IEnumerator WaitForSecoundsForObjectTimeCoroutine(float waitDuration)
		{
			var timeCnt = 0.0f;
			while (timeCnt < waitDuration)
			{
				timeCnt += ObjectDeltaTime;
				yield return null;
			}
		}
	}
}