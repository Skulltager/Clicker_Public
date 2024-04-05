namespace SheetCodes
{
	//Generated code, do not edit!

	public enum BiomeIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Cave")] Cave = 1,
	}

	public static class BiomeIdentifierExtension
	{
		public static BiomeRecord GetRecord(this BiomeIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.BiomeModel.GetRecord(identifier, editableRecord);
		}
	}
}
