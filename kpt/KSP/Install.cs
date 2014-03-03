using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace KSP {
	public class Install {
		public static Install create ( DirectoryInfo location ) {
			if ( !location.Exists )
				throw new MissingException( location );

			var install = new Install( location );
			install.availableParts_ = findPartsIn( install.gameDataFolder, install.partsFolder );
			return install;
		}

		private Install ( DirectoryInfo location ) {
			location_ = location;
		}

		public PartSet availableParts { get { return availableParts_; } }

		public PartSet usedParts { get { return usedParts_; } }

		public PartSet unusedParts {
			get { 
				var result = new PartSet( availableParts );
				result.ExceptWith( usedParts );
				return result;
			}
		}

		// Note: expensive call
		private static PartSet findPartsIn( params DirectoryInfo[] folders ) {
			var result = new PartSet();
			foreach ( var folder in folders )
				foreach ( var partFile in folder.EnumerateFiles( partFileNameGlob_, SearchOption.AllDirectories ) )
					result.UnionWith( Part.parseFromCfg( partFile ) );
			return result;
		}

		// Note: expensive call
		public void findSaveGames () {
			foreach ( var saveFolder in savesFolder.GetDirectories() ) {
				if ( skipScenarios_.Contains( saveFolder.Name ) )
					continue;
				var save = SaveGame.create( saveFolder, availableParts );
				if ( save == null )
					continue;
				saveGames_.Add( save );
				usedParts_.UnionWith( save.parts );
			}
		}

		public List<SaveGame> saveGames { get { return saveGames_; } }

		private static readonly HashSet<string> skipScenarios_ = 
			new HashSet<string> { "scenarios", "training" };

		private DirectoryInfo gameDataFolder { get { return Strut.Find.foldersIn( location_, "GameData" ).First(); } }
		private DirectoryInfo partsFolder { get { return Strut.Find.foldersIn( location_, "Parts" ).First(); } }
		private DirectoryInfo savesFolder { get { return Strut.Find.foldersIn( location_, "saves" ).First(); } }

		private PartSet availableParts_;
		private readonly PartSet usedParts_ = new PartSet();
		private readonly List<SaveGame> saveGames_ = new List<SaveGame>();

		private const string partFileNameGlob_ = "*.cfg";
		private readonly DirectoryInfo location_;

		public class MissingException : System.Exception {
			public MissingException( DirectoryInfo path ) { this.path = path; }
			public readonly DirectoryInfo path;
		}	
	}
}