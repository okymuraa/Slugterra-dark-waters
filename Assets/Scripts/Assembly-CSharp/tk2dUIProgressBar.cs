using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIProgressBar")]
public class tk2dUIProgressBar : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action OnProgressComplete__BackingField;

	public Transform scalableBar;

	public tk2dClippedSprite clippedSpriteBar;

	public tk2dSlicedSprite slicedSpriteBar;

	public tk2dRadialSprite radialSpriteBar;

	private bool initializedSlicedSpriteDimensions;

	private Vector2 emptySlicedSpriteDimensions = Vector2.zero;

	private Vector2 fullSlicedSpriteDimensions = Vector2.zero;

	private Vector2 currentDimensions = Vector2.zero;

	[SerializeField]
	private float percent;

	private bool isProgressComplete;

	public GameObject sendMessageTarget;

	public string SendMessageOnProgressCompleteMethodName = string.Empty;

	public float Value
	{
		get
		{
			return percent;
		}
		set
		{
			percent = Mathf.Clamp(value, 0f, 1f);
			if (!Application.isPlaying)
			{
				return;
			}
			if (clippedSpriteBar != null)
			{
				clippedSpriteBar.clipTopRight = new Vector2(Value, 1f);
			}
			else if (scalableBar != null)
			{
				scalableBar.localScale = new Vector3(Value, scalableBar.localScale.y, scalableBar.localScale.z);
			}
			else if (slicedSpriteBar != null)
			{
				InitializeSlicedSpriteDimensions();
				float newX = Mathf.Lerp(emptySlicedSpriteDimensions.x, fullSlicedSpriteDimensions.x, Value);
				currentDimensions.Set(newX, fullSlicedSpriteDimensions.y);
				slicedSpriteBar.dimensions = currentDimensions;
			}
			else if (radialSpriteBar != null)
			{
				radialSpriteBar.SetValue(Value);
			}
			if (!isProgressComplete && Value == 1f)
			{
				isProgressComplete = true;
				if (OnProgressComplete__BackingField != null)
				{
					OnProgressComplete__BackingField();
				}
				if (sendMessageTarget != null && SendMessageOnProgressCompleteMethodName.Length > 0)
				{
					sendMessageTarget.SendMessage(SendMessageOnProgressCompleteMethodName, this, SendMessageOptions.RequireReceiver);
				}
			}
			else if (isProgressComplete && Value < 1f)
			{
				isProgressComplete = false;
			}
		}
	}

	public event Action OnProgressComplete
	{
		add
		{
			Action action = OnProgressComplete__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnProgressComplete__BackingField, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action action = OnProgressComplete__BackingField;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref OnProgressComplete__BackingField, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private void Start()
	{
		InitializeSlicedSpriteDimensions();
		InitializeRadialSprite();
		Value = percent;
	}

	private void InitializeSlicedSpriteDimensions()
	{
		if (!initializedSlicedSpriteDimensions)
		{
			if (slicedSpriteBar != null)
			{
				tk2dSpriteDefinition currentSprite = slicedSpriteBar.CurrentSprite;
				Vector3 vector = currentSprite.boundsData[1];
				fullSlicedSpriteDimensions = slicedSpriteBar.dimensions;
				emptySlicedSpriteDimensions.Set((slicedSpriteBar.borderLeft + slicedSpriteBar.borderRight) * vector.x / currentSprite.texelSize.x, fullSlicedSpriteDimensions.y);
			}
			initializedSlicedSpriteDimensions = true;
		}
	}

	private void InitializeRadialSprite()
	{
	}
}
