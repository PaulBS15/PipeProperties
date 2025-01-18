using MyLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;

namespace UCon {
   [SupportedOSPlatform("windows8.0")]

   public class Unit : Token {

      public double Factor { get; internal set; }
      public string FullName { get; internal set; }
      public string BaseUnit { get; }
      public string NISTValue { get; }
      public string Comment1 { get; }
      public string Comment2 { get; }
      public double Multiplier { internal get; set; }
      public bool IsGauge { private get; set; }

      private static Dictionary<string, double> prefixes;
      private static List<string> suffixes;

      public static Dictionary<string, double> Prefixes {
         get {
            return prefixes;
         }
      }

      public static List<string> Suffixes {
         get {
            return suffixes;
         }
      }

      private const int nDim = 7;
      private readonly double[] d = new double[nDim];

      public double L {
         get { return d[0]; }
      }

      public double M {
         get { return d[1]; }
      }

      public double T {
         get { return d[2]; }
      }

      public double I {
         get { return d[3]; }
      }

      public double N {
         get { return d[4]; }
      }

      public double Θ {
         get { return d[5]; }
      }

      public double J {
         get { return d[6]; }
      }

      public double[] D {
         get { return d; }
      }

      public Unit(string Name, double Value, double[] Dim, string BaseUnit, string _, string NISTValue, string Comment1, string Comment2) : this(Name, Value, Dim) {
         this.BaseUnit = BaseUnit;
         this.NISTValue = NISTValue;
         this.Comment1 = Comment1;
         this.Comment2 = Comment2;
      }

      public Unit(double Value, double[] Dim) : base("unit", 0) {
         this.Factor = Value;
         for (int i = 0; i < nDim; i++) {
            this.d[i] = Dim[i];
         }
         this.Multiplier = 1.0;
      }

      private Unit(string Name, double Value, double[] Dim) : base(Name, 0) {
         this.Factor = Value;
         for (int i = 0; i < nDim; i++) {
            this.d[i] = Dim[i];
         }
         this.Multiplier = 1.0;
      }

      internal Unit() : base("unit", 0) {
         this.Factor = 0.0;
         for (int i = 0; i < nDim; i++) {
            this.d[i] = 0.0;
         }
         this.Multiplier = 1.0;
      }

      public override string ToString() {

         StringBuilder sb = new();
         sb.Append("Name: " + this.FullName + ", ");
         sb.Append("Value: " + this.Factor + ", ");
         sb.Append("\t\t" + BaseDimensions());
         return sb.ToString();
      }

      public string BaseDimensions() {

         StringBuilder sb = new();
         sb.Append("L: " + this.d[0] + ", ");
         sb.Append("M: " + this.d[1] + ", ");
         sb.Append("T: " + this.d[2] + ", ");
         sb.Append("I: " + this.d[3] + ", ");
         sb.Append("N: " + this.d[4] + ", ");
         sb.Append("Θ: " + this.d[5] + ", ");
         sb.Append("J: " + this.d[6]);

         return sb.ToString();
      }

      public Unit Copy() {
         Unit u = new() {
            FullName = base.Symbol,
            Factor = this.Factor
         };
         for (int i = 0; i < nDim; i++) {
            u.D[i] = this.d[i];
         }
         return u;
      }
      public bool EquivalentTo(Unit compare) {
         bool r = true;
         Unit u = this / compare;
         foreach (double d in u.D) {
            if (Math.Abs(d / 1e-6) > 1.0) {
               r = false;
               break;
            }
         }
         return r;
      }

      public bool IsPressureUnit {
         get {

            bool result = false;
            if (this.M == 1 && this.L == -1 && this.T == -2 && this.I == 0 && this.J == 0 && this.N == 0 && this.Θ == 0) result = true;
            return result;
         }
      }

      public static Unit operator *(Unit Arg1, Unit Arg2) {
         Unit u = new();
         for (int i = 0; i < nDim; i++) {
            u.d[i] = Arg1.D[i] + Arg2.D[i];
         }
         u.Factor = Arg1.Multiplier * Arg1.Factor * Arg2.Multiplier * Arg2.Factor;
         return u;
      }

      public static Unit operator /(Unit Arg1, Unit Arg2) {
         Unit u = new();
         if (Arg2.Factor == 0) throw new DivideByZeroException();

         for (int i = 0; i < nDim; i++) {
            u.d[i] = Arg1.D[i] - Arg2.D[i];
         }
         u.Factor = Arg1.Multiplier * Arg1.Factor / (Arg2.Multiplier * Arg2.Factor);
         return u;
      }

      public static Unit operator +(Unit Arg1, Unit Arg2) {
         Unit u = new() {
            Factor = Arg1.Multiplier * Arg1.Factor + Arg2.Multiplier * Arg2.Factor
         };
         return u;
      }

