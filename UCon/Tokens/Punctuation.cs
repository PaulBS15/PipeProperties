namespace UCon {
	sealed class Punctuation : Token {
		Punctuation(string Word) : base(Word, 0) { }

		/// <summary>
		/// Grouping and Function Argument list starter.
		/// </summary>
		public static Punctuation LeftParenthesis { get; } = new Punctuation("(");

		/// <summary>
		/// Grouping and Function Argument list ender.
		/// </summary>
		public static Punctuation RightParenthesis { get; } = new Punctuation(")");

		/// <summary>
		/// Function Argument Separator.
		/// </summary>
		public static Punctuation Comma { get; } = new Punctuation(",");

		public static Punctuation Space { get; } = new Punctuation(" ");
	}
}
