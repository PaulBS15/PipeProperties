using System.Collections;
using System.Collections.Generic;

namespace UCon {

	public class TokenList<T> : IEnumerable<T> where T : Token {

		protected readonly Dictionary<string, T> List = new Dictionary<string, T>();

		public TokenList(params T[] Items) {
			Add(Items);
		}

		public void Add(params T[] Items) {
			foreach (var item in Items) {
				List.Add(item.Symbol, item);
			}
		}

		public void Clear() => List.Clear();

		public bool Remove(string Symbol) => List.Remove(Symbol);

		public virtual bool Contains(string Symbol) => List.ContainsKey(Symbol);

		public virtual T this[string key] {
			get {
				if (List.ContainsKey(key)) {
					return List[key];
				}
				throw new KeyNotFoundException(key);
			}
		}

		public virtual IEnumerator<T> GetEnumerator() {
			return List.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public TokenList<T> Branch() => new BranchedTokenList<T>(this);
	}

}
