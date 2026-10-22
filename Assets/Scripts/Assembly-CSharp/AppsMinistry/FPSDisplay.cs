using Slugterra.Configuration;
using TinyTeam.Debuger;
using UnityEngine;
using UnityEngine.Profiling;

namespace AppsMinistry
{
	public sealed class FPSDisplay : MonoBehaviour
	{
		public int v_TargetFrameRate = 60;

		private float updateInterval = 0.5f;

		private int x_location = 100;

		private int y_location = 20;

		private float lastInterval;

		private float frames;

		private float fps;

		private float v_Timer;

		private float v_StartTime;

		private string _buildTimestamp;

		private float[] fpsAll;

		private float fpsAlltemp;

		private byte f;

		private byte t;

		private byte c;

		private bool isDevInfoEnabled;

		private void Awake()
		{
			useGUILayout = false;
			fpsAll = new float[100];
			Application.targetFrameRate = 60;
			isDevInfoEnabled = PlayerPrefs.GetInt("devInfo", 0) == 1;
		}

		private void OnEnable()
		{
			Console.RegisterCommand("showDevInfo", CMDShowDevInfo, "Show Developers Information");
		}

		private void OnDisable()
		{
			Console.UnRegisterCommand("showDevInfo");
		}

		private void OnGUI()
		{
			if (isDevInfoEnabled)
			{
				GUI.color = Color.white;
				GUI.Label(new Rect(Screen.width - x_location, Screen.height - y_location - 15, 100f, 30f), string.Format("FPS: {0:F2}", fps));
				GUI.Label(new Rect(Screen.width - x_location, Screen.height - y_location, 100f, 30f), string.Format("FPS_A: {0:F2}", fpsAlltemp));
				GUI.Label(new Rect(Screen.width - x_location - 100, Screen.height - y_location, 100f, 30f), string.Format("Time: {0:F2}", v_Timer));
				if (Debug.isDebugBuild)
				{
					GUI.Label(new Rect(Screen.width - x_location - 250, Screen.height - y_location, 150f, 30f), string.Format("Memory: {0:F2}mb", (float)Profiler.usedHeapSize / 1048576f));
				}
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.Label(new Rect(10f, Screen.height - y_location * 3, 280f, 30f), string.Format("Screen resolution: {0}x{1}", Screen.width, Screen.height));
				GUI.Label(new Rect(10f, Screen.height - y_location * 2, 280f, 30f), string.Format("Build info: {0} {1}", AppConfiguration.Name, _buildTimestamp));
				GUI.Label(new Rect(10f, Screen.height - y_location, 280f, 30f), string.Format("Scene: {0}", Application.loadedLevelName));
			}
		}

		private void Start()
		{
			lastInterval = Time.realtimeSinceStartup;
			v_StartTime = lastInterval;
			frames = 0f;
			Application.targetFrameRate = v_TargetFrameRate;
			TextAsset textAsset = (TextAsset)Resources.Load("BuildTimestamp", typeof(TextAsset));
			if (textAsset != null)
			{
				_buildTimestamp = textAsset.text;
			}
		}

		private void Update()
		{
			frames++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			v_Timer = realtimeSinceStartup - v_StartTime;
			if (!(realtimeSinceStartup > lastInterval + updateInterval))
			{
				return;
			}
			fps = frames / (realtimeSinceStartup - lastInterval);
			frames = 0f;
			lastInterval = realtimeSinceStartup;
			fpsAll[f] = fps;
			if (c != 100)
			{
				c = 0;
				for (t = 0; t < fpsAll.Length; t++)
				{
					fpsAlltemp += fpsAll[t];
					if (fpsAll[t] != 0f)
					{
						c++;
					}
				}
				fpsAlltemp /= (int)c;
			}
			else
			{
				for (t = 0; t < fpsAll.Length; t++)
				{
					fpsAlltemp += fpsAll[t];
				}
				fpsAlltemp /= 100f;
			}
			f++;
			if (f == fpsAll.Length)
			{
				f = 0;
			}
		}

		private object CMDShowDevInfo(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "What do you want with DevInfo ? for enable type \"showDevInfo 1\", for disable type \"showDevInfo 0\"";
			}
			string text = args[1];
			if (text == "0")
			{
				isDevInfoEnabled = false;
				PlayerPrefs.SetInt("devInfo", 0);
			}
			if (text == "1")
			{
				isDevInfoEnabled = true;
				PlayerPrefs.SetInt("devInfo", 1);
			}
			return "Wrong Parameters for showDevInfo Command. For enable type \"showDevInfo 1\", for disable type \"showDevInfo 0\"";
		}
	}
}
