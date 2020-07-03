using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	/// <summary>
	/// The mode that a listener is operating in.
	/// </summary>
	public enum PersistentListenerMode2
	{
		/// <summary>
		/// The listener will use the function binding specified by the event.
		/// </summary>
		EventDefined,
		/// <summary>
		/// The listener will bind to zero argument functions.
		/// </summary>
		Void,
		/// <summary>
		/// The listener will bind to an Object type argument functions.
		/// </summary>
		Object,
		/// <summary>
		/// The listener will bind to an int argument functions.
		/// </summary>
		Int,
		/// <summary>
		/// The listener will bind to a float argument functions.
		/// </summary>
		Float,
		/// <summary>
		/// The listener will bind to a string argument functions.
		/// </summary>
		String,
		/// <summary>
		/// The listener will bind to a bool argument functions.
		/// </summary>
		Bool,
		/// <summary>
		/// The listener will bind to an enum argument functions.
		/// </summary>
		Enum,
		/// <summary>
		/// The listener will bind to a Vector2 argument functions.
		/// </summary>
		Vector2,
		/// <summary>
		/// The listener will bind to a Vector2Int argument functions.
		/// </summary>
		Vector2Int,
		/// <summary>
		/// The listener will bind to a Vector3 argument functions.
		/// </summary>
		Vector3,
		/// <summary>
		/// The listener will bind to a Vector3Int argument functions.
		/// </summary>
		Vector3Int,
		/// <summary>
		/// The listener will bind to a Vector4 argument functions.
		/// </summary>
		Vector4,
		/// <summary>
		/// The listener will bind to a LayerMask argument functions.
		/// </summary>
		LayerMask,
		/// <summary>
		/// The listener will bind to a Color argument functions.
		/// </summary>
		Color
	}

	/// <summary>
	/// Use this attribute to turn an integer parameter into a layer enum
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
	public class LayerAttribute : PropertyAttribute
	{
		/// <summary>
		/// The default value to display
		/// </summary>
		public readonly int defaultValue = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="layerIndex">The layer index</param>
		public LayerAttribute(int layerIndex = 0)
		{
			if (string.IsNullOrEmpty(LayerMask.LayerToName(layerIndex)))
				layerIndex = 0;

			defaultValue = layerIndex;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="layerName">The layer name</param>
		public LayerAttribute(string layerName)
		{
			int layer = LayerMask.NameToLayer(layerName);
			defaultValue = layer > -1 ? layer : 0;
		}
	}

	[Serializable]
	class ArgumentCache2 : ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("objectArgument")]
		[SerializeField]
		private Object m_ObjectArgument;
		[SerializeField]
		[FormerlySerializedAs("objectArgumentAssemblyTypeName")]
		private string m_ObjectArgumentAssemblyTypeName;
		[FormerlySerializedAs("intArgument")]
		[SerializeField]
		private int m_IntArgument;
		[FormerlySerializedAs("floatArgument")]
		[SerializeField]
		private float m_FloatArgument;
		[SerializeField]
		[FormerlySerializedAs("stringArgument")]
		private string m_StringArgument;
		[SerializeField]
		private bool m_BoolArgument;
		[SerializeField]
		[FormerlySerializedAs("enumArgument")]
		private Enum m_EnumArgument;
		[SerializeField]
		[FormerlySerializedAs("vector2Argument")]
		private Vector2 m_Vector2Argument;
#if UNITY_2017_2_OR_NEWER
		[SerializeField]
		[FormerlySerializedAs("vector2IntArgument")]
		private Vector2Int m_Vector2IntArgument;
#endif
		[SerializeField]
		[FormerlySerializedAs("vector3Argument")]
		private Vector3 m_Vector3Argument;
#if UNITY_2017_2_OR_NEWER
		[SerializeField]
		[FormerlySerializedAs("vector3IntArgument")]
		private Vector3Int m_Vector3IntArgument;
#endif
		[SerializeField]
		[FormerlySerializedAs("vector4Argument")]
		private Vector4 m_Vector4Argument;
		[SerializeField]
		[FormerlySerializedAs("layerMaskArgument")]
		private LayerMask m_LayerMaskArgument;
		[SerializeField]
		[FormerlySerializedAs("colorArgument")]
		private Color m_ColorArgument;

		public Object unityObjectArgument
		{
			get { return m_ObjectArgument; }
			set
			{
				m_ObjectArgument = value;
				m_ObjectArgumentAssemblyTypeName = value == null ? string.Empty : value.GetType().AssemblyQualifiedName;
			}
		}

		public string unityObjectArgumentAssemblyTypeName
		{
			get { return m_ObjectArgumentAssemblyTypeName; }
		}

		public int intArgument
		{
			get { return m_IntArgument; }
			set
			{
				m_IntArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(int).AssemblyQualifiedName;
			}
		}

		public float floatArgument
		{
			get { return m_FloatArgument; }
			set
			{
				m_FloatArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(float).AssemblyQualifiedName;
			}
		}

		public string stringArgument
		{
			get { return m_StringArgument; }
			set
			{
				m_StringArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(string).AssemblyQualifiedName;
			}
		}

		public bool boolArgument
		{
			get { return m_BoolArgument; }
			set
			{
				m_BoolArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(bool).AssemblyQualifiedName;
			}
		}

		public Enum enumArgument
		{
			get { return m_EnumArgument; }
			set
			{
				m_EnumArgument = value;
				m_ObjectArgumentAssemblyTypeName = value == null ? string.Empty : value.GetType().AssemblyQualifiedName;
			}
		}

		public Vector2 vector2Argument
		{
			get { return m_Vector2Argument; }
			set
			{
				m_Vector2Argument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(Vector2).AssemblyQualifiedName;
			}
		}
