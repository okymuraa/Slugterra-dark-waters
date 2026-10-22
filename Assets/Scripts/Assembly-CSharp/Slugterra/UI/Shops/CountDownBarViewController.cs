using System;
using System.Collections;
using AppsMinistry.Core;
using DG.Tweening;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class CountDownBarViewController : MonoBehaviour
	{
		public const string ASSISTANT_TITLE_KEY = "asistant_recovery";

		public const string SLUGS_TITLE_KEY = "slug_recovery";

		public const string BLASTER_TITLE_KEY = "blaster_recovery";

		public const string ICON_HEART = "icon_injury";

		public const string ICON_REPAIR = "icon_repair-red";

		[SerializeField]
		private TextMesh countDownTimer;

		[SerializeField]
		private TextMesh _title;

		[SerializeField]
		private tk2dBaseSprite _icon;

		[SerializeField]
		private bool _playAnim = true;

		private TimeSpan _timer = TimeSpan.FromSeconds(0.0);

		private TimeSpan _StartTimer;

		private string _displayString;

		private Tweener _transformTweener;

		private bool _show;

		public const float MoveAnimationTime = 0.35f;

		public static readonly Vector3 HidePosition = new Vector3(0f, 0.5f, -0.1f);

		public static readonly Vector3 ShowPosition = new Vector3(0f, -1.2f, -0.1f);

		private void OnEnable()
		{
			if (_playAnim)
			{
				transform.localPosition = HidePosition;
			}
			_show = false;
			StartCoroutine(UpdateCoroutine());
		}

		public void SetTimer(TimeSpan newTimer)
		{
			_timer = newTimer;
		}

		public CountDownBarViewController SetTitle(string key)
		{
			if (!string.IsNullOrEmpty(key))
			{
				_title.text = MonoSingleton<Localization>.Instance.GetValueByKey(key);
			}
			return this;
		}

		public CountDownBarViewController SetIcon(string key)
		{
			if (!string.IsNullOrEmpty(key))
			{
				_icon.SetSprite(key);
			}
			return this;
		}

		private IEnumerator UpdateCoroutine()
		{
			while (true)
			{
				yield return new WaitForEndOfFrame();
				_timer -= TimeSpan.FromSeconds(Time.deltaTime);
				_displayString = string.Format("{0:00}:{1:00}:{2:00}", _timer.Hours, _timer.Minutes, Mathf.Clamp(_timer.Seconds, 0f, 60f));
				if (countDownTimer.text != _displayString)
				{
					countDownTimer.text = _displayString;
				}
			}
		}

		public void Show(bool value)
		{
			if (_show == value)
			{
				return;
			}
			if (_playAnim)
			{
				if (_transformTweener != null && !_transformTweener.IsComplete())
				{
					_transformTweener.Kill();
				}
				if (value)
				{
					_transformTweener = transform.DOLocalMove(ShowPosition, 0.35f).OnStart(() =>
					{
						gameObject.SetActive(true);
					});
				}
				else
				{
					_transformTweener = transform.DOLocalMove(HidePosition, 0.35f).OnComplete(() =>
					{
						gameObject.SetActive(false);
					});
				}
			}
			else
			{
				gameObject.SetActive(value);
			}
			_show = value;
		}
	}
}
