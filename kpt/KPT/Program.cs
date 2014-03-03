using System;

namespace KPT {
	static class Program {
		public static void Main ( string[] args ) {
			try {
				string userInstallFolder;

				try {
					userInstallFolder = args[0];
				}
				catch ( ArgumentOutOfRangeException )
				{
					Console.WriteLine( "Usage: kpt <ksp-install-path>" );
					return;
				}

				var ksp = KSP.Install.create( new System.IO.DirectoryInfo( userInstallFolder ) );

				Console.WriteLine( "==========\nAvailable Parts" );
				foreach ( var part in ksp.availableParts )
					Console.WriteLine( "    {0}", part );

				Console.WriteLine( "==========\nSave Games" );
				ksp.findSaveGames();
				foreach ( var save in ksp.saveGames )
					Console.WriteLine( save );

				Console.WriteLine( "==========\nUsage Report" );
				Console.WriteLine( "    Using {0} of {1} total parts", ksp.usedParts.Count, ksp.availableParts.Count );
				Console.WriteLine( "    Using {0} of {1} squad parts", ksp.usedParts.fromSquad.Count, ksp.availableParts.fromSquad.Count );
				Console.WriteLine( "    Using {0} of {1} mod parts", ksp.usedParts.fromMod.Count, ksp.availableParts.fromMod.Count );

				Console.WriteLine( "==========\nUnused Parts" );
				foreach ( var part in ksp.unusedParts )
					Console.WriteLine( "    {0}", part );
			}
			catch ( Strut.MissingFileException ex ) {
				Console.WriteLine( "Cannot find required file '{0}': {1}", ex.file.FullName, ex.Message );
				return;
			}
			catch ( Strut.MissingFolderException ex ) {
				Console.WriteLine( "Cannot find required folder '{0}': {1}", ex.folder.FullName, ex.Message );
				return;
			}
			catch ( KSP.Install.MissingException ex ) {
				Console.WriteLine( "Cannot find Kerbal Space Program install in '{0}': {1}", ex.path, ex.Message );
				return;
			}
			catch ( Exception ex ) {
				Console.WriteLine( "Unhandled exception: {0}", ex.Message );
				return;
			}
		}
	}
}
