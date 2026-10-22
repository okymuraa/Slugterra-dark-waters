using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	[AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : ABSAnimationComponent
	{
		public float delay;

		public float duration = 1f;

		public Ease easeType = Ease.OutQuad;

		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public LoopType loopType;

		public int loops = 1;

		public string id = string.Empty;

		public bool isRelative;

		public bool isFrom;

		public bool isIndependentUpdate;

		public bool autoKill = true;

		public bool isActive = true;

		public bool isValid;

		public Component target;

		public DOTweenAnimationType animationType;

		public bool autoPlay = true;

		public float endValueFloat;

		public Vector3 endValueV3;

		public Color endValueColor = new Color(1f, 1f, 1f, 1f);

		public string endValueString = string.Empty;

		public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

		public bool optionalBool0;

		public float optionalFloat0;

		public int optionalInt0;

		public RotateMode optionalRotationMode;

		public ScrambleMode optionalScrambleMode;

		public string optionalString;

		private int _playCount = -1;

		private void Awake()
		{
			if (isActive && isValid)
			{
				CreateTween();
			}
		}

		private void OnDestroy()
		{
			if (tween != null && tween.IsActive())
			{
				tween.Kill();
			}
			tween = null;
		}

		public void CreateTween()
		{
			if (target == null)
			{
				Debug.LogWarning(string.Format("{0} :: This tween's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", gameObject.name), gameObject);
				return;
			}
			Type type = target.GetType();
			switch (animationType)
			{
			case DOTweenAnimationType.Move:
				if (type.IsSameOrSubclassOf(typeof(RectTransform)))
				{
					tween = ((RectTransform)target).DOAnchorPos3D(endValueV3, duration, optionalBool0);
				}
				else if (type.IsSameOrSubclassOf(typeof(Transform)))
				{
					tween = ((Transform)target).DOMove(endValueV3, duration, optionalBool0);
				}
				else if (type.IsSameOrSubclassOf(typeof(Rigidbody2D)))
				{
					tween = ((Rigidbody2D)target).DOMove(endValueV3, duration, optionalBool0);
				}
				else if (type.IsSameOrSubclassOf(typeof(Rigidbody)))
				{
					tween = ((Rigidbody)target).DOMove(endValueV3, duration, optionalBool0);
				}
				break;
			case DOTweenAnimationType.LocalMove:
				tween = transform.DOLocalMove(endValueV3, duration, optionalBool0);
				break;
			case DOTweenAnimationType.Rotate:
				if (type.IsSameOrSubclassOf(typeof(Transform)))
				{
					tween = ((Transform)target).DORotate(endValueV3, duration, optionalRotationMode);
				}
				else if (type.IsSameOrSubclassOf(typeof(Rigidbody2D)))
				{
					tween = ((Rigidbody2D)target).DORotate(endValueFloat, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Rigidbody)))
				{
					tween = ((Rigidbody)target).DORotate(endValueV3, duration, optionalRotationMode);
				}
				break;
			case DOTweenAnimationType.LocalRotate:
				tween = transform.DOLocalRotate(endValueV3, duration, optionalRotationMode);
				break;
			case DOTweenAnimationType.Scale:
				tween = transform.DOScale((!optionalBool0) ? endValueV3 : new Vector3(endValueFloat, endValueFloat, endValueFloat), duration);
				break;
			case DOTweenAnimationType.Color:
				isRelative = false;
				if (type.IsSameOrSubclassOf(typeof(SpriteRenderer)))
				{
					tween = ((SpriteRenderer)target).DOColor(endValueColor, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Renderer)))
				{
					tween = ((Renderer)target).material.DOColor(endValueColor, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Image)))
				{
					tween = ((Image)target).DOColor(endValueColor, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Text)))
				{
					tween = ((Text)target).DOColor(endValueColor, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(tk2dTextMesh)))
				{
					tween = ((tk2dTextMesh)target).DOColor(endValueColor, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(tk2dBaseSprite)))
				{
					tween = ((tk2dBaseSprite)target).DOColor(endValueColor, duration);
				}
				break;
			case DOTweenAnimationType.Fade:
				isRelative = false;
				if (type.IsSameOrSubclassOf(typeof(SpriteRenderer)))
				{
					tween = ((SpriteRenderer)target).DOFade(endValueFloat, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Renderer)))
				{
					tween = ((Renderer)target).material.DOFade(endValueFloat, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Image)))
				{
					tween = ((Image)target).DOFade(endValueFloat, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(Text)))
				{
					tween = ((Text)target).DOFade(endValueFloat, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(tk2dTextMesh)))
				{
					tween = ((tk2dTextMesh)target).DOFade(endValueFloat, duration);
				}
				else if (type.IsSameOrSubclassOf(typeof(tk2dBaseSprite)))
				{
					tween = ((tk2dBaseSprite)target).DOFade(endValueFloat, duration);
				}
				break;
			case DOTweenAnimationType.Text:
				if (type.IsSameOrSubclassOf(typeof(Text)))
				{
					tween = ((Text)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
				}
				else if (type.IsSameOrSubclassOf(typeof(tk2dTextMesh)))
				{
					tween = ((tk2dTextMesh)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
				}
				break;
			case DOTweenAnimationType.PunchPosition:
				if (type.IsSameOrSubclassOf(typeof(RectTransform)))
				{
					tween = ((RectTransform)target).DOPunchAnchorPos(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
				}
				else if (type.IsSameOrSubclassOf(typeof(Transform)))
				{
					tween = ((Transform)target).DOPunchPosition(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
				}
				break;
			case DOTweenAnimationType.PunchScale:
				tween = transform.DOPunchScale(endValueV3, duration, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.PunchRotation:
				tween = transform.DOPunchRotation(endValueV3, duration, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.ShakePosition:
				if (type.IsSameOrSubclassOf(typeof(RectTransform)))
				{
					tween = ((RectTransform)target).DOShakeAnchorPos(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
				}
				if (type.IsSameOrSubclassOf(typeof(Transform)))
				{
					tween = ((Transform)target).DOShakePosition(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
				}
				break;
			case DOTweenAnimationType.ShakeScale:
				tween = transform.DOShakeScale(duration, endValueV3, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.ShakeRotation:
				tween = transform.DOShakeRotation(duration, endValueV3, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.CameraAspect:
				tween = ((Camera)target).DOAspect(endValueFloat, duration);
				break;
			case DOTweenAnimationType.CameraBackgroundColor:
				tween = ((Camera)target).DOColor(endValueColor, duration);
				break;
			case DOTweenAnimationType.CameraFieldOfView:
				tween = ((Camera)target).DOFieldOfView(endValueFloat, duration);
				break;
			case DOTweenAnimationType.CameraOrthoSize:
				tween = ((Camera)target).DOOrthoSize(endValueFloat, duration);
				break;
			case DOTweenAnimationType.CameraPixelRect:
				tween = ((Camera)target).DOPixelRect(endValueRect, duration);
				break;
			case DOTweenAnimationType.CameraRect:
				tween = ((Camera)target).DORect(endValueRect, duration);
				break;
			}
			if (tween == null)
			{
				return;
			}
			if (isFrom)
			{
				((Tweener)tween).From(isRelative);
			}
			else
			{
				tween.SetRelative(isRelative);
			}
			tween.SetTarget(gameObject).SetDelay(delay).SetLoops(loops, loopType)
				.SetAutoKill(autoKill)
				.OnKill(() =>
				{
					tween = null;
				});
			if (easeType == Ease.INTERNAL_Custom)
			{
				tween.SetEase(easeCurve);
			}
			else
			{
				tween.SetEase(easeType);
			}
			if (!string.IsNullOrEmpty(id))
			{
				tween.SetId(id);
			}
			tween.SetUpdate(isIndependentUpdate);
			if (hasOnStart)
			{
				if (onStart != null)
				{
					tween.OnStart(onStart.Invoke);
				}
			}
			else
			{
				onStart = null;
			}
			if (hasOnPlay)
			{
				if (onPlay != null)
				{
					tween.OnPlay(onPlay.Invoke);
				}
			}
			else
			{
				onPlay = null;
			}
			if (hasOnUpdate)
			{
				if (onUpdate != null)
				{
					tween.OnUpdate(onUpdate.Invoke);
				}
			}
			else
			{
				onUpdate = null;
			}
			if (hasOnStepComplete)
			{
				if (onStepComplete != null)
				{
					tween.OnStepComplete(onStepComplete.Invoke);
				}
			}
			else
			{
				onStepComplete = null;
			}
			if (hasOnComplete)
			{
				if (onComplete != null)
				{
					tween.OnComplete(onComplete.Invoke);
				}
			}
			else
			{
				onComplete = null;
			}
			if (autoPlay)
			{
				tween.Play();
			}
			else
			{
				tween.Pause();
			}
		}

		public override void DOPlay()
		{
			DOTween.Play(gameObject);
		}

		public override void DOPlayBackwards()
		{
			DOTween.PlayBackwards(gameObject);
		}

		public override void DOPlayForward()
		{
			DOTween.PlayForward(gameObject);
		}

		public override void DOPause()
		{
			DOTween.Pause(gameObject);
		}

		public override void DOTogglePause()
		{
			DOTween.TogglePause(gameObject);
		}

		public override void DORewind()
		{
			_playCount = -1;
			DOTweenAnimation[] components = gameObject.GetComponents<DOTweenAnimation>();
			for (int num = components.Length - 1; num > -1; num--)
			{
				Tween tween = components[num].tween;
				if (tween != null && tween.IsInitialized())
				{
					components[num].tween.Rewind();
				}
			}
		}

		public override void DORestart(bool fromHere = false)
		{
			_playCount = -1;
			if (tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(tween);
				}
				return;
			}
			if (fromHere && isRelative)
			{
				ReEvaluateRelativeTween();
			}
			DOTween.Restart(gameObject);
		}

		public override void DOComplete()
		{
			DOTween.Complete(gameObject);
		}

		public override void DOKill()
		{
			DOTween.Kill(gameObject);
			tween = null;
		}

		public void DOPlayById(string id)
		{
			DOTween.Play(gameObject, id);
		}

		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		public void DOPlayNext()
		{
			DOTweenAnimation[] components = GetComponents<DOTweenAnimation>();
			while (_playCount < components.Length - 1)
			{
				_playCount++;
				DOTweenAnimation dOTweenAnimation = components[_playCount];
				if (dOTweenAnimation != null && dOTweenAnimation.tween != null && !dOTweenAnimation.tween.IsPlaying() && !dOTweenAnimation.tween.IsComplete())
				{
					dOTweenAnimation.tween.Play();
					break;
				}
			}
		}

		public void DORewindAndPlayNext()
		{
			_playCount = -1;
			DOTween.Rewind(gameObject);
			DOPlayNext();
		}

		public void DORestartById(string id)
		{
			_playCount = -1;
			DOTween.Restart(gameObject, id);
		}

		public void DORestartAllById(string id)
		{
			_playCount = -1;
			DOTween.Restart(id);
		}

		public List<Tween> GetTweens()
		{
			return DOTween.TweensByTarget(gameObject);
		}

		private void ReEvaluateRelativeTween()
		{
			if (animationType == DOTweenAnimationType.Move)
			{
				((Tweener)tween).ChangeEndValue(transform.position + endValueV3, true);
			}
			else if (animationType == DOTweenAnimationType.LocalMove)
			{
				((Tweener)tween).ChangeEndValue(transform.localPosition + endValueV3, true);
			}
		}
	}
}