      public static Unit operator -(Unit Arg1, Unit Arg2) {
         Unit u = new() {
            Factor = Arg1.Multiplier * Arg1.Factor - Arg2.Multiplier * Arg2.Factor
         };
         return u;
      }

      public static Unit operator -(Unit Arg1) {
         Unit u = new() {
            Factor = -Arg1.Factor
         };
         return u;
      }

      public static implicit operator Number(Unit u) {
         for (int i = 0; i < nDim; i++) {
            if (u.D[i] != 0) {
               throw new Exception();
            }
         }
         return new Number(u.Factor);
      }

      internal static Unit[] LoadUnits(string ResourceFile) {

         List<Unit> unitList = [];
         //if (typeof(T) != typeof(Unit)) return;   // continue only if List is a dictionary whose values are of type Unit
         Assembly assem = Assembly.GetExecutingAssembly();

         using (Stream stream = assem.GetManifestResourceStream("UCon.Resources." + ResourceFile)) {
            try {
               string line;
               using StreamReader rdr = new(stream);
               line = rdr.ReadLine();
               string pattern = @"(\S+\s)+";
               MatchCollection matches = Regex.Matches(line, pattern);

               //Get starting positions of all the fields in the Unit Definition file
               //Note: The last field in the first line must have a space after it for the Regex expression to work

               int[] cols = new int[matches.Count];
               string[] fields = new string[matches.Count];
               int i = 0;
               foreach (Match match in matches) {
                  cols[i++] = match.Index;
                  //Logger.LogIfDebugging(match.Value + " " + match.Index.ToString());
               }
               i--;

               //Loop through all the units and extract information

               int count = 1;
               double[] dim = new double[7];
               while ((line = rdr.ReadLine()) != null) {
                  count++;
                  //Logger.LogIfDebugging("Read Line " + count.ToString() + ": " + line);

                  //Read all the fields (as strings)

                  int len = line.Length;
                  int j = 0;
                  while (cols[j] < len) {
                     if (j == i || cols[j + 1] > len) {
                        fields[j] = line[cols[j]..].Trim();
                        j++;
                        break;
                     }
                     else {
                        //fields[j] = line.Substring(cols[j], cols[j + 1] - cols[j]).Trim();
                        fields[j] = line[cols[j]..cols[j + 1]].Trim();
                        j++;
                     }
                  }

                  //Null any fields that didn't have an entry

                  for (int k = j; k <= i; k++) {
                     fields[k] = null;
                  }

                  //Convert the fields that represent double values to double

                  for (int k = 0; k < 7; k++) {
                     if (!double.TryParse(fields[2 + k], out double dval)) {
                        Logger.LogIfDebugging("Error converting dimension " + k.ToString() + "for Unit " + fields[0]);
                     }
                     else dim[k] = dval;
                  }

                  if (!double.TryParse(fields[9], out double value)) {
                     Logger.LogIfDebugging("Error converting conversion factor for Unit " + fields[9]);
                  }

                  //Instantiate a unit object and add it to the list

                  Unit u = new(fields[0], value, dim, fields[1], fields[11], fields[10], fields[12], fields[13]);
                  unitList.Add(u);

                  //Reset the field array

                  for (int k = 0; k < fields.Length; k++) {
                     fields[k] = null;
                  }

               }
            }
            catch {
               Logger.LogIfDebugging("Error occured in LoadUnits while reading: " + ResourceFile);
            }
         }
         return [.. unitList];
      }

      internal static void LoadPrefixes(string ResourceFile) {

         if (prefixes != null) return;
         prefixes = [];

         Assembly assem = Assembly.GetExecutingAssembly();
         using Stream stream = assem.GetManifestResourceStream("UCon.Resources." + ResourceFile);
         try {
            string line;
            using StreamReader rdr = new(stream);
            while ((line = rdr.ReadLine()) != null) {
               string prefix1 = line[..5].Trim();
               string prefix2 = line.Substring(6, 2).Trim();
               if (double.TryParse(line[9..], out double result)) {
                  if (prefix1.Length > 0) prefixes.Add(prefix1, result);
                  prefixes.Add(prefix2, result);
               }
            }
         }
         catch {
            Logger.LogIfDebugging("Error occured in LoadPrefixes while reading: " + ResourceFile);
         }
      }

      internal static void LoadSuffixes(string ResourceFile) {

         if (suffixes != null) return;
         suffixes = [];

         Assembly assem = Assembly.GetExecutingAssembly();
         using Stream stream = assem.GetManifestResourceStream("UCon.Resources." + ResourceFile);
         try {
            string line;
            using StreamReader rdr = new(stream);
            while ((line = rdr.ReadLine()) != null) {
               suffixes.Add(line);
            }
         }
         catch {
            Logger.LogIfDebugging("Error occured in LoadSuffixes while reading: " + ResourceFile);
         }

      }

   }	//class
}		//namespace
