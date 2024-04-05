namespace SheetCodes
{
	//Generated code, do not edit!

	public enum CellMaterialIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Stone")] Stone = 2,
		[Identifier("Sand")] Sand = 3,
		[Identifier("Brick")] Brick = 4,
		[Identifier("Gravel")] Gravel = 9,
		[Identifier("Lava")] Lava = 10,
		[Identifier("Dunes")] Dunes = 11,
		[Identifier("Grass")] Grass = 12,
		[Identifier("Iron")] Iron = 13,
		[Identifier("Sandstone")] Sandstone = 14,
		[Identifier("Open Sky Wall")] OpenSkyWall = 1,
		[Identifier("Mars Wall")] MarsWall = 5,
		[Identifier("Clouded Sky Wall")] CloudedSkyWall = 6,
		[Identifier("Space Wall")] SpaceWall = 7,
		[Identifier("Underwater Wall")] UnderwaterWall = 8,
		[Identifier("Open Sky Ceiling")] OpenSkyCeiling = 15,
		[Identifier("Clouded Sky Ceiling")] CloudedSkyCeiling = 16,
		[Identifier("Mars Ceiling")] MarsCeiling = 17,
		[Identifier("Space Ceiling")] SpaceCeiling = 18,
		[Identifier("Underwater Ceiling")] UnderwaterCeiling = 19,
	}

	public static class CellMaterialIdentifierExtension
	{
		public static CellMaterialRecord GetRecord(this CellMaterialIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.CellMaterialModel.GetRecord(identifier, editableRecord);
		}
	}
}
