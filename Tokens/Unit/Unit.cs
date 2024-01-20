using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace UCon {

	public class Unit : Token {

		public double Value { get; internal set; }
		public string FullName { get; }
		public string BaseUnit { get; }
		public string NISTValue { get; }
		public string Comment1 { get; }
		public string Comment2 { get; }
		public double Multiplier { internal get;  set; }
		public bool IsGauge { private get; set; }

		private static Dictionary<string,double> prefixes;
		private static List<string> suffixes;

		public static Dictionary<string,double> Prefixes {
			get {
				return prefixes;
			}
		}

		public static List<string> Suffixes { 
			get {
				return suffixes;
			}
		}

		private const int nDim=7;
		private double[] d =  new double[nDim];

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

		public Unit(string Name,double Value, double[] Dim, string BaseUnit, string FullName, string NISTValue, string Comment1, string Comment2) : this(Name, Value, Dim)  {
			this.BaseUnit = BaseUnit;
			this.NISTValue = NISTValue;
			this.Comment1 = Comment1;
			this.Comment2 = Comment2;
		}

		public Unit(double Value, double[] Dim) : base("unit", 0) {
			this.Value = Value;
			for (int i = 0; i < nDim; i++) {
				this.d[i] = Dim[i];
			}
			this.Multiplier = 1.0;
		}

		private Unit(string Name, double Value, double[] Dim) : base(Name, 0) {
			this.Value = Value;
			for (int i = 0; i < nDim; i++) {
				this.d[i] = Dim[i];
			}
			this.Multiplier = 1.0;
		}

		internal Unit() : base("unit",0) {
			this.Value = 0.0;
			for (int i = 0; i < nDim; i++) {
				this.d[i] = 0.0;
			}
			this.Multiplier = 1.0;
		}

		public override string ToString() {

			StringBuilder sb = new StringBuilder();
			sb.Append("Name: " + this.Symbol + ", ");
			sb.Append("Value: " + this.Value + ", ");
			sb.Append("\t\t" + BaseDimensions());
			return sb.ToString();
		}

		public string BaseDimensions() {

			StringBuilder sb = new StringBuilder();
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
			Unit u = new Unit();
			u.Value = this.Value;
			for (int i = 0; i < nDim; i++) {
				u.D[i] = this.d[i];
			}
			return u;
		}


		public static Unit operator *(Unit Arg1, Unit Arg2) {
			Unit u = new Unit();
			for (int i = 0; i < nDim; i++) {
				u.d[i] = Arg1.D[i] + Arg2.D[i];
			}
			u.Value = Arg1.Multiplier*Arg1.Value * Arg2.Multiplier*Arg2.Value;
			return u;
		}

		public static Unit operator /(Unit Arg1, Unit Arg2) {
			Unit u = new Unit();
			if (Arg2.Value == 0) throw new DivideByZeroException();

			for (int i = 0; i < nDim; i++) {
				u.d[i] = Arg1.D[i] - Arg2.D[i];
			}
			u.Value = Arg1.Multiplier*Arg1.Value / (Arg2.Multiplier*Arg2.Value);
			return u;
		}

		public static Unit operator +(Unit Arg1, Unit Arg2) {
			Unit u = new Unit();
			u.Value = Arg1.Multiplier*Arg1.Value + Arg2.Multiplier*Arg2.Value;
			return u;
		}

		public static Unit operator -(Unit Arg1, Unit Arg2) {
			Unit u = new Unit();
			u.Value = Arg1.Multiplier*Arg1.Value - Arg2.Multiplier*Arg2.Value;
			return u;
		}

		public static Unit operator -(Unit Arg1) {
			Unit u = new Unit();
			u.Value = -Arg1.Value;
			return u;
		}

		public static implicit operator Number(Unit u) {
			for (int i = 0; i < nDim; i++) {
				if (u.D[i] != 0) {
					throw new Exception();
				}
			}
			return new Number(u.Value);
		}

		internal static Unit[] LoadUnits(string ResourceFile) {

			List<Unit> unitList = new List<Unit>();
			//if (typeof(T) != typeof(Unit)) return;   // continue only if List is a dictionary whose values are of type Unit
			Assembly assem = Assembly.GetExecutingAssembly();

			using (Stream stream = assem.GetManifestResourceStream("UCon.Resources." + ResourceFile)) {
				try {
					string line;
					using (StreamReader rdr = new StreamReader(stream)) {
						line = rdr.ReadLine();
						string pattern = @"(\S+\s)+";
						MatchCollection matches = Regex.Matches(line,pattern);

						//Get starting positions of all the fields in the Unit Definition file
						//Note: The last field in the first line must have a space after it for the Regex expression to work

						int[] cols = new int[matches.Count];
						string[] fields = new string[matches.Count];
						int i = 0;
						foreach (Match match in matches) {
							cols[i++] = match.Index;
							//Debug.WriteLine(match.Value + " " + match.Index.ToString());
						}
						i--;

						//Loop through all the units and extract information

						int count = 1;
						double[] dim = new double[7];
						while ((line = rdr.ReadLine()) != null) {
							count++;
							//Debug.WriteLine("Read Line " + count.ToString() + ": " + line);

							//Read all the fields (as strings)

							int len = line.Length;
							int j=0;
							while (cols[j] < len) {
								if (j == i || cols[j + 1] > len) {
									fields[j] = line.Substring(cols[j]).Trim();
									j++;
									break;
								}
								else {
									fields[j] = line.Substring(cols[j], cols[j + 1] - cols[j]).Trim();
									j++;
								}
							}

							//Null any fields that didn't have an entry

							for (int k = j; k <= i; k++) {
								fields[k] = null;
							}

							//Convert the fields that represent double values to double

							for (int k = 0; k < 7; k++) {
								double dval;
								if (!double.TryParse(fields[2 + k], out dval)) {
									Debug.WriteLine("Error converting dimension " + k.ToString() + "for Unit " + fields[0]);
								}
								else dim[k] = dval;
							}

							double value;
							if (!double.TryParse(fields[9], out value)) {
								Debug.WriteLine("Error converting conversion factor for Unit " + fields[9]);
							}

							//Instantiate a unit object and add it to the list

							Unit u = new Unit(fields[0], value, dim, fields[1], fields[11], fields[10], fields[12], fields[13]);
							unitList.Add(u);

							//Reset the field array

							for (int k = 0; k < fields.Length; k++) {
								fields[k] = null;
							}
						}
					}
				}
				catch {
					Debug.WriteLine("Error occured in LoadUnits while reading: " + ResourceFile);
				}
			}
			return unitList.ToArray();
		}

		internal static void LoadPrefixes(string ResourceFile) {

			if (prefixes != null) return;
			prefixes = new Dictionary<string, double>();

			Assembly assem = Assembly.GetExecutingAssembly();
			using (Stream stream = assem.GetManifestResourceStream("UCon.Resources." + ResourceFile)) {
				try {
					string line;
					using (StreamReader rdr = new StreamReader(stream)) {
						while ((line = rdr.ReadLine()) != null) {
							string prefix1 = line.Substring(0,5).Trim();
							string prefix2 = line.Substring(6,2).Trim();
							if (double.TryParse(line.Substring(9), out double result)) {
								if (prefix1.Length > 0) prefixes.Add(prefix1, result);
								prefixes.Add(prefix2, result);
							}
						}
					}
				}
				catch {
					Debug.WriteLine("Error occured in LoadPrefixes while reading: "  + ResourceFile);
				}
			}
		}

		internal static void LoadSuffixes(string ResourceFile) {

			if (suffixes != null) return;
			suffixes = new List<string>();

			Assembly assem = Assembly.GetExecutingAssembly();
			using (Stream stream = assem.GetManifestResourceStream("UCon.Resources." + ResourceFile)) {
				try {
					string line;
					using (StreamReader rdr = new StreamReader(stream)) {
						while ((line = rdr.ReadLine()) != null) {
							suffixes.Add(line);
						}
					}
				}
				catch {
					Debug.WriteLine("Error occured in LoadSuffixes while reading: "  + ResourceFile);
				}
			}
		}

	}	//class
}		//namespace
