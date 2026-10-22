using Slugterra.Game.Units.Ally;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using UnityEngine;

public class UnLimitArena : MonoBehaviour
{
	private EliController _eliController;

	private BossProntoController _prontoController;

	private TrixieController _trixieController;

	private void Start()
	{
		_eliController = Object.FindObjectOfType<EliController>();
		_prontoController = Object.FindObjectOfType<BossProntoController>();
		_trixieController = Object.FindObjectOfType<TrixieController>();
	}

	private void Update()
	{
		if (_eliController != null && _eliController.Model.Health < 300f)
		{
			_eliController.Model.Health += 10f;
		}
		if (_prontoController != null && _prontoController.Model.Health < 300f)
		{
			_prontoController.Model.Health += 10f;
		}
		if (_trixieController != null && _trixieController.Model.Health < 300f)
		{
			_trixieController.Model.Health += 10f;
		}
	}
}
