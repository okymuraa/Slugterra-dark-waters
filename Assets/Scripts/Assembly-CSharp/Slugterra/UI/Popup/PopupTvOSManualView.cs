using System;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupTvOSManualView : PopupView
	{
		public Action popupClose;

		[SerializeField]
		private GameObject _arenaControls;

		[SerializeField]
		private GameObject _campaignControls;

		[SerializeField]
		private GameObject _btnNext;

		[SerializeField]
		private GameObject _btnPrevios;

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnShowHelpForControllerEvent -= ShowHelp;
				DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= Close;
				DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent -= OnButtonUpEvent;
			}
		}

		protected override void OnStart()
		{
			DonDestroyMonoSingleton<InputManager>.Instance.OnShowHelpForControllerEvent += ShowHelp;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += Close;
			DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent += OnButtonUpEvent;
			gameObject.SetActive(false);
			ShowCampaignControls();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		public void ShowHelp()
		{
			Debug.Log("PopupTvOSManualView::ShowHelp");
			gameObject.SetActive(true);
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null && currentGameLevel.Type == LevelType.Arena)
			{
				ShowArenaControls();
			}
			else
			{
				ShowCampaignControls();
			}
		}

		public override void Close()
		{
			gameObject.SetActive(false);
			if (popupClose != null)
			{
				popupClose();
			}
			popupClose = null;
		}

		private void OnButtonUpEvent(InputEvent e)
		{
			if (e == InputEvent.Right && (bool)_btnNext)
			{
				ShowArenaControls();
			}
			if (e == InputEvent.Left && (bool)_btnPrevios)
			{
				ShowCampaignControls();
			}
		}

		private void ShowArenaControls()
		{
			if ((bool)_btnNext)
			{
				_btnNext.SetActive(false);
			}
			if ((bool)_btnPrevios)
			{
				_btnPrevios.SetActive(true);
			}
			if ((bool)_campaignControls)
			{
				_campaignControls.SetActive(false);
			}
			if ((bool)_arenaControls)
			{
				_arenaControls.SetActive(true);
			}
		}

		private void ShowCampaignControls()
		{
			if ((bool)_btnNext)
			{
				_btnNext.SetActive(true);
			}
			if ((bool)_btnPrevios)
			{
				_btnPrevios.SetActive(false);
			}
			if ((bool)_campaignControls)
			{
				_campaignControls.SetActive(true);
			}
			if ((bool)_arenaControls)
			{
				_arenaControls.SetActive(false);
			}
		}
	}
}
