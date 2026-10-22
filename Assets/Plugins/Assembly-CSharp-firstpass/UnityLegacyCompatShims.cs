using System;
using UnityEngine;

#if UNITY_2018_3_OR_NEWER
namespace UnityEngine
{
	public struct Particle
	{
		public Vector3 position;
		public Vector3 velocity;
		public float energy;
		public float startEnergy;
		public float size;
		public float rotation;
		public float angularVelocity;
		public Color color;
	}

	public class ParticleEmitter : Component
	{
		public bool emit;
		public float minSize;
		public float maxSize;
		public float minEnergy;
		public float maxEnergy;
		public float minEmission;
		public float maxEmission;
		public Vector3 worldVelocity;
		public Vector3 localVelocity;
		public Vector3 rndVelocity;
		public bool useWorldSpace;
		public bool rndRotation;
		public float angularVelocity;
		public float rndAngularVelocity;
		public Particle[] particles = new Particle[0];
		public int particleCount => (particles != null) ? particles.Length : 0;

		public void Emit() { }
		public void Emit(int count) { }
		public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color) { }
		public void ClearParticles() { }
	}

	public class ParticleAnimator : Component
	{
		public bool doesAnimateColor;
		public Color[] colorAnimation = new Color[5];
		public Vector3 worldRotationAxis;
		public Vector3 localRotationAxis;
		public float sizeGrow;
		public Vector3 rndForce;
		public Vector3 force;
		public float damping;
		public bool autodestruct;
	}

	public class ParticleRenderer : Renderer
	{
	}

	public static class LegacyLineRendererExtensions
	{
		public static void SetVertexCount(this LineRenderer lr, int count)
		{
			if (lr != null) lr.positionCount = count;
		}

		public static void SetWidth(this LineRenderer lr, float start, float end)
		{
			if (lr != null)
			{
				lr.startWidth = start;
				lr.endWidth = end;
			}
		}

		public static void SetColors(this LineRenderer lr, Color start, Color end)
		{
			if (lr != null)
			{
				lr.startColor = start;
				lr.endColor = end;
			}
		}
	}
}
#endif

#if UNITY_2019_3_OR_NEWER
public class GUIElement : Behaviour
{
	public Rect GetScreenRect(Camera camera = null)
	{
		return Rect.zero;
	}

	public bool HitTest(Vector3 screenPosition, Camera camera = null)
	{
		return false;
	}
}

public class GUIText : GUIElement
{
	public string text = string.Empty;
	private Material _material;
	public Material material
	{
		get
		{
			if (_material == null)
			{
				Shader shader = Shader.Find("Sprites/Default") ?? Shader.Find("UI/Default");
				if (shader != null)
				{
					_material = new Material(shader);
				}
			}
			return _material;
		}
		set
		{
			_material = value;
		}
	}
	public Font font;
	public TextAlignment alignment;
	public TextAnchor anchor;
	public float lineSpacing = 1f;
	public float tabSize = 4f;
	public int fontSize;
	public FontStyle fontStyle;
	public bool richText = true;
	public Color color = Color.white;
	public Vector2 pixelOffset;
}

public class GUITexture : GUIElement
{
	public Color color = Color.white;
	public Texture texture;
	public Rect pixelInset;
	public RectOffset border;
}

public class GUILayer : Behaviour
{
	public GUIElement HitTest(Vector3 screenPosition)
	{
		return null;
	}
}
#endif

#if UNITY_2019_1_OR_NEWER
namespace UnityEngine.Networking
{
	public static class NetworkClient
	{
		public static bool active => false;
	}

	public static class NetworkServer
	{
		public static bool active => false;
	}
}
#endif

#if UNITY_2018_3_OR_NEWER
namespace Boo.Lang
{
	using System.Collections;
	using System.Collections.Generic;

	public abstract class GenericGenerator<T> : IEnumerable<T>, IEnumerable
	{
		public abstract IEnumerator<T> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public abstract class GenericGeneratorEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable
	{
		protected T _current;
		protected int _state;

		public T Current => _current;
		object IEnumerator.Current => _current;

		public abstract bool MoveNext();

		protected bool Yield(int state, T value)
		{
			_state = state;
			_current = value;
			return true;
		}

		protected bool YieldDefault(int state)
		{
			_state = state;
			_current = default(T);
			return false;
		}

		public virtual void Reset()
		{
			throw new NotSupportedException();
		}

		public virtual void Dispose()
		{
		}
	}
}

namespace Boo.Lang.Runtime
{
	public static class RuntimeServices
	{
		public static bool EqualityOperator(object lhs, object rhs)
		{
			return object.Equals(lhs, rhs);
		}
	}
}

namespace UnityScript.Lang
{
	public static class Extensions
	{
		public static int get_length(System.Array array)
		{
			return (array != null) ? array.Length : 0;
		}
	}
}
#endif
