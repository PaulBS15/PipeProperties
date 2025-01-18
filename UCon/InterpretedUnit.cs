namespace UCon {
   internal class InterpretedUnit {

      Unit _unit;
      bool _gaugePressure;

      public InterpretedUnit(Unit Unit, bool IsGaugePressure) {
         _unit = Unit;
         _gaugePressure = IsGaugePressure;
      }

      public bool IsGauge => _gaugePressure;

      public Unit Unit => _unit;
   }
}
