using System;

namespace UCon {
   public class InvalidNumberException : Exception {
      public InvalidNumberException(string s) : base($"{s} is not a valid number.") { }
   }
}
