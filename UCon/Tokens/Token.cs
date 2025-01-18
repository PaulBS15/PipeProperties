namespace UCon {

	public abstract class Token {

		static int lasthashcode = 0;
		protected int hashcode;

		public int Priority { get; }
		public string Symbol { get; }
		public bool IsRightAssociated { get; set; }


		protected Token(string Symbol, int Priority) {
			this.Symbol = Symbol;
			this.Priority = Priority;
			hashcode = ++lasthashcode;
		}

		public override int GetHashCode() => hashcode;
		public override string ToString() => Symbol;

		public static bool operator <=(Token T1, Token T2) {
			return T1.IsRightAssociated ? T1.Priority < T2.Priority
												 : T1.Priority <= T2.Priority;
		}

		public static bool operator >=(Token T1, Token T2) {
			return T1.Priority >= T2.Priority;
		}

	}
}
