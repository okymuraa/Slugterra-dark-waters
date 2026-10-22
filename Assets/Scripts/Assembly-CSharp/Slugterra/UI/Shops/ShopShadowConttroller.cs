using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopShadowConttroller : MonoBehaviour
	{
		[SerializeField]
		private List<tk2dUIScrollableArea> scrolls;

		[SerializeField]
		private tk2dSlicedSprite shadow;

		[SerializeField]
		private bool IsLeftShadow;

		private void OnEnable()
		{
			scrolls.ForEach((tk2dUIScrollableArea obj) =>
			{
				obj.OnScroll += OnScroll;
			});
			if (IsLeftShadow)
			{
				shadow.color = GetNewColor(0f);
			}
		}

		private Color GetNewColor(float alpha)
		{
			Color color = shadow.color;
			color.a = alpha;
			return color;
		}

		private void OnDisable()
		{
			scrolls.ForEach((tk2dUIScrollableArea obj) =>
			{
				obj.OnScroll -= OnScroll;
			});
		}

		private void OnScroll(tk2dUIScrollableArea scroll)
		{
			float num = 0f;
			if (IsLeftShadow)
			{
				num = ((!(scroll.Value <= 0.05f)) ? 1f : (scroll.Value * 20f));
			}
			else
			{
				num = ((!(scroll.Value >= 0.95f)) ? 1f : ((1f - scroll.Value) * 20f));
			}
			tk2dSlicedSprite tk2dSlicedSprite2 = shadow;
			Color newColor = GetNewColor(num);
			shadow.color = newColor;
			tk2dSlicedSprite2.color = newColor;
		}

		public void ActiveScrollTutorial(byte TargetAlpha, byte step)
		{
			if (gameObject.activeSelf)
			{
				StartCoroutine(Shadow(TargetAlpha, step));
			}
		}

		private IEnumerator Shadow(byte TargetAlpha, byte step)
		{
			float alpha = shadow.color.a;
			float offset = 1f / (float)(int)step;
			for (int i = 0; i < step; i++)
			{
				if (TargetAlpha == 0)
				{
					alpha -= offset;
					if (alpha < 0f)
					{
						alpha = 0f;
					}
				}
				if (TargetAlpha > 0)
				{
					alpha += offset;
					if (alpha > 1f)
					{
						alpha = 1f;
					}
				}
				tk2dSlicedSprite tk2dSlicedSprite2 = shadow;
				Color newColor = GetNewColor(alpha);
				shadow.color = newColor;
				tk2dSlicedSprite2.color = newColor;
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}
	}
}
