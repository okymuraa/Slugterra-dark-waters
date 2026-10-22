using UnityEngine;

namespace DG.Tweening
{
	public static class ShortcutExtensionsTk2d
	{
		public static Tweener DOScale(this tk2dBaseSprite target, Vector3 endValue, float duration)
		{
			return DOTween.To(() => target.scale, (Vector3 x) =>
			{
				target.scale = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOScaleX(this tk2dBaseSprite target, float endValue, float duration)
		{
			return DOTween.To(() => target.scale, (Vector3 x) =>
			{
				target.scale = x;
			}, new Vector3(endValue, 0f, 0f), duration).SetOptions(AxisConstraint.X).SetTarget(target);
		}

		public static Tweener DOScaleY(this tk2dBaseSprite target, float endValue, float duration)
		{
			return DOTween.To(() => target.scale, (Vector3 x) =>
			{
				target.scale = x;
			}, new Vector3(0f, endValue, 0f), duration).SetOptions(AxisConstraint.Y).SetTarget(target);
		}

		public static Tweener DOScaleZ(this tk2dBaseSprite target, float endValue, float duration)
		{
			return DOTween.To(() => target.scale, (Vector3 x) =>
			{
				target.scale = x;
			}, new Vector3(0f, 0f, endValue), duration).SetOptions(AxisConstraint.Z).SetTarget(target);
		}

		public static Tweener DOColor(this tk2dBaseSprite target, Color endValue, float duration)
		{
			return DOTween.To(() => target.color, (Color x) =>
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOColor(this TextMesh target, Color endValue, float duration)
		{
			return DOTween.To(() => target.color, (Color x) =>
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOFade(this tk2dBaseSprite target, float endValue, float duration)
		{
			return DOTween.ToAlpha(() => target.color, (Color x) =>
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOScale(this tk2dSlicedSprite target, Vector2 endValue, float duration)
		{
			return DOTween.To(() => target.dimensions, (Vector2 x) =>
			{
				target.dimensions = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOScaleX(this tk2dSlicedSprite target, float endValue, float duration)
		{
			return DOTween.To(() => target.dimensions, (Vector2 x) =>
			{
				target.dimensions = x;
			}, new Vector2(endValue, 0f), duration).SetOptions(AxisConstraint.X).SetTarget(target);
		}

		public static Tweener DOScaleY(this tk2dSlicedSprite target, float endValue, float duration)
		{
			return DOTween.To(() => target.dimensions, (Vector2 x) =>
			{
				target.dimensions = x;
			}, new Vector2(0f, endValue), duration).SetOptions(AxisConstraint.Y).SetTarget(target);
		}

		public static Tweener DOColor(this tk2dTextMesh target, Color endValue, float duration)
		{
			return DOTween.To(() => target.color, (Color x) =>
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOFade(this tk2dTextMesh target, float endValue, float duration)
		{
			return DOTween.ToAlpha(() => target.color, (Color x) =>
			{
				target.color = x;
			}, endValue, duration).SetTarget(target);
		}

		public static Tweener DOText(this tk2dTextMesh target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
		{
			return DOTween.To(() => target.text, (string x) =>
			{
				target.text = x;
			}, endValue, duration).SetOptions(richTextEnabled, scrambleMode, scrambleChars).SetTarget(target);
		}
	}
}
