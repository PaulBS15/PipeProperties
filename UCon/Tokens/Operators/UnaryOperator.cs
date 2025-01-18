using System;
using System.Runtime.Versioning;

namespace UCon {
   [SupportedOSPlatform("windows8.0")]


   public sealed class UnaryOperator : Operator {

      readonly Func<Unit, Unit> f;
      const string UnaryArgumentError = "Arguments must all be of type Number";

      /// <summary>
      /// Create a new Unary Operator.
      /// </summary>
      /// <param name="Symbol">Symbol for the Operator.</param>
      /// <param name="Priority">Priority of the Operator.</param>
      /// <param name="Function">Function to Calculate Result.</param>
      /// <param name="NumbersOnly">Flag indicating operator only works on Number types</param>
      public UnaryOperator(string Symbol, int Priority, Func<Unit, Unit> F, bool NumbersOnly) : base(Symbol, 1, Priority, NumbersOnly) {
         f = F;
         if (Symbol.Length > 1) throw new FormatException("Unary Operator symbol must consist only a single letter.");
      }

      public UnaryOperator(string Symbol, int Priority, Func<Unit, Unit> F) : this(Symbol, Priority, F, true) {
      }

      /// <summary>
      /// Invoke the Operator on an Argument.
      /// </summary>
      public Unit Invoke(Unit Arg) => f(Arg);

      /// <summary>
      /// Implementation of <see cref="IEvaluatable{T}"/>
      /// </summary>
      public override Unit Invoke(Unit[] Parameters) {
         if (NumbersOnly) {
            throw new ArgumentException(UnaryArgumentError);
         }
         return Invoke(Parameters[0]);
      }

      public override Number Invoke(Number[] Parameters) {
         return Invoke(Parameters[0]);
      }
   }
}
