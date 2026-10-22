using System;

[Serializable]
public class LevelOptions
{
	public enum Music
	{
		_internal = 0,
		_external = 1,
		_stop = 2
	}

	public Music _music;

	public string levelName;

	public string bgmusic;

	public float delay = 1f;
}
