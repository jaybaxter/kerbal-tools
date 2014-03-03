using System.Collections.Generic;

namespace KSP {
	public class PartSet : HashSet<Part> {
		public PartSet () : base() {}

		public PartSet ( IEnumerable<Part> other ) : base( other ) {}

		public PartSet fromMod { 
			get {
				var result = new PartSet( this );
				result.RemoveWhere( p => p.isFromSquad );
				return result;
			}
		}

		public PartSet fromSquad {
			get {
				var result = new PartSet( this );
				result.RemoveWhere( p => p.isFromMod );
				return result;
			}
		}
	}
}

