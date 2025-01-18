using System;
using System.Runtime.Versioning;

namespace UCon {
   [SupportedOSPlatform("windows8.0")]
   public class InvalidPowerExceptipn : Exception {

      public InvalidPowerExceptipn(Unit x, Unit y) : base($"Exponent {y.FullName} in power is not dimensionaless for base {x.FullName}.") { }
   }
}
