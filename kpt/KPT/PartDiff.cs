namespace KPT {
	public class PartDiff {
		public PartDiff ( KSP.Install install, KSP.SaveGame saveGame ) {
			install_ = install;
			saveGame_ = saveGame;
		}

		public KSP.PartSet unusedModParts { 
			get {
				var result = new KSP.PartSet( install_.availableParts.fromMod );
				result.ExceptWith( saveGame_.parts.fromMod );
				return result; 
			}
		}

		private readonly KSP.Install install_;
		private readonly KSP.SaveGame saveGame_;
	}
}

