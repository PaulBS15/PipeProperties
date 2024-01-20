using System;

namespace UCon {

	public class BinaryOperator : Operator {

		Func<Unit,Unit,Unit> f;
		const string BinaryArgumentError = "Arguments must all be of type Number";

		public BinaryOperator(string Symbol, int Priority, Func<Unit, Unit, Unit> F, bool NumbersOnly) : base(Symbol, 2, Priority, NumbersOnly) {
			f = F;
		}

		public BinaryOperator(string Symbol, int Priority, Func<Unit, Unit, Unit> F) : this(Symbol, Priority, F, true) {
		}

		Unit Invoke(Unit Arg1, Unit Arg2) {
			return f(Arg1, Arg2);
		}

		public override Unit Invoke(Unit[] Parameters) {
			if (NumbersOnly) {
				for (int i = 0; i < Parameters[0].D.Length; i++) {
					if (Parameters[0].D[i] != 0 || Parameters[1].D[i] != 0) {
						throw new ArgumentException();
					}
				}
			}
			return Invoke(Parameters[0], Parameters[1]);
		}

		public override Number Invoke(Number[] Parameters) {
			return Invoke(Parameters[0], Parameters[1]);
		}
	}
}
