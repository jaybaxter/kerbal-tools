using System.Collections.Generic;
using System.IO;

namespace Strut {
	public abstract class Find {

		public static DirectoryInfo folderIn ( DirectoryInfo rootFolder, string pattern ) {
			return foldersIn( rootFolder, pattern ).GetEnumerator().Current;
		}
	
		public static IEnumerable<DirectoryInfo> foldersIn ( DirectoryInfo rootFolder, string pattern ) {
			return rootFolder.EnumerateDirectories( pattern, SearchOption.TopDirectoryOnly );
		}

		public static IEnumerable<FileInfo> filesBelow ( DirectoryInfo rootFolder, string pattern ) {
			return rootFolder.EnumerateFiles( pattern, SearchOption.AllDirectories );
		}

		public static IEnumerable<FileInfo> filesIn ( DirectoryInfo rootFolder, string pattern ) {
			return rootFolder.EnumerateFiles( pattern, SearchOption.TopDirectoryOnly );
		}
	}

	public class MissingFileException : System.Exception {
		public MissingFileException( FileInfo file ) { this.file = file; }
		public readonly FileInfo file;
	}

	public class MissingFolderException : System.Exception {
		public MissingFolderException( DirectoryInfo folder ) { this.folder = folder; }
		public readonly DirectoryInfo folder;
	}
}
