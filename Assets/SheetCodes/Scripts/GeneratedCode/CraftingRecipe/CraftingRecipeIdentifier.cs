namespace SheetCodes
{
	//Generated code, do not edit!

	public enum CraftingRecipeIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone Pick")] StonePick = 1,
	}

	public static class CraftingRecipeIdentifierExtension
	{
		public static CraftingRecipeRecord GetRecord(this CraftingRecipeIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.CraftingRecipeModel.GetRecord(identifier, editableRecord);
		}
	}
}
