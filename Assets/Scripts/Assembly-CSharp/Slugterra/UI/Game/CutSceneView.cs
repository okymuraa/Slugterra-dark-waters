using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI;
using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class CutSceneView : UIView
	{
		[SerializeField]
		private GameObject _topScreen;

		[SerializeField]
		private GameObject _bottomScreen;

		[SerializeField]
		private tk2dButton _skipButton;

		[SerializeField]
		private GameObject _skipButtonTextMobile;

		[SerializeField]
		private GameObject _skipButtonTextTvios;

		[SerializeField]
		private tk2dSprite _speakerIcon;

		[SerializeField]
		private TextMesh _dialogueText;

		public tk2dButton SkipButton
		{
			get
			{
				return _skipButton;
			}
		}

		public string SpeakerIcon
		{
			get
			{
				return _speakerIcon.GetSpriteName();
			}
			set
			{
				_speakerIcon.SetSprite(value);
			}
		}

		public string DialogueText
		{
			get
			{
				return _dialogueText.text;
			}
			set
			{
				_dialogueText.text = value;
			}
		}

		private void Awake()
		{
			if (_skipButtonTextMobile != null)
			{
				_skipButtonTextMobile.SetActive(true);
			}
			if (_skipButtonTextTvios != null)
			{
				_skipButtonTextTvios.SetActive(false);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_dialogueText);
			DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent += SkipTutor;
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent -= SkipTutor;
			}
		}

		private void SkipTutor(InputEvent e)
		{
			if (e == InputEvent.Enter && _skipButton != null && _skipButton.targetObject != null && _skipButton.gameObject.activeSelf)
			{
				_skipButton.targetObject.SendMessage(_skipButton.messageName);
			}
		}

		public void ShowScreens(bool show)
		{
			if (show)
			{
				_topScreen.transform.DOLocalMoveY(-0.65f, 0.5f);
				_bottomScreen.transform.DOLocalMoveY(0.65f, 0.5f);
			}
			else
			{
				_topScreen.transform.DOLocalMoveY(1.4f, 0.5f);
				_bottomScreen.transform.DOLocalMoveY(-1.4f, 0.5f);
			}
		}
	}
}
