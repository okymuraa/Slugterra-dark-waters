using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Scene.Daily
{
	public static class DailySceneManager
	{
		private static List<Daily> _dailes;

		private static int[] _dailyOrder;

		private static int _minGeometyRepeatSeparation;

		private static int[] _schedule;

		private static DateTime _scheduleStartDate;

		private static int[] Schedule
		{
			get
			{
				if (_schedule == null)
				{
					if (PlayerPrefsSystem.HasKey("ScheduleStartDate"))
					{
						_schedule = ParseSchedule(PlayerPrefs.GetString("DailySchedule"));
					}
					else
					{
						InitSchedule();
					}
				}
				return _schedule;
			}
		}

		private static DateTime ScheduleStartDate
		{
			get
			{
				if (_scheduleStartDate.Year == 1)
				{
					if (PlayerPrefsSystem.HasKey("ScheduleStartDate"))
					{
						DateTime.TryParse(PlayerPrefs.GetString("ScheduleStartDate"), out _scheduleStartDate);
					}
					else
					{
						InitSchedule();
					}
				}
				return _scheduleStartDate;
			}
		}

		static DailySceneManager()
		{
			_minGeometyRepeatSeparation = 2;
			_dailes = new List<Daily>();
			_schedule = null;
			_scheduleStartDate = default(DateTime);
			_dailes.Add(new Daily(typeof(DailyMission_01), typeof(DailyMission_01_Assault_01)));
			_dailes.Add(new Daily(typeof(DailyMission_01), typeof(DailyMission_01_Defence_01)));
			_dailes.Add(new Daily(typeof(DailyMission_01), typeof(DailyMission_01_Escort_01)));
			_dailes.Add(new Daily(typeof(DailyMission_02), typeof(DailyMission_02_Assault)));
			_dailes.Add(new Daily(typeof(DailyMission_02), typeof(DailyMission_02_Defence)));
			_dailes.Add(new Daily(typeof(DailyMission_02), typeof(DailyMission_02_Escort)));
			_dailes.Add(new Daily(typeof(DailyMission_03), typeof(DailyMission_03_Assault_01)));
			_dailes.Add(new Daily(typeof(DailyMission_03), typeof(DailyMission_03_Assault_02)));
			_dailes.Add(new Daily(typeof(DailyMission_03), typeof(DailyMission_03_Defence)));
			_dailes.Add(new Daily(typeof(DailyMission_04), typeof(DailyMission_04_Assault)));
			_dailes.Add(new Daily(typeof(DailyMission_04), typeof(DailyMission_04_Defence)));
			_dailes.Add(new Daily(typeof(DailyMission_04), typeof(DailyMission_04_Escort)));
		}

		public static Daily GetDaily()
		{
			TimeSpan timeSpan = DateTime.Today - ScheduleStartDate;
			if (Schedule.Length != _dailes.Count || Mathf.Abs(timeSpan.Days) >= _dailes.Count)
			{
				InitSchedule();
			}
			int num = 0;
			num = Mathf.Abs(timeSpan.Days) % _dailes.Count;
			return _dailes[Schedule[num]];
		}

		public static Type GetGeometryForDailyLogic(Type logic)
		{
			for (int i = 0; i < _dailes.Count; i++)
			{
				Daily daily = _dailes[i];
				if (daily.Logic == logic)
				{
					return daily.Geometry;
				}
			}
			return null;
		}

		public static bool IsCurrentDailyMissionComplete()
		{
			Daily daily = GetDaily();
			LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(daily.Logic);
			if (levelItem.GetObjectiveStatus(0) == 1 && levelItem.GetObjectiveStatus(1) == 1 && levelItem.GetObjectiveStatus(2) == 1)
			{
				return true;
			}
			return false;
		}

		public static void SendPushForFirstDailyMission()
		{
			if (!PlayerPrefsSystem.HasKey("first_missions_available"))
			{
				PlayerPrefs.SetInt("first_missions_available", 1);
			}
		}

		private static void InitSchedule()
		{
			_scheduleStartDate = DateTime.Today;
			_schedule = CreateNewDailySchedule();
		}

		private static int[] ParseSchedule(string schedule)
		{
			string[] array = schedule.Split(';');
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array[i];
				int result;
				if (int.TryParse(s, out result))
				{
					array2[i] = result;
				}
			}
			return array2;
		}

		public static void ClearScores()
		{
			Daily daily = GetDaily();
			foreach (Daily daile in _dailes)
			{
				if (daile != daily)
				{
					LevelItem levelItem = MonoSingleton<AppController>.Instance.Model.GetLevelItem(daile.Logic);
					levelItem.SetObjectiveStatus(0, 0);
					levelItem.SetObjectiveStatus(1, 0);
					levelItem.SetObjectiveStatus(2, 0);
				}
			}
		}

		private static int[] CreateNewDailySchedule()
		{
			int[] array = new int[_dailes.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i;
			}
			int num = array.Length;
			while (--num > 0)
			{
				bool flag = true;
				int num2;
				do
				{
					flag = true;
					num2 = UnityEngine.Random.Range(0, num);
					if (num <= _minGeometyRepeatSeparation)
					{
						continue;
					}
					for (int j = 1; j <= _minGeometyRepeatSeparation && num + j < array.Length; j++)
					{
						if (_dailes[array[num2]].Geometry == _dailes[array[num + j]].Geometry)
						{
							flag = false;
							break;
						}
					}
				}
				while (!flag);
				int num3 = array[num2];
				array[num2] = array[num];
				array[num] = num3;
			}
			string text = null;
			for (int k = 0; k < array.Length; k++)
			{
				text = text + array[k] + ";";
			}
			text = text.Remove(text.Length - 1);
			PlayerPrefs.SetString("DailySchedule", text);
			PlayerPrefs.SetString("ScheduleStartDate", DateTime.Today.ToString());
			return ParseSchedule(text);
		}
	}
}
