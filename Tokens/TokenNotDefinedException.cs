using System;

namespace UCon {
	public class TokenNotDefinedException : Exception {
		/// <summary>
		/// Name of the undefined token.
		/// </summary>
		public string TokenName { get; }

		/// <summary>
		/// Create a new instance of <see cref="TokenNotDefinedException"/>.
		/// </summary>
		/// <param name="TokenName">Name of the undefined Token.</param>
		public TokenNotDefinedException(string TokenName) : base($"Token: {TokenName} is not defined.") {
			this.TokenName = TokenName;
		}
	}
}
