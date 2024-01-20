using System;
using System.Linq;

namespace UCon {

	static class Utilities {
		public static bool Is<T>(this T Value, params T[] Candidates) {
			return Candidates.Contains(Value);
		}

		public static bool Any(this string Item, Func<char,bool> Predicate) {
			return Item.Cast<char>().Any(Predicate);
		}
	}
}
