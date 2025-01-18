using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Line class is used to convert Linear Dimension (eg Fahrenheit to Celsius)
namespace UCon {
	public class LinearCoefficients {
		public double A { get; private set; }
		public double B { get; private set; }

		public LinearCoefficients(double A, double B) {
			this.A = A;
			this.B = B;
		}
	}
}
