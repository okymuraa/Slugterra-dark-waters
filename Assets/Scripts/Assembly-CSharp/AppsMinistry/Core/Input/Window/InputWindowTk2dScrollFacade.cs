using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowTk2dScrollFacade : InputWindowView
	{
		[SerializeField]
		private tk2dUIScrollableArea _scrollableArea;

		[SerializeField]
		private List<InputWindowTransition> _firstViewTransitions;

		[SerializeField]
		private List<InputWindowTransition> _lastViewTransitions;

		[SerializeField]
		private List<GameObject> _ignoredElements;

		[SerializeField]
		private bool _allElementsNeedTransitions;

		[SerializeField]
		private InputEvent _previousViewEvent = InputEvent.Up;

		[SerializeField]
		private InputEvent _nextViewEvent = InputEvent.Down;

		[SerializeField]
		private List<InputWindowTk2dScrollView> _cachedViews;

		[SerializeField]
		private InputWindowTk2dScrollView _lastSelectedView;

		private Ease easeType = Ease.InOutSine;

		private Tweener tweener;

		[SerializeField]
		private float _tweenDuration = 0.4f;

		[SerializeField]
		private float _elementScrollSize;

		public tk2dUIScrollableArea ScrollableArea
		{
			get
			{
				return _scrollableArea;
			}
		}

		public InputWindowTk2dScrollView LastSelectedView
		{
			get
			{
				return _lastSelectedView;
			}
			set
			{
				_lastSelectedView = value;
			}
		}

		protected override void Awake()
		{
			if (_scrollableArea == null)
			{
				_scrollableArea = GetComponent<tk2dUIScrollableArea>();
			}
		}

		public void ReInitialize()
		{
			Initialize();
		}

		public override bool IsActive()
		{
			return enabled && gameObject.activeSelf && gameObject.activeInHierarchy && _cachedViews != null && _cachedViews.Count > 0 && HaveActiveCashed();
		}

		private InputWindowTk2dScrollView GetLastSelectedView()
		{
			InputWindowTk2dScrollView inputWindowTk2dScrollView = null;
			if (_cachedViews == null || _cachedViews.Count == 0)
			{
				return inputWindowTk2dScrollView;
			}
			foreach (InputWindowTk2dScrollView cachedView in _cachedViews)
			{
				foreach (InputWindowElement orderedElement in cachedView.OrderedElements)
				{
					if (orderedElement.IsSelected)
					{
						inputWindowTk2dScrollView = cachedView;
						break;
					}
				}
				if (inputWindowTk2dScrollView != null)
				{
					break;
				}
			}
			if (inputWindowTk2dScrollView == null)
			{
				foreach (InputWindowTk2dScrollView cachedView2 in _cachedViews)
				{
					if (cachedView2.IsActive())
					{
						inputWindowTk2dScrollView = cachedView2;
						break;
					}
				}
			}
			return inputWindowTk2dScrollView;
		}

		private bool HaveActiveCashed()
		{
			foreach (InputWindowTk2dScrollView cachedView in _cachedViews)
			{
				if (cachedView.IsActive())
				{
					return true;
				}
			}
			return false;
		}

		private void Initialize()
		{
			int count = _ignoredElements.Count;
			int num = 0;
			_cachedViews = new List<InputWindowTk2dScrollView>();
			_cachedViews.AddRange(_scrollableArea.contentContainer.GetComponentsInChildren<InputWindowTk2dScrollView>());
			if (Application.loadedLevelName == "SurvivalMapScene")
			{
				for (int i = 0; i < _cachedViews.Count - 1; i++)
				{
					bool flag = false;
					foreach (InputWindowElement element in _cachedViews[i].Elements)
					{
						flag = flag || element.isActiveAndEnabled;
					}
					if (!flag)
					{
						_cachedViews.Remove(_cachedViews[i]);
					}
				}
			}
			foreach (InputWindowTk2dScrollView cachedView in _cachedViews)
			{
				if (num == 0 && _firstViewTransitions != null)
				{
					cachedView.AddTransition(_firstViewTransitions);
					foreach (InputWindowTransition firstViewTransition in _firstViewTransitions)
					{
						firstViewTransition.View.AddTransition(new InputWindowTransition(cachedView, new List<InputEvent> { _nextViewEvent }));
					}
				}
				else if (num == _cachedViews.Count - 1 - count && _lastViewTransitions != null)
				{
					cachedView.AddTransition(_lastViewTransitions);
					foreach (InputWindowTransition lastViewTransition in _lastViewTransitions)
					{
						lastViewTransition.View.AddTransition(new InputWindowTransition(cachedView, new List<InputEvent> { _previousViewEvent }));
					}
				}
				else if (_allElementsNeedTransitions && _firstViewTransitions != null)
				{
					cachedView.AddTransition(_firstViewTransitions);
					foreach (InputWindowTransition firstViewTransition2 in _firstViewTransitions)
					{
						firstViewTransition2.View.AddTransition(new InputWindowTransition(cachedView, new List<InputEvent> { _nextViewEvent }));
					}
				}
				if (ViewTransitions != null && ViewTransitions.Count > 0)
				{
					cachedView.AddTransition(ViewTransitions);
				}
				InputWindowTransition inputWindowTransition = null;
				if (num > 0 && _cachedViews.Count > num - 1)
				{
					InputWindowTk2dScrollView view = _cachedViews[num - 1];
					inputWindowTransition = new InputWindowTransition(view, new List<InputEvent> { _previousViewEvent });
					cachedView.AddTransition(inputWindowTransition);
				}
				if (num < _cachedViews.Count - 1)
				{
					InputWindowTk2dScrollView view2 = _cachedViews[num + 1];
					inputWindowTransition = new InputWindowTransition(view2, new List<InputEvent> { _nextViewEvent });
					cachedView.AddTransition(inputWindowTransition);
				}
				num++;
			}
			DonDestroyMonoSingleton<InputWindowController>.Instance.UpdateViews();
		}

		public override void SelectElement(InputWindowElement element)
		{
			if (_lastSelectedView == null || !_lastSelectedView.IsActive())
			{
				_lastSelectedView = GetLastSelectedView();
			}
			if (_lastSelectedView != null)
			{
				DonDestroyMonoSingleton<InputWindowController>.Instance.SetActiveView(_lastSelectedView);
			}
		}

		public override void SelectElement(LinkedListNode<InputWindowElement> element)
		{
			InputWindowElement element2 = null;
			SelectElement(element2);
		}

		private void Start()
		{
			Invoke("Initialize", Time.deltaTime);
		}

		public void TweenScrollTo(float value)
		{
			if (tweener != null)
			{
				tweener.Kill(true);
				tweener = null;
			}
			tweener = DOTween.To(() => _scrollableArea.Value, (float newVal) =>
			{
				_scrollableArea.Value = newVal;
			}, value, _tweenDuration).SetEase(easeType).SetUpdate(true);
		}
	}
}
