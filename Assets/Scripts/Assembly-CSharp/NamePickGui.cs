using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatGui))]
public class NamePickGui : MonoBehaviour
{
	private const string UserNamePlayerPref = "NamePickUserName";

	public ChatGui chatNewComponent;

	public RectTransform AppIdInputPanel;

	public RectTransform NameInputPanel;

	public InputField idInput;

	public void Start()
	{
		chatNewComponent = Object.FindObjectOfType<ChatGui>();
		bool flag = false;
		AppIdInputPanel.gameObject.SetActive(flag);
		NameInputPanel.gameObject.SetActive(!flag);
		string text = PlayerPrefs.GetString("NamePickUserName");
		if (!string.IsNullOrEmpty(text))
		{
			idInput.text = text;
		}
	}

	public void OpenDashboard()
	{
		Application.OpenURL("https://www.exitgames.com/en/Chat/Dashboard");
	}

	public void EndEditOnEnter()
	{
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
			StartChat();
		}
	}

	public void StartChat()
	{
		ChatGui chatGui = Object.FindObjectOfType<ChatGui>();
		chatGui.UserName = idInput.text.Trim();
		chatGui.enabled = true;
		enabled = false;
		AppIdInputPanel.gameObject.SetActive(false);
		NameInputPanel.gameObject.SetActive(false);
		PlayerPrefs.SetString("NamePickUserName", chatGui.UserName);
	}
}
