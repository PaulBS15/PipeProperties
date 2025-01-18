using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UCon {
	class InvalidParameterException : Exception {
		/// <summary>
		/// Name of the undefined Unit.
		/// </summary>
		public string Error { get; }

		/// <summary>
		/// Create a new instance of <see cref="InvalidParameterException"/>.
		/// </summary>
		/// <param name="UnitName">Name of the undefined Unit that does not reduce to a pressure.</param>
		public InvalidParameterException(string Error) : base(Error) {
			this.Error = Error;
		}


	}
}
