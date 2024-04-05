namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ItemCategoryIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Resource")] Resource = 1,
		[Identifier("Weapon")] Weapon = 2,
		[Identifier("Pickaxe")] Pickaxe = 3,
		[Identifier("Axe")] Axe = 4,
	}

	public static class ItemCategoryIdentifierExtension
	{
		public static ItemCategoryRecord GetRecord(this ItemCategoryIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ItemCategoryModel.GetRecord(identifier, editableRecord);
		}
	}
}
