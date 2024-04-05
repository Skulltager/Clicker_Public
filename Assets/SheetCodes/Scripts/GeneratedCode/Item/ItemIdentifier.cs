namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ItemIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Rat Hair")] RatHair = 1,
		[Identifier("Rat Tail")] RatTail = 2,
		[Identifier("Bronze Coins")] BronzeCoins = 3,
		[Identifier("Flint")] Flint = 4,
		[Identifier("Pebbles")] Pebbles = 5,
		[Identifier("Stone")] Stone = 6,
		[Identifier("Stone Pick")] StonePick = 7,
		[Identifier("Stone Sword")] StoneSword = 8,
		[Identifier("Stone Axe")] StoneAxe = 9,
	}

	public static class ItemIdentifierExtension
	{
		public static ItemRecord GetRecord(this ItemIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ItemModel.GetRecord(identifier, editableRecord);
		}
	}
}
