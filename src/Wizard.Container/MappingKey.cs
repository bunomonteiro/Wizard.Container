using System;

namespace Wizard.Container
{
	/// <summary>
	///     Mapping key. See <see cref="Wizard" />
	/// </summary>
	internal class MappingKey
	{
		/// <summary>
		///     Creates a new instance of <see cref="MappingKey" />
		/// </summary>
		/// <param name="type">Type of the dependency</param>
		/// <param name="instanceName">Name of the instance</param>
		/// <exception cref="ArgumentNullException">type</exception>
		public MappingKey(Type type, string instanceName)
		{
			if (type == null) { throw new ArgumentNullException(nameof(type)); }

			this.Type = type;
			this.InstanceName = instanceName;
		}

		/// <summary>
		///     Type of the dependency
		/// </summary>
		public Type Type { get; }

		/// <summary>
		///     Name of the instance (optional)
		/// </summary>
		public string InstanceName { get; }

		/// <summary>
		///     Returns the hash code for this instance
		/// </summary>
		/// <returns>The hash code for this instance</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				const int multiplier = 31;
				var hash = this.GetType().GetHashCode();

				hash = hash*multiplier + this.Type.GetHashCode();
				hash = hash*multiplier + (this.InstanceName?.GetHashCode() ?? 0);

				return hash;
			}
		}

		/// <summary>
		///     Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object</param>
		/// <returns>
		///     <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj == null) { return false; }

			var compareTo = obj as MappingKey;

			if (ReferenceEquals(this, compareTo)) { return true; }

			if (compareTo == null) { return false; }

			return this.Type == compareTo.Type
			       && string.Equals(this.InstanceName, compareTo.InstanceName, StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		///     For debugging purposes only
		/// </summary>
		/// <returns>Returns a string that represents the current object.</returns>
		public override string ToString()
			=> $"{this.InstanceName ?? "[null]"} ({this.Type.FullName}) - hash code: {this.GetHashCode()}";

		/// <summary>
		///     In case you need to return an error to the client application
		/// </summary>
		/// <returns>Returns an humanized string</returns>
		public string ToTraceString() => $"Instance Name: {this.InstanceName ?? "[null]"} ({this.Type.FullName})";
	}
}