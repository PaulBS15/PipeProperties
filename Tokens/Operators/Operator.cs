using System;

namespace UCon {

	public abstract class Operator : Token, IEvaluatable {

		const string OperatorFormatError = "An Operator Name should not onsist of Letters, Digits, or Punctuations - '(', ')', or ','";
		public int NumOfParams { get; }
		public bool NumbersOnly { get; }


		protected Operator(string Symbol, int NumOfParams, int Priority, bool NumbersOnly) : base(Symbol, Priority) {
			this.NumOfParams = NumOfParams;
			this.NumbersOnly = NumbersOnly;

			if (Symbol.Any(Character => char.IsLetterOrDigit(Character) || Character.Is('(', ')', ','))) {
				throw new FormatException(OperatorFormatError);
			}
		}

		//public abstract Unit Invoke(Unit n1, Unit n2);
		public abstract Unit Invoke(Unit[] Parameters);

		public abstract Number Invoke(Number[] Parameters);
	}
}
