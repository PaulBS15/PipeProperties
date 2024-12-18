using MyLogger;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;

namespace UCon {
   [SupportedOSPlatform("windows8.0")]
   public class TokenList<T> : IEnumerable<T> where T : Token {

      protected readonly Dictionary<string, T> List = [];
      protected bool AddSymbol = false;

      public TokenList(params T[] Items) {
         Add(Items);
      }

      public void Add(params T[] Items) {
         foreach (var item in Items) {
            AddSymbol = true;
            List.Add(item.Symbol, item);
            if (item.Symbol.Length == 1 && AddSymbol) {
               char[] c = [item.Symbol[0]];
               //string s;
               //s = item.Symbol;
               //c[0] = s[0];
               Logger.LogIfDebugging($"Item Symbol {item.Symbol}, Character Code: {ByteCodeToString(c, Encoding.BigEndianUnicode)}.");
               if (char.IsLetter(c[0])) {
                  Logger.LogIfDebugging($"{c[0]} is a letter or digit");
               }
               else {
                  Logger.LogIfDebugging($"{c[0]} is not a letter or digit");
               }
            }
         }
      }

      private string ByteCodeToString(char[] C, Encoding EncodingType) {

         string s = string.Empty;
         byte[] bytes = EncodingType.GetBytes(C);
         foreach (byte b in bytes) {
            s += $"{b,0:x2} ";
         }
         return s.Trim();
      }
      public void Add(bool AddSymbolToValidLetter, params T[] Items) {
         AddSymbol = AddSymbolToValidLetter;
         Add(Items);
      }

      public void Clear() => List.Clear();

      public bool Remove(string Symbol) => List.Remove(Symbol);

      public virtual bool Contains(string Symbol) => List.ContainsKey(Symbol);

      public virtual T this[string key] {
         get {
            if (List.TryGetValue(key, out T value)) {
               return value;
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
