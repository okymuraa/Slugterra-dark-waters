using System.Collections.Generic;
using UnityEngine;

public class LoadLevelAdditiveRandom : MonoBehaviour
{
	public List<string> levelName;

	private int levelChoise;

	private void Start()
	{
		levelChoise = Random.Range(0, levelName.Count);
		Application.LoadLevelAdditive(levelName[levelChoise]);
		MonoBehaviour.print("Level type loaded: " + levelName);
	}
}
