using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.Plugins;
using AppsMinistry.Scene;
using Slugterra.Configuration;
using Slugterra.Game.Units.Ally.Assistant;
using UnityEngine;

namespace Slugterra.Scene
{
	public class SlugterraSceneController : SceneController
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnBackEvent__BackingField;

		private bool _onlyOneEscDownPerFrame;

		public static event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnLoad()
		{
			if (string.IsNullOrEmpty(tk2dSystem.CurrentPlatform))
			{
				tk2dSystem.CurrentPlatform = "2x";
			}
			MonoSingleton<Slugterra.App.AppController>.Instance.Load();
			base.OnLoad();
		}

		protected override void OnLoaded()
		{
			if (string.IsNullOrEmpty(tk2dSystem.CurrentPlatform))
			{
				tk2dSystem.CurrentPlatform = "2x";
			}
			MonoSingleton<Slugterra.App.AppController>.Instance.Load();
			SceneLoader.OnSceneChanged();
			Time.timeScale = 1f;
			if (!DonDestroyMonoSingleton<InputManager>.Instance.enabled)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.enabled = true;
			}
			DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += Back;
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.currentLevel = GetType().Name;
		}

		protected override void OnClose()
		{
			base.OnClose();
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= Back;
			_onlyOneEscDownPerFrame = true;
		}

		private void Update()
		{
		}

		protected virtual void OnBack()
		{
		}

		private void Back()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
			OnBack();
		}

		protected FollowerController[] GetFollowers()
		{
			FollowerController[] array = UnityEngine.Object.FindObjectsOfType<FollowerController>();
			List<FollowerController> list = new List<FollowerController>();
			FollowerController[] array2 = array;
			foreach (FollowerController followerController in array2)
			{
				if (followerController.Type == AllyType.Follower)
				{
					list.Add(followerController);
				}
			}
			return list.ToArray();
		}
	}
}
