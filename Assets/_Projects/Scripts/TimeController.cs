using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive.ManagerSystem;

namespace OneWeekGamejam.Charge
{
	public class TimeController : SingletonMonoBehaviour<TimeController>
	{
		public Dictionary<string, TimeGroup> TimeKeyGroupPair { get; private set; } = null;
		public TimeGroup GlobalTimeGroup { get; private set; } = null;

		protected override void Awake()
		{
			base.Awake();
			TimeKeyGroupPair = new Dictionary<string, TimeGroup>();
			GlobalTimeGroup = CreateTimeGroup("Global", null);
		}

		/// <summary>
		/// TimeGroupÇçÏê¨Ç∑ÇÈ
		/// </summary>
		/// <param name="key"></param>
		/// <param name="parentTimeGroup"></param>
		/// <returns></returns>
		public TimeGroup CreateTimeGroup(string key, TimeGroup parent)
		{
			var timeGroup = new TimeGroup();
			parent?.SetChildTimeGroup(timeGroup);
			TimeKeyGroupPair.Add(key, timeGroup);
			return timeGroup;
		}

		void Update()
		{
			GlobalTimeGroup.UpdateTimeDeltaTime(Time.deltaTime);
		}
	}

	public class TimeGroup
	{
		float _baseDeltaTime = 0.0f;
		float _deltaTime = 0.0f;
		public List<TimeGroup> ChildTimeGroupList { get; private set; } = null;
		public float TimeScale { get; private set; } = 1.0f;
		public float DeltaTime { get; private set; } = 0.0f;
		public TimeGroup()
		{
			ChildTimeGroupList = new List<TimeGroup>();
			TimeScale = 1.0f;
		}

		public void SetChildTimeGroup(TimeGroup timeGroup)
		{
			ChildTimeGroupList.Add(timeGroup);
		}

		public void Pause()
		{
			SetTimeScale(0.0f);
		}

		public void Resume()
		{
			SetTimeScale(1.0f);
		}

		public void SetTimeScale(float timeScale)
		{
			TimeScale = timeScale;
			UpdateTimeDeltaTime(_baseDeltaTime);
		}

		public void UpdateTimeDeltaTime(float deltaTime)
		{
			_baseDeltaTime = deltaTime;
			DeltaTime = deltaTime * TimeScale;
			foreach (var child in ChildTimeGroupList)
			{
				child.UpdateTimeDeltaTime(DeltaTime);
			}
		}
	}

}