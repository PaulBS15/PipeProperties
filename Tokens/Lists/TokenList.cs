using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UCon {

	public class TokenList<T> : IEnumerable<T> where T : Token {

		protected readonly Dictionary<string, T> List = new Dictionary<string, T>();

		public TokenList(params T[] Items) {
			Add(Items);
		}

		public void Add(params T[] Items) {
			foreach (var item in Items) {
				List.Add(item.Symbol, item);
				if (item.Symbol.Length == 1) {
					char[] c = new char[1];
					string s;
					s = item.Symbol;
					c[0] = s[0];
					Debug.WriteLine($"Item Symbol {item.Symbol}, Character Code: {Encoding.ASCII.GetBytes(c)[0]}.");
				}
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
