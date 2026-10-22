using SA.Common.Pattern;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SA_BackButton : DefaultPreviewButton
{
	public static string firstLevel = string.Empty;

	public string LevelName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	private void Start()
	{
		if (firstLevel != string.Empty)
		{
			Object.Destroy(gameObject);
			return;
		}
		Object.DontDestroyOnLoad(gameObject);
		if (firstLevel == string.Empty)
		{
			firstLevel = LevelName;
		}
	}

	private void FixedUpdate()
	{
		if (LevelName.Equals(firstLevel))
		{
			GetComponent<Renderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
		}
		else
		{
			GetComponent<Renderer>().enabled = true;
			GetComponent<Collider>().enabled = true;
		}
	}

	protected override void OnClick()
	{
		base.OnClick();
		GoBack();
	}

	private void GoBack()
	{
		Singleton<SALevelLoader>.Instance.LoadLevel(firstLevel);
	}
}
