using System;
using UnityEngine;

namespace AppsMinistry.Core.Command
{
	public class MonoCommand : MonoBehaviour
	{
		private object[] _args;

		protected bool IsExecuteOn;

		private PersistentCommandAttribute _persistentCommandAttribute;

		public object[] Args
		{
			get
			{
				return _args;
			}
		}

		public virtual object Result
		{
			get
			{
				return null;
			}
		}

		public bool IsFinished { get; protected set; }

		public bool IsSuccesses { get; protected set; }

		protected virtual void OnStart(object[] args)
		{
		}

		protected virtual void OnReleaseResources()
		{
		}

		protected virtual void OnUpdate()
		{
		}

		private void Start()
		{
			OnStart(Args);
		}

		private void Update()
		{
			OnUpdate();
		}

		private void OnDestroy()
		{
			OnReleaseResources();
		}

		public static T Execute<T>(params object[] args) where T : MonoCommand
		{
			return Execute(typeof(T), args) as T;
		}

		public static MonoCommand Execute(Type type, params object[] args)
		{
			MonoCommand monoCommand = ExecuteOn(type, new GameObject(type.Name), args);
			monoCommand.IsExecuteOn = false;
			return monoCommand;
		}

		public static T ExecuteOn<T>(GameObject target, params object[] args) where T : MonoCommand
		{
			return ExecuteOn(typeof(T), target, args) as T;
		}

		public static MonoCommand ExecuteOn(Type type, GameObject target, params object[] args)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(type);
			SingletonCommandAttribute singletonCommandAttribute = null;
			AppCommandAttribute appCommandAttribute = null;
			PersistentCommandAttribute persistentCommandAttribute = null;
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is AppCommandAttribute)
				{
					appCommandAttribute = attribute as AppCommandAttribute;
				}
				else if (attribute is SingletonCommandAttribute)
				{
					singletonCommandAttribute = attribute as SingletonCommandAttribute;
				}
				else if (attribute is PersistentCommandAttribute)
				{
					persistentCommandAttribute = attribute as PersistentCommandAttribute;
				}
			}
			MonoCommand monoCommand = null;
			if (singletonCommandAttribute != null || persistentCommandAttribute != null)
			{
				monoCommand = target.GetComponent(type) as MonoCommand;
				if (monoCommand != null && (!monoCommand.IsFinished || persistentCommandAttribute != null))
				{
					monoCommand._args = args;
					monoCommand.OnStart(args);
					if (persistentCommandAttribute != null)
					{
						monoCommand.IsFinished = false;
						monoCommand.enabled = true;
					}
					return monoCommand;
				}
			}
			monoCommand = target.AddComponent(type) as MonoCommand;
			monoCommand.IsExecuteOn = true;
			monoCommand._args = args;
			if (appCommandAttribute != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(monoCommand);
			}
			return monoCommand;
		}
	}
	public class MonoCommand<T> : MonoCommand where T : MonoCommand<T>
	{
		private AsyncTask<T> _asyncToken;

		public AsyncTask<T> AsyncToken
		{
			get
			{
				if (_asyncToken == null)
				{
					_asyncToken = new AsyncTask<T>(this as T);
				}
				return _asyncToken;
			}
		}

		protected virtual void OnFinish()
		{
		}

		public void Finish(bool finish = true)
		{
			if (IsFinished)
			{
				return;
			}
			IsFinished = true;
			IsSuccesses = finish;
			AsyncToken.OnFinish(finish);
			OnFinish();
			Attribute[] customAttributes = Attribute.GetCustomAttributes(GetType());
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is PersistentCommandAttribute)
				{
					enabled = false;
					return;
				}
			}
			if (IsExecuteOn)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}
}
