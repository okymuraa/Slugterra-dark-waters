using System.Collections.Generic;
using System.Linq;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.Scene;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowController : DonDestroyMonoSingleton<InputWindowController>
	{
		private InputWindowView _activeView;

		[SerializeField]
		private List<InputWindowView> _views = new List<InputWindowView>();

		protected override void OnCreate()
		{
			gameObject.SetActive(false);
		}

		public void UpdateViews()
		{
			if (!gameObject.activeSelf)
			{
				return;
			}
			if (DonDestroyMonoSingleton<PopupManager>.Instance.IsShowing)
			{
				PopupView currentPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetCurrentPopup();
				List<InputWindowView> componentsInHierarchy = currentPopup.gameObject.GetComponentsInHierarchy<InputWindowView>();
				if (componentsInHierarchy != null)
				{
					for (int i = 0; i <= componentsInHierarchy.Count - 1; i++)
					{
						if (componentsInHierarchy[i] == null)
						{
							componentsInHierarchy.RemoveAt(i);
						}
					}
				}
				InputWindowView inputWindowView = ((componentsInHierarchy == null || componentsInHierarchy.Count <= 0) ? null : componentsInHierarchy.OrderByDescending((InputWindowView view) => view.Priority).FirstOrDefault((InputWindowView view) => view.IsActive()));
				if (componentsInHierarchy != null && componentsInHierarchy.Count > 0 && inputWindowView != null)
				{
					SetActiveView(inputWindowView);
				}
				else if (_activeView == null || (_activeView != null && !_activeView.IsActive()))
				{
					SelectViewOnScene();
				}
			}
			else
			{
				SelectViewOnScene();
			}
			DonDestroyMonoSingleton<PopupManager>.Instance.OnPopupClose -= OnPopupCloseHandler;
			DonDestroyMonoSingleton<PopupManager>.Instance.OnPopupClose += OnPopupCloseHandler;
		}

		private void SelectViewOnScene()
		{
			if (_views == null || _views.Count <= 0)
			{
				return;
			}
			IOrderedEnumerable<InputWindowView> orderedEnumerable = _views.OrderByDescending((InputWindowView a) => a.Priority);
			foreach (InputWindowView item in orderedEnumerable)
			{
				if (item == null || !item.IsActive())
				{
					continue;
				}
				SetActiveView(item);
				break;
			}
		}

		public void AddView(InputWindowView view)
		{
			if (!(view == null) && gameObject.activeSelf)
			{
				if (_views == null)
				{
					_views = new List<InputWindowView>();
				}
				if (!_views.Contains(view))
				{
					_views.Add(view);
				}
				if ((_activeView != null && _activeView.Priority < view.Priority) || DonDestroyMonoSingleton<PopupManager>.Instance.IsShowing || _activeView == null || (_activeView != null && !_activeView.IsActive()))
				{
					UpdateViews();
				}
			}
		}

		private void OnEnable()
		{
			SceneLoader.OnSceneChangedEvent += OnSceneChangedEventHandler;
		}

		private void OnDisable()
		{
			SceneLoader.OnSceneChangedEvent -= OnSceneChangedEventHandler;
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent -= OnButtonUpEventHandler;
			}
			if (DonDestroyMonoSingleton<PopupManager>.IsAlive)
			{
				DonDestroyMonoSingleton<PopupManager>.Instance.OnPopupClose -= OnPopupCloseHandler;
			}
		}

		public void SetActiveView(InputWindowView view)
		{
			if (view == null)
			{
				Debug.LogWarning("Trying set null view!");
				return;
			}
			if (!view.IsActive())
			{
				Debug.LogWarning("Try set inactive view!");
				return;
			}
			if (_activeView != null)
			{
				_activeView.DisableSelection();
			}
			_activeView = view;
			_activeView.SelectElement(_activeView.GetLastSelectedElement());
		}

		public void SetActiveView(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			List<InputWindowView> componentsInHierarchy = go.GetComponentsInHierarchy<InputWindowView>();
			if (componentsInHierarchy.Count != 0)
			{
				SetActiveView(componentsInHierarchy.OrderByDescending((InputWindowView a) => a.Priority).FirstOrDefault());
			}
		}

		private void OnLevelWasLoaded(int level)
		{
			if (_views == null)
			{
				_views = new List<InputWindowView>();
				return;
			}
			_views.RemoveAll((InputWindowView a) => a == null);
		}

		private void ChangeElements(InputEvent e)
		{
			if (_activeView == null)
			{
				return;
			}
			IOrderedEnumerable<InputWindowTransition> orderedEnumerable = _activeView.ViewTransitions.OrderByDescending((InputWindowTransition a) => a.Priority);
			InputWindowTransition inputWindowTransition = ((orderedEnumerable == null) ? null : orderedEnumerable.FirstOrDefault((InputWindowTransition a) => a.TransitionConditions.Contains(e) && a.View.IsActive()));
			LinkedListNode<InputWindowElement> lastSelectedElement = _activeView.GetLastSelectedElement();
			LinkedListNode<InputWindowElement> linkedListNode = null;
			if (lastSelectedElement != null && lastSelectedElement.Value != null)
			{
				if (e == lastSelectedElement.Value.TransitionToNext)
				{
					linkedListNode = lastSelectedElement.Next;
				}
				else if (e == lastSelectedElement.Value.TransitionToPrevious)
				{
					linkedListNode = lastSelectedElement.Previous;
				}
			}
			if ((linkedListNode == null || linkedListNode.Value == null) && inputWindowTransition == null)
			{
				_activeView.UseAction(e);
			}
			else if ((linkedListNode == null || linkedListNode.Value == null) && inputWindowTransition != null)
			{
				SetActiveView(inputWindowTransition.View);
			}
			else
			{
				_activeView.SelectElement(linkedListNode.Value);
			}
		}

		private void OnButtonUpEventHandler(InputEvent e)
		{
			if (_activeView == null)
			{
				Debug.LogWarning("active View is null!");
				return;
			}
			if (e != InputEvent.Enter)
			{
				ChangeElements(e);
				return;
			}
			LinkedListNode<InputWindowElement> lastSelectedElement = _activeView.GetLastSelectedElement();
			if (lastSelectedElement != null && lastSelectedElement.Value != null)
			{
				lastSelectedElement.Value.Select();
			}
			_activeView.UseAction(e);
		}

		private void OnSceneChangedEventHandler()
		{
			DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent -= OnButtonUpEventHandler;
			DonDestroyMonoSingleton<InputManager>.Instance.OnButtonUpEvent += OnButtonUpEventHandler;
		}

		private void OnPopupCloseHandler(PopupView popupView)
		{
			if ((_activeView != null && !_activeView.IsActive()) || _activeView == null)
			{
				UpdateViews();
			}
		}
	}
}
