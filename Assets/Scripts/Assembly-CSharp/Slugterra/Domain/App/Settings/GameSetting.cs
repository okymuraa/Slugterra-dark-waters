using System.IO;
using AppsMinistry.Domain;
using AppsMinistry.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Slugterra.Domain.App.Settings
{
	public class GameSetting : DomainObject
	{
		private SlugControl _slugControl;

		private float _musicVolume;

		private float _sfxVolume;

		public const float DEFAULT_SFX_VOLUME = 0.57f;

		public const float DEFAULT_MUSIC_VOLUME = 0.5f;

		public SlugControl SlugControl
		{
			get
			{
				return _slugControl;
			}
			set
			{
				_slugControl = value;
				CommitAndNotify();
			}
		}

		public float MusicVolume
		{
			get
			{
				return _musicVolume;
			}
			set
			{
				float num = Mathf.Clamp01(value);
				if (Mathf.Abs(_musicVolume - num) > Mathf.Epsilon)
				{
					_musicVolume = num;
					CommitAndNotify();
				}
			}
		}

		public float SoundFxVolume
		{
			get
			{
				return _sfxVolume;
			}
			set
			{
				float num = Mathf.Clamp01(value);
				if (Mathf.Abs(_sfxVolume - num) > Mathf.Epsilon)
				{
					_sfxVolume = num;
					CommitAndNotify();
				}
			}
		}

		public GameSetting()
		{
			_sfxVolume = 0.57f;
			_musicVolume = 0.5f;
			_slugControl = SlugControl.Tap;
		}

		public static void Serialize(GameSetting objectToSerialize)
		{
			string stringToSave = JsonConvert.SerializeObject(objectToSerialize);
			IOSystem.SaveStrings(Application.persistentDataPath, "GameSettings.cfg", stringToSave);
		}

		public static GameSetting Deserialize()
		{
			string empty = string.Empty;
			GameSetting gameSetting = null;
			if (IOSystem.IsFileExist(Application.persistentDataPath, Path.GetFileName("GameSettings.cfg")))
			{
				empty = IOSystem.ReadStrings(Application.persistentDataPath, Path.GetFileName("GameSettings.cfg"));
				return JsonConvert.DeserializeObject<GameSetting>(empty);
			}
			return new GameSetting();
		}
	}
}
