using AppsMinistry.Core;
using Slugterra.App;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioBackgroundMusicManager : MonoBehaviour
{
	public LevelOptions[] levelBgMusicOptions;

	private AudioSource aSource;

	private string previousLevelLoaded;

	private AudioClip aClip;

	private void Awake()
	{
		if (transform.parent == null)
		{
			Object.DontDestroyOnLoad(gameObject);
		}
		aSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!(previousLevelLoaded != SceneManager.GetActiveScene().name))
		{
			return;
		}
		for (int i = 0; i < levelBgMusicOptions.Length; i++)
		{
			string levelName = levelBgMusicOptions[i].levelName;
			string text = SceneManager.GetActiveScene().name;
			if (levelName == text || levelName + "_HD" == text)
			{
				Accept(i);
			}
		}
		previousLevelLoaded = SceneManager.GetActiveScene().name;
	}

	public void Accept(int _levelNumber)
	{
		Debug.LogWarning("AudioBackgroundMusicManager::Accept Level Name: " + SceneManager.GetActiveScene().name);
		MonoSingleton<AppController>.Instance.Model.GameSetting.CommitAndNotify();
		if (levelBgMusicOptions[_levelNumber].bgmusic != string.Empty)
		{
			if (levelBgMusicOptions[_levelNumber]._music == LevelOptions.Music._internal && aSource.clip.name != levelBgMusicOptions[_levelNumber].bgmusic)
			{
				aSource.clip = Resources.Load("Sounds/music/" + levelBgMusicOptions[_levelNumber].bgmusic) as AudioClip;
				aSource.PlayDelayed(levelBgMusicOptions[_levelNumber].delay);
			}
			if (aSource.clip.name == levelBgMusicOptions[_levelNumber].bgmusic && !aSource.isPlaying)
			{
				aSource.Play();
			}
		}
		if (levelBgMusicOptions[_levelNumber]._music == LevelOptions.Music._stop)
		{
			aSource.Stop();
		}
	}
}
