using System.Collections.Generic;
using TinyTeam.Debuger;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightMapContainer : MonoBehaviour
{
	public List<LevelLightmaps> levelLightmaps;

	public List<CustomLightmapData> customLightmapData = new List<CustomLightmapData>();

	[SerializeField]
	private bool drawLightmaps;

	[SerializeField]
	private bool drawStoredLightmaps;

	private Texture2D blackTexture;

	private void OnEnable()
	{
		Console.RegisterCommand("showLightmapData", CMDShowLighmapData, "Returns information about lightmaps in the current scene");
		Console.RegisterCommand("showStoredLightmapData", CMDShowStoredLighmapData, "Returns information about stored lightmaps in the current scene");
		Console.RegisterCommand("setStoredLightmaps", CMDSetStoredLighmaps, "SetStored lightMapData");
		Console.RegisterCommand("updateLightmapMeshRenderers", CMDUpdateLightmapMeshRenderers, "Update parameters in mesh renderers");
		Console.RegisterCommand("drawStoredLightmaps", CMDDrawStoredLighmaps, "Draw stored in editor light maps");
		Console.RegisterCommand("drawLightmaps", CMDDrawLighmaps, "draw lightmaps from LightmapSettings");
		blackTexture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
	}

	private void OnDisable()
	{
		Console.UnRegisterCommand("showLightmapData");
		Console.UnRegisterCommand("showStoredLightmapData");
		Console.UnRegisterCommand("setStoredLightmaps");
	}

	private object CMDShowLighmapData(string[] args)
	{
		LightmapData[] lightmaps = LightmapSettings.lightmaps;
		LightmapsMode lightmapsMode = LightmapSettings.lightmapsMode;
		string text = string.Format("For Scene {0} found {1} lightmaps, LighMapMode is: {2} \n", SceneManager.GetActiveScene().name, lightmaps.Length, lightmapsMode);
		if (lightmaps != null && lightmaps.Length > 0)
		{
			for (int i = 0; i < lightmaps.Length; i++)
			{
				LightmapData lightmapData = lightmaps[i];
				string text2 = string.Format("LighMapData[{0}]:", i) + string.Format("\nFar is {0} with resolution {1}x{2}\n", (!(lightmapData.lightmapColor == null)) ? lightmapData.lightmapColor.name : "null", (!(lightmapData.lightmapColor == null)) ? lightmapData.lightmapColor.width : 0, (!(lightmapData.lightmapColor == null)) ? lightmapData.lightmapColor.height : 0) + string.Format("\nNear is {0} with resolution {1}x{2}\n", (!(lightmapData.lightmapDir == null)) ? lightmapData.lightmapDir.name : "null", (!(lightmapData.lightmapDir == null)) ? lightmapData.lightmapDir.width : 0, (!(lightmapData.lightmapDir == null)) ? lightmapData.lightmapDir.height : 0);
				text += text2;
			}
		}
		return text;
	}

	private object CMDSetStoredLighmaps(string[] args)
	{
		List<LightmapData> list = new List<LightmapData>();
		for (int i = 0; i < levelLightmaps.Count; i++)
		{
			list.Add(new LightmapData
			{
				lightmapColor = levelLightmaps[i].lightmapFar,
				lightmapDir = levelLightmaps[i].lightmapNear
			});
		}
		LightmapSettings.lightmaps = list.ToArray();
		return "Successfull";
	}

	private object CMDShowStoredLighmapData(string[] args)
	{
		string text = string.Format("For Scene {0} stored {1} lightmaps\n", SceneManager.GetActiveScene().name, levelLightmaps.Count);
		for (int i = 0; i < levelLightmaps.Count; i++)
		{
			string text2 = string.Format("LighMapData[{0}]:", i) + string.Format("\nFar is {0} with resolution {1}x{2}\n", (!(levelLightmaps[i].lightmapFar == null)) ? levelLightmaps[i].lightmapFar.name : "null", (!(levelLightmaps[i].lightmapFar == null)) ? levelLightmaps[i].lightmapFar.width : 0, (!(levelLightmaps[i].lightmapFar == null)) ? levelLightmaps[i].lightmapFar.height : 0) + string.Format("\nNear is {0} with resolution {1}x{2}\n", (!(levelLightmaps[i].lightmapNear == null)) ? levelLightmaps[i].lightmapNear.name : "null", (!(levelLightmaps[i].lightmapNear == null)) ? levelLightmaps[i].lightmapNear.width : 0, (!(levelLightmaps[i].lightmapNear == null)) ? levelLightmaps[i].lightmapNear.height : 0);
			text += text2;
		}
		return text;
	}

	private object CMDDrawStoredLighmaps(string[] args)
	{
		if (args == null || args.Length == 1)
		{
			return "Usage: drawStoredLightmaps 1 or 0";
		}
		string text = args[1];
		drawStoredLightmaps = text == "1";
		return "Done";
	}

	private object CMDDrawLighmaps(string[] args)
	{
		if (args == null || args.Length == 1)
		{
			return "Usage: drawStoredLightmaps 1 or 0";
		}
		string text = args[1];
		drawLightmaps = text == "1";
		return "Done";
	}

	private object CMDUpdateLightmapMeshRenderers(string[] args)
	{
		for (int i = 0; i < this.customLightmapData.Count; i++)
		{
			CustomLightmapData customLightmapData = this.customLightmapData[i];
			customLightmapData.meshRenderer.lightmapIndex = customLightmapData.lightmapIndex;
			customLightmapData.meshRenderer.lightmapScaleOffset = customLightmapData.lightmapScaleOffset;
			customLightmapData.meshRenderer.enabled = false;
			customLightmapData.meshRenderer.enabled = true;
		}
		return "Done";
	}

	private void FillLightmapsData()
	{
		levelLightmaps.Clear();
		this.customLightmapData.Clear();
		List<MeshRenderer> list = new List<MeshRenderer>(Object.FindObjectsOfType<MeshRenderer>());
		if (LightmapSettings.lightmaps != null)
		{
			for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
			{
				levelLightmaps.Add(new LevelLightmaps
				{
					lightmapFar = LightmapSettings.lightmaps[i].lightmapColor,
					lightmapNear = LightmapSettings.lightmaps[i].lightmapDir
				});
			}
		}
		foreach (MeshRenderer item in list)
		{
			if (item != null && item.lightmapIndex != -1 && item.lightmapIndex != 65535)
			{
				CustomLightmapData customLightmapData = new CustomLightmapData();
				customLightmapData.meshRenderer = item;
				customLightmapData.lightmapIndex = item.lightmapIndex;
				customLightmapData.lightmapScaleOffset = item.lightmapScaleOffset;
				this.customLightmapData.Add(customLightmapData);
			}
		}
	}

	private void OnGUI()
	{
		if (drawStoredLightmaps)
		{
			for (int i = 0; i < levelLightmaps.Count; i++)
			{
				GUI.DrawTexture(new Rect(new Vector2(30f, (float)Screen.height - 200f * (float)(i + 1)), new Vector2(64f, 64f)), (!(levelLightmaps[i].lightmapFar != null)) ? blackTexture : levelLightmaps[i].lightmapFar, ScaleMode.ScaleToFit, false);
				GUI.DrawTexture(new Rect(new Vector2(130f, (float)Screen.height - 200f * (float)(i + 1)), new Vector2(64f, 64f)), (!(levelLightmaps[i].lightmapNear != null)) ? blackTexture : levelLightmaps[i].lightmapNear, ScaleMode.ScaleToFit, false);
			}
		}
		if (drawLightmaps && LightmapSettings.lightmaps != null && LightmapSettings.lightmaps.Length > 0)
		{
			for (int j = 0; j < LightmapSettings.lightmaps.Length; j++)
			{
				GUI.DrawTexture(new Rect(new Vector2(330f, (float)Screen.height - 200f * (float)(j + 1)), new Vector2(64f, 64f)), (!(LightmapSettings.lightmaps[j].lightmapColor != null)) ? blackTexture : LightmapSettings.lightmaps[j].lightmapColor, ScaleMode.ScaleToFit, false);
				GUI.DrawTexture(new Rect(new Vector2(430f, (float)Screen.height - 200f * (float)(j + 1)), new Vector2(64f, 64f)), (!(LightmapSettings.lightmaps[j].lightmapDir != null)) ? blackTexture : LightmapSettings.lightmaps[j].lightmapDir, ScaleMode.ScaleToFit, false);
			}
		}
	}
}
