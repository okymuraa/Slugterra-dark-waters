using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

public class ColorPerPlayer : PunBehaviour
{
	public Color[] Colors = new Color[4]
	{
		Color.red,
		Color.blue,
		Color.yellow,
		Color.green
	};

	public const string ColorProp = "pc";

	public bool ShowColorLabel;

	public Rect ColorLabelArea = new Rect(0f, 50f, 100f, 200f);

	public Texture2D img;

	public Color MyColor = Color.grey;

	private bool afterOnJoinedRoom;

	public bool ColorPicked { get; set; }

	public override void OnJoinedRoom()
	{
		afterOnJoinedRoom = true;
		SelectColor();
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		SelectColor();
	}

	public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		if (afterOnJoinedRoom)
		{
			PhotonPlayer photonPlayer = playerAndUpdatedProps[0] as PhotonPlayer;
			if (photonPlayer == null || !photonPlayer.isLocal)
			{
				SelectColor();
			}
		}
	}

	public override void OnLeftRoom()
	{
		Reset();
	}

	public void Reset()
	{
		MyColor = Color.grey;
		ColorPicked = false;
		afterOnJoinedRoom = false;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("pc", null);
		PhotonNetwork.player.SetCustomProperties(hashtable);
	}

	private void OnGUI()
	{
		if (ColorPicked && ShowColorLabel)
		{
			GUILayout.BeginArea(ColorLabelArea);
			GUILayout.BeginHorizontal();
			Color color = GUI.color;
			GUI.color = MyColor;
			GUILayout.Label(img);
			GUI.color = color;
			string text = ((!PhotonNetwork.isMasterClient) ? "is your color" : "is your color\nyou are the Master Client");
			GUILayout.Label(text);
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}

	public void SelectColor()
	{
		if (ColorPicked)
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		PhotonPlayer[] otherPlayers = PhotonNetwork.otherPlayers;
		foreach (PhotonPlayer photonPlayer in otherPlayers)
		{
			if (photonPlayer.customProperties.ContainsKey("pc"))
			{
				int item = (int)photonPlayer.customProperties["pc"];
				hashSet.Add(item);
			}
			else if (photonPlayer.ID < PhotonNetwork.player.ID)
			{
				return;
			}
		}
		if (hashSet.Count == Colors.Length)
		{
			Debug.LogWarning("No color available! All picked. Colors length should match MaxPlayers of the room.");
			return;
		}
		for (int j = 0; j < Colors.Length; j++)
		{
			if (!hashSet.Contains(j))
			{
				Color myColor = Colors[j];
				MyColor = myColor;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("pc", j);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				ColorPicked = true;
				break;
			}
		}
	}
}
