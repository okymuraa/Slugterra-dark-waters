using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class ScaleAnimUI : MonoBehaviour
	{
		[SerializeField]
		private Vector3 targetScale = Vector3.zero;

		[SerializeField]
		private float duration = 2f;

		[SerializeField]
		private float delay = 0.3f;

		[SerializeField]
		private Ease easeType = Ease.Linear;

		[SerializeField]
		private Vector3 storedScale = Vector3.zero;

		private bool isInit;

		private Tweener scaleTween;

		public float Delay
		{
			get
			{
				return delay;
			}
			set
			{
				delay = value;
			}
		}

		private void OnEnable()
		{
			storedScale = transform.localScale;
			if (isInit)
			{
				scaleTween = transform.DOScale(targetScale, duration).SetDelay(delay).SetEase(easeType)
					.SetUpdate(true);
			}
		}

		private void Start()
		{
			scaleTween = transform.DOScale(targetScale, duration).SetDelay(delay).SetEase(easeType)
				.SetUpdate(true);
			isInit = true;
		}

		private void OnDisable()
		{
			scaleTween.Rewind();
			transform.localScale = storedScale;
		}
	}
}
