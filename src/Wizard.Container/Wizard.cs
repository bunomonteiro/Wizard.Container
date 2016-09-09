using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;

namespace Wizard.Container
{
	/// <summary>
	///     The simplest IoC container based on Rui Jarimba basic IoC container
	/// </summary>
	public class Wizard
	{
		/// <summary>
		///     Key: object containing the type of the object to resolve and the name of the instance (if any);
		///     Value: delegate that creates the instance of the object
		/// </summary>
		private readonly ConcurrentDictionary<MappingKey, Func<dynamic, object>> _mappings;

		/// <summary>
		///     Creates a new instance of <see cref="Wizard" />
		/// </summary>
		public Wizard() { this._mappings = new ConcurrentDictionary<MappingKey, Func<dynamic, object>>(); }

		/// <summary>
		///     Gets the number of registered types.
		/// </summary>
		public int Count => this._mappings.Count;

		/// <summary>
		///     Register a type mapping
		/// </summary>
		/// <param name="type">Type that will be requested and returned</param>
		/// <param name="instanceName">Instance name (optional)</param>
		public void Register(Type type, string instanceName = null) => this.Register(type, type, instanceName);

		/// <summary>
		///     Register a type mapping
		/// </summary>
		/// <param name="from">Type that will be requested</param>
		/// <param name="to">Type that will actually be returned</param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="InvalidOperationException" />
		public void Register(Type from, Type to, string instanceName = null)
		{
			if (to == null) { throw new ArgumentNullException(nameof(to)); }

			if (!from.GetTypeInfo().IsAssignableFrom(to.GetTypeInfo()))
			{
				throw new InvalidOperationException(
					$"Error trying to register the instance: '{@from.FullName}' is not assignable from '{to.FullName}'");
			}

			Func<dynamic, object> createInstanceDelegate = args => Activator.CreateInstance(to);
			this.Register(from, createInstanceDelegate, instanceName);
		}

		/// <summary>
		///     Register a type mapping
		/// </summary>
		/// <typeparam name="TFrom">Type that will be requested</typeparam>
		/// <typeparam name="TTo">Type that will actually be returned</typeparam>
		/// <param name="instanceName">Instance name (optional)</param>
		public void Register<TFrom, TTo>(string instanceName = null) where TTo : TFrom
			=> this.Register(typeof (TFrom), typeof (TTo), instanceName);

		/// <summary>
		///     Register a type mapping
		/// </summary>
		/// <param name="type">Type that will be requested</param>
		/// <param name="createInstanceDelegate">
		///     A delegate that will be used to
		///     create an instance of the requested object
		/// </param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="InvalidOperationException" />
		public void Register(Type type, Func<dynamic, object> createInstanceDelegate, string instanceName = null)
		{
			if (type == null) { throw new ArgumentNullException(nameof(type)); }

			if (createInstanceDelegate == null) { throw new ArgumentNullException(nameof(createInstanceDelegate)); }

			var key = new MappingKey(type, instanceName);

			this._mappings.TryAdd(key, createInstanceDelegate);
		}

		/// <summary>
		///     Register a type mapping
		/// </summary>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <typeparam name="T">Type that will be requested and returned</typeparam>
		public void Register<T>(string instanceName = null) => this.Register(typeof (T), instanceName);


		/// <summary>
		///     Register a type mapping
		/// </summary>
		/// <typeparam name="T">Type that will be requested</typeparam>
		/// <param name="createInstanceDelegate">
		///     A delegate that will be used to
		///     create an instance of the requested object
		/// </param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <exception cref="ArgumentNullException" />
		public void Register<T>(Func<dynamic, T> createInstanceDelegate, string instanceName = null)
		{
			if (createInstanceDelegate == null) { throw new ArgumentNullException(nameof(createInstanceDelegate)); }

			var createInstance = createInstanceDelegate as Func<dynamic, object>;
			this.Register(typeof (T), createInstance, instanceName);
		}

