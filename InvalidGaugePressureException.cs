using System;

namespace UCon {
	public class InvalidGaugePressureException : Exception {
		public InvalidGaugePressureException() : base("Unit conversion results in a negative absolute pressure.") { }
	}
}
