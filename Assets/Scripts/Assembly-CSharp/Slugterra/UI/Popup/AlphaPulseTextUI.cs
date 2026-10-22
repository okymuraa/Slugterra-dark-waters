using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class AlphaPulseTextUI : MonoBehaviour
	{
		[SerializeField]
		private bool autostart = true;

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
		private TextMesh text;

		private bool isInit;

		private bool isStarted;

		private Tweener tweener;

		public float TextColorAlpha
		{
			get
			{
				return text.color.a;
			}
			set
			{
				Color color = text.color;
				color.a = value;
				text.color = color;
			}
		}

		private void Awake()
		{
			if (text != null)
			{
				text.gameObject.SetActive(true);
				TextColorAlpha = 0f;
			}
		}

		private void OnEnable()
		{
			if (isInit && (autostart || isStarted))
			{
				Tween();
			}
		}

		private void Start()
		{
			if (text != null)
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
			TextColorAlpha = 0f;
			isStarted = false;
		}

		public void LaunchAnimation(bool isOn = true)
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

		private void Tween()
		{
			if (tweener != null)
			{
				tweener.Kill(true);
				tweener = null;
			}
			text.gameObject.SetActive(true);
			TextColorAlpha = fromAlpha;
			Color color = text.color;
			color.a = targetAlpha;
			tweener = text.DOColor(color, duration).SetLoops(loops, LoopType.Yoyo).SetDelay(delay)
				.SetUpdate(true);
			isStarted = true;
		}
	}
}