#if UNITY_2017_2_OR_NEWER
		public Vector2Int vector2IntArgument
		{
			get { return m_Vector2IntArgument; }
			set
			{
				m_Vector2IntArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(Vector2Int).AssemblyQualifiedName;
			}
		}
#endif

		public Vector3 vector3Argument
		{
			get { return m_Vector3Argument; }
			set
			{
				m_Vector3Argument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(Vector3).AssemblyQualifiedName;
			}
		}
#if UNITY_2017_2_OR_NEWER
		public Vector3Int vector3IntArgument
		{
			get { return m_Vector3IntArgument; }
			set
			{
				m_Vector3IntArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(Vector3Int).AssemblyQualifiedName;
			}
		}
#endif

		public Vector4 vector4Argument
		{
			get { return m_Vector4Argument; }
			set
			{
				m_Vector4Argument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(Vector4).AssemblyQualifiedName;
			}
		}

		public LayerMask layerMaskArgument
		{
			get { return m_LayerMaskArgument; }
			set
			{
				m_LayerMaskArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(LayerMask).AssemblyQualifiedName;
			}
		}

		public Color colorArgument
		{
			get { return m_ColorArgument; }
			set
			{
				m_ColorArgument = value;
				m_ObjectArgumentAssemblyTypeName = typeof(Color).AssemblyQualifiedName;
			}
		}

		private void TidyAssemblyTypeName()
		{
			if (string.IsNullOrEmpty(m_ObjectArgumentAssemblyTypeName))
				return;

			int num = int.MaxValue;
			int val1_1 = m_ObjectArgumentAssemblyTypeName.IndexOf(", Version=");
			if (val1_1 != -1)
				num = Math.Min(val1_1, num);
			int val1_2 = m_ObjectArgumentAssemblyTypeName.IndexOf(", Culture=");
			if (val1_2 != -1)
				num = Math.Min(val1_2, num);
			int val1_3 = m_ObjectArgumentAssemblyTypeName.IndexOf(", PublicKeyToken=");
			if (val1_3 != -1)
				num = Math.Min(val1_3, num);
			if (num == int.MaxValue)
				return;

			m_ObjectArgumentAssemblyTypeName = m_ObjectArgumentAssemblyTypeName.Substring(0, num);
		}

		public void OnBeforeSerialize()
		{
			TidyAssemblyTypeName();
		}

		public void OnAfterDeserialize()
		{
			TidyAssemblyTypeName();
		}
	}

	public abstract class BaseInvokableCall2
	{
		protected string targetAssembly = null;
		protected string methodName = null;

		protected BaseInvokableCall2()
		{
		}

		protected BaseInvokableCall2(object target, MethodInfo function)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			if (function == null)
				throw new ArgumentNullException("function");

			targetAssembly = target.GetType().AssemblyQualifiedName;
			methodName = function.Name;
		}

		public abstract void Invoke(object[] args);

		protected static void ThrowOnInvalidArg<T>(object arg, string methodName, string targetAssembly)
		{
			if (arg != null && !(arg is T))
				throw new ArgumentException(string.Format("Passed argument 'arg' on method '{0}' for target '{1}' is of the wrong type. Type:{2} Expected:{3}", methodName, targetAssembly, arg.GetType(), typeof(T)));
		}

		protected static bool AllowInvoke(Delegate @delegate)
		{
			object target = @delegate.Target;
			if (target == null)
				return true;
			Object @object = target as Object;
			if (!ReferenceEquals(@object, null))
				return @object != null;
			return true;
		}

		public abstract bool Find(object targetObj, MethodInfo method);
	}

	class InvokableCall2 : BaseInvokableCall2
	{
		private event UnityAction Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), target, theFunction);
		}

		public InvokableCall2(UnityAction action)
		{
			Delegate += action;
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}

		public override void Invoke(object[] args)
		{
			if (AllowInvoke(Delegate))
				Delegate();
		}

		public void Invoke()
		{
			if (AllowInvoke(Delegate))
				Delegate();
		}
	}

	class InvokableCall2<T1> : BaseInvokableCall2
	{
		protected event UnityAction<T1> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1>)System.Delegate.CreateDelegate(typeof(UnityAction<T1>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 1)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0]);
		}

		public virtual void Invoke(T1 args0)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 2)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 2");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1]);
		}

		public void Invoke(T1 args0, T2 args1)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 3)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 3");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 4)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 4");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4, T5> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4, T5> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4, T5>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4, T5>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4, T5> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 5)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 5");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			ThrowOnInvalidArg<T5>(args[4], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3, T5 args4)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3, args4);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4, T5, T6> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4, T5, T6> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4, T5, T6>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4, T5, T6>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4, T5, T6> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 6)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 6");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			ThrowOnInvalidArg<T5>(args[4], methodName, targetAssembly);
			ThrowOnInvalidArg<T6>(args[5], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3, T5 args4, T6 args5)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3, args4, args5);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4, T5, T6, T7> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4, T5, T6, T7> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4, T5, T6, T7>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4, T5, T6, T7>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4, T5, T6, T7> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 7)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 7");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			ThrowOnInvalidArg<T5>(args[4], methodName, targetAssembly);
			ThrowOnInvalidArg<T6>(args[5], methodName, targetAssembly);
			ThrowOnInvalidArg<T7>(args[6], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3, T5 args4, T6 args5, T7 args6)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3, args4, args5, args6);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4, T5, T6, T7, T8> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4, T5, T6, T7, T8> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4, T5, T6, T7, T8>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4, T5, T6, T7, T8>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4, T5, T6, T7, T8> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 8)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 8");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			ThrowOnInvalidArg<T5>(args[4], methodName, targetAssembly);
			ThrowOnInvalidArg<T6>(args[5], methodName, targetAssembly);
			ThrowOnInvalidArg<T7>(args[6], methodName, targetAssembly);
			ThrowOnInvalidArg<T8>(args[7], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3, T5 args4, T6 args5, T7 args6, T8 args7)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3, args4, args5, args6, args7);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 9)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 9");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			ThrowOnInvalidArg<T5>(args[4], methodName, targetAssembly);
			ThrowOnInvalidArg<T6>(args[5], methodName, targetAssembly);
			ThrowOnInvalidArg<T7>(args[6], methodName, targetAssembly);
			ThrowOnInvalidArg<T8>(args[7], methodName, targetAssembly);
			ThrowOnInvalidArg<T9>(args[8], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7], (T9)args[8]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3, T5 args4, T6 args5, T7 args6, T8 args7, T9 args8)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3, args4, args5, args6, args7, args8);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class InvokableCall2<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : BaseInvokableCall2
	{
		protected event UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Delegate;

		public InvokableCall2(object target, MethodInfo theFunction) : base(target, theFunction)
		{
			Delegate += (UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)System.Delegate.CreateDelegate(typeof(UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>), target, theFunction);
		}

		public InvokableCall2(UnityAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
		{
			Delegate += action;
		}

		public override void Invoke(object[] args)
		{
			if (args.Length != 10)
				throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 10");
			ThrowOnInvalidArg<T1>(args[0], methodName, targetAssembly);
			ThrowOnInvalidArg<T2>(args[1], methodName, targetAssembly);
			ThrowOnInvalidArg<T3>(args[2], methodName, targetAssembly);
			ThrowOnInvalidArg<T4>(args[3], methodName, targetAssembly);
			ThrowOnInvalidArg<T5>(args[4], methodName, targetAssembly);
			ThrowOnInvalidArg<T6>(args[5], methodName, targetAssembly);
			ThrowOnInvalidArg<T7>(args[6], methodName, targetAssembly);
			ThrowOnInvalidArg<T8>(args[7], methodName, targetAssembly);
			ThrowOnInvalidArg<T9>(args[8], methodName, targetAssembly);
			ThrowOnInvalidArg<T10>(args[9], methodName, targetAssembly);
			if (!AllowInvoke(Delegate))
				return;
			Delegate((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7], (T9)args[8], (T10)args[9]);
		}

		public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3, T5 args4, T6 args5, T7 args6, T8 args7, T9 args8, T10 args9)
		{
			if (AllowInvoke(Delegate))
				Delegate(args0, args1, args2, args3, args4, args5, args6, args7, args8, args9);
		}

		public override bool Find(object targetObj, MethodInfo method)
		{
			return Delegate.Target == targetObj && Delegate.Method.Equals(method);
		}
	}

	class CachedInvokableCall2<T1> : InvokableCall2<T1>
	{
		private readonly object[] m_Args = new object[1];

		public CachedInvokableCall2(Object target, MethodInfo theFunction, T1 arg) : base(target, theFunction)
		{
			m_Args[0] = arg;
		}

		public override void Invoke(object[] args)
		{
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2> : InvokableCall2<T1, T2>
	{
		private readonly object[] m_Args = new object[2];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3> : InvokableCall2<T1, T2, T3>
	{
		private readonly object[] m_Args = new object[3];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4> : InvokableCall2<T1, T2, T3, T4>
	{
		private readonly object[] m_Args = new object[4];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4, T5> : InvokableCall2<T1, T2, T3, T4, T5>
	{
		private readonly object[] m_Args = new object[5];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_Args[4] = arg4;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4, T5, T6> : InvokableCall2<T1, T2, T3, T4, T5, T6>
	{
		private readonly object[] m_Args = new object[6];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, T6 arg5) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_Args[4] = arg4;
			m_Args[5] = arg5;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4, T5, T6, T7> : InvokableCall2<T1, T2, T3, T4, T5, T6, T7>
	{
		private readonly object[] m_Args = new object[7];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, T6 arg5, T7 arg6) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_Args[4] = arg4;
			m_Args[5] = arg5;
			m_Args[6] = arg6;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4, T5, T6, T7, T8> : InvokableCall2<T1, T2, T3, T4, T5, T6, T7, T8>
	{
		private readonly object[] m_Args = new object[8];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, T6 arg5, T7 arg6, T8 arg7) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_Args[4] = arg4;
			m_Args[5] = arg5;
			m_Args[6] = arg6;
			m_Args[7] = arg7;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4, T5, T6, T7, T8, T9> : InvokableCall2<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		private readonly object[] m_Args = new object[9];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, T6 arg5, T7 arg6, T8 arg7, T9 arg8) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_Args[4] = arg4;
			m_Args[5] = arg5;
			m_Args[6] = arg6;
			m_Args[7] = arg7;
			m_Args[8] = arg8;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	class UpdatableInvokableCall<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : InvokableCall2<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
	{
		private readonly object[] m_Args = new object[10];

		private bool m_IsEventDefined = false;

		public UpdatableInvokableCall(Object target, MethodInfo theFunction, bool isEventDefined, T1 arg0, T2 arg1, T3 arg2, T4 arg3, T5 arg4, T6 arg5, T7 arg6, T8 arg7, T9 arg8, T10 arg9) : base(target, theFunction)
		{
			m_Args[0] = arg0;
			m_Args[1] = arg1;
			m_Args[2] = arg2;
			m_Args[3] = arg3;
			m_Args[4] = arg4;
			m_Args[5] = arg5;
			m_Args[6] = arg6;
			m_Args[7] = arg7;
			m_Args[8] = arg8;
			m_Args[9] = arg9;
			m_IsEventDefined = isEventDefined;
		}

		public override void Invoke(object[] args)
		{
			if (m_IsEventDefined)
				for (int i = 0; i < args.Length; i++)
					m_Args[i] = args[i];
			base.Invoke(m_Args);
		}
	}

	[Serializable]
	class PersistentCall2
	{
		[FormerlySerializedAs("arguments")]
		[SerializeField]
		private List<ArgumentCache2> m_Arguments = new List<ArgumentCache2>();
		[SerializeField]
		[FormerlySerializedAs("enabled")]
		[FormerlySerializedAs("m_Enabled")]
		private UnityEventCallState m_CallState = UnityEventCallState.RuntimeOnly;
		[SerializeField]
		[FormerlySerializedAs("instance")]
		private Object m_Target;
		[FormerlySerializedAs("methodName")]
		[SerializeField]
		private string m_MethodName;
		[SerializeField]
		[FormerlySerializedAs("modes")]
		private List<PersistentListenerMode2> m_Modes = new List<PersistentListenerMode2>();

		public Object target
		{
			get { return m_Target; }
		}

		public string methodName
		{
			get { return m_MethodName; }
		}

		public List<ArgumentCache2> arguments
		{
			get { return m_Arguments; }
		}

		public UnityEventCallState callState
		{
			get { return m_CallState; }
			set { m_CallState = value; }
		}

		public List<PersistentListenerMode2> modes
		{
			get { return m_Modes; }
			set { m_Modes = value; }
		}

		public bool IsValid()
		{
			if (target != null)
				return !string.IsNullOrEmpty(methodName);
			return false;
		}

		public BaseInvokableCall2 GetRuntimeCall(UnityEventBase2 theEvent, object[] parameters)
		{
			if (m_CallState == UnityEventCallState.RuntimeOnly && !Application.isPlaying)
				return null;
			if (m_CallState == UnityEventCallState.Off || theEvent == null)
				return null;
			if (m_Modes.Count == 0)
				return null;

			MethodInfo method = theEvent.FindMethod(this);
			if (method == null)
				return null;

			if (m_Modes.Count == 1)
			{
				switch (m_Modes[0])
				{
					case PersistentListenerMode2.EventDefined:
						return theEvent.GetDelegate(target, method);
					case PersistentListenerMode2.Void:
						return new InvokableCall2(target, method);
					case PersistentListenerMode2.Object:
						return GetObjectCall(target, method, m_Arguments[0]);
					case PersistentListenerMode2.Int:
						return new CachedInvokableCall2<int>(target, method, m_Arguments[0].intArgument);
					case PersistentListenerMode2.Float:
						return new CachedInvokableCall2<float>(target, method, m_Arguments[0].floatArgument);
					case PersistentListenerMode2.String:
						return new CachedInvokableCall2<string>(target, method, m_Arguments[0].stringArgument);
					case PersistentListenerMode2.Bool:
						return new CachedInvokableCall2<bool>(target, method, m_Arguments[0].boolArgument);
					case PersistentListenerMode2.Enum:
						return new CachedInvokableCall2<int>(target, method, m_Arguments[0].intArgument);
					case PersistentListenerMode2.Vector2:
						return new CachedInvokableCall2<Vector2>(target, method, m_Arguments[0].vector2Argument);
#if UNITY_2017_2_OR_NEWER
					case PersistentListenerMode2.Vector2Int:
						return new CachedInvokableCall2<Vector2Int>(target, method, m_Arguments[0].vector2IntArgument);
#endif
					case PersistentListenerMode2.Vector3:
						return new CachedInvokableCall2<Vector3>(target, method, m_Arguments[0].vector3Argument);
#if UNITY_2017_2_OR_NEWER
					case PersistentListenerMode2.Vector3Int:
						return new CachedInvokableCall2<Vector3Int>(target, method, m_Arguments[0].vector3IntArgument);
#endif
					case PersistentListenerMode2.Vector4:
						return new CachedInvokableCall2<Vector4>(target, method, m_Arguments[0].vector4Argument);
					case PersistentListenerMode2.LayerMask:
						return new CachedInvokableCall2<LayerMask>(target, method, m_Arguments[0].layerMaskArgument);
					case PersistentListenerMode2.Color:
						return new CachedInvokableCall2<Color>(target, method, m_Arguments[0].colorArgument);
					default:
						return null;
				}
			}

			return GetInvokableCall(target, method, m_Modes, m_Arguments.ToArray(), parameters);
		}

		private static BaseInvokableCall2 GetObjectCall(Object target, MethodInfo method, ArgumentCache2 argument)
		{
			Type type = typeof(Object);
			if (!string.IsNullOrEmpty(argument.unityObjectArgumentAssemblyTypeName))
				type = Type.GetType(argument.unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object);
			ConstructorInfo constructor = typeof(CachedInvokableCall2<>).MakeGenericType(type).GetConstructor(new Type[3] { typeof(Object), typeof(MethodInfo), type });
			Object @object = argument.unityObjectArgument;
			if (@object != null && !type.IsAssignableFrom(@object.GetType()))
				@object = null;
			return constructor.Invoke(new object[3] { target, method, @object }) as BaseInvokableCall2;
		}

		private static BaseInvokableCall2 GetInvokableCall(Object target, MethodInfo method, List<PersistentListenerMode2> modes, ArgumentCache2[] arguments, object[] informedParameters)
		{
			Type[] typeArguments = new Type[arguments.Length];
			Type[] types = new Type[arguments.Length + 3];
			object[] constructorParameters = new object[arguments.Length + 3];

			types[0] = typeof(Object);
			types[1] = typeof(MethodInfo);
			types[2] = typeof(bool);

			constructorParameters[0] = target;
			constructorParameters[1] = method;
			constructorParameters[2] = false;

			for (int i = 0; i < typeArguments.Length; i++)
			{
				typeArguments[i] = Type.GetType(arguments[i].unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object);
				types[i + 3] = typeArguments[i];

				if (modes[i] == PersistentListenerMode2.EventDefined)
				{
					constructorParameters[2] = true;
					constructorParameters[i + 3] = informedParameters[i];
				}
				else
					constructorParameters[i + 3] = GetValue(typeArguments[i], arguments[i]);
			}

			Type invokableType = typeof(InvokableCall2);

			if (typeArguments.Length == 2)
				invokableType = typeof(UpdatableInvokableCall<,>);
			else if (typeArguments.Length == 3)
				invokableType = typeof(UpdatableInvokableCall<,,>);
			else if (typeArguments.Length == 4)
				invokableType = typeof(UpdatableInvokableCall<,,,>);
			else if (typeArguments.Length == 5)
				invokableType = typeof(UpdatableInvokableCall<,,,,>);
			else if (typeArguments.Length == 6)
				invokableType = typeof(UpdatableInvokableCall<,,,,,>);
			else if (typeArguments.Length == 7)
				invokableType = typeof(UpdatableInvokableCall<,,,,,,>);
			else if (typeArguments.Length == 8)
				invokableType = typeof(UpdatableInvokableCall<,,,,,,,>);
			else if (typeArguments.Length == 9)
				invokableType = typeof(UpdatableInvokableCall<,,,,,,,,>);
			else if (typeArguments.Length == 10)
				invokableType = typeof(UpdatableInvokableCall<,,,,,,,,,>);

			ConstructorInfo constructor = invokableType.MakeGenericType(typeArguments).GetConstructor(types);

			return constructor == null ? null : constructor.Invoke(constructorParameters) as BaseInvokableCall2;
		}

		private static object GetValue(Type type, ArgumentCache2 argument)
		{
			if (type == typeof(int) || type.IsEnum)
				return argument.intArgument;
			else if (type == typeof(float))
				return argument.floatArgument;
			else if (type == typeof(string))
				return argument.stringArgument;
			else if (type == typeof(bool))
				return argument.boolArgument;
			else if (type == typeof(Vector2))
				return argument.vector2Argument;
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(Vector2Int))
				return argument.vector2IntArgument;
#endif
			else if (type == typeof(Vector3))
				return argument.vector3Argument;
#if UNITY_2017_2_OR_NEWER
			else if (type == typeof(Vector3Int))
				return argument.vector3IntArgument;
#endif
			else if (type == typeof(Vector4))
				return argument.vector4Argument;
			else if (type == typeof(LayerMask))
				return argument.layerMaskArgument;
			else if (type == typeof(Color))
				return argument.colorArgument;

			return argument.unityObjectArgument;
		}

		public void RegisterPersistentListener(Object ttarget, string methodName)
		{
			m_Target = ttarget;
			m_MethodName = methodName;
		}

		public void UnregisterPersistentListener()
		{
			m_MethodName = string.Empty;
			m_Target = null;
		}
	}

	[Serializable]
	class PersistentCallGroup2
	{
		[SerializeField]
		[FormerlySerializedAs("m_Listeners")]
		private List<PersistentCall2> m_Calls;

		public int Count
		{
			get { return m_Calls.Count; }
		}

		public PersistentCallGroup2()
		{
			m_Calls = new List<PersistentCall2>();
		}

		public PersistentCall2 GetListener(int index)
		{
			return m_Calls[index];
		}

		public IEnumerable<PersistentCall2> GetListeners()
		{
			return m_Calls;
		}

		public void AddListener()
		{
			m_Calls.Add(new PersistentCall2());
		}

		public void AddListener(PersistentCall2 call)
		{
			m_Calls.Add(call);
		}

		public void RemoveListener(int index)
		{
			m_Calls.RemoveAt(index);
		}

		public void Clear()
		{
			m_Calls.Clear();
		}

		public void RegisterEventPersistentListener(int index, Object targetObj, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.EventDefined);
		}

		public void RegisterVoidPersistentListener(int index, Object targetObj, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Void);
		}

		public void RegisterObjectPersistentListener(int index, Object targetObj, Object argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Object);
			listener.arguments.Add(new ArgumentCache2() { unityObjectArgument = argument });
		}

		public void RegisterIntPersistentListener(int index, Object targetObj, int argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Int);
			listener.arguments.Add(new ArgumentCache2() { intArgument = argument });
		}

		public void RegisterFloatPersistentListener(int index, Object targetObj, float argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Float);
			listener.arguments.Add(new ArgumentCache2() { floatArgument = argument });
		}

		public void RegisterStringPersistentListener(int index, Object targetObj, string argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.String);
			listener.arguments.Add(new ArgumentCache2() { stringArgument = argument });
		}

		public void RegisterBoolPersistentListener(int index, Object targetObj, bool argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Bool);
			listener.arguments.Add(new ArgumentCache2() { boolArgument = argument });
		}

		public void RegisterEnumPersistentListener(int index, Object targetObj, Enum argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Enum);
			listener.arguments.Add(new ArgumentCache2() { enumArgument = argument });
		}

		public void RegisterVector2PersistentListener(int index, Object targetObj, Vector2 argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Vector2);
			listener.arguments.Add(new ArgumentCache2() { vector2Argument = argument });
		}
