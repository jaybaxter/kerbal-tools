using System.IO;
using System.Text;

namespace KSP {
	public class SaveGame {
		public static SaveGame create ( DirectoryInfo location, PartSet library ) {
			if ( !location.Exists )
				throw new Strut.MissingFolderException( location );
			var game = new SaveGame( location );
			game.parts_.UnionWith( findParts( game.location_, library ) );
			return game;
		}

		public PartSet parts { get { return parts_; } }

		public override string ToString () {
			var result = new StringBuilder();
			result.AppendFormat( 
				"[SaveGame '{0}' using {1} Squad and {2} mod parts]\n", 
				location_.Name, 
				parts.fromSquad.Count,
				parts.fromMod.Count );
			foreach ( var part in parts )
				result.AppendFormat( "    {0}\n", part );
			return result.ToString();
		}

		private SaveGame ( DirectoryInfo location ) {
			location_ = location;
		}

		private static PartSet findParts ( DirectoryInfo saveFolder, PartSet library ) {
			var saveFile = new FileInfo( Strut.String.combinePath( saveFolder.FullName, "persistent.sfs" ) );
			if ( !saveFile.Exists )
				throw new Strut.MissingFileException( saveFile );
			return Part.parseFromSfs( saveFile, library );
		}

		private readonly PartSet parts_ = new PartSet();
		private readonly DirectoryInfo location_;
	}
}
