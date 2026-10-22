using System;
using AppsMinistry.Core.Sound;
using Slugterra.Game.Mission;
using UnityEngine;

public class AnimationPlayObjectivTarget : ObjectiveTarget
{
	[Serializable]
	public class AnimationSettings
	{
		public Animation animation;

		public AnimationClip clip;
	}

	[SerializeField]
	private GameObject _target;

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

	protected override void OnCreate()
	{
		if (_target == null)
		{
			Debug.LogWarning("DestroyObjectiveTarget: target is null");
		}
		if (_target == gameObject)
		{
			Debug.LogWarning("DestroyObjectiveTarget: bad target");
		}
		if (_target != null)
		{
			Debug.LogWarning("transform.position = _target.transform.position");
		}
	}

	protected override void OnUpdate()
	{
		if (!Completed && (_target == null || (_target != null && !_target.activeSelf)))
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
			Completed = true;
		}
	}
}
