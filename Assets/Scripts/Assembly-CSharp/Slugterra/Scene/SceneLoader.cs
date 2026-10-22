using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.Plugins;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.Scene.Daily;
using Slugterra.Scene.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slugterra.Scene
{
	public static class SceneLoader
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnSceneChangedEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action OnSceneReload__BackingField;

		public static bool IsNextActionLevelLoading;

		private static Type _baseLoadingScene;

		private static List<Type> _prevScenes;

		private static Type _nextScene;

		private static string prevAssetBundleLoaded;

		private static AsyncOperation _currentAsyncOperation;

		public static AsyncOperation CurrentAsyncOperation
		{
			get
			{
				return _currentAsyncOperation;
			}
		}

		public static event Action OnSceneChangedEvent
		{
			add
			{
				Action action = OnSceneChangedEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSceneChangedEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSceneChangedEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSceneChangedEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static event Action OnSceneReload
		{
			add
			{
				Action action = OnSceneReload__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSceneReload__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSceneReload__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSceneReload__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		static SceneLoader()
		{
			prevAssetBundleLoaded = string.Empty;
			_nextScene = typeof(MainSceneController);
			_baseLoadingScene = typeof(LoadingSceneController);
			_prevScenes = new List<Type>();
			_prevScenes.Add(typeof(MainSceneController));
		}

		public static void LoadScene<T>(bool withLoader = false) where T : SceneController
		{
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.multiplayerWithBot = false;
			if (withLoader)
			{
				LoadScene(_baseLoadingScene, typeof(T));
			}
			else
			{
				LoadScene(typeof(T));
			}
		}

		public static void ReloadScene()
		{
			if (OnSceneReload__BackingField != null)
			{
				OnSceneReload__BackingField();
			}
			LoadScene(_baseLoadingScene, _nextScene);
		}

		public static void LoadScene(Type nextScene, bool withLoader = false, bool isPrev = false, bool isAsync = false, bool isAdditive = false)
		{
			if (withLoader)
			{
				LoadScene(_baseLoadingScene, nextScene);
				return;
			}
			if (!isPrev && (_prevScenes.Count == 0 || _prevScenes[_prevScenes.Count - 1] != _nextScene) && _nextScene != nextScene)
			{
				_prevScenes.Add(_nextScene);
			}
			_nextScene = nextScene;
			if (nextScene.Name.Contains("Daily"))
			{
				nextScene = DailySceneManager.GetGeometryForDailyLogic(nextScene);
			}
			DonDestroyMonoSingleton<PopupManager>.Instance.Clear();
			DonDestroyMonoSingleton<InputManager>.Instance.Clear();
			Attribute[] customAttributes = Attribute.GetCustomAttributes(nextScene);
			SceneNameAttribute sceneNameAttribute = null;
			BundleSceneAttribute bundleSceneAttribute = null;
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is SceneNameAttribute)
				{
					sceneNameAttribute = attribute as SceneNameAttribute;
				}
				if (attribute is BundleSceneAttribute)
				{
					bundleSceneAttribute = attribute as BundleSceneAttribute;
				}
			}
			AnalyticsManager.LogScene(sceneNameAttribute.Name);
			bool flag = false;
			bool flag2 = false;
			string empty = string.Empty;
			if (flag && !string.IsNullOrEmpty(empty))
			{
				prevAssetBundleLoaded = empty;
				DonDestroyMonoSingleton<DownloadManager>.Instance.LoadLevel(sceneNameAttribute.Name, empty, isAsync, isAdditive);
			}
			else if (isAsync)
			{
				_currentAsyncOperation = SceneManager.LoadSceneAsync(sceneNameAttribute.Name);
			}
			else
			{
				SceneManager.LoadScene(sceneNameAttribute.Name);
			}
		}

		private static void LoadScene(Type loadingScene, Type nextScene, bool isPrev = false, bool isAsync = false)
		{
			DonDestroyMonoSingleton<PopupManager>.Instance.Clear();
			DonDestroyMonoSingleton<InputManager>.Instance.Clear();
			IsNextActionLevelLoading = true;
			if (loadingScene == null)
			{
				LoadScene(nextScene);
				return;
			}
			Attribute[] customAttributes = Attribute.GetCustomAttributes(loadingScene);
			SceneNameAttribute sceneNameAttribute = null;
			bool flag = false;
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is SceneNameAttribute)
				{
					sceneNameAttribute = attribute as SceneNameAttribute;
				}
				if (attribute is LoadingSceneAttribute)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Time.timeScale = 1f;
				if (!isPrev && (_prevScenes.Count == 0 || _prevScenes[_prevScenes.Count - 1] != _nextScene))
				{
					_prevScenes.Add(_nextScene);
				}
				_nextScene = nextScene;
				AnalyticsManager.LogScene(sceneNameAttribute.Name);
				if (isAsync)
				{
					_currentAsyncOperation = SceneManager.LoadSceneAsync(sceneNameAttribute.Name);
				}
				else
				{
					SceneManager.LoadScene(sceneNameAttribute.Name);
				}
			}
			else
			{
				LoadScene(nextScene);
			}
		}

		public static void LoadPrevScene(bool withLoader = false)
		{
			Type nextScene = typeof(MainSceneController);
			if (_prevScenes.Count > 0)
			{
				nextScene = _prevScenes[_prevScenes.Count - 1];
				_prevScenes.RemoveAt(_prevScenes.Count - 1);
			}
			if (!withLoader)
			{
				LoadScene(nextScene, false, true);
			}
			else
			{
				LoadScene(_baseLoadingScene, nextScene, true);
			}
		}

		public static void LoadFromPrev<TNext>(bool withLoader = false) where TNext : SceneController
		{
			int num = -1;
			for (int num2 = _prevScenes.Count - 1; num2 >= 0; num2--)
			{
				if (_prevScenes[num2].Equals(typeof(TNext)))
				{
					num = num2;
					break;
				}
			}
			_prevScenes.RemoveRange(num + 1, _prevScenes.Count - num - 1);
			LoadPrevScene(withLoader);
		}

		public static void LoadNextScene(bool isAsync = false)
		{
			LoadScene(_nextScene, false, false, isAsync);
		}

		public static void LoadAdditiveScene(bool isAsync = false)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(_nextScene);
			SceneNameAttribute sceneNameAttribute = null;
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is SceneNameAttribute)
				{
					sceneNameAttribute = attribute as SceneNameAttribute;
				}
			}
			UnityEngine.Debug.Log("SceneLoader::LoadAdditiveScene Load " + DailySceneManager.GetDaily().Logic.Name + " isAsync " + isAsync);
			if (isAsync)
			{
				_currentAsyncOperation = SceneManager.LoadSceneAsync(sceneNameAttribute.Name, LoadSceneMode.Additive);
			}
			else
			{
				SceneManager.LoadScene(sceneNameAttribute.Name, LoadSceneMode.Additive);
			}
		}

		public static void OnSceneChanged()
		{
			IsNextActionLevelLoading = false;
			if (OnSceneChangedEvent__BackingField != null)
			{
				OnSceneChangedEvent__BackingField();
			}
		}
	}
}
