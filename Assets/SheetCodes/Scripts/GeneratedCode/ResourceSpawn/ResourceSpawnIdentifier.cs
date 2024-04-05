namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ResourceSpawnIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone")] Stone = 1,
		[Identifier("Rat")] Rat = 2,
	}

	public static class ResourceSpawnIdentifierExtension
	{
		public static ResourceSpawnRecord GetRecord(this ResourceSpawnIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ResourceSpawnModel.GetRecord(identifier, editableRecord);
		}
	}
}
