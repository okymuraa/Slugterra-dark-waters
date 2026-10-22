using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Domain;
using Slugterra.UI.Game;
using Slugterra.UI.Popup;
using UnityEngine;

public class ArenaInfoGameState : MonoBehaviour
{
	private bool weitFrame;

	private Canvas infoCam;

	public PopupConfirmView _popup;

	private GameController _gameController;

	private void Start()
	{
		_gameController = Object.FindObjectOfType<GameController>();
		_popup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
		infoCam = gameObject.GetComponentInChildren<Canvas>();
	}

	private void LateUpdate()
	{
		if (_gameController.Model.State != GameState.Play || _popup.gameObject.activeSelf)
		{
			if (infoCam.enabled)
			{
				infoCam.enabled = false;
				weitFrame = true;
			}
		}
		else if (!infoCam.enabled)
		{
			if (weitFrame)
			{
				weitFrame = false;
			}
			else
			{
				infoCam.enabled = true;
			}
		}
	}
}
