using System.Collections.Generic;
using UnityEngine;

namespace AppsMinistry.Core.Input.Window
{
	public class InputWindowForceSwitch : MonoBehaviour
	{
		[SerializeField]
		protected List<InputWindowElement> _elements = new List<InputWindowElement>();

		protected LinkedList<InputWindowElement> _orderedElements = new LinkedList<InputWindowElement>();

		private LinkedListNode<InputWindowElement> LastSelectedElement { get; set; }

		public LinkedList<InputWindowElement> OrderedElements
		{
			get
			{
				return _orderedElements;
			}
		}
	}
}
