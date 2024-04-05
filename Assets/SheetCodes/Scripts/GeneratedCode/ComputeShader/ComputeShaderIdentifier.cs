namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ComputeShaderIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Map Copier")] MapCopier = 1,
		[Identifier("Biome Map Distance Calculator")] BiomeMapDistanceCalculator = 2,
		[Identifier("Biome Generator")] BiomeGenerator = 3,
	}

	public static class ComputeShaderIdentifierExtension
	{
		public static ComputeShaderRecord GetRecord(this ComputeShaderIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ComputeShaderModel.GetRecord(identifier, editableRecord);
		}
	}
}