		/// <summary>
		///     Unregister a type mapping
		/// </summary>
		/// <typeparam name="TFrom">Type that will be unregistered</typeparam>
		/// <param name="instanceName">
		///     Instance name (optional).
		/// </param>
		public void Unregister<TFrom>(string instanceName = null) => this.Unregister(typeof (TFrom), instanceName);


		/// <summary>
		///     Unregister a type mapping
		/// </summary>
		/// <param name="from">Type that will be unregistered</param>
		/// <param name="instanceName">
		///     Instance name (optional).
		/// </param>
		/// <exception cref="ArgumentNullException" />
		public void Unregister(Type from, string instanceName = null)
		{
			if (from == null) { throw new ArgumentNullException(nameof(@from)); }

			var key = new MappingKey(from, instanceName);
			if (!this._mappings.ContainsKey(key)) { return; }
			Func<dynamic, object> outValue;
			this._mappings.TryRemove(key, out outValue);
		}

		/// <summary>
		///     Unregister all type mapping
		/// </summary>
		public void UnregisterAll() => this._mappings.Clear();

		/// <summary>
		///     Check if a particular type/instance name has been registered with the container
		/// </summary>
		/// <param name="type">Type to check registration for</param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>
		///     <c>true</c>if the type/instance name has been registered
		///     with the container; otherwise <c>false</c>
		/// </returns>
		/// <exception cref="ArgumentNullException" />
		public bool IsRegistered(Type type, string instanceName = null)
		{
			if (type == null) { throw new ArgumentNullException(nameof(type)); }

			var key = new MappingKey(type, instanceName);
			return this._mappings.ContainsKey(key);
		}

		/// <summary>
		///     Check if a particular type/instance name has been registered with the container
		/// </summary>
		/// <typeparam name="T">Type to check registration for</typeparam>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>
		///     <c>true</c>if the type/instance name has been registered
		///     with the container; otherwise <c>false</c>
		/// </returns>
		public bool IsRegistered<T>(string instanceName = null) => this.IsRegistered(typeof (T), instanceName);

		/// <summary>
		///     Resolve an instance of the requested type from the container.
		/// </summary>
		/// <param name="type">Requested type</param>
		/// <param name="parameters">Constructor parameters</param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>The retrieved object</returns>
		/// <exception cref="InvalidOperationException" />
		public object Create(Type type, dynamic parameters, string instanceName = null)
		{
			var key = new MappingKey(type, instanceName);
			Func<dynamic, object> createInstance;

			if (!this._mappings.TryGetValue(key, out createInstance))
			{
				throw new InvalidOperationException($"Could not find mapping for type '{type.FullName}'");
			}

			return createInstance(parameters);
		}

		/// <summary>
		///     Resolve an instance of the requested type from the container.
		/// </summary>
		/// <param name="type">Requested type</param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>The retrieved object</returns>
		public object Create(Type type, string instanceName = null) => this.Create(type, null, instanceName);

		/// <summary>
		///     Resolve an instance of the requested type from the container.
		/// </summary>
		/// <typeparam name="T">Requested type</typeparam>
		/// <param name="parameters">Constructor parameters</param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>The retrieved object</returns>
		public T Create<T>(dynamic parameters, string instanceName = null)
			=> (T) this.Create(typeof (T), parameters, instanceName);

		/// <summary>
		///     Resolve an instance of the requested type from the container.
		/// </summary>
		/// <typeparam name="T">Requested type</typeparam>
		/// <param name="parameters">Constructor parameters</param>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>The retrieved object</returns>
		public T Create<T>(ExpandoObject parameters, string instanceName = null)
			=> (T) this.Create(typeof (T), (dynamic) parameters, instanceName);

		/// <summary>
		///     Resolve an instance of the requested type from the container.
		/// </summary>
		/// <typeparam name="T">Requested type</typeparam>
		/// <param name="instanceName">Instance name (optional)</param>
		/// <returns>The retrieved object</returns>
		public T Create<T>(string instanceName = null) => (T) this.Create(typeof (T), null, instanceName);
	}
}