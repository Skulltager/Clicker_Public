namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ResourceSpawnWeightIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stones")] Stones = 1,
		[Identifier("Rats")] Rats = 2,
	}

	public static class ResourceSpawnWeightIdentifierExtension
	{
		public static ResourceSpawnWeightRecord GetRecord(this ResourceSpawnWeightIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ResourceSpawnWeightModel.GetRecord(identifier, editableRecord);
		}
	}
}
