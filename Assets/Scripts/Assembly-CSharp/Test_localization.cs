using AppsMinistry.Core;
using Slugterra.Scene;
using UnityEngine;

public class Test_localization : SlugterraCutScene
{
	public enum Locale
	{
		Russian = 0,
		Italian = 1,
		German = 2,
		French = 3,
		Checz = 4,
		Portuguese_brazil = 5,
		English = 6,
		Spanish = 7,
		Turkish = 8,
		Dutch = 9,
		Norwegian = 10
	}

	public Locale locale;

	public int cur_index;

	public TextMesh[] _key;

	public TextMesh[] all;

	public string[] tempall;

	public string[] Key_Localization;

	private int oldlocaleindex;

	private void Start()
	{
		_cutSceneView.ShowScreens(true);
		_cutSceneView.SpeakerIcon = "icon_kord";
		all = Object.FindObjectsOfType<TextMesh>();
		tempall = new string[all.Length];
		for (int i = 0; i < all.Length; i++)
		{
			tempall[i] = all[i].text;
		}
		UpdateLocale();
	}

	private void UpdateLocale()
	{
		for (int i = 0; i < all.Length; i++)
		{
			for (int j = 0; j < Key_Localization.Length; j++)
			{
				if (tempall[i] == Key_Localization[j])
				{
					all[i].text = MonoSingleton<Localization>.Instance[Key_Localization[j]];
				}
			}
		}
	}

	private void Update()
	{
		if (oldlocaleindex != locale.GetHashCode())
		{
			oldlocaleindex = locale.GetHashCode();
			UpdateLocale();
		}
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			cur_index++;
			if (cur_index >= Key_Localization.Length)
			{
				cur_index--;
			}
			while (Key_Localization[cur_index] == string.Empty)
			{
				cur_index++;
			}
		}
		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			cur_index--;
			if (cur_index < 0)
			{
				cur_index++;
			}
			while (Key_Localization[cur_index] == string.Empty)
			{
				cur_index--;
			}
		}
		_cutSceneView.DialogueText = MonoSingleton<Localization>.Instance[Key_Localization[cur_index]];
		_key[0].text = Key_Localization[cur_index];
		for (int i = 1; i < _key.Length; i++)
		{
			_key[i].text = MonoSingleton<Localization>.Instance[Key_Localization[cur_index]];
		}
	}
}
