using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class MinimapView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBuyIntelPackEvent__BackingField;

		[SerializeField]
		private UnityEngine.Camera _minimapCamera;

		[SerializeField]
		private UnityEngine.Camera _legendCamera;

		[SerializeField]
		private GameObject _intelPackButton;

		[SerializeField]
		private TextMesh _price;

		[SerializeField]
		private MinimapLegendView _legendView;

		[SerializeField]
		private GameObject _saleInStore;

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBuyIntelPackEvent
		{
			add
			{
				Action action = OnBuyIntelPackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyIntelPackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBuyIntelPackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyIntelPackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_minimapCamera.gameObject.SetActive(true);
			_legendCamera.gameObject.SetActive(true);
		}

		protected override void OnDisable()
		{
			_minimapCamera.gameObject.SetActive(false);
			_legendCamera.gameObject.SetActive(false);
			base.OnDisable();
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnBuyIntelPackClick()
		{
			if (OnBuyIntelPackEvent__BackingField != null)
			{
				OnBuyIntelPackEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (!(model is PackItem))
			{
				return;
			}
			PackItem packItem = model as PackItem;
			if (packItem.Type == PackType.First)
			{
				LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
				bool flag = currentGameLevel != null && packItem.IsBoughtForLevel(currentGameLevel.Name);
				_intelPackButton.SetActive(!flag);
				if (!flag)
				{
					PackItemConfiguration packConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetPackConfig(packItem.Type);
					_price.text = ((int)packConfig.GamePrice.Value/*cast due to constrained. prefix*/).ToString();
				}
				_legendView.ActiveAdditionalLegend(flag);
				int num = (flag ? (1 << LayerMask.NameToLayer("MinimapLegendExt")) : 0);
				_legendCamera.cullingMask = (1 << LayerMask.NameToLayer("MinimapLegend")) | num;
			}
		}

		public override void Close()
		{
			OnBtnBackClick();
		}
	}
}
