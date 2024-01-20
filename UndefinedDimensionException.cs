using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UCon {
	class UndefinedDimensionException : Exception {
		public UndefinedDimensionException() : base("Dimension structure is ambiguous or undefined") { }
	}
}