#if UNITY_2017_2_OR_NEWER
		public void RegisterVector2IntPersistentListener(int index, Object targetObj, Vector2Int argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Vector2Int);
			listener.arguments.Add(new ArgumentCache2() { vector2IntArgument = argument });
		}
#endif

		public void RegisterVector3PersistentListener(int index, Object targetObj, Vector3 argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Vector3);
			listener.arguments.Add(new ArgumentCache2() { vector3Argument = argument });
		}
#if UNITY_2017_2_OR_NEWER
		public void RegisterVector3IntPersistentListener(int index, Object targetObj, Vector3Int argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Vector3Int);
			listener.arguments.Add(new ArgumentCache2() { vector3IntArgument = argument });
		}
#endif

		public void RegisterVector4PersistentListener(int index, Object targetObj, Vector4 argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.Vector4);
			listener.arguments.Add(new ArgumentCache2() { vector4Argument = argument });
		}

		public void RegisterLayerMaskPersistentListener(int index, Object targetObj, LayerMask argument, string methodName)
		{
			PersistentCall2 listener = GetListener(index);
			listener.RegisterPersistentListener(targetObj, methodName);
			listener.modes.Add(PersistentListenerMode2.LayerMask);
			listener.arguments.Add(new ArgumentCache2() { layerMaskArgument = argument });
		}

		public void UnregisterPersistentListener(int index)
		{
			GetListener(index).UnregisterPersistentListener();
		}

		public void RemoveListeners(Object target, string methodName)
		{
			List<PersistentCall2> persistentCallList = new List<PersistentCall2>();
			for (int index = 0; index < m_Calls.Count; ++index)
			{
				if (m_Calls[index].target == target && m_Calls[index].methodName == methodName)
					persistentCallList.Add(m_Calls[index]);
			}
			m_Calls.RemoveAll(new Predicate<PersistentCall2>(persistentCallList.Contains));
		}

		public void Initialize(InvokableCallList2 invokableList, UnityEventBase2 unityEventBase, object[] parameters)
		{
			foreach (var persistentCall in m_Calls)
			{
				if (!persistentCall.IsValid())
					continue;

				var call = persistentCall.GetRuntimeCall(unityEventBase, parameters);
				if (call != null)
					invokableList.AddPersistentInvokableCall(call);
			}
		}
	}

	class InvokableCallList2
	{
		private readonly List<BaseInvokableCall2> m_PersistentCalls = new List<BaseInvokableCall2>();
		private readonly List<BaseInvokableCall2> m_RuntimeCalls = new List<BaseInvokableCall2>();

		private readonly List<BaseInvokableCall2> m_ExecutingCalls = new List<BaseInvokableCall2>();

		private bool m_NeedsUpdate = true;

		public int Count
		{
			get { return m_PersistentCalls.Count + m_RuntimeCalls.Count; }
		}

		public void AddPersistentInvokableCall(BaseInvokableCall2 call)
		{
			m_PersistentCalls.Add(call);
			m_NeedsUpdate = true;
		}

		public void AddListener(BaseInvokableCall2 call)
		{
			m_RuntimeCalls.Add(call);
			m_NeedsUpdate = true;
		}

		public void RemoveListener(object targetObj, MethodInfo method)
		{
			List<BaseInvokableCall2> baseInvokableCallList = new List<BaseInvokableCall2>();
			for (int index = 0; index < m_RuntimeCalls.Count; ++index)
			{
				if (m_RuntimeCalls[index].Find(targetObj, method))
					baseInvokableCallList.Add(m_RuntimeCalls[index]);
			}
			m_RuntimeCalls.RemoveAll(new Predicate<BaseInvokableCall2>(baseInvokableCallList.Contains));
			m_NeedsUpdate = true;
		}

		public void Clear()
		{
			m_RuntimeCalls.Clear();
			m_NeedsUpdate = true;
		}

		public void ClearPersistent()
		{
			m_PersistentCalls.Clear();
			m_NeedsUpdate = true;
		}

		public List<BaseInvokableCall2> PrepareInvoke()
		{
			if (m_NeedsUpdate)
			{
				m_ExecutingCalls.Clear();
				m_ExecutingCalls.AddRange(m_PersistentCalls);
				m_ExecutingCalls.AddRange(m_RuntimeCalls);
				m_NeedsUpdate = false;
			}

			return m_ExecutingCalls;
		}

		public void Invoke(object[] parameters)
		{
			if (m_NeedsUpdate)
			{
				m_ExecutingCalls.Clear();
				m_ExecutingCalls.AddRange(m_PersistentCalls);
				m_ExecutingCalls.AddRange(m_RuntimeCalls);
				m_NeedsUpdate = false;
			}
			for (int index = 0; index < m_ExecutingCalls.Count; ++index)
				m_ExecutingCalls[index].Invoke(parameters);
		}
	}

	[Serializable]
	public abstract class UnityEventBase2 : ISerializationCallbackReceiver
	{
		private bool m_CallsDirty = true;
		private InvokableCallList2 m_Calls;

		[SerializeField]
		[FormerlySerializedAs("m_PersistentListeners")]
		private PersistentCallGroup2 m_PersistentCalls;

#pragma warning disable 414 //used by serialized properties
		[SerializeField]
		private string m_TypeName;
#pragma warning restore 414

		protected UnityEventBase2()
		{
			m_Calls = new InvokableCallList2();
			m_PersistentCalls = new PersistentCallGroup2();
			m_TypeName = GetType().AssemblyQualifiedName;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			DirtyPersistentCalls();
			m_TypeName = GetType().AssemblyQualifiedName;
		}

		protected abstract MethodInfo FindMethod_Impl(object targetObj, string name);

		internal abstract BaseInvokableCall2 GetDelegate(object target, MethodInfo theFunction);

		internal MethodInfo FindMethod(PersistentCall2 call)
		{
			Type[] argumentTypes = new Type[call.arguments.Count];

			for (int i = 0; i < argumentTypes.Length; i++)
				argumentTypes[i] = Type.GetType(call.arguments[i].unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object);

			return GetValidMethodInfo(call.target, call.methodName, argumentTypes);
		}

		internal MethodInfo FindMethod(object listener, string name, PersistentListenerMode2 mode, Type argumentType)
		{
			switch (mode)
			{
				case PersistentListenerMode2.EventDefined:
					return FindMethod_Impl(listener, name);
				case PersistentListenerMode2.Void:
					return GetValidMethodInfo(listener, name, new Type[0]);
				case PersistentListenerMode2.Object:
					return GetValidMethodInfo(listener, name, new Type[1] { argumentType ?? typeof(Object) });
				case PersistentListenerMode2.Int:
					return GetValidMethodInfo(listener, name, new Type[1] { typeof(int) });
				case PersistentListenerMode2.Float:
					return GetValidMethodInfo(listener, name, new Type[1] { typeof(float) });
				case PersistentListenerMode2.String:
					return GetValidMethodInfo(listener, name, new Type[1] { typeof(string) });
				case PersistentListenerMode2.Bool:
					return GetValidMethodInfo(listener, name, new Type[1] { typeof(bool) });
				case PersistentListenerMode2.Enum:
					return GetValidMethodInfo(listener, name, new Type[1] { typeof(Enum) });
				default:
					return null;
			}
		}

		public MethodInfo FindMethod(object listener, string name, PersistentListenerMode2 mode, Type[] argumentTypes = null)
		{
			if (mode == PersistentListenerMode2.EventDefined)
				return FindMethod_Impl(listener, name);

			return GetValidMethodInfo(listener, name, argumentTypes);
		}

		/// <summary>
		///   <para>Get the number of registered persistent listeners.</para>
		/// </summary>
		public int GetPersistentEventCount()
		{
			return m_PersistentCalls.Count;
		}

		/// <summary>
		///   <para>Get the target component of the listener at index index.</para>
		/// </summary>
		/// <param name="index">Index of the listener to query.</param>
		public Object GetPersistentTarget(int index)
		{
			PersistentCall2 listener = m_PersistentCalls.GetListener(index);
			if (listener != null)
				return listener.target;
			return null;
		}

		/// <summary>
		///   <para>Get the target method name of the listener at index index.</para>
		/// </summary>
		/// <param name="index">Index of the listener to query.</param>
		public string GetPersistentMethodName(int index)
		{
			PersistentCall2 listener = m_PersistentCalls.GetListener(index);
			if (listener != null)
				return listener.methodName;
			return string.Empty;
		}

		private void DirtyPersistentCalls()
		{
			m_Calls.ClearPersistent();
			m_CallsDirty = true;
		}

		private void RebuildPersistentCallsIfNeeded(object[] parameters)
		{
			if (!m_CallsDirty)
				return;
			m_PersistentCalls.Initialize(m_Calls, this, parameters);
			m_CallsDirty = false;
		}

		/// <summary>
		///   <para>Modify the execution state of a persistent listener.</para>
		/// </summary>
		/// <param name="index">Index of the listener to query.</param>
		/// <param name="state">State to set.</param>
		public void SetPersistentListenerState(int index, UnityEventCallState state)
		{
			PersistentCall2 listener = m_PersistentCalls.GetListener(index);
			if (listener != null)
				listener.callState = state;
			DirtyPersistentCalls();
		}

		protected void AddListener(object targetObj, MethodInfo method)
		{
			m_Calls.AddListener(GetDelegate(targetObj, method));
		}

		internal void AddCall(BaseInvokableCall2 call)
		{
			m_Calls.AddListener(call);
		}

		protected void RemoveListener(object targetObj, MethodInfo method)
		{
			m_Calls.RemoveListener(targetObj, method);
		}

		/// <summary>
		/// Remove all listeners from the event.
		/// </summary>
		public void RemoveAllListeners()
		{
			m_Calls.Clear();
		}

		protected List<BaseInvokableCall2> PrepareInvoke(params object[] parameters)
		{
			RebuildPersistentCallsIfNeeded(parameters);
			return m_Calls.PrepareInvoke();
		}

		protected void Invoke(object[] parameters)
		{
			List<BaseInvokableCall2> calls = PrepareInvoke(parameters);

			for (var i = 0; i < calls.Count; i++)
				calls[i].Invoke(parameters);
		}

		public override string ToString()
		{
			return base.ToString() + " " + GetType().FullName;
		}

		/// <summary>
		///   <para>Given an object, function name, and a list of argument types; find the method that matches.</para>
		/// </summary>
		/// <param name="obj">Object to search for the method.</param>
		/// <param name="functionName">Function name to search for.</param>
		/// <param name="argumentTypes">Argument types for the function.</param>
		public static MethodInfo GetValidMethodInfo(object obj, string functionName, Type[] argumentTypes)
		{
			for (Type type = obj.GetType(); type != typeof(object) && type != null; type = type.BaseType)
			{
				MethodInfo method = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null);
				if (method != null)
				{
					ParameterInfo[] parameters = method.GetParameters();

					if (parameters.Length != argumentTypes.Length)
						continue;

					bool flag = true;
					int index = 0;
					foreach (ParameterInfo parameterInfo in parameters)
					{
						flag = argumentTypes[index].IsPrimitive == parameterInfo.ParameterType.IsPrimitive;
						if (flag)
							++index;
						else
							break;
					}
					if (flag)
						return method;
				}
			}
			return null;
		}

		protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode2 mode)
		{
			return ValidateRegistration(method, targetObj, mode, typeof(Object));
		}

		protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode2 mode, Type argumentType)
		{
			if (method == null)
				throw new ArgumentNullException("method", string.Format("Can not register null method on {0} for callback!", targetObj));
			Object @object = targetObj as Object;
			if (@object == null || @object.GetInstanceID() == 0)
				throw new ArgumentException(string.Format("Could not register callback {0} on {1}. The class {2} does not derive from UnityEngine.Object", method.Name, targetObj, (targetObj != null ? targetObj.GetType().ToString() : "null")));
			if (method.IsStatic)
				throw new ArgumentException(string.Format("Could not register listener {0} on {1} static functions are not supported.", method, GetType()));
			if (FindMethod(targetObj, method.Name, mode, argumentType) != null)
				return true;
			Debug.LogWarning(string.Format("Could not register listener {0}.{1} on {2} the method could not be found.", targetObj, method, GetType()));
			return false;
		}

		internal void AddPersistentListener()
		{
			m_PersistentCalls.AddListener();
		}

		protected void RegisterPersistentListener(int index, object targetObj, MethodInfo method)
		{
			if (!ValidateRegistration(method, targetObj, PersistentListenerMode2.EventDefined))
				return;
			m_PersistentCalls.RegisterEventPersistentListener(index, targetObj as Object, method.Name);
			DirtyPersistentCalls();
		}

		internal void RemovePersistentListener(Object target, MethodInfo method)
		{
			if (method == null || method.IsStatic || (target == null || target.GetInstanceID() == 0))
				return;
			m_PersistentCalls.RemoveListeners(target, method.Name);
			DirtyPersistentCalls();
		}

		internal void RemovePersistentListener(int index)
		{
			m_PersistentCalls.RemoveListener(index);
			DirtyPersistentCalls();
		}

		internal void UnregisterPersistentListener(int index)
		{
			m_PersistentCalls.UnregisterPersistentListener(index);
			DirtyPersistentCalls();
		}

		internal void AddVoidPersistentListener(UnityAction call)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterVoidPersistentListener(persistentEventCount, call);
		}

		internal void RegisterVoidPersistentListener(int index, UnityAction call)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Void))
					return;
				m_PersistentCalls.RegisterVoidPersistentListener(index, call.Target as Object, call.Method.Name);
				DirtyPersistentCalls();
			}
		}

		internal void AddIntPersistentListener(UnityAction<int> call, int argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterIntPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterIntPersistentListener(int index, UnityAction<int> call, int argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Int))
					return;
				m_PersistentCalls.RegisterIntPersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}

		internal void AddFloatPersistentListener(UnityAction<float> call, float argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterFloatPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterFloatPersistentListener(int index, UnityAction<float> call, float argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Float))
					return;
				m_PersistentCalls.RegisterFloatPersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}

		internal void AddBoolPersistentListener(UnityAction<bool> call, bool argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterBoolPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterBoolPersistentListener(int index, UnityAction<bool> call, bool argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Bool))
					return;
				m_PersistentCalls.RegisterBoolPersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}

		internal void AddEnumPersistentListener(UnityAction<Enum> call, Enum argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterEnumPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterEnumPersistentListener(int index, UnityAction<Enum> call, Enum argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Bool))
					return;
				m_PersistentCalls.RegisterEnumPersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}

		internal void AddStringPersistentListener(UnityAction<string> call, string argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterStringPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterStringPersistentListener(int index, UnityAction<string> call, string argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.String))
					return;
				m_PersistentCalls.RegisterStringPersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}

		internal void AddObjectPersistentListener<T>(UnityAction<T> call, T argument) where T : Object
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterObjectPersistentListener<T>(persistentEventCount, call, argument);
		}

		internal void RegisterObjectPersistentListener<T>(int index, UnityAction<T> call, T argument) where T : Object
		{
			if (call == null)
				throw new ArgumentNullException("call", "Registering a Listener requires a non null call");
			if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Object, !(argument == null) ? argument.GetType() : typeof(Object)))
				return;
			m_PersistentCalls.RegisterObjectPersistentListener(index, call.Target as Object, argument, call.Method.Name);
			DirtyPersistentCalls();
		}

		internal void AddVector2PersistentListener(UnityAction<Vector2> call, Vector2 argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterVector2PersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterVector2PersistentListener(int index, UnityAction<Vector2> call, Vector2 argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Vector2))
					return;
				m_PersistentCalls.RegisterVector2PersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}
#if UNITY_2017_2_OR_NEWER
		internal void AddVector2IntPersistentListener(UnityAction<Vector2Int> call, Vector2Int argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterVector2IntPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterVector2IntPersistentListener(int index, UnityAction<Vector2Int> call, Vector2Int argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Vector2Int))
					return;
				m_PersistentCalls.RegisterVector2PersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}
#endif

		internal void AddVector3PersistentListener(UnityAction<Vector3> call, Vector3 argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterVector3PersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterVector3PersistentListener(int index, UnityAction<Vector3> call, Vector3 argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Bool))
					return;
				m_PersistentCalls.RegisterVector3PersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}
#if UNITY_2017_2_OR_NEWER
		internal void AddVector3IntPersistentListener(UnityAction<Vector3Int> call, Vector3Int argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterVector3IntPersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterVector3IntPersistentListener(int index, UnityAction<Vector3Int> call, Vector3Int argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Bool))
					return;
				m_PersistentCalls.RegisterVector3IntPersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}
#endif

		internal void AddVector4PersistentListener(UnityAction<Vector4> call, Vector4 argument)
		{
			int persistentEventCount = GetPersistentEventCount();
			AddPersistentListener();
			RegisterVector4PersistentListener(persistentEventCount, call, argument);
		}

		internal void RegisterVector4PersistentListener(int index, UnityAction<Vector4> call, Vector4 argument)
		{
			if (call == null)
			{
				Debug.LogWarning("Registering a Listener requires an action");
			}
			else
			{
				if (!ValidateRegistration(call.Method, call.Target, PersistentListenerMode2.Bool))
					return;
				m_PersistentCalls.RegisterVector4PersistentListener(index, call.Target as Object, argument, call.Method.Name);
				DirtyPersistentCalls();
			}
		}
	}
}