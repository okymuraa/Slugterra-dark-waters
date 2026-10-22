using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Command;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Scene.Game;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class BoostEffect : MonoCommand<BoostEffect>
	{
		protected UnitController controller;

		protected ConsumableProperties boost;

		private GameObject _effect;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			controller = (UnitController)args[0];
			boost = (ConsumableProperties)args[1];
			if (GetConsumableType(GetType()) == ConsumableType.None)
			{
				return;
			}
			GameObject gameObject = null;
			gameObject = ((MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ScorchedSparksTutorial_1_1) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ScorchedSparks_1_1) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ScorchedSparks_1_2) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ScorchedSparks_1_3) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ScorchedSparks_1_4) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(Arena_1_5)) ? GameFactory.GetBoostEffectGamePlay(GetConsumableType(GetType())) : GameFactory.GetBoostEffectArena(GetConsumableType(GetType())));
			if (GetConsumableType(GetType()) == ConsumableType.First)
			{
				Camera[] allCameras = Camera.allCameras;
				for (int i = 0; i < allCameras.Length; i++)
				{
					if (allCameras[i].CompareTag("MainUICamera"))
					{
						_effect = UnityEngine.Object.Instantiate(gameObject, allCameras[i].transform);
					}
				}
			}
			else
			{
				_effect = UnityEngine.Object.Instantiate(gameObject, controller.transform);
			}
			_effect.transform.localPosition = Vector3.zero;
		}

		private ConsumableType GetConsumableType(Type type)
		{
			if (type == typeof(ArmorBoost))
			{
				return ConsumableType.ArmorBoost;
			}
			if (type == typeof(SpeedMotion))
			{
				return ConsumableType.SpeedBoost;
			}
			if (type == typeof(SlugFoodBoost))
			{
				return ConsumableType.None;
			}
			if (type == typeof(FirstAidKit))
			{
				return ConsumableType.First;
			}
			return ConsumableType.None;
		}

		protected override void OnFinish()
		{
			UnityEngine.Object.Destroy(_effect, 0.5f);
		}
	}
}
