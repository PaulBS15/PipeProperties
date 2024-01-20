using System.Collections.Generic;
using System.Linq;

namespace UCon {
	class BranchedTokenList<T> : TokenList<T>	where T : Token {

		readonly TokenList<T> parent;

		public BranchedTokenList(TokenList<T> Parent) {
			parent = Parent;
		}

		public override bool Contains(string Symbol) => base.Contains(Symbol) || parent.Contains(Symbol);

		public override T this[string Symbol] {
			get {
				if (base.Contains(Symbol))	return base[Symbol];
				if (parent.Contains(Symbol)) return parent[Symbol];

				//if (typeof(T) == typeof(Unit)) {
				//	string prefix1 = Symbol.Substring(1,1);
				//	string prefix2 = Symbol.Substring(1,2);
				//	if (Unit.Prefixes.ContainsKey(prefix1) || Unit.Prefixes.ContainsKey(prefix2)) {
				//		if (base.Contains(Symbol.Substring(2))) {
				//			Unit unit = this[Symbol.Substring(2)];
				//		}
				//	}
				//	return null;
				//}


				throw new KeyNotFoundException(Symbol);
			}
			
		}

		public override IEnumerator<T> GetEnumerator() => List.Values.Union(parent).GetEnumerator();
	}
}
