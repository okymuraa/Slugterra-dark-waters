using System.Collections.Generic;
using UnityEngine;

public class MiniDebug : MonoBehaviour
{
	private List<string> _log = new List<string>();

	private Vector2 _logPosition = Vector2.zero;

	private bool _showLog;

	private float _touchTime;

	private void Awake()
	{
		Application.logMessageReceived += (string condition, string stackTrace, LogType type) =>
		{
			_log.Add(string.Format("[{0}] {1} \n\t\t* {2}", type, condition, stackTrace));
		};
		Object.DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if (Input.touchCount == 3)
		{
			_touchTime += Time.unscaledDeltaTime;
		}
		else
		{
			_touchTime = 0f;
		}
		if (_touchTime >= 1f)
		{
			_touchTime = 0f;
			_showLog = !_showLog;
		}
		if (Input.GetKeyUp(KeyCode.L))
		{
			_showLog = !_showLog;
		}
	}

	private void OnGUI()
	{
		if (!_showLog)
		{
			return;
		}
		GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height / 2));
		GUILayout.BeginHorizontal(GUI.skin.box);
		_logPosition = GUILayout.BeginScrollView(_logPosition);
		foreach (string item in _log)
		{
			GUILayout.Label(item);
		}
		if (GUILayout.Button("Close"))
		{
			_showLog = false;
		}
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}
