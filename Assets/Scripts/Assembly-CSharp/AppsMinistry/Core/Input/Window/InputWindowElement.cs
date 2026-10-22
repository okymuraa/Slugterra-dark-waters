using System.Collections.Generic;
using System.Linq;
using AppsMinistry.Core.Input.Window.Conditions;
using AppsMinistry.Utils;
using Slugterra.UI.Popup;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowElement : MonoBehaviour
	{
		[SerializeField]
		protected int _orderID;

		[SerializeField]
		private InputEvent _previousTransition = InputEvent.Up;

		[SerializeField]
		private InputEvent _nextTransition = InputEvent.Down;

		[SerializeField]
		private List<AlphaPulseAnimUI> _cachedPulseAnims = new List<AlphaPulseAnimUI>();

		private Color _bakedColor;

		private Color _selectedColor = Color.white;

		[SerializeField]
		private List<GameObject> _glows;

		[SerializeField]
		private bool _keepActiveGlowOnDeselect;

		[SerializeField]
		private bool _hideGlowOnPush;

		private bool _cachedToggleOnState;

		private Dictionary<AlphaPulseAnimUI, bool> _glowInitialStates = new Dictionary<AlphaPulseAnimUI, bool>();

		private InputWindowElementCondition _advanceCondition;

		public int OrderID
		{
			get
			{
				return _orderID;
			}
		}

		public InputEvent TransitionToPrevious
		{
			get
			{
				return _previousTransition;
			}
			set
			{
				_previousTransition = value;
			}
		}

		public InputEvent TransitionToNext
		{
			get
			{
				return _nextTransition;
			}
			set
			{
				_nextTransition = value;
			}
		}

		public bool IsSelected { get; private set; }

		public virtual bool IsActive()
		{
			bool flag = !(_advanceCondition != null) || _advanceCondition.IsActive();
			return enabled && gameObject.activeSelf && gameObject.activeInHierarchy && flag;
		}

		private void Awake()
		{
			if (_cachedPulseAnims != null && _cachedPulseAnims.Count == 0)
			{
				_cachedPulseAnims = gameObject.GetComponentsInHierarchy<AlphaPulseAnimUI>();
			}
			foreach (AlphaPulseAnimUI cachedPulseAnim in _cachedPulseAnims)
			{
				if (!_glowInitialStates.ContainsKey(cachedPulseAnim))
				{
					_glowInitialStates.Add(cachedPulseAnim, cachedPulseAnim.enabled);
				}
			}
			_advanceCondition = GetComponent<InputWindowElementCondition>();
			tk2dUIToggleButton toggleComponent = GetToggleComponent();
			if (!(toggleComponent != null))
			{
				return;
			}
			foreach (GameObject glow in _glows)
			{
				if (glow.transform.IsChildOf(toggleComponent.onStateGO.transform))
				{
					_cachedToggleOnState = glow.activeSelf;
				}
			}
		}

		public virtual void SetState(bool isSelected)
		{
			tk2dUIToggleButton toggleComponent = GetToggleComponent();
			SetGlow(isSelected);
			foreach (GameObject glow in _glows)
			{
				if (glow != null)
				{
					if (toggleComponent == null && !_keepActiveGlowOnDeselect)
					{
						glow.SetActive(isSelected);
					}
					if (toggleComponent != null && !isSelected && glow.transform.IsChildOf(toggleComponent.onStateGO.transform))
					{
						glow.SetActive(_cachedToggleOnState);
					}
					else if (toggleComponent != null)
					{
						glow.SetActive(isSelected);
					}
				}
			}
			IsSelected = isSelected;
		}

		public virtual void Select()
		{
			tk2dUIToggleButton toggleComponent = GetToggleComponent();
			if (toggleComponent != null && toggleComponent.enabled && toggleComponent.gameObject.activeInHierarchy && toggleComponent.gameObject.activeSelf)
			{
				toggleComponent.uiItem.SimulateClick();
				SetGlow(IsSelected);
				if (_hideGlowOnPush)
				{
					SetGlow(false);
				}
				return;
			}
			tk2dButton tk2dButton2 = GetComponent<tk2dButton>();
			if (tk2dButton2 == null)
			{
				tk2dButton2 = GetComponentInChildren<tk2dButton>(false);
			}
			if (tk2dButton2 != null && tk2dButton2.enabled && tk2dButton2.gameObject.activeInHierarchy && tk2dButton2.gameObject.activeSelf)
			{
				tk2dButton2.SimulateClick();
				SetGlow(IsSelected);
				if (_hideGlowOnPush)
				{
					SetGlow(false);
				}
				return;
			}
			tk2dUIItem tk2dUIItem2 = GetComponent<tk2dUIItem>();
			if (tk2dUIItem2 == null)
			{
				tk2dUIItem2 = GetComponentInChildren<tk2dUIItem>(false);
			}
			if (tk2dUIItem2 != null && tk2dUIItem2.enabled && tk2dUIItem2.gameObject.activeInHierarchy && tk2dUIItem2.gameObject.activeSelf)
			{
				tk2dUIItem2.SimulateClick();
				SetGlow(IsSelected);
				if (_hideGlowOnPush)
				{
					SetGlow(false);
				}
			}
		}

		private void SetGlow(bool isSelected)
		{
			if (_cachedPulseAnims == null || _cachedPulseAnims.Count < 1)
			{
				return;
			}
			if (isSelected)
			{
				CacheColor();
			}
			Color color = ((!isSelected) ? _bakedColor : _selectedColor);
			foreach (AlphaPulseAnimUI cachedPulseAnim in _cachedPulseAnims)
			{
				if (!(cachedPulseAnim.Sprite == null))
				{
				}
			}
			foreach (GameObject glow in _glows)
			{
				if (glow != null)
				{
					glow.SetActive(isSelected);
				}
			}
		}

		private tk2dUIToggleButton GetToggleComponent()
		{
			tk2dUIToggleButton tk2dUIToggleButton2 = GetComponent<tk2dUIToggleButton>();
			if (tk2dUIToggleButton2 == null)
			{
				tk2dUIToggleButton2 = GetComponentInChildren<tk2dUIToggleButton>(false);
			}
			return tk2dUIToggleButton2;
		}

		private void CacheColor()
		{
			AlphaPulseAnimUI alphaPulseAnimUI = _cachedPulseAnims.First((AlphaPulseAnimUI a) => a.gameObject.activeSelf);
			tk2dBaseSprite tk2dBaseSprite2 = ((!(alphaPulseAnimUI != null) || !(alphaPulseAnimUI.Sprite != null)) ? null : alphaPulseAnimUI.Sprite.transform.parent.GetComponent<tk2dBaseSprite>());
			_bakedColor = ((!(tk2dBaseSprite2 != null)) ? _bakedColor : tk2dBaseSprite2.color);
		}

		private void RecheckGlowInitialStates()
		{
			foreach (AlphaPulseAnimUI cachedPulseAnim in _cachedPulseAnims)
			{
				if (!_glowInitialStates.ContainsKey(cachedPulseAnim))
				{
					_glowInitialStates.Add(cachedPulseAnim, cachedPulseAnim.enabled);
				}
			}
		}
	}
}
