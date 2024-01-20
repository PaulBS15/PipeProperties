using System;

namespace UCon {
	public class InvalidPressureUnitException : Exception {
		/// <summary>
		/// Name of the undefined Unit.
		/// </summary>
		public string UnitName { get; }

		/// <summary>
		/// Create a new instance of <see cref="InvalidPressureUnitException"/>.
		/// </summary>
		/// <param name="UnitName">Name of the undefined Unit that does not reduce to a pressure.</param>
		public InvalidPressureUnitException(string UnitName) : base(UnitName + " does not reduce to a pressure.") {
			this.UnitName = UnitName;
		}

	}
}
