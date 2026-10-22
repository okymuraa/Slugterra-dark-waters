using System.Collections.Generic;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public abstract class ShopItemController : UIController<ShopItemView>, IObserver
	{
		private object _model;

		private bool _selected;

		protected ShopItemShaderProxy shopItemShaderProxy;

		public object Model
		{
			get
			{
				return _model;
			}
		}

		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				Select(value);
			}
		}

		public ShopItemShaderProxy ShopItemShaderProxy
		{
			get
			{
				return shopItemShaderProxy;
			}
		}

		private void Select(bool value)
		{
			if (value != _selected)
			{
				_selected = value;
				if (_selected)
				{
					OnSelect();
				}
				else
				{
					OnDeselect();
				}
			}
		}

		protected virtual void OnSelect()
		{
		}

		protected virtual void OnDeselect()
		{
		}

		protected virtual void OnApplyModel(object model)
		{
		}

		public void ApplyModel(object model)
		{
			if (model != null)
			{
				if (_model != null && _model is Observable)
				{
					(_model as Observable).RemoveObserver(this);
				}
				_model = model;
				if (_model is Observable && _model != null)
				{
					(_model as Observable).AddObserver(this);
				}
				OnApplyModel(_model);
			}
		}

		protected virtual void ObjectChanged(object model)
		{
		}

		public void OnObjectChanged(object observable)
		{
			if (observable != null)
			{
				ObjectChanged(observable);
			}
		}

		protected abstract GameObject CreatePreview(object model);

		public static T Create<T>(object model, Transform parent = null) where T : ShopItemController
		{
			if (model != null)
			{
				GameObject gameObject = new GameObject(model.GetType().Name);
				gameObject.transform.parent = parent;
				gameObject.transform.localPosition = Vector3.zero;
				T result = gameObject.AddComponent<T>();
				result.ApplyModel(model);
				GameObject gameObject2 = result.CreatePreview(model);
				if (gameObject2 == null)
				{
					gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
				}
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.position = Vector3.zero;
				gameObject2.transform.localPosition = Vector3.zero;
				return result;
			}
			return (T)null;
		}

		public static List<ShopItemController> CreateList<T, I>(IEnumerable<I> items, Transform parent = null) where T : ShopItemController
		{
			List<ShopItemController> list = new List<ShopItemController>();
			foreach (I item in items)
			{
				T val = Create<T>(item, parent);
				if (val != null)
				{
					list.Add(val);
				}
			}
			list.Reverse();
			return list;
		}

		protected override void OnReleaseResource()
		{
			if (_model != null && _model is Observable)
			{
				(_model as Observable).RemoveObserver(this);
			}
		}
	}
}
