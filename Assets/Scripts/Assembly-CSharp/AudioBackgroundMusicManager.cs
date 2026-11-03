using System;
using AppsMinistry.Core;
using Slugterra.App;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioBackgroundMusicManager : MonoBehaviour
{
	private static AudioBackgroundMusicManager _instance;

	public static AudioBackgroundMusicManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<AudioBackgroundMusicManager>();
				if (_instance == null && Application.isPlaying)
				{
					GameObject go = new GameObject("BackgroundMusicManager");
					_instance = go.AddComponent<AudioBackgroundMusicManager>();
					UnityEngine.Object.DontDestroyOnLoad(go);
				}
			}
			return _instance;
		}
		private set
		{
			_instance = value;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void AutoInit()
	{
		if (Application.isPlaying && Instance != null)
		{
			Instance.CheckSceneMusic(SceneManager.GetActiveScene().name);
		}
	}

	public LevelOptions[] levelBgMusicOptions;

	private AudioSource aSource;

	private string previousLevelLoaded;

	private AudioClip aClip;

	private static readonly LevelOptions[] DefaultOptions = new LevelOptions[]
	{
		new LevelOptions { levelName = "MainScene", bgmusic = "boss_2", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "MapScene", bgmusic = "Rockin", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "ShopScene", bgmusic = "", delay = 1f, _music = LevelOptions.Music._external },
		new LevelOptions { levelName = "SettingsScene", bgmusic = "", delay = 1f, _music = LevelOptions.Music._external },
		new LevelOptions { levelName = "BackpackScene", bgmusic = "", delay = 1f, _music = LevelOptions.Music._external },
		new LevelOptions { levelName = "SurvivalMapScene", bgmusic = "Rockin", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "MultiplayerMapScene", bgmusic = "Rockin", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "Multiplayer_Map_01", bgmusic = "DubMetal", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "Multiplayer_Tutorial", bgmusic = "battle_5", delay = 2.5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_1_ShaneTutor", bgmusic = "battle_0", delay = 2.5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_1_ScorchedSparksTutotial", bgmusic = "battle_0", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_1_ScorchedSparks", bgmusic = "battle_14_GE", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_2_ScorchedSparks", bgmusic = "battle_7", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_3_ScorchedSparks", bgmusic = "battle_8", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_4_ScorchedSparks", bgmusic = "battle_2", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_5_Arena", bgmusic = "battle_14_GE", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "2_1_QuietLawn", bgmusic = "battle_6", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "2_2_QuietLawn", bgmusic = "boss_1", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "2_3_QuietLawn", bgmusic = "battle_12_GE", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "2_4_QuietLawn", bgmusic = "battle_4", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "2_5_QuietLawn", bgmusic = "battle_10_GE", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "3_1_DarkPeriphery", bgmusic = "battle_3", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "3_2_DarkPeriphery", bgmusic = "battle_5", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "3_3_DarkPeriphery", bgmusic = "battle_0", delay = 6f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "4_1_Deadweed", bgmusic = "shadow_dub", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "4_2_Deadweed", bgmusic = "battle_11_GE", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "4_3_Deadweed", bgmusic = "battle_5", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "5_1_GhoulDep", bgmusic = "battle_11_GE", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "5_2_GhoulDep", bgmusic = "battle_7", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "6_1_Train", bgmusic = "awesome", delay = 1f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "6_2_Train", bgmusic = "battle_1", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "6_3_Train", bgmusic = "battle_9", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_DailyMission_01", bgmusic = "battle_11_GE", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_DailyMission_02", bgmusic = "awesome", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_DailyMission_03", bgmusic = "battle_4", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_DailyMission_04", bgmusic = "battle_3", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_DesertBase_1", bgmusic = "battle_10_GE", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "0_DesertBase_2", bgmusic = "battle_2", delay = 5f, _music = LevelOptions.Music._internal },
		new LevelOptions { levelName = "1_East_Survival_1", bgmusic = "battle_6", delay = 2f, _music = LevelOptions.Music._internal }
	};

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		Instance = this;

		if (transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		InitAudioSource();
		EnsureOptionsInitialized();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Start()
	{
		CheckSceneMusic(SceneManager.GetActiveScene().name);
	}

	private void InitAudioSource()
	{
		if (aSource == null)
		{
			aSource = GetComponent<AudioSource>();
		}
		if (aSource == null)
		{
			aSource = gameObject.AddComponent<AudioSource>();
		}

		aSource.loop = true;
		aSource.playOnAwake = false;

		if (aSource.outputAudioMixerGroup == null)
		{
			AudioMixer audioMixer = Resources.Load("sounds/mixers/MasterMixer") as AudioMixer ?? Resources.Load("Sounds/Mixers/MasterMixer") as AudioMixer;
			if (audioMixer != null)
			{
				AudioMixerGroup[] matchingGroups = audioMixer.FindMatchingGroups("Music");
				if (matchingGroups != null && matchingGroups.Length > 0)
				{
					aSource.outputAudioMixerGroup = matchingGroups[0];
				}
			}
		}

		if (aSource.volume <= 0.01f)
		{
			aSource.volume = 1f;
		}
	}

	private void EnsureOptionsInitialized()
	{
		if (levelBgMusicOptions == null || levelBgMusicOptions.Length == 0)
		{
			levelBgMusicOptions = DefaultOptions;
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		CheckSceneMusic(scene.name);
	}

	private void Update()
	{
		string activeSceneName = SceneManager.GetActiveScene().name;
		if (previousLevelLoaded != activeSceneName)
		{
			CheckSceneMusic(activeSceneName);
		}
	}

	private void CheckSceneMusic(string currentScene)
	{
		if (string.IsNullOrEmpty(currentScene))
		{
			return;
		}

		previousLevelLoaded = currentScene;
		EnsureOptionsInitialized();

		int matchedIndex = FindOptionIndex(currentScene, levelBgMusicOptions);
		if (matchedIndex >= 0)
		{
			Accept(matchedIndex);
			return;
		}

		// Fallback to default list if not matched in serialized options
		if (levelBgMusicOptions != DefaultOptions)
		{
			int fallbackIndex = FindOptionIndex(currentScene, DefaultOptions);
			if (fallbackIndex >= 0)
			{
				AcceptOption(DefaultOptions[fallbackIndex]);
			}
		}
	}

	private int FindOptionIndex(string sceneName, LevelOptions[] options)
	{
		if (options == null)
		{
			return -1;
		}

		// 1. Exact match or _HD match
		for (int i = 0; i < options.Length; i++)
		{
			if (options[i] == null) continue;
			string levelName = options[i].levelName;
			if (string.Equals(levelName, sceneName, StringComparison.OrdinalIgnoreCase) ||
			    string.Equals(levelName + "_HD", sceneName, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}

		// 2. Prefix match for variants (e.g. 0_DailyMission_01_Assault -> 0_DailyMission_01)
		for (int j = 0; j < options.Length; j++)
		{
			if (options[j] == null) continue;
			string levelName = options[j].levelName;
			if (!string.IsNullOrEmpty(levelName) && sceneName.StartsWith(levelName, StringComparison.OrdinalIgnoreCase))
			{
				return j;
			}
		}

		return -1;
	}

	public void Accept(int _levelNumber)
	{
		if (levelBgMusicOptions == null || _levelNumber < 0 || _levelNumber >= levelBgMusicOptions.Length)
		{
			return;
		}

		AcceptOption(levelBgMusicOptions[_levelNumber]);
	}

	private void AcceptOption(LevelOptions opt)
	{
		if (opt == null)
		{
			return;
		}

		Debug.LogWarning("AudioBackgroundMusicManager::Accept Level Name: " + SceneManager.GetActiveScene().name + " -> track: " + opt.bgmusic);

		try
		{
			if (MonoSingleton<AppController>.Instance != null &&
			    MonoSingleton<AppController>.Instance.Model != null &&
			    MonoSingleton<AppController>.Instance.Model.GameSetting != null)
			{
				MonoSingleton<AppController>.Instance.Model.GameSetting.CommitAndNotify();
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("AudioBackgroundMusicManager: GameSetting notification warning: " + ex.Message);
		}

		InitAudioSource();

		if (opt._music == LevelOptions.Music._stop)
		{
			if (aSource != null && aSource.isPlaying)
			{
				aSource.Stop();
			}
			return;
		}

		if (opt._music == LevelOptions.Music._external)
		{
			return;
		}

		if (opt._music == LevelOptions.Music._internal && !string.IsNullOrEmpty(opt.bgmusic))
		{
			string targetTrack = opt.bgmusic;
			bool isSameClip = (aSource.clip != null && string.Equals(aSource.clip.name, targetTrack, StringComparison.OrdinalIgnoreCase));

			if (!isSameClip)
			{
				AudioClip audioClip = Resources.Load<AudioClip>("sounds/music/" + targetTrack);
				if (audioClip == null)
				{
					audioClip = Resources.Load<AudioClip>("Sounds/music/" + targetTrack);
				}
				if (audioClip == null)
				{
					audioClip = Resources.Load<AudioClip>(targetTrack);
				}

				if (audioClip != null)
				{
					aSource.Stop();
					aSource.clip = audioClip;
					aSource.loop = true;
					if (opt.delay > 0f)
					{
						aSource.PlayDelayed(opt.delay);
					}
					else
					{
						aSource.Play();
					}
				}
				else
				{
					Debug.LogWarning("AudioBackgroundMusicManager: Failed to load track: " + targetTrack);
				}
			}
			else
			{
				if (!aSource.isPlaying)
				{
					aSource.loop = true;
					aSource.Play();
				}
			}
		}
	}
}
