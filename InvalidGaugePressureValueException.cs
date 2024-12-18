using System;

namespace UCon {
   internal class InvalidGaugePressureValueException : Exception {
      public InvalidGaugePressureValueException() : base("Entered value for gauge pressure results in negative absolute pressure") { }
   }
}
