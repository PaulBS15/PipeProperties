using System.Runtime.Versioning;

namespace UCon {
   [SupportedOSPlatform("windows8.0")]


   public class Number : Token {
      public double Value { get; }


      public Number(double Value) : base("number", 0) {
         this.Value = Value;
      }

      public static Number operator *(Number Arg1, Number Arg2) {
         return new Number(Arg1.Value*Arg2.Value);
      }

      public static implicit operator Unit(Number n) {
         return new Unit(n.Value, [0, 0, 0, 0, 0, 0, 0]);
      }
   }
}
