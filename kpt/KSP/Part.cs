using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace KSP {
	public class Part : IEquatable<Part> {
		enum ParseState { Unknown, LookingForPart, LookingForName };

		public static IEnumerable<Part> parseFromCfg ( FileInfo partFile ) {
			if ( !partFile.Exists )
				throw new Strut.MissingFileException( partFile );

			var state = ParseState.LookingForPart;
			var result = new List<Part>();
			// TODO: catch IO exceptions
			using ( var reader = new System.IO.StreamReader( partFile.FullName ) ) {
				string line;
				while ( ( line = reader.ReadLine() ) != null ) {
					switch ( state ) {
						case ParseState.LookingForPart:
							if ( partRegex_.IsMatch( line ) )
								state = ParseState.LookingForName;
							break;

						case ParseState.LookingForName:
							if ( nameRegex_.IsMatch( line ) ) {
								var match = nameRegex_.Match( line );
								var partName = match.Groups["name"].Value;
								if ( !skipParts_.Contains( partName ) )
									result.Add( new Part( partFile, partName ) );
								state = ParseState.LookingForPart;
							}
							break;
					}
				}
			}
			return result;
		}

		public static PartSet parseFromSfs ( FileInfo saveFile, PartSet library ) {
			if ( !saveFile.Exists )
				throw new Strut.MissingFileException( saveFile );

			var state = ParseState.LookingForPart;
			var result = new PartSet();
			// TODO: catch IO exceptions
			using ( var reader = new StreamReader( saveFile.FullName ) ) {
				string line;
				while ( ( line = reader.ReadLine() ) != null ) {
					switch ( state ) {
						case ParseState.LookingForPart:
							if ( partRegex_.IsMatch( line ) )
								state = ParseState.LookingForName;
							break;

						case ParseState.LookingForName:
							if ( nameRegex_.IsMatch( line ) ) {
								var match = nameRegex_.Match( line );
								var partName = match.Groups ["name"].Value;
								try {
									if ( !skipParts_.Contains( partName ) )
										result.Add( library.First( part => part.loadedName.Equals( partName ) ) );
								}
								catch ( Exception ex ) {
									Console.WriteLine( 
										">>> Save '{0}' uses part '{1}' that is not in install: {2}",
										saveFile.FullName,
										partName,
										ex.Message );
								}
								state = ParseState.LookingForPart;
							}
							break;
					}
				}
			}
			return result;
		}

		public override string ToString () {
			var fullName = string.Format( "[{0}/{1}]", modName, name );
			return string.Format( "{0,-52} {1}", fullName, pathBelowGameData );
		}
	
		public bool Equals ( Part other ) { return name.Equals( other.name ); }

		public override int GetHashCode () { return name.GetHashCode(); }

		public readonly string name;
		public readonly string loadedName;
		public readonly FileInfo file;
		public readonly string pathBelowGameData;
		public readonly string modName;
		public readonly bool isFromSquad;
		public bool isFromMod { get { return !isFromSquad; } }

		private static readonly Regex partRegex_ = new Regex( @"^\s*PART\s*{?" );
		private static readonly Regex nameRegex_ = new Regex( @"^\s*name\s*=\s*(?<name>\S+)" );

		private static readonly HashSet<string> skipParts_ = 
			new HashSet<string> { "flag", "kerbalEVA" };

		private Part ( FileInfo file, string name ) { 
			this.file = file;
			this.name = name;

			// Because Squad converts underscores to dots in part names as they are loaded.  
			// Save games retain this conversion.
			loadedName = name.Replace( '_', '.' );

			pathBelowGameData = file.FullName.Substring( 
				file.FullName.IndexOf( 
					"GameData/", StringComparison.InvariantCulture ) + 
					"GameData/".Length );

			var firstSeparatorIndex = pathBelowGameData.IndexOfAny( new [] { '/', '\\' } );
			modName = pathBelowGameData.Substring( 0, firstSeparatorIndex );

			isFromSquad = modName.Equals( "Squad" );
		}
	}
}
