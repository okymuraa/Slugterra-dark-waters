using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.Domain.Game.HealthRestoration
{
	public abstract class AbstractHealthConfiguration
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnUpdateValues__BackingField;

		public bool IsLoaded { get; protected set; }

		protected abstract string CONFIGURATION_FILE_NAME { get; }

		public event Action OnUpdateValues
		{
			add
			{
				Action action = OnUpdateValues__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUpdateValues__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnUpdateValues__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUpdateValues__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public void Initialize()
		{
			Load();
		}

		protected AbstractHealthData GetHealthConfiguration<K>(K key, ref Dictionary<K, RestoreHealthData> config)
		{
			if (!IsLoaded)
			{
				Load();
			}
			if (!config.ContainsKey(key))
			{
				UnityEngine.Debug.LogWarning("Health configuration doesn't contains key:" + key.ToString());
				UnityEngine.Debug.LogWarning("Create new config for " + key.ToString());
				RestoreHealthData restoreHealthData = new RestoreHealthData();
				config.Add(key, restoreHealthData);
				return restoreHealthData;
			}
			return config[key];
		}

		protected abstract void LoadFromJSON(string str);

		protected abstract string GetJSONObject();

		protected abstract void InitDefaultValues();

		protected void Load()
		{
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName(CONFIGURATION_FILE_NAME)))
			{
				string str = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName(CONFIGURATION_FILE_NAME));
				LoadFromJSON(str);
			}
			else
			{
				InitDefaultValues();
			}
			IsLoaded = true;
		}

		public void Save()
		{
			string jSONObject = GetJSONObject();
			IOSystem.SaveStrings(Application.persistentDataPath, CONFIGURATION_FILE_NAME, jSONObject, false, true);
			RaiseOnUpdateValues();
		}

		public void RaiseOnUpdateValues()
		{
			if (OnUpdateValues__BackingField != null)
			{
				OnUpdateValues__BackingField();
			}
		}
	}
}
