namespace Strut {
	public abstract class String {
		public static string combinePath ( params object[] args ) { 
			string result = "";
			foreach ( var arg in args )
				result = System.IO.Path.Combine( result, arg.ToString() );
			return result;
		}
	}
}
	