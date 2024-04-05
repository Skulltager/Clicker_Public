namespace SheetCodes
{
	//Generated code, do not edit!

	public enum CraftingCategoryIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Weapon")] Weapon = 1,
		[Identifier("Pickaxe")] Pickaxe = 2,
		[Identifier("Hammer")] Hammer = 3,
	}

	public static class CraftingCategoryIdentifierExtension
	{
		public static CraftingCategoryRecord GetRecord(this CraftingCategoryIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.CraftingCategoryModel.GetRecord(identifier, editableRecord);
		}
	}
}
