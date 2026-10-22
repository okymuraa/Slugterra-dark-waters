using Slugterra.Game.Units.Mechanisms;
using Slugterra.Game.Wave;
using Slugterra.Scene;
using UnityEngine;

public class CutScene_3_2_4 : SlugterraCutScene
{
	[SerializeField]
	private MovePath AB_5_Path;

	[SerializeField]
	private GameObject EnableSpawnerCollider;

	private WagonController _trailer;

	private void Start()
	{
		_trailer = Object.FindObjectOfType<WagonController>();
		_trailer.SetSpeed(2f);
		_trailer.OnMove(AB_5_Path);
		EnableSpawnerCollider.SetActive(true);
	}
}
