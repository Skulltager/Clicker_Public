namespace SheetCodes
{
	//Generated code, do not edit!

	public enum CraftingInputIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone Pick _ Stone")] StonePick_Stone = 1,
		[Identifier("Stone Pick _ Flint")] StonePick_Flint = 2,
	}

	public static class CraftingInputIdentifierExtension
	{
		public static CraftingInputRecord GetRecord(this CraftingInputIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.CraftingInputModel.GetRecord(identifier, editableRecord);
		}
	}
}
