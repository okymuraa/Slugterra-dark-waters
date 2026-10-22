using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using UnityEngine;
using UnityEngine.UI;

public class ChatGui : MonoBehaviour, IChatClientListener
{
	public string ChatAppId;

	public string[] ChannelsToJoinOnConnect;

	public int HistoryLengthToFetch;

	private string selectedChannelName;

	public ChatClient chatClient;

	public RectTransform ChatPanel;

	public InputField InputFieldChat;

	public Text CurrentChannelText;

	public Toggle ChannelToggleToInstantiate;

	private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

	public bool ShowState = true;

	public Text StateText;

	private string userIdInput = string.Empty;

	private static string WelcomeText = "Welcome to chat. Type \\help to list commands.";

	private static string HelpText = "\n\\subscribe <list of channelnames> or \\s <list of channelnames> subscribes channels.\n\\unsubscribe <list of channelnames> or \\u <list of channelnames> leaves channels.\n\\join <channelname> or \\j <channelname> switches the active channel.\n\\msg <username> <message> send private message to user.\n\\clear clears the current chat tab. private chats get closed.\n\\help gets this help message.";

	public int TestLength = 2048;

	private byte[] testBytes = new byte[2048];

	public string UserName { get; set; }

	public void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		ChatPanel.gameObject.SetActive(true);
		Application.runInBackground = true;
		if (string.IsNullOrEmpty(ChatAppId))
		{
			Debug.LogError("You need to set the chat app ID in the inspector in order to continue.");
			return;
		}
		if (string.IsNullOrEmpty(UserName))
		{
			UserName = "user" + Environment.TickCount % 99;
		}
		chatClient = new ChatClient(this);
		chatClient.Connect(ChatAppId, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(UserName));
		ChannelToggleToInstantiate.gameObject.SetActive(false);
		Debug.Log("Connecting as: " + UserName);
	}

	public void OnApplicationQuit()
	{
		if (chatClient != null)
		{
			chatClient.Disconnect();
		}
	}

	public void Update()
	{
		if (chatClient != null)
		{
			chatClient.Service();
		}
		if (StateText == null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			StateText.gameObject.SetActive(ShowState);
		}
	}

	public void OnEnterSend()
	{
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
			SendChatMessage(InputFieldChat.text);
			InputFieldChat.text = string.Empty;
		}
	}

	public void OnClickSend()
	{
		if (InputFieldChat != null)
		{
			SendChatMessage(InputFieldChat.text);
			InputFieldChat.text = string.Empty;
		}
	}

	private void SendChatMessage(string inputLine)
	{
		if (string.IsNullOrEmpty(inputLine))
		{
			return;
		}
		if ("test".Equals(inputLine))
		{
			if (TestLength != testBytes.Length)
			{
				testBytes = new byte[TestLength];
			}
			chatClient.SendPrivateMessage(chatClient.AuthValues.UserId, testBytes, true);
		}
		bool flag = chatClient.PrivateChannels.ContainsKey(selectedChannelName);
		string target = string.Empty;
		if (flag)
		{
			string[] array = selectedChannelName.Split(':');
			target = array[1];
		}
		if (inputLine[0].Equals('\\'))
		{
			string[] array2 = inputLine.Split(new char[1] { ' ' }, 2);
			if (array2[0].Equals("\\help"))
			{
				PostHelpToCurrentChannel();
			}
			if (array2[0].Equals("\\state"))
			{
				int num = int.Parse(array2[1]);
				chatClient.SetOnlineStatus(num, new string[1] { "i am state " + num });
			}
			else if ((array2[0].Equals("\\subscribe") || array2[0].Equals("\\s")) && !string.IsNullOrEmpty(array2[1]))
			{
				chatClient.Subscribe(array2[1].Split(' ', ','));
			}
			else if ((array2[0].Equals("\\unsubscribe") || array2[0].Equals("\\u")) && !string.IsNullOrEmpty(array2[1]))
			{
				chatClient.Unsubscribe(array2[1].Split(' ', ','));
			}
			else if (array2[0].Equals("\\clear"))
			{
				ChatChannel channel;
				if (flag)
				{
					chatClient.PrivateChannels.Remove(selectedChannelName);
				}
				else if (chatClient.TryGetChannel(selectedChannelName, flag, out channel))
				{
					channel.ClearMessages();
				}
			}
			else if (array2[0].Equals("\\msg") && !string.IsNullOrEmpty(array2[1]))
			{
				string[] array3 = array2[1].Split(new char[2] { ' ', ',' }, 2);
				if (array3.Length >= 2)
				{
					string target2 = array3[0];
					string message = array3[1];
					chatClient.SendPrivateMessage(target2, message);
				}
			}
			else if ((array2[0].Equals("\\join") || array2[0].Equals("\\j")) && !string.IsNullOrEmpty(array2[1]))
			{
				string[] array4 = array2[1].Split(new char[2] { ' ', ',' }, 2);
				if (channelToggles.ContainsKey(array4[0]))
				{
					ShowChannel(array4[0]);
					return;
				}
				chatClient.Subscribe(new string[1] { array4[0] });
			}
			else
			{
				Debug.Log("The command '" + array2[0] + "' is invalid.");
			}
		}
		else if (flag)
		{
			chatClient.SendPrivateMessage(target, inputLine);
		}
		else
		{
			chatClient.PublishMessage(selectedChannelName, inputLine);
		}
	}

	private void PostHelpToCurrentChannel()
	{
		CurrentChannelText.text += HelpText;
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		switch (level)
		{
		case DebugLevel.ERROR:
			Debug.LogError(message);
			break;
		case DebugLevel.WARNING:
			Debug.LogWarning(message);
			break;
		default:
			Debug.Log(message);
			break;
		}
	}

	public void OnConnected()
	{
		if (ChannelsToJoinOnConnect != null && ChannelsToJoinOnConnect.Length > 0)
		{
			chatClient.Subscribe(ChannelsToJoinOnConnect, HistoryLengthToFetch);
		}
		chatClient.AddFriends(new string[2] { "tobi", "ilya" });
		chatClient.SetOnlineStatus(2);
	}

	public void OnDisconnected()
	{
	}

	public void OnChatStateChange(ChatState state)
	{
		StateText.text = state.ToString();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		foreach (string channelName in channels)
		{
			chatClient.PublishMessage(channelName, "says 'hi'.");
			if (ChannelToggleToInstantiate != null)
			{
				InstantiateChannelButton(channelName);
			}
		}
		Debug.Log("OnSubscribed: " + string.Join(", ", channels));
		ShowChannel(channels[0]);
	}

	private void InstantiateChannelButton(string channelName)
	{
		if (channelToggles.ContainsKey(channelName))
		{
			Debug.Log("Skipping creation for an existing channel toggle.");
			return;
		}
		Toggle toggle = UnityEngine.Object.Instantiate(ChannelToggleToInstantiate);
		toggle.gameObject.SetActive(true);
		toggle.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
		toggle.transform.SetParent(ChannelToggleToInstantiate.transform.parent, false);
		channelToggles.Add(channelName, toggle);
	}

	public void OnUnsubscribed(string[] channels)
	{
		foreach (string text in channels)
		{
			if (channelToggles.ContainsKey(text))
			{
				Toggle toggle = channelToggles[text];
				UnityEngine.Object.Destroy(toggle.gameObject);
				channelToggles.Remove(text);
				Debug.Log("Unsubscribed from channel '" + text + "'.");
				if (text == selectedChannelName && channelToggles.Count > 0)
				{
					IEnumerator<KeyValuePair<string, Toggle>> enumerator = channelToggles.GetEnumerator();
					enumerator.MoveNext();
					ShowChannel(enumerator.Current.Key);
					enumerator.Current.Value.isOn = true;
				}
			}
			else
			{
				Debug.Log("Can't unsubscribe from channel '" + text + "' because you are currently not subscribed to it.");
			}
		}
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		if (channelName.Equals(selectedChannelName))
		{
			ShowChannel(selectedChannelName);
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		InstantiateChannelButton(channelName);
		byte[] array = message as byte[];
		if (array != null)
		{
			Debug.Log("Message with byte[].Length: " + array.Length);
		}
		if (selectedChannelName.Equals(channelName))
		{
			ShowChannel(channelName);
		}
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));
	}

	public void AddMessageToSelectedChannel(string msg)
	{
		ChatChannel channel = null;
		if (!chatClient.TryGetChannel(selectedChannelName, out channel))
		{
			Debug.Log("AddMessageToSelectedChannel failed to find channel: " + selectedChannelName);
		}
		else if (channel != null)
		{
			channel.Add("Bot", msg);
		}
	}

	public void ShowChannel(string channelName)
	{
		if (string.IsNullOrEmpty(channelName))
		{
			return;
		}
		ChatChannel channel = null;
		if (!chatClient.TryGetChannel(channelName, out channel))
		{
			Debug.Log("ShowChannel failed to find channel: " + channelName);
			return;
		}
		selectedChannelName = channelName;
		CurrentChannelText.text = channel.ToStringMessages();
		Debug.Log("ShowChannel: " + selectedChannelName);
		foreach (KeyValuePair<string, Toggle> channelToggle in channelToggles)
		{
			channelToggle.Value.isOn = ((channelToggle.Key == channelName) ? true : false);
		}
	}
}
