using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class MoveAnimUI : MonoBehaviour
	{
		[SerializeField]
		private Vector3 targetPosition = Vector3.zero;

		[SerializeField]
		private float duration = 2f;

		[SerializeField]
		private float delay = 0.3f;

		[SerializeField]
		private Ease easeType = Ease.Linear;

		[SerializeField]
		private Vector3 storedPosition = Vector3.zero;

		[SerializeField]
		private bool isUseZ;

		private bool isInit;

		private Tweener moveTweenX;

		private Tweener moveTweenY;

		private Tweener moveTweenZ;

		public Vector3 TargetPosition
		{
			get
			{
				return targetPosition;
			}
		}

		private void OnEnable()
		{
			storedPosition = transform.localPosition;
			if (isInit)
			{
				moveTweenX = transform.DOLocalMoveX(targetPosition.x, duration).SetDelay(delay).SetEase(easeType)
					.SetUpdate(true);
				moveTweenY = transform.DOLocalMoveY(targetPosition.y, duration).SetDelay(delay).SetEase(easeType)
					.SetUpdate(true);
				if (isUseZ)
				{
					moveTweenZ = transform.DOLocalMoveZ(targetPosition.z, duration).SetDelay(delay).SetEase(easeType)
						.SetUpdate(true);
				}
			}
		}

		private void Start()
		{
			moveTweenX = transform.DOLocalMoveX(targetPosition.x, duration).SetDelay(delay).SetEase(easeType)
				.SetUpdate(true);
			moveTweenY = transform.DOLocalMoveY(targetPosition.y, duration).SetDelay(delay).SetEase(easeType)
				.SetUpdate(true);
			if (isUseZ)
			{
				moveTweenZ = transform.DOLocalMoveZ(targetPosition.z, duration).SetDelay(delay).SetEase(easeType)
					.SetUpdate(true);
			}
			isInit = true;
		}

		private void OnDisable()
		{
			moveTweenX.Rewind();
			moveTweenY.Rewind();
			if (isUseZ)
			{
				moveTweenZ.Rewind();
			}
			transform.localPosition = storedPosition;
		}
	}
}
