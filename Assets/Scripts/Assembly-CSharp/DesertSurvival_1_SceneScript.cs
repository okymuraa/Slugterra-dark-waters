using Slugterra.Game.Wave;
using UnityEngine;

public class DesertSurvival_1_SceneScript : MonoBehaviour
{
	[SerializeField]
	private SurvivalWaveController survivalWaveController;

	public int waveFinish;

	public GameObject[] enableObject;

	public GameObject[] disableObject;

	private void OnEnable()
	{
		survivalWaveController.OnWaveFinishedEvent += OnWaveFinishedEvent;
	}

	private void OnDisable()
	{
		survivalWaveController.OnWaveFinishedEvent -= OnWaveFinishedEvent;
	}

	private void OnWaveFinishedEvent(int wave)
	{
		if (wave != waveFinish)
		{
			return;
		}
		for (int i = 0; i < enableObject.Length; i++)
		{
			if (enableObject[i] != null)
			{
				enableObject[i].SetActive(true);
			}
		}
		for (int j = 0; j < disableObject.Length; j++)
		{
			if (disableObject[j] != null)
			{
				disableObject[j].SetActive(false);
			}
		}
	}
}
