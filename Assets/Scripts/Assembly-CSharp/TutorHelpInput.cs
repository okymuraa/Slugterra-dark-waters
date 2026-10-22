using System;
using UnityEngine;

public class TutorHelpInput : MonoBehaviour
{
	public enum TypeInput
	{
		touch = 0,
		appleTV_pult = 1,
		appleTV_gamepud = 2
	}

	[Serializable]
	public class TouchHelp
	{
		[Serializable]
		public class helpCell
		{
			public string key;

			public GameObject Object;

			[HideInInspector]
			public int DefaultLayer;

			[HideInInspector]
			public Vector3 DefaultScale;

			[HideInInspector]
			public Transform[] Children;

			public bool CanMove = true;
		}

		public helpCell[] Helps;

		public void OnStart()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					Helps[i].Children = Helps[i].Object.GetComponentsInChildren<Transform>();
					Helps[i].DefaultLayer = Helps[i].Object.layer;
					Helps[i].DefaultScale = Helps[i].Object.transform.localScale;
				}
			}
		}

		public bool Show(string helpkey, Transform point, Vector3 offset)
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].key == helpkey && Helps[i].Object != null)
				{
					if (point != null && Helps[i].CanMove)
					{
						Helps[i].Object.transform.position = point.position + offset;
					}
					Helps[i].Object.SetActive(true);
					return true;
				}
			}
			return false;
		}

		public void Hide()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					Helps[i].Object.SetActive(false);
				}
			}
		}

		public void Hide(string helpkey, Transform hidePoint = null)
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].key == helpkey && Helps[i].Object != null)
				{
					Helps[i].Object.SetActive(false);
					if (hidePoint != null && Helps[i].CanMove)
					{
						Helps[i].Object.transform.position = hidePoint.position;
					}
					break;
				}
			}
		}

		public void Delete(string helpkey)
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].key == helpkey)
				{
					Helps[i].key = "null";
					UnityEngine.Object.Destroy(Helps[i].Object);
				}
			}
		}

		public void HideLayer()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					for (int j = 0; j < Helps[i].Children.Length; j++)
					{
						Helps[i].Children[j].gameObject.layer = LayerMask.NameToLayer("UI_CUT_Text");
					}
					Helps[i].Object.layer = LayerMask.NameToLayer("UI_CUT_Text");
				}
			}
		}

		public void ShowLayer()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					for (int j = 0; j < Helps[i].Children.Length; j++)
					{
						Helps[i].Children[j].gameObject.layer = Helps[i].DefaultLayer;
					}
					Helps[i].Object.layer = Helps[i].DefaultLayer;
				}
			}
		}

		public void HideScaleLayer()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					Helps[i].Object.transform.localScale = Vector3.zero;
				}
			}
		}

		public void ShowScaleLayer()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					Helps[i].Object.transform.localScale = Helps[i].DefaultScale;
				}
			}
		}
	}

	[Serializable]
	public class AppleTV_PultHelp
	{
		[Serializable]
		public class helpCell
		{
			public string key;

			public GameObject Object;

			[HideInInspector]
			public int DefaultLayer;

			[HideInInspector]
			public Transform[] Children;

			public bool CanMove = true;
		}

		public helpCell[] Helps;

		public void OnStart()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					Helps[i].Children = Helps[i].Object.GetComponentsInChildren<Transform>();
					Helps[i].DefaultLayer = Helps[i].Object.layer;
				}
			}
		}

		public bool Show(string helpkey, Transform point, Vector3 offset)
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].key == helpkey && Helps[i].Object != null)
				{
					if (point != null && Helps[i].CanMove)
					{
						Helps[i].Object.transform.position = point.position + offset;
					}
					Helps[i].Object.SetActive(true);
					return true;
				}
			}
			return false;
		}

		public void Hide()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					Helps[i].Object.SetActive(false);
				}
			}
		}

		public void Hide(string helpkey, Transform hidePoint = null)
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].key == helpkey && Helps[i].Object != null)
				{
					Helps[i].Object.SetActive(false);
					if (hidePoint != null && Helps[i].CanMove)
					{
						Helps[i].Object.transform.position = hidePoint.position;
					}
					break;
				}
			}
		}

		public void Delete(string helpkey)
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].key == helpkey)
				{
					Helps[i].key = "null";
					UnityEngine.Object.Destroy(Helps[i].Object);
				}
			}
		}

		public void HideLayer()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					for (int j = 0; j < Helps[i].Children.Length; j++)
					{
						Helps[i].Children[j].gameObject.layer = LayerMask.NameToLayer("UI_CUT_Text");
					}
					Helps[i].Object.layer = LayerMask.NameToLayer("UI_CUT_Text");
				}
			}
		}

		public void ShowLayer()
		{
			for (int i = 0; i < Helps.Length; i++)
			{
				if (Helps[i].Object != null)
				{
					for (int j = 0; j < Helps[i].Children.Length; j++)
					{
						Helps[i].Children[j].gameObject.layer = Helps[i].DefaultLayer;
					}
					Helps[i].Object.layer = Helps[i].DefaultLayer;
				}
			}
		}
	}

	private int oldType;

	public TypeInput curInput;

	public string OldHelpkey;

	private Transform OldPoint;

	private Vector3 OldOffset;

	private bool hide;

	private bool scale;

	public TouchHelp touchHelp;

	public AppleTV_PultHelp appleTV_pultHelp;

	private void Awake()
	{
		oldType = curInput.GetHashCode();
		touchHelp.OnStart();
		appleTV_pultHelp.OnStart();
		HideHelp();
	}

	public void ShowHelp(string helpkey, Transform point, Vector3 offset)
	{
		if (curInput == TypeInput.touch && touchHelp.Show(helpkey, point, offset))
		{
			OldHelpkey = helpkey;
			OldPoint = point;
			OldOffset = offset;
		}
		if (curInput == TypeInput.appleTV_pult && appleTV_pultHelp.Show(helpkey, point, offset))
		{
			OldHelpkey = helpkey;
			OldPoint = point;
			OldOffset = offset;
		}
	}

	private void HideHelp()
	{
		touchHelp.Hide();
		appleTV_pultHelp.Hide();
	}

	public void HideLayer()
	{
		if (!hide)
		{
			touchHelp.HideLayer();
			appleTV_pultHelp.HideLayer();
			hide = true;
		}
	}

	public void ShowLayer()
	{
		if (hide)
		{
			touchHelp.ShowLayer();
			appleTV_pultHelp.ShowLayer();
			hide = false;
		}
	}

	public void HideScaleLayer()
	{
		if (!scale)
		{
			touchHelp.HideScaleLayer();
			scale = true;
		}
	}

	public void ShowScaleLayer()
	{
		if (scale)
		{
			touchHelp.ShowScaleLayer();
			scale = false;
		}
	}

	public void DisableHelp()
	{
		OldHelpkey = "null";
		touchHelp.Hide();
		appleTV_pultHelp.Hide();
	}

	public void DisableHelp(string helpkey, Transform hidePoint = null)
	{
		OldHelpkey = "null";
		if (hidePoint == null)
		{
			touchHelp.Hide(helpkey);
			appleTV_pultHelp.Hide(helpkey);
		}
		else
		{
			touchHelp.Hide(helpkey, hidePoint);
			appleTV_pultHelp.Hide(helpkey, hidePoint);
		}
	}

	public void DelHelp(string helpkey)
	{
		OldHelpkey = "null";
		touchHelp.Delete(helpkey);
		appleTV_pultHelp.Delete(helpkey);
	}
}
