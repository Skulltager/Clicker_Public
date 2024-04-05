namespace SheetCodes
{
	//Generated code, do not edit!

	public enum WorldResourceIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone Rock")] StoneRock = 1,
	}

	public static class WorldResourceIdentifierExtension
	{
		public static WorldResourceRecord GetRecord(this WorldResourceIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.WorldResourceModel.GetRecord(identifier, editableRecord);
		}
	}
}
