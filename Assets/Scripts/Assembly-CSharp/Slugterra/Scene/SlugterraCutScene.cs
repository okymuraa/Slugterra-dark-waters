using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Scene
{
	public abstract class SlugterraCutScene : MonoBehaviour
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<object, bool> OnPlayCutSceneEvent__BackingField;

		protected CutSceneView _cutSceneView;

		protected bool _isSkipEnable;

		public static event Action<object, bool> OnPlayCutSceneEvent
		{
			add
			{
				Action<object, bool> action = OnPlayCutSceneEvent__BackingField;
				Action<object, bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayCutSceneEvent__BackingField, (Action<object, bool>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<object, bool> action = OnPlayCutSceneEvent__BackingField;
				Action<object, bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayCutSceneEvent__BackingField, (Action<object, bool>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected virtual void OnCreate()
		{
		}

		private void Awake()
		{
			OnCreate();
			CutSceneView[] array = UnityEngine.Object.FindObjectsOfType<CutSceneView>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].SkipButton != null)
				{
					_cutSceneView = array[i];
				}
			}
		}

		protected virtual void ShowScreens(bool show)
		{
			_cutSceneView.ShowScreens(show);
		}

		protected void RisePlayCutSceneEvent(bool isPlaying)
		{
			if (OnPlayCutSceneEvent__BackingField != null)
			{
				OnPlayCutSceneEvent__BackingField(this, isPlaying);
			}
		}

		protected virtual void _OnEnable()
		{
		}

		private void OnEnable()
		{
			_OnEnable();
		}

		protected virtual void _OnDisable()
		{
		}

		private void OnDisable()
		{
			_OnDisable();
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			}
		}
	}
}
