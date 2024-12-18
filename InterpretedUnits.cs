using System;
using System.Collections.Generic;

namespace UCon {
   internal sealed class InterpretedUnits {

      private static readonly Lazy<InterpretedUnits> _instance = new(() => new InterpretedUnits());
      private static Dictionary<string, InterpretedUnit> _unitDictionary;

      private InterpretedUnits() {
         _unitDictionary = [];
      }

      public static InterpretedUnits Instance => _instance.Value;

      public static InterpretedUnit GetUnit(string UnitName) {
         _unitDictionary.TryGetValue(UnitName, out var unit);
         return unit;
      }

      public void Add(string UnitName, InterpretedUnit Unit) {
         if (!_unitDictionary.ContainsKey(UnitName)) {
            _unitDictionary.Add(UnitName, Unit);
         }
      }

      public bool TryGetValue(string UnitName, out InterpretedUnit interpretedUnit) {
         bool b = _unitDictionary.TryGetValue(UnitName, out InterpretedUnit iu);
         interpretedUnit = iu;
         return b;
      }

      public Dictionary<string, InterpretedUnit> Dictionary { get { return _unitDictionary; } }
   }
}
