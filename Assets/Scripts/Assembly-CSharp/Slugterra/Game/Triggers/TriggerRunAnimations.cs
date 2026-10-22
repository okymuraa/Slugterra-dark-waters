using System;
using AppsMinistry.Core.Sound;
using UnityEngine;

namespace Slugterra.Game.Triggers
{
	public class TriggerRunAnimations : Trigger
	{
		[Serializable]
		public class AnimationSettings
		{
			public Animation animation;

			public AnimationClip clip;
		}

		[SerializeField]
		private AnimationSettings[] _animations;

		[SerializeField]
		private SoundContainer _sound;

		protected override void OnStart()
		{
			if (_sound != null)
			{
				_sound.RegisterInSoundManager();
			}
		}

		protected override void OnTrigger()
		{
			AnimationSettings[] animations = _animations;
			foreach (AnimationSettings animationSettings in animations)
			{
				animationSettings.animation.clip = animationSettings.clip;
				animationSettings.animation.Play();
			}
			if (_sound != null)
			{
				_sound.Play();
			}
		}
	}
}
