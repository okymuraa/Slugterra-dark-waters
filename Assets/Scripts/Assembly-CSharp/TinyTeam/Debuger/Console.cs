using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TinyTeam.Debuger
{
	public class Console : MonoBehaviour
	{
		public enum MessageType
		{
			NORMAL = 0,
			WARNING = 1,
			ERROR = 2,
			SYSTEM = 3,
			INPUT = 4,
			OUTPUT = 5,
			UNITY = 6
		}

		private struct Message
		{
			private string text;

			private string formatted;

			public string customType;

			public MessageType type;

			public static Color defaultColor = Color.white;

			public static Color warningColor = Color.yellow;

			public static Color errorColor = Color.red;

			public static Color systemColor = Color.green;

			public static Color inputColor = Color.green;

			public static Color outputColor = Color.cyan;

			public Color color { get; private set; }

			public Message(object messageObject, MessageType messageType, Color displayColor, string customType)
			{
				this = default(Message);
				Set(messageObject, messageType, displayColor, customType);
			}

			public void Set(object messageObject, MessageType messageType, Color displayColor, string customType)
			{
				color = displayColor;
				if (messageObject == null)
				{
					text = "<null>";
				}
				else if (messageType == MessageType.SYSTEM || messageType == MessageType.OUTPUT || messageType == MessageType.INPUT || messageType == MessageType.UNITY)
				{
					text = messageObject.ToString();
				}
				else
				{
					text = "[" + DateTime.Now.ToLongTimeString() + "] " + messageObject.ToString();
				}
				formatted = string.Empty;
				type = messageType;
				this.customType = customType;
			}

			public static Message Log(object message, string customType)
			{
				return CreateNew(message, MessageType.NORMAL, defaultColor, customType);
			}

			public static Message Log(object message, string customType, Color col)
			{
				return CreateNew(message, MessageType.NORMAL, col, customType);
			}

			public static Message Log(object message, MessageType messageType)
			{
				return CreateNew(message, messageType, defaultColor, string.Empty);
			}

			public static Message System(object message)
			{
				return CreateNew(message, MessageType.SYSTEM, systemColor, string.Empty);
			}

			public static Message Warning(object message, string customType)
			{
				return CreateNew(message, MessageType.WARNING, warningColor, customType);
			}

			public static Message Error(object message, string customType)
			{
				return CreateNew(message, MessageType.ERROR, errorColor, customType);
			}

			public static Message Output(object message)
			{
				return CreateNew(message, MessageType.OUTPUT, outputColor, string.Empty);
			}

			public static Message Input(object message)
			{
				return CreateNew(message, MessageType.INPUT, inputColor, string.Empty);
			}

			public static Message Unity(object message, Color unityColor)
			{
				return CreateNew(message, MessageType.UNITY, unityColor, string.Empty);
			}

			public override string ToString()
			{
				return ToGUIString();
			}

			public string ToGUIString()
			{
				if (!string.IsNullOrEmpty(formatted))
				{
					return formatted;
				}
				switch (type)
				{
				case MessageType.INPUT:
					formatted = "<color=#" + ColorToHex(color) + ">>>> " + text + "</color>\n";
					break;
				case MessageType.OUTPUT:
				{
					string[] array = text.Trim('\n').Split('\n');
					StringBuilder stringBuilder = new StringBuilder();
					int num = 0;
					while (array != null && num < array.Length)
					{
						stringBuilder.AppendLine("= " + array[num]);
						num++;
					}
					formatted = "<color=#" + ColorToHex(color) + ">" + stringBuilder.ToString() + "</color>";
					break;
				}
				case MessageType.SYSTEM:
					formatted = "<color=#" + ColorToHex(color) + "># " + text + "</color>\n";
					break;
				case MessageType.WARNING:
					formatted = "<color=#" + ColorToHex(color) + ">* " + text + "</color>\n";
					break;
				case MessageType.ERROR:
					formatted = "<color=#" + ColorToHex(color) + ">** " + text + "</color>\n";
					break;
				case MessageType.UNITY:
					formatted = "<color=#" + ColorToHex(color) + ">*** " + text + "</color>";
					break;
				default:
					formatted = "<color=#" + ColorToHex(color) + ">" + text + "</color>\n";
					break;
				}
				return formatted;
			}
		}

		private class History
		{
			private List<string> history = new List<string>();

			private int index;

			private string current = string.Empty;

			public void Add(string item)
			{
				history.Add(item);
				index = 0;
				current = item;
			}

			public string Fetch(string cur, bool next)
			{
				if (index == 0)
				{
					current = cur;
				}
				if (history.Count == 0)
				{
					return current;
				}
				index += ((!next) ? 1 : (-1));
				if (history.Count + index < 0 || history.Count + index > history.Count - 1)
				{
					index = 0;
					return current;
				}
				current = history[history.Count + index];
				return current;
			}
		}

		private bool isInit;

		private static Console _instance;

		private static object _lock = new object();

		private static bool applicationIsQuitting = false;

		public static bool isConsoleAllowed = false;

		private static bool m_isOpen = false;

		private float preOpenTime;

		public Action delegateInitEventSystem;

		public Action<Console> delegateInitCMD;

		private GameObject ui_root_obj;

		private Text text_content;

		private ScrollRect sr_content;

		private InputField input;

		private Text text_pageCount;

		private GameObject ui_fps_root;

		private Text text_fps;

		internal bool isNeedRefreshContent = true;

		private static int maxLinesForDisplay = 5000;

		private static int maxLinesForAsyncRecord = 5000;

		private int beginMsgIndex;

		private int endMsgIndex;

		private int maxMsgCount = 1;

		private bool needAutoToMax = true;

		private int currentShowMessageType = -1;

		private string currentShowMessageCustom = string.Empty;

		private int fpsMode;

		private FPSCounter fps;

		private MemoryCounter memoryCounter;

		private Dictionary<string, Func<string[], object>> _cmdTable = new Dictionary<string, Func<string[], object>>();

		private Dictionary<string, string> _cmdTableDiscribes = new Dictionary<string, string>();

		private static Queue<Message> _messages = new Queue<Message>();

		private static Dictionary<string, Queue<Message>> customQueueMessages = new Dictionary<string, Queue<Message>>();

		private History _history = new History();

		private bool isLogMessageRegistered;

		private bool enableStackTrace;

		private Queue<string> LogQueue = new Queue<string>();

		private object thisLock = new object();

		private StreamWriter streamWrite;

		private Thread logThread;

		private int asyncWriteCounter;

		private bool isAsyncWritingLog;

		private string _logBuff = string.Empty;

		public static Console Instance
		{
			get
			{
				if (applicationIsQuitting)
				{
					return null;
				}
				lock (_lock)
				{
					if (_instance == null && _instance == null)
					{
						_instance = UnityEngine.Object.FindObjectOfType(typeof(Console)) as Console;
						if (_instance == null)
						{
							GameObject gameObject = new GameObject("(singleton) " + typeof(Console).ToString());
							_instance = gameObject.AddComponent<Console>();
							_instance.Init();
						}
					}
					return _instance;
				}
			}
		}

		public static bool IsOpen
		{
			get
			{
				return m_isOpen;
			}
			set
			{
				Instance.currentShowMessageCustom = string.Empty;
				Instance.maxMsgCount = Instance.CaculateCurrentMaxMsgCount();
				Instance.endMsgIndex = Instance.maxMsgCount - 1;
				Instance.beginMsgIndex = Instance.FindBeginMsgIndexFromEnd();
				Instance.isNeedRefreshContent = true;
				Instance.needAutoToMax = true;
				if (Instance.ui_root_obj == null && value)
				{
					Instance.DrawWindow(true);
				}
				else if (Instance.ui_root_obj != null)
				{
					Instance.input.text = string.Empty;
					Instance.ui_root_obj.SetActive(value);
				}
				if (value && EventSystem.current != null)
				{
					EventSystem.current.enabled = true;
				}
				m_isOpen = value;
			}
		}

		private int currentPersent
		{
			get
			{
				Message[] array = GetAllCurrentTypeMessages().ToArray();
				if (maxMsgCount == -1 || maxMsgCount == 0 || array.Length == 0)
				{
					return 0;
				}
				if (endMsgIndex == array.Length - 1)
				{
					return 100;
				}
				return (int)((float)endMsgIndex * 1f / (float)(array.Length - 1) * 100f);
			}
		}

		private string logFolder
		{
			get
			{
				string empty = string.Empty;
				empty = ((!Application.isEditor) ? Path.Combine(Application.persistentDataPath, "Log") : Path.Combine(Application.dataPath, "../Log"));
				if (!Directory.Exists(empty))
				{
					Directory.CreateDirectory(empty);
				}
				return empty;
			}
		}

		private string logPath
		{
			get
			{
				string path = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log";
				return Path.Combine(logFolder, path);
			}
		}

		private string logBuff
		{
			get
			{
				lock (thisLock)
				{
					int num = Mathf.Min(10, LogQueue.Count);
					for (int i = 0; i < num; i++)
					{
						_logBuff = _logBuff + LogQueue.Dequeue() + "\n";
						asyncWriteCounter++;
					}
					return _logBuff;
				}
			}
		}

		public void OnDestroy()
		{
			if (isInit)
			{
				applicationIsQuitting = true;
				_lock = null;
				_instance = null;
			}
			StopAsyncWriteLog();
		}

		private static Queue<Message> FindCustomQueue(string customType)
		{
			if (string.IsNullOrEmpty(customType))
			{
				return _messages;
			}
			customType = customType.ToLower();
			Queue<Message> value = null;
			if (!customQueueMessages.TryGetValue(customType, out value))
			{
				value = new Queue<Message>();
				customQueueMessages[customType] = value;
			}
			return value;
		}

		private static Message CreateNew(object messageObject, MessageType messageType, Color displayColor, string customType)
		{
			Queue<Message> queue = FindCustomQueue(customType);
			if (queue.Count > maxLinesForDisplay)
			{
				Message result = queue.Dequeue();
				result.Set(messageObject, messageType, displayColor, customType);
				return result;
			}
			Message message = new Message(messageObject, messageType, displayColor, customType);
			queue.Enqueue(message);
			return message;
		}

		private void Init()
		{
			isInit = true;
			fps = new FPSCounter();
			memoryCounter = new MemoryCounter();
			LogMessage(Message.System("TinyTeam Console\nEnter> '?' For Help"));
			RegisterCommandCallback("clear", CMDClear, "Clear All Log");
			RegisterCommandCallback("sys", CMDSystemInfo, "Show System Info");
			RegisterCommandCallback("?", CMDHelp, "Show All CMD");
			RegisterCommandCallback(":", CMDShowMessageType, "Control Show Log Type (-1:all,0:normal,1:warn,2:error,3:system,4:input,5:output,6:unitylog)");
			RegisterCommandCallback("fps", CMDShowFPS, "Fps Conrtol");
			RegisterCommandCallback("mem", CMDShowMemory, "Memory Detail Show");
			RegisterCommandCallback("log", CMDStartAsyncLoging, "Start Asyn Loging");
			RegisterCommandCallback("stack", CMDEnableStackTrace, "Stack");
			RegisterCommandCallback("clean_cache", CMDCleanCache, "Clean Asset Bundles Cache");
			enableStackTrace = PlayerPrefs.GetInt("enableStackTrace", 0) != 0;
			if (delegateInitCMD != null)
			{
				delegateInitCMD(this);
			}
			RegisterLogCallBack();
			if (!applicationIsQuitting && !Application.isMobilePlatform && !Application.isEditor)
			{
			}
		}

		public void RegisterLogCallBack()
		{
			if (!isLogMessageRegistered)
			{
				Application.logMessageReceived += HandleUnityLog;
				isLogMessageRegistered = true;
			}
		}

		public void Awake()
		{
			if (!isInit)
			{
				Init();
			}
			else
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}

		private void OnLevelWasLoaded(int level)
		{
			InitEventSystem();
		}

		private void HandleUnityLog(string condition, string stackTrace, LogType type)
		{
			string text = condition + "\n";
			Color grey = Color.grey;
			if (enableStackTrace)
			{
				string text2 = text;
				text = text2 + "<i><color=#" + ColorToHex(Color.grey) + "><size=9>" + stackTrace + " </size> </color></i>";
			}
			Color unityColor = new Color(0.3882f, 0.7725f, 1f, 1f);
			switch (type)
			{
			case LogType.Error:
			case LogType.Exception:
				unityColor = Color.red;
				if (!enableStackTrace)
				{
					string text2 = text;
					text = text2 + "<i><color=#" + ColorToHex(Color.grey) + "><size=9>" + stackTrace + " </size> </color></i>";
				}
				break;
			case LogType.Warning:
				unityColor = Color.yellow;
				break;
			}
			LogMessage(Message.Unity(text, unityColor));
		}

		private void UpdateKeyCode()
		{
			if (!isConsoleAllowed)
			{
				return;
			}
			if (Input.GetKeyUp(KeyCode.BackQuote))
			{
				IsOpen = !IsOpen;
			}
			if (Input.touchCount == 3 && Time.realtimeSinceStartup - preOpenTime >= 1f)
			{
				for (int i = 0; i < Input.touchCount; i++)
				{
					if (!((double)Input.GetTouch(i).position.y >= (double)Screen.height * 0.5))
					{
						return;
					}
				}
				preOpenTime = Time.realtimeSinceStartup;
				IsOpen = !IsOpen;
			}
			if (IsOpen && input.isFocused)
			{
				if (!Input.GetKeyUp(KeyCode.Return))
				{
					if (Input.GetKeyUp(KeyCode.UpArrow))
					{
						input.text = _history.Fetch(input.text, true);
					}
					else if (Input.GetKeyUp(KeyCode.DownArrow))
					{
						input.text = _history.Fetch(input.text, false);
					}
				}
			}
			else
			{
				if (!IsOpen)
				{
					return;
				}
				if (Input.GetKeyUp(KeyCode.LeftBracket))
				{
					AddPage(-1);
				}
				else if (Input.GetKeyUp(KeyCode.RightBracket))
				{
					AddPage(1);
				}
				if (Input.touchCount != 1)
				{
					return;
				}
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					if (touch.position.x <= (float)Screen.width * 0.5f && touch.position.y >= (float)Screen.height * 0.9f)
					{
						AddPage(-1);
					}
					else if (touch.position.x > (float)Screen.width * 0.5f && touch.position.y >= (float)Screen.height * 0.9f)
					{
						AddPage(1);
					}
				}
			}
		}

		public void Update()
		{
			UpdateKeyCode();
			if (isAsyncWritingLog && asyncWriteCounter >= maxLinesForAsyncRecord)
			{
				asyncWriteCounter = 0;
				CreateNewLogFile();
			}
			if (ui_fps_root != null && fps.Update())
			{
				if (fpsMode == 2)
				{
					memoryCounter.Update(1);
					text_fps.text = fps.current.ToString("F0") + "\n" + memoryCounter.ToString();
				}
				else
				{
					text_fps.text = fps.current.ToString("F0");
				}
			}
			if (IsOpen && isNeedRefreshContent)
			{
				isNeedRefreshContent = false;
				DrawingContent();
			}
		}

		private static string ColorToHex(Color32 color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		private static Color HexToColor(string hex)
		{
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, byte.MaxValue);
		}

		private void InitEventSystem()
		{
			EventSystem eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
			if (eventSystem == null)
			{
				if (delegateInitEventSystem != null)
				{
					delegateInitEventSystem();
					return;
				}
				GameObject gameObject = new GameObject("EventSystem");
				eventSystem = gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		private void DrawWindow(bool isHide)
		{
			transform.position = Vector3.zero;
			GameObject gameObject = (ui_root_obj = new GameObject("TinyTeam_Console_UI"));
			gameObject.transform.parent = transform;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.pixelPerfect = false;
			canvas.sortingOrder = 88888;
			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			if (!Application.isEditor)
			{
				canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
				canvasScaler.referenceResolution = new Vector2(960f, 640f);
				canvasScaler.matchWidthOrHeight = 1f;
			}
			GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
			graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.All;
			InitEventSystem();
			InitBlackground(gameObject);
			InitScrollView(gameObject);
			InitScrollContent(sr_content.gameObject);
			InitInput(gameObject);
		}

		private void InitBlackground(GameObject windows)
		{
			GameObject gameObject = new GameObject("blackground");
			gameObject.transform.parent = windows.transform;
			Image image = gameObject.AddComponent<Image>();
			image.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			image.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
			image.rectTransform.anchorMin = new Vector2(0f, 0.3f);
			image.rectTransform.anchorMax = new Vector2(1f, 1f);
			image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			image.color = new Color(0f, 0f, 0f, 0.8f);
		}

		private void InitScrollView(GameObject windows)
		{
			GameObject gameObject = new GameObject("scrollview");
			gameObject.transform.parent = windows.transform;
			sr_content = gameObject.AddComponent<ScrollRect>();
			sr_content.horizontal = false;
			Image image = gameObject.AddComponent<Image>();
			image.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			image.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, -30f);
			image.rectTransform.anchorMin = new Vector2(0f, 0.3f);
			image.rectTransform.anchorMax = new Vector2(1f, 1f);
			image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			image.color = new Color(0f, 0f, 0f, 1f);
			Mask mask = gameObject.AddComponent<Mask>();
			mask.showMaskGraphic = false;
		}

		private void InitScrollContent(GameObject scrollView)
		{
			GameObject gameObject = new GameObject("content");
			gameObject.transform.parent = scrollView.transform;
			text_content = gameObject.AddComponent<Text>();
			text_content.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			text_content.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
			text_content.rectTransform.anchorMin = new Vector2(0f, 0f);
			text_content.rectTransform.anchorMax = new Vector2(1f, 1f);
			text_content.rectTransform.pivot = new Vector2(0f, 0f);
			text_content.color = new Color(1f, 1f, 1f, 1f);
			text_content.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			text_content.fontSize = 12;
			text_content.alignment = TextAnchor.LowerLeft;
			text_content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			scrollView.GetComponent<ScrollRect>().content = text_content.rectTransform;
		}

		private void InitInput(GameObject windows)
		{
			GameObject gameObject = new GameObject("input");
			gameObject.transform.parent = windows.transform;
			Image image = gameObject.AddComponent<Image>();
			image.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			image.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -15f, 30f);
			image.rectTransform.anchorMin = new Vector2(0f, 0.3f);
			image.rectTransform.anchorMax = new Vector2(1f, 0.3f);
			image.rectTransform.pivot = new Vector2(0f, 0f);
			image.color = new Color(0f, 0f, 0f, 1f);
			GameObject gameObject2 = new GameObject("placeHolder");
			gameObject2.transform.parent = gameObject.transform;
			Text text = gameObject2.AddComponent<Text>();
			text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 5f, -10f);
			text.rectTransform.anchorMin = new Vector2(0f, 0f);
			text.rectTransform.anchorMax = new Vector2(1f, 1f);
			text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			text.color = Color.grey;
			text.text = ">";
			GameObject gameObject3 = new GameObject("pageamount");
			gameObject3.transform.parent = gameObject.transform;
			text_pageCount = gameObject3.AddComponent<Text>();
			text_pageCount.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
			text_pageCount.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 5f, -10f);
			text_pageCount.rectTransform.anchorMin = new Vector2(0f, 0f);
			text_pageCount.rectTransform.anchorMax = new Vector2(1f, 1f);
			text_pageCount.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			text_pageCount.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			text_pageCount.color = Color.grey;
			text_pageCount.text = currentPersent + "%";
			text_pageCount.alignment = TextAnchor.MiddleRight;
			GameObject gameObject4 = new GameObject("inputText");
			gameObject4.transform.parent = gameObject.transform;
			Text text2 = gameObject4.AddComponent<Text>();
			text2.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10f, -20f);
			text2.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 5f, -10f);
			text2.rectTransform.anchorMin = new Vector2(0f, 0f);
			text2.rectTransform.anchorMax = new Vector2(1f, 1f);
			text2.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			text2.color = Color.white;
			text2.supportRichText = false;
			input = gameObject.AddComponent<InputField>();
			input.textComponent = text2;
			input.onEndEdit.AddListener(ProcessInput);
		}

		private void DrawingContent()
		{
			string text = string.Empty;
			Message[] array = GetAllCurrentTypeMessages().ToArray();
			endMsgIndex = Math.Min(endMsgIndex, GetAllCurrentTypeMessages().ToArray().Length - 1);
			for (int i = beginMsgIndex; i <= endMsgIndex; i++)
			{
				text += array[i].ToGUIString();
			}
			text_content.text = text;
			text_pageCount.text = currentPersent + "%";
		}

		private void DrawFPS()
		{
			if (!(ui_fps_root != null))
			{
				GameObject gameObject = (ui_fps_root = new GameObject("fps"));
				gameObject.transform.parent = transform;
				Canvas canvas = gameObject.AddComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				canvas.pixelPerfect = true;
				canvas.sortingOrder = 10000;
				gameObject.AddComponent<CanvasScaler>();
				GameObject gameObject2 = new GameObject("text");
				gameObject2.transform.parent = ui_fps_root.transform;
				text_fps = gameObject2.AddComponent<Text>();
				text_fps.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0f, 0f);
				text_fps.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
				text_fps.rectTransform.anchorMin = new Vector2(0f, 0f);
				text_fps.rectTransform.anchorMax = new Vector2(1f, 1f);
				text_fps.rectTransform.pivot = new Vector2(0f, 0f);
				text_fps.color = new Color(1f, 1f, 1f, 1f);
				text_fps.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
				text_fps.fontSize = 14;
				text_fps.alignment = TextAnchor.UpperRight;
				text_fps.text = "fps";
			}
		}

		private object CMDClose(string[] args)
		{
			IsOpen = false;
			return "closed";
		}

		private object CMDClear(string[] args)
		{
			ClearLog();
			return "clear";
		}

		private object CMDHelp(string[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Command List: ");
			stringBuilder.AppendLine("--------------------------");
			foreach (string key in _cmdTable.Keys)
			{
				stringBuilder.AppendLine(_cmdTableDiscribes[key] + "    [enter]> " + key);
			}
			stringBuilder.Append("--------------------------");
			return stringBuilder.ToString();
		}

		private object CMDSystemInfo(string[] args)
		{
			Log("Unity Ver: " + Application.unityVersion, MessageType.OUTPUT);
			Log("Platform: " + Application.platform, MessageType.OUTPUT);
			Log("Language: " + Application.systemLanguage, MessageType.OUTPUT);
			Log(string.Format("Level: {0} [{1}]", SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().buildIndex), MessageType.OUTPUT);
			Log("Data Path: " + Application.dataPath, MessageType.OUTPUT);
			Log("Persistent Path: " + Application.persistentDataPath, MessageType.OUTPUT);
			Log("deviceModel " + SystemInfo.deviceModel, MessageType.OUTPUT);
			Log("deviceName " + SystemInfo.deviceName, MessageType.OUTPUT);
			Log("deviceType " + SystemInfo.deviceType, MessageType.OUTPUT);
			Log("deviceUniqueIdentifier " + SystemInfo.deviceUniqueIdentifier, MessageType.OUTPUT);
			Log("graphicsDeviceID " + SystemInfo.graphicsDeviceID, MessageType.OUTPUT);
			Log("graphicsDeviceName " + SystemInfo.graphicsDeviceName, MessageType.OUTPUT);
			Log("graphicsDeviceVendor " + SystemInfo.graphicsDeviceVendor, MessageType.OUTPUT);
			Log("graphicsDeviceVendorID " + SystemInfo.graphicsDeviceVendorID, MessageType.OUTPUT);
			Log("graphicsDeviceVersion " + SystemInfo.graphicsDeviceVersion, MessageType.OUTPUT);
			Log("graphicsMemorySize " + SystemInfo.graphicsMemorySize, MessageType.OUTPUT);
			Log("graphicsShaderLevel " + SystemInfo.graphicsShaderLevel, MessageType.OUTPUT);
			Log("operatingSystem " + SystemInfo.operatingSystem, MessageType.OUTPUT);
			Log("processorCount " + SystemInfo.processorCount, MessageType.OUTPUT);
			Log("processorType " + SystemInfo.processorType, MessageType.OUTPUT);
			Log("supportedRenderTargetCount " + SystemInfo.supportedRenderTargetCount, MessageType.OUTPUT);
			Log("supports3DTextures " + SystemInfo.supports3DTextures, MessageType.OUTPUT);
			Log("supportsAccelerometer " + SystemInfo.supportsAccelerometer, MessageType.OUTPUT);
			Log("supportsComputeShaders " + SystemInfo.supportsComputeShaders, MessageType.OUTPUT);
			Log("supportsGyroscope " + SystemInfo.supportsGyroscope, MessageType.OUTPUT);
			Log("supportsImageEffects " + SystemInfo.supportsImageEffects, MessageType.OUTPUT);
			Log("supportsInstancing " + SystemInfo.supportsInstancing, MessageType.OUTPUT);
			Log("supportsLocationService " + SystemInfo.supportsLocationService, MessageType.OUTPUT);
			Log("supportsRenderTextures " + SystemInfo.supportsRenderTextures, MessageType.OUTPUT);
			Log("supportsShadows " + SystemInfo.supportsShadows, MessageType.OUTPUT);
			Log("supportsVibration " + SystemInfo.supportsVibration, MessageType.OUTPUT);
			Log("systemMemorySize " + SystemInfo.systemMemorySize, MessageType.OUTPUT);
			Log("CurrentResolution: " + Screen.currentResolution.width + " * " + Screen.currentResolution.height, MessageType.OUTPUT);
			Log("Dpi: " + Screen.dpi, MessageType.OUTPUT);
			Log("Profiler.enabled = : " + Profiler.enabled, MessageType.OUTPUT);
			GC.Collect();
			Log(string.Format("Total memory: {0:###,###,###,##0} kb", (float)GC.GetTotalMemory(true) / 1024f), MessageType.OUTPUT);
			return "end.";
		}

		private object CMDShowMessageType(string[] args)
		{
			needAutoToMax = true;
			int result = 0;
			if (args == null || args.Length == 1 || !int.TryParse(args[1], out result))
			{
				currentShowMessageType = -1;
				return "bad argument";
			}
			currentShowMessageType = result;
			return (result != -1) ? Enum.Parse(typeof(MessageType), result.ToString()).ToString() : "ALL";
		}

		private object CMDShowFPS(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "Type fps 1 or fps 0";
			}
			int result = 0;
			if (int.TryParse(args[1], out result))
			{
				if (result == 1 || result == 2)
				{
					fpsMode = result;
					DrawFPS();
				}
				else
				{
					fpsMode = 0;
					if (ui_fps_root != null)
					{
						UnityEngine.Object.Destroy(ui_fps_root.gameObject);
					}
				}
			}
			return string.Empty;
		}

		private object CMDCleanCache(string[] args)
		{
			Caching.CleanCache();
			return "Cash Clean";
		}

		private object CMDEnableStackTrace(string[] args)
		{
			if (args == null || args.Length == 1)
			{
				return "Type stack 1 or stack 0";
			}
			int result = 0;
			if (int.TryParse(args[1], out result))
			{
				if (result != 0)
				{
					enableStackTrace = true;
				}
				else
				{
					enableStackTrace = false;
				}
				PlayerPrefs.SetInt("enableStackTrace", result);
			}
			return string.Empty;
		}

		private object CMDShowMemory(string[] args)
		{
			double num = 1.0 / 1048576.0;
			List<Texture> list = new List<Texture>(Resources.FindObjectsOfTypeAll(typeof(Texture)) as Texture[]);
			Log("----------TEXTURE-----------", MessageType.OUTPUT);
			Log("Texture Count:" + list.Count, MessageType.OUTPUT);
			list.Sort((Texture a, Texture b) =>
			{
				int runtimeMemorySize = Profiler.GetRuntimeMemorySize(a);
				int runtimeMemorySize2 = Profiler.GetRuntimeMemorySize(b);
				if (runtimeMemorySize > runtimeMemorySize2)
				{
					return -1;
				}
				return (runtimeMemorySize < runtimeMemorySize2) ? 1 : 0;
			});
			for (int num2 = 0; num2 < list.Count && num2 < 100; num2++)
			{
				Log(list[num2].name + " : " + ((double)Profiler.GetRuntimeMemorySize(list[num2]) * num).ToString("#0.##") + "mb", MessageType.OUTPUT);
			}
			List<AudioClip> list2 = new List<AudioClip>(Resources.FindObjectsOfTypeAll(typeof(AudioClip)) as AudioClip[]);
			Log("----------Sound-----------", MessageType.OUTPUT);
			Log("Sound Count:" + list2.Count, MessageType.OUTPUT);
			list2.Sort((AudioClip a, AudioClip b) =>
			{
				int runtimeMemorySize = Profiler.GetRuntimeMemorySize(a);
				int runtimeMemorySize2 = Profiler.GetRuntimeMemorySize(b);
				if (runtimeMemorySize > runtimeMemorySize2)
				{
					return -1;
				}
				return (runtimeMemorySize < runtimeMemorySize2) ? 1 : 0;
			});
			for (int num3 = 0; num3 < list2.Count; num3++)
			{
				Log(list2[num3].name + " : " + ((double)Profiler.GetRuntimeMemorySize(list2[num3]) * num).ToString("#0.##") + "mb", MessageType.OUTPUT);
			}
			return "end.";
		}

		private object CMDStartAsyncLoging(string[] args)
		{
			if (!isAsyncWritingLog)
			{
				StartAsyncWriteLog();
				return "Start Async Log";
			}
			return "Stop Async Log";
		}

		private void LogMessage(Message msg)
		{
			if (IsOpen)
			{
				maxMsgCount = CaculateCurrentMaxMsgCount();
				if (needAutoToMax)
				{
					isNeedRefreshContent = true;
					endMsgIndex = maxMsgCount - 1;
					beginMsgIndex = FindBeginMsgIndexFromEnd();
				}
			}
			if (isAsyncWritingLog)
			{
				AddToAsyncWriteBuff(msg.ToGUIString());
			}
		}

		private void ClearLog()
		{
			_messages.Clear();
			customQueueMessages.Clear();
			isNeedRefreshContent = true;
		}

		private int CaculateCurrentMaxMsgCount()
		{
			return GetAllCurrentTypeMessages().Count;
		}

		private void RegisterCommandCallback(string commandString, Func<string[], object> commandCallback, string CMD_Discribes)
		{
			try
			{
				if (!_cmdTable.ContainsKey(commandString))
				{
					_cmdTable[commandString] = commandCallback;
					_cmdTableDiscribes.Add(commandString, CMD_Discribes);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		}

		private void UnRegisterCommandCallback(string commandString)
		{
			try
			{
				_cmdTable.Remove(commandString);
				_cmdTableDiscribes.Remove(commandString);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		}

		private void EvalInputString(string inputString)
		{
			string[] array = inputString.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				LogMessage(Message.Input(string.Empty));
				return;
			}
			LogMessage(Message.Input(inputString));
			string key = array[0];
			if (_cmdTable.ContainsKey(key))
			{
				LogMessage(Message.Output(_cmdTable[key](array)));
				_history.Add(inputString);
			}
		}

		public void ExecCMDInputString(string inputCMD)
		{
			EvalInputString(inputCMD);
		}

		internal static object Log(object message, string customType)
		{
			if (Instance == null)
			{
				return null;
			}
			Instance.LogMessage(Message.Log(message, customType));
			return message;
		}

		internal static object Log(object message, string customType, Color col)
		{
			if (Instance == null)
			{
				return null;
			}
			Instance.LogMessage(Message.Log(message, customType, col));
			return message;
		}

		internal static object Log(object message, MessageType messageType)
		{
			if (Instance == null)
			{
				return null;
			}
			Instance.LogMessage(Message.Log(message, messageType));
			return message;
		}

		internal static object LogWarning(object message, string customType)
		{
			if (Instance == null)
			{
				return null;
			}
			Instance.LogMessage(Message.Warning(message, customType));
			return message;
		}

		internal static object LogError(object message, string customType)
		{
			if (Instance == null)
			{
				return null;
			}
			Instance.LogMessage(Message.Error(message, customType));
			return message;
		}

		public static void Clear()
		{
			if (Instance != null)
			{
				Instance.ClearLog();
			}
		}

		public static void RegisterCommand(string commandString, Func<string[], object> commandCallback, string CMD_Discribes)
		{
			if (Instance != null)
			{
				Instance.RegisterCommandCallback(commandString, commandCallback, CMD_Discribes);
			}
		}

		public static void UnRegisterCommand(string commandString)
		{
			if (Instance != null)
			{
				Instance.UnRegisterCommandCallback(commandString);
			}
		}

		private void ProcessInput(string cmd)
		{
			if (Application.isMobilePlatform || Input.GetButtonDown("Submit"))
			{
				EvalInputString(cmd);
				maxMsgCount = CaculateCurrentMaxMsgCount();
				if (needAutoToMax)
				{
					endMsgIndex = maxMsgCount - 1;
					beginMsgIndex = FindBeginMsgIndexFromEnd();
					isNeedRefreshContent = true;
				}
			}
			input.text = string.Empty;
		}

		private void AddPage(int val)
		{
			if (val == 1)
			{
				if (endMsgIndex < maxMsgCount - 1)
				{
					beginMsgIndex = endMsgIndex;
					endMsgIndex = FindEndMsgIndexFromBegin();
					if (endMsgIndex == maxMsgCount - 1)
					{
						needAutoToMax = true;
						beginMsgIndex = FindBeginMsgIndexFromEnd();
					}
					else
					{
						needAutoToMax = false;
					}
				}
			}
			else if (beginMsgIndex > 0)
			{
				endMsgIndex = beginMsgIndex;
				beginMsgIndex = FindBeginMsgIndexFromEnd();
				if (beginMsgIndex == 0)
				{
					endMsgIndex = FindEndMsgIndexFromBegin();
				}
				needAutoToMax = false;
			}
			isNeedRefreshContent = true;
		}

		private List<Message> GetAllCurrentTypeMessages()
		{
			List<Message> list = new List<Message>();
			Message[] array;
			lock (_lock)
			{
				array = FindCustomQueue(currentShowMessageCustom).ToArray();
			}
			for (int i = 0; i < array.Length; i++)
			{
				Message item = array[i];
				if (currentShowMessageType == -1 || item.type == (MessageType)currentShowMessageType)
				{
					if (string.IsNullOrEmpty(currentShowMessageCustom) && string.IsNullOrEmpty(item.customType))
					{
						list.Add(item);
					}
					else if (!string.IsNullOrEmpty(currentShowMessageCustom) && currentShowMessageCustom.Equals(item.customType, StringComparison.OrdinalIgnoreCase))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		private int FindBeginMsgIndexFromEnd()
		{
			List<Message> allCurrentTypeMessages = GetAllCurrentTypeMessages();
			string text = string.Empty;
			for (int num = endMsgIndex; num >= 0; num--)
			{
				text += allCurrentTypeMessages[num].ToGUIString();
				if (text.Length >= 10000)
				{
					return num;
				}
			}
			return 0;
		}

		private int FindEndMsgIndexFromBegin()
		{
			List<Message> allCurrentTypeMessages = GetAllCurrentTypeMessages();
			string text = string.Empty;
			for (int i = beginMsgIndex; i < allCurrentTypeMessages.Count; i++)
			{
				text += allCurrentTypeMessages[i].ToGUIString();
				if (text.Length >= 10000)
				{
					return i;
				}
			}
			return allCurrentTypeMessages.Count - 1;
		}

		public void StartAsyncWriteLog()
		{
			CreateNewLogFile();
			logThread = new Thread(RunningAsyncWritingLog);
			logThread.Priority = System.Threading.ThreadPriority.Lowest;
			logThread.Name = "LogThread";
			logThread.Start(logThread);
			isAsyncWritingLog = true;
		}

		public void StopAsyncWriteLog()
		{
			if (streamWrite != null)
			{
				streamWrite.Flush();
				streamWrite.Close();
				streamWrite = null;
			}
			if (logThread != null)
			{
				logThread.Abort();
				logThread = null;
			}
			LogQueue.Clear();
			isAsyncWritingLog = false;
		}

		public void CreateNewLogFile()
		{
			try
			{
				if (streamWrite != null)
				{
					streamWrite.Flush();
					streamWrite.Close();
				}
				streamWrite = new StreamWriter(logPath, true);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("CreateNewLogFile exception:" + ex.Message);
			}
		}

		private void AddToAsyncWriteBuff(string text)
		{
			lock (thisLock)
			{
				LogQueue.Enqueue(text);
			}
		}

		private void RunningAsyncWritingLog()
		{
			while (true)
			{
				if (!string.IsNullOrEmpty(logBuff))
				{
					streamWrite.Write(_logBuff);
					streamWrite.Flush();
					_logBuff = string.Empty;
				}
				Thread.Sleep(150);
			}
		}
	}
}
