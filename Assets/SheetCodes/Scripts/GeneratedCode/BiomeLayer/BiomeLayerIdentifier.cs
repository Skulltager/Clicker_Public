namespace SheetCodes
{
	//Generated code, do not edit!

	public enum BiomeLayerIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Layer 1")] Layer1 = 1,
		[Identifier("Layer 2")] Layer2 = 2,
		[Identifier("Layer 3")] Layer3 = 3,
		[Identifier("Layer 4")] Layer4 = 4,
		[Identifier("Layer 5")] Layer5 = 5,
		[Identifier("Layer 6")] Layer6 = 6,
	}

	public static class BiomeLayerIdentifierExtension
	{
		public static BiomeLayerRecord GetRecord(this BiomeLayerIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.BiomeLayerModel.GetRecord(identifier, editableRecord);
		}
	}
}
