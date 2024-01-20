//using System.Windows.Forms;

using System.Text.RegularExpressions;

namespace UCon {
	public static class ConvertDimension {

		public static double Convert(string dimension, bool InInches = false) {

			string pattern = @"^(-?)(\d*(?:\.?\d*))\s*(\""|\')?(\s*\-?\s*)?((?:\d*(?:\.?\d*))|(?:\d*))?\s*(\d*\/\d*)?\s*(\""|\')?\s*";
			double firstNum;
			double secondNum;
			double frac;
			bool firstUnit = true;
			bool secondUnit = false;

			if (dimension.Contains("..")) throw new UndefinedDimensionException();
			Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

			MatchCollection matches = r.Matches(dimension);
			if (matches.Count!=1) throw new UndefinedDimensionException();
			if (matches[0].Length != dimension.Length) throw new UndefinedDimensionException();

			GroupCollection grps = matches[0].Groups;

			//int i = 0;
			//foreach (Group g in grps) { Console.WriteLine(i++ + " : " + g); }

			string[] strG = new string[grps.Count];
			for (int j = 0; j<grps.Count; j++) strG[j]=grps[j].ToString();

			// measurement unit without number errors

			if (!(strG[7].Length > 0 && strG[6].StartsWith("/"))) {
				//if (strG[3].Length > 0 ^ strG[7].Length > 0) throw new UndefinedDimensionException();
				if (!((strG[3].Length > 0 && strG[2].Length > 0) || (strG[7].Length > 0 && strG[2].Length > 0)
						|| (strG[7].Length > 0 && (strG[5].Length > 0 || strG[6].Length > 0))
						|| (strG[3].Length == 0 && strG[7].Length == 0))) throw new UndefinedDimensionException();
			}

			// get the measurement units

			if (strG[3].Length == 0 && strG[7].Length == 0) {
				firstUnit = true;
				secondUnit = false;
			}
			//else if (strG[3].Length == 0 ^ strG[7].Length == 0) throw new UndefinedDimensionException();
			else {
				if (strG[3] == "\"") firstUnit = false;
				if (strG[7] == "'") secondUnit = true;
			}

			// unit of measurement present without preceding number error

			if (strG[2].Length == 0 && strG[3].Length > 0) throw new UndefinedDimensionException();

			// get the numbers

			if (double.TryParse(strG[2], out double dummy)) firstNum=dummy;
			else firstNum = -1.0;
			if (double.TryParse(strG[5], out dummy)) secondNum = dummy;
			else secondNum = dummy;
			if (firstNum < 0 && secondNum < 0) throw new UndefinedDimensionException();

			//if (strG[3].Length == 0 && strG[4].Length == 0 && strG[2].EndsWith(".") && strG[5].StartsWith(".")) throw new UndefinedDimensionException(); // check for d..d

			// get the fraction if present

			frac = 0.0;

			if (strG[6].StartsWith("/")) {
				if (!double.TryParse(strG[6].Substring(1), out double denominator)) throw new UndefinedDimensionException();

				if (secondNum > 0) {
					secondNum = secondNum/denominator;
					if (strG[3].Length ==0) firstUnit = secondUnit;
				}
				else if (firstNum > 0) {
					secondNum = firstNum/denominator;
					firstNum = 0.0;
				}
				else throw new UndefinedDimensionException();  // strG[5] and strG[2] are empty with a slash as first character in strG[6]
			}

			else if (strG[6].Length > 0) {  // ...is strG[6] a fraction
				int slashPos = strG[6].IndexOf('/');
				if (slashPos >= 0) {
					if (!double.TryParse(strG[6].Substring(0, slashPos), out frac)) throw new UndefinedDimensionException();
					if (!double.TryParse(strG[6].Substring(++slashPos, strG[6].Length-slashPos), out double denominator)) throw new UndefinedDimensionException();
					frac /= denominator;
					secondNum += frac;
				}
				else throw new UndefinedDimensionException(); // strG[6] is not empty and a slash was not found (should never happen)
			}
			if (!firstUnit) firstNum /= 12.0;
			if (!secondUnit) secondNum /=12.0;

			double sign = grps[1].ToString().Trim() == "-" ? -1.0 : 1.0;
			return !InInches ? sign*(firstNum + secondNum) : sign*12.0*(firstNum + secondNum);
		}
	}
}
