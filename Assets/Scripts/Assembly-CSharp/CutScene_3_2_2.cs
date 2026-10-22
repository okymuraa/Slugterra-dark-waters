using Slugterra.Game.Units.Mechanisms;
using Slugterra.Game.Wave;
using Slugterra.Scene;
using UnityEngine;

public class CutScene_3_2_2 : SlugterraCutScene
{
	[SerializeField]
	private MovePath AB_3_Path;

	[SerializeField]
	private GameObject EnableSpawnerCollider;

	private WagonController _trailer;

	private void Start()
	{
		_trailer = Object.FindObjectOfType<WagonController>();
		_trailer.SetSpeed(2f);
		_trailer.OnMove(AB_3_Path);
		EnableSpawnerCollider.SetActive(true);
	}
}
