using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class AlphaPulseAnimUI : MonoBehaviour
	{
		[SerializeField]
		private bool autostart = true;

		[SerializeField]
		private bool resetToFromValue;

		[SerializeField]
		private float targetAlpha = 1f;

		[SerializeField]
		private float fromAlpha;

		[SerializeField]
		private float duration = 1f;

		[SerializeField]
		private int loops = -1;

		[SerializeField]
		private float delay;

		[SerializeField]
		private Ease easeType = Ease.InOutSine;

		[SerializeField]
		private tk2dBaseSprite sprite;

		private bool isInit;

		private bool isStarted;

		private Tweener tweener;

		public tk2dBaseSprite Sprite
		{
			get
			{
				return sprite;
			}
		}

		public float SpriteAlpha
		{
			get
			{
				return sprite.color.a;
			}
			set
			{
				Color color = sprite.color;
				color.a = value;
				sprite.color = color;
			}
		}

		private void Awake()
		{
			if (sprite != null)
			{
				sprite.gameObject.SetActive(true);
				SpriteAlpha = ((!resetToFromValue) ? 0f : fromAlpha);
			}
		}

		private void OnEnable()
		{
			if (sprite != null && isInit && (autostart || isStarted))
			{
				Tween();
			}
		}

		private void Start()
		{
			if (sprite != null)
			{
				if (autostart)
				{
					Tween();
				}
				isInit = true;
			}
		}

		private void OnDisable()
		{
			if (isInit)
			{
				StopAnimation();
			}
			isStarted = true;
		}

		private void StopAnimation()
		{
			if (tweener != null)
			{
				tweener.Kill(true);
				tweener = null;
			}
			SpriteAlpha = ((!resetToFromValue) ? 0f : fromAlpha);
			isStarted = false;
		}

		public void LaunchAnimation(bool isOn = true)
		{
			if (!(sprite == null))
			{
				if (isOn)
				{
					Tween();
				}
				else
				{
					StopAnimation();
				}
			}
		}

		private void Tween()
		{
			if (tweener != null)
			{
				tweener.Kill(true);
				tweener = null;
			}
			sprite.gameObject.SetActive(true);
			SpriteAlpha = fromAlpha;
			Color color = sprite.color;
			color.a = targetAlpha;
			tweener = sprite.DOColor(color, duration).SetLoops(loops, LoopType.Yoyo).SetDelay(delay)
				.SetUpdate(true);
			isStarted = true;
		}
	}
}
