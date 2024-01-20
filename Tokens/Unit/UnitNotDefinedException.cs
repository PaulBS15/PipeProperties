using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCon {
	public class UnitNotDefinedException : Exception {
		/// <summary>
		/// Name of the undefined token.
		/// </summary>
		public string UnitName { get; }

		/// <summary>
		/// Create a new instance of <see cref="TokenNotDefinedException"/>.
		/// </summary>
		/// <param name="UnitName">Name of the undefined Token.</param>
		public UnitNotDefinedException(string UnitName) : base(UnitName + " is not defined.") {
			this.UnitName = UnitName;
		}
	}
}