using System;
using UnityEngine;

public class tk2dSystem : ScriptableObject
{
	public const string guidPrefix = "tk2d/tk2d_";

	public const string assetName = "tk2d/tk2dSystem";

	public const string assetFileName = "tk2dSystem.asset";

	[NonSerialized]
	public tk2dAssetPlatform[] assetPlatforms = new tk2dAssetPlatform[3]
	{
		new tk2dAssetPlatform("1x", 1f),
		new tk2dAssetPlatform("2x", 2f),
		new tk2dAssetPlatform("4x", 4f)
	};

	private static tk2dSystem _inst;

	private static string currentPlatform = "2x";

	[SerializeField]
	private tk2dResourceTocEntry[] allResourceEntries = new tk2dResourceTocEntry[0];

	public static tk2dSystem inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = Resources.Load("tk2d/tk2dSystem", typeof(tk2dSystem)) as tk2dSystem;
				if (_inst == null)
				{
					_inst = ScriptableObject.CreateInstance<tk2dSystem>();
				}
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(_inst);
				}
			}
			return _inst;
		}
	}

	public static tk2dSystem inst_NoCreate
	{
		get
		{
			if (_inst == null)
			{
				_inst = Resources.Load("tk2d/tk2dSystem", typeof(tk2dSystem)) as tk2dSystem;
			}
			return _inst;
		}
	}

	public static string CurrentPlatform
	{
		get
		{
			return currentPlatform;
		}
		set
		{
			if (value != currentPlatform)
			{
				currentPlatform = value;
			}
		}
	}

	public static bool OverrideBuildMaterial
	{
		get
		{
			return false;
		}
	}

	private tk2dSystem()
	{
	}

	public static tk2dAssetPlatform GetAssetPlatform(string platform)
	{
		tk2dSystem tk2dSystem2 = inst_NoCreate;
		if (tk2dSystem2 == null)
		{
			return null;
		}
		for (int i = 0; i < tk2dSystem2.assetPlatforms.Length; i++)
		{
			if (tk2dSystem2.assetPlatforms[i].name == platform)
			{
				return tk2dSystem2.assetPlatforms[i];
			}
		}
		return null;
	}

	private T LoadResourceByGUIDImpl<T>(string guid) where T : UnityEngine.Object
	{
		tk2dResource tk2dResource2 = Resources.Load("tk2d/tk2d_" + guid, typeof(tk2dResource)) as tk2dResource;
		if (tk2dResource2 != null && tk2dResource2.objectReference != null)
		{
			if (tk2dResource2.objectReference is T exactObj)
			{
				return exactObj;
			}
			if (typeof(Component).IsAssignableFrom(typeof(T)) && tk2dResource2.objectReference is GameObject go)
			{
				T comp = go.GetComponent<T>();
				if (comp != null)
				{
					return comp;
				}
			}
		}

#if UNITY_EDITOR
		string prefabName = null;
		switch (guid)
		{
			case "ff7876079fd80ea4b80d3f0b5b6263f3": prefabName = "GUIAtlas@2x"; break;
			case "bfd6bbb621550ba4a84237fd8c1a76f1": prefabName = "GUIAtlas@1x"; break;
			case "afb08fc54c6229b4aa24e06b2ad7ffda": prefabName = "TVOSAtlas@2x"; break;
			case "f086d7d2421464147b439dc1b5cf7fe9":
			case "823534fa1c5044a1381b96118b469add": prefabName = "TVOSAtlas@1x"; break;
			case "11aa13aceaf8e9940a4da8eb784117af": prefabName = "CharacterAtlas@2x"; break;
			case "f454a2654ab71dd438f08db0f021c712":
			case "ea23cd17fff0b426c9c56b7643c7c6fe": prefabName = "CharacterAtlas@1x"; break;
			case "dd9f8a77c505eff42bd8548626305e3d": prefabName = "LoadingAtlas@2x"; break;
			case "0f0f0cbba234cc44e9e162570671d335":
			case "474764e86047b4d99a408203010407b5": prefabName = "LoadingAtlas@1x"; break;
		}
		if (prefabName != null)
		{
			GameObject pGo = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObject/" + prefabName + ".prefab");
			if (pGo != null)
			{
				if (typeof(Component).IsAssignableFrom(typeof(T)))
				{
					T pComp = pGo.GetComponent<T>();
					if (pComp != null)
					{
						return pComp;
					}
				}
				if (pGo is T exactGo)
				{
					return exactGo;
				}
			}
		}
#endif
		return (T)null;
	}

	private T LoadResourceByNameImpl<T>(string name) where T : UnityEngine.Object
	{
		for (int i = 0; i < allResourceEntries.Length; i++)
		{
			if (allResourceEntries[i] != null && allResourceEntries[i].assetName == name)
			{
				return LoadResourceByGUIDImpl<T>(allResourceEntries[i].assetGUID);
			}
		}
		return (T)null;
	}

	public static T LoadResourceByGUID<T>(string guid) where T : UnityEngine.Object
	{
		return inst.LoadResourceByGUIDImpl<T>(guid);
	}

	public static T LoadResourceByName<T>(string guid) where T : UnityEngine.Object
	{
		return inst.LoadResourceByNameImpl<T>(guid);
	}
}
