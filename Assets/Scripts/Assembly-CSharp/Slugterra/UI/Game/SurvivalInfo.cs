using System;
using AppsMinistry.Core;
using Slugterra.Game.Mission;
using UnityEngine;

namespace Slugterra.UI.Game
{
	internal class SurvivalInfo : MonoBehaviour
	{
		[SerializeField]
		private TextMesh _time;

		[SerializeField]
		private TextMesh _enemies;

		private void Update()
		{
			float time = MonoSingleton<MissionStatisticsController>.Instance.Model.Time;
			string arg;
			string arg2;
			string arg3;
			if (TimeSpan.FromSeconds(time).Days == 0)
			{
				string[] array = TimeSpan.FromSeconds(time).ToString().Split(':', ':', '.');
				arg = array[2];
				arg2 = array[1];
				arg3 = array[0];
			}
			else
			{
				string[] array = TimeSpan.FromSeconds(time).ToString().Split(':', ':', ':', '.');
				arg = array[3];
				arg2 = array[2];
				int result;
				int.TryParse(array[1], out result);
				arg3 = (result + TimeSpan.FromSeconds(time).Days * 24).ToString();
			}
			_time.text = string.Format("{0}:{1}:{2}", arg3, arg2, arg);
			_enemies.text = MonoSingleton<MissionStatisticsController>.Instance.Model.KilledEnemyCount.ToString();
		}
	}
}
