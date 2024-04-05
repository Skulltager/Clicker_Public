namespace SheetCodes
{
	//Generated code, do not edit!

	public enum CraftingOutputIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone Pick")] StonePick = 1,
		[Identifier("Stone Sword")] StoneSword = 2,
		[Identifier("Stone Hammer")] StoneHammer = 3,
	}

	public static class CraftingOutputIdentifierExtension
	{
		public static CraftingOutputRecord GetRecord(this CraftingOutputIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.CraftingOutputModel.GetRecord(identifier, editableRecord);
		}
	}
}
