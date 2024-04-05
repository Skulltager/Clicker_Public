namespace SheetCodes
{
	//Generated code, do not edit!

	public enum PrefabCollectionIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Prefabs")] Prefabs = 1,
	}

	public static class PrefabCollectionIdentifierExtension
	{
		public static PrefabCollectionRecord GetRecord(this PrefabCollectionIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.PrefabCollectionModel.GetRecord(identifier, editableRecord);
		}
	}
}
