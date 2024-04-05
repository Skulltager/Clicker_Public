namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ToolBaseStatsIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Horrendous Stone Pick")] HorrendousStonePick = 2,
		[Identifier("Terrible Stone Pick")] TerribleStonePick = 3,
		[Identifier("Bad Stone Pick")] BadStonePick = 4,
		[Identifier("Fair Stone Pick")] FairStonePick = 5,
		[Identifier("Average Stone Pick")] AverageStonePick = 1,
		[Identifier("Decent Stone Pick")] DecentStonePick = 6,
		[Identifier("Good Stone Pick")] GoodStonePick = 7,
		[Identifier("Excellent Stone Pick")] ExcellentStonePick = 8,
		[Identifier("Masterwork Stone Pick")] MasterworkStonePick = 9,
		[Identifier("Horrendous Stone Sword")] HorrendousStoneSword = 10,
		[Identifier("Terrible Stone Sword")] TerribleStoneSword = 11,
		[Identifier("Bad Stone Sword")] BadStoneSword = 12,
		[Identifier("Fair Stone Sword")] FairStoneSword = 13,
		[Identifier("Average Stone Sword")] AverageStoneSword = 14,
		[Identifier("Decent Stone Sword")] DecentStoneSword = 15,
		[Identifier("Good Stone Sword")] GoodStoneSword = 16,
		[Identifier("Excellent Stone Sword")] ExcellentStoneSword = 17,
		[Identifier("Masterwork Stone Sword")] MasterworkStoneSword = 18,
		[Identifier("Horrendous Stone Axe")] HorrendousStoneAxe = 19,
		[Identifier("Terrible Stone Axe")] TerribleStoneAxe = 20,
		[Identifier("Bad Stone Axe")] BadStoneAxe = 21,
		[Identifier("Fair Stone Axe")] FairStoneAxe = 22,
		[Identifier("Average Stone Axe")] AverageStoneAxe = 23,
		[Identifier("Decent Stone Axe")] DecentStoneAxe = 24,
		[Identifier("Good Stone Axe")] GoodStoneAxe = 25,
		[Identifier("Excellent Stone Axe")] ExcellentStoneAxe = 26,
		[Identifier("Masterwork Stone Axe")] MasterworkStoneAxe = 27,
	}

	public static class ToolBaseStatsIdentifierExtension
	{
		public static ToolBaseStatsRecord GetRecord(this ToolBaseStatsIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ToolBaseStatsModel.GetRecord(identifier, editableRecord);
		}
	}
}
