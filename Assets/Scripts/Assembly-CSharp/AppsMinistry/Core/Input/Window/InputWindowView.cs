using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowView : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Higher is more priority")]
		private int _priority;

		[SerializeField]
		protected List<InputWindowElement> _elements = new List<InputWindowElement>();

		protected LinkedList<InputWindowElement> _orderedElements = new LinkedList<InputWindowElement>();

		[SerializeField]
		private List<InputWindowTransition> _viewTransitions = new List<InputWindowTransition>();

		private bool _selectionDisable;

		public int Priority
		{
			get
			{
				return _priority;
			}
			set
			{
				_priority = value;
			}
		}

		public List<InputWindowElement> Elements
		{
			get
			{
				return _elements;
			}
		}

		public LinkedList<InputWindowElement> OrderedElements
		{
			get
			{
				return _orderedElements;
			}
		}

		public List<InputWindowTransition> ViewTransitions
		{
			get
			{
				return _viewTransitions;
			}
		}

		private LinkedListNode<InputWindowElement> LastSelectedElement { get; set; }

		protected virtual void Awake()
		{
			SortElements();
		}

		protected virtual void OnEnable()
		{
			NotifyController();
		}

		protected virtual void OnDisable()
		{
			NotifyController();
		}

		protected void NotifyController()
		{
			if (DonDestroyMonoSingleton<InputWindowController>.IsAlive)
			{
				DonDestroyMonoSingleton<InputWindowController>.Instance.AddView(this);
			}
		}

		public virtual void SortElements()
		{
			_elements.RemoveAll((InputWindowElement obj) => obj == null);
			if (_elements != null && _elements.Count > 0)
			{
				_orderedElements = new LinkedList<InputWindowElement>(from b in _elements
					orderby b.OrderID
					where b.IsActive()
					select b);
			}
		}

		public void AddElement(InputWindowElement item)
		{
			_elements.Add(item);
		}

		private void CheckElements()
		{
			foreach (InputWindowElement element in _elements)
			{
				if (element.IsActive() && !_orderedElements.Contains(element))
				{
					_orderedElements.AddLast(element);
				}
				if (element.IsActive() || !_orderedElements.Contains(element))
				{
					continue;
				}
				if (LastSelectedElement != null && LastSelectedElement.Value == element)
				{
					if (LastSelectedElement.Previous != null && LastSelectedElement.Previous.Value != null && LastSelectedElement.Previous.Value.IsActive())
					{
						LastSelectedElement = LastSelectedElement.Previous;
					}
					else if (LastSelectedElement.Next != null && LastSelectedElement.Next.Value != null && LastSelectedElement.Next.Value.IsActive())
					{
						LastSelectedElement = LastSelectedElement.Next;
					}
				}
				_orderedElements.Remove(element);
			}
			_orderedElements = new LinkedList<InputWindowElement>(_orderedElements.OrderBy((InputWindowElement a) => a.OrderID));
		}

		public virtual void ReCheckElements()
		{
			CheckElements();
			if (LastSelectedElement == null || _orderedElements.Last != null)
			{
				LastSelectedElement = _orderedElements.Last;
			}
		}

		public LinkedListNode<InputWindowElement> GetElement(int orderId)
		{
			InputWindowElement value = _elements.Find((InputWindowElement a) => a.OrderID == orderId);
			return OrderedElements.Find(value);
		}

		public virtual LinkedListNode<InputWindowElement> GetLastSelectedElement()
		{
			CheckElements();
			if (LastSelectedElement == null)
			{
				LastSelectedElement = OrderedElements.First;
			}
			return LastSelectedElement;
		}

		public virtual void SelectElement(InputWindowElement element)
		{
			if (element == null)
			{
				Debug.LogWarningFormat("Can't select null element!");
			}
			SortElements();
			LastSelectedElement.Value.SetState(false);
			LinkedListNode<InputWindowElement> linkedListNode = OrderedElements.Find(element);
			if (linkedListNode == null || linkedListNode.Value == null)
			{
				Debug.LogWarningFormat("Unable to find orderID:{0} in view {1}", element.OrderID, gameObject.name);
				LastSelectedElement = OrderedElements.First;
			}
			else
			{
				LastSelectedElement = linkedListNode;
			}
			if (LastSelectedElement != null && LastSelectedElement.Value != null)
			{
				LastSelectedElement.Value.SetState(true);
			}
		}

		public virtual void SelectElement(LinkedListNode<InputWindowElement> element)
		{
			if (element != null)
			{
				SelectElement(element.Value);
				_selectionDisable = false;
			}
		}

		public virtual void SelectActiveElement()
		{
			foreach (InputWindowElement element in _elements)
			{
				if (element != null && element.IsActive())
				{
					SelectElement(element);
					_selectionDisable = false;
				}
			}
		}

		public void AddTransition(InputWindowTransition transition)
		{
			if (!(transform == null))
			{
				if (_viewTransitions == null)
				{
					_viewTransitions = new List<InputWindowTransition>();
				}
				_viewTransitions.Add(transition);
			}
		}

		public void AddTransition(List<InputWindowTransition> transitions)
		{
			if (transitions == null)
			{
				return;
			}
			foreach (InputWindowTransition transition in transitions)
			{
				AddTransition(transition);
			}
		}

		public virtual void DisableSelection()
		{
			if (LastSelectedElement != null && LastSelectedElement.Value != null)
			{
				LastSelectedElement.Value.SetState(false);
			}
			_selectionDisable = true;
		}

		public virtual void DisableAllSelections()
		{
			foreach (InputWindowElement element in _elements)
			{
				if (element != null)
				{
					element.SetState(false);
				}
			}
			_selectionDisable = true;
		}

		public virtual void UseAction(InputEvent e)
		{
		}

		public virtual bool IsActive()
		{
			bool flag = true;
			if (_elements != null)
			{
				foreach (InputWindowElement element in _elements)
				{
					if (element == null)
					{
						Debug.Log("InputWindowView::IsActive item == null");
						return false;
					}
				}
			}
			int num = ((_elements != null && _elements.Count > 0) ? _elements.FindAll((InputWindowElement a) => a.IsActive()).Count : 0);
			flag &= gameObject.activeSelf;
			flag &= gameObject.activeInHierarchy;
			flag &= enabled;
			return flag & (num > 0);
		}

		protected void PickAllInternalElements(GameObject go)
		{
			if (_elements == null)
			{
				_elements = new List<InputWindowElement>();
			}
			InputWindowElement[] componentsInChildren = go.GetComponentsInChildren<InputWindowElement>(true);
			InputWindowElement[] array = componentsInChildren;
			foreach (InputWindowElement inputWindowElement in array)
			{
				if (!_elements.Contains(inputWindowElement))
				{
					InputWindowView[] componentsInParent = inputWindowElement.GetComponentsInParent<InputWindowView>(true);
					if (componentsInParent == null || componentsInParent.Length <= 1 || !(componentsInParent[0].gameObject != gameObject))
					{
						_elements.Add(inputWindowElement);
					}
				}
			}
		}

		private void Update()
		{
			if (LastSelectedElement != null && !IsActive())
			{
				DonDestroyMonoSingleton<InputWindowController>.Instance.UpdateViews();
				LastSelectedElement = null;
			}
			if (LastSelectedElement != null && IsActive() && !LastSelectedElement.Value.IsActive() && !_selectionDisable)
			{
				SelectElement(GetLastSelectedElement());
			}
		}
	}
}
