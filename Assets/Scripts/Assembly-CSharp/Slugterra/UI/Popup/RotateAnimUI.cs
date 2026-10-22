using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class RotateAnimUI : MonoBehaviour
	{
		[SerializeField]
		private Vector3 targetRotation = Vector3.zero;

		[SerializeField]
		private float duration = 2f;

		[SerializeField]
		private float delay = 0.3f;

		[SerializeField]
		private bool _loop;

		[SerializeField]
		private Ease easeType = Ease.Linear;

		private Quaternion storedRotation = Quaternion.identity;

		private bool isInit;

		private Tweener rotateTween;

		private void OnEnable()
		{
			storedRotation = transform.localRotation;
			if (isInit)
			{
				rotateTween = transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360).SetDelay(delay).SetEase(easeType)
					.SetUpdate(true);
			}
			if (_loop)
			{
				rotateTween.SetLoops(-1);
			}
		}

		private void Start()
		{
			rotateTween = transform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360).SetDelay(delay).SetEase(easeType)
				.SetUpdate(true);
			if (_loop)
			{
				rotateTween.SetLoops(-1);
			}
			isInit = true;
		}

		private void OnDisable()
		{
			rotateTween.Rewind();
			transform.localRotation = storedRotation;
		}
	}
}
