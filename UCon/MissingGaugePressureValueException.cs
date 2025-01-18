using System;

namespace UCon {
   public class MissingGaugePressureValueException : Exception {
      public MissingGaugePressureValueException() : base("Value must be present for gauge pressure conversion.") { }
   }
}
