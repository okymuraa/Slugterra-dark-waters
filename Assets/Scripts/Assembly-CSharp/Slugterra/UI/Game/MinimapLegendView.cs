using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class MinimapLegendView : UIView
	{
		[SerializeField]
		private TextMesh _enemyLabel;

		[SerializeField]
		private tk2dSprite _enemyIcon;

		[SerializeField]
		private TextMesh _consumableLabel;

		[SerializeField]
		private tk2dSprite _consumableIcon;

		public void ActiveAdditionalLegend(bool value)
		{
			float alpha = ((!value) ? 0.34f : 1f);
			SetAlpha(_enemyLabel, alpha);
			SetAlpha(_enemyIcon, alpha);
			SetAlpha(_consumableLabel, alpha);
			SetAlpha(_consumableIcon, alpha);
		}

		private void SetAlpha(TextMesh textMesh, float alpha)
		{
			Color color = textMesh.color;
			color.a = Mathf.Clamp01(alpha);
			textMesh.color = color;
		}

		private void SetAlpha(tk2dSprite sprite, float alpha)
		{
			Color color = sprite.color;
			color.a = Mathf.Clamp01(alpha);
			sprite.color = color;
		}
	}
}
