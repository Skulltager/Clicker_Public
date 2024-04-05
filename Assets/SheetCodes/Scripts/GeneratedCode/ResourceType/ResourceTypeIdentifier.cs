namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ResourceTypeIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone")] Stone = 1,
		[Identifier("Rat")] Rat = 2,
	}

	public static class ResourceTypeIdentifierExtension
	{
		public static ResourceTypeRecord GetRecord(this ResourceTypeIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ResourceTypeModel.GetRecord(identifier, editableRecord);
		}
	}
}
