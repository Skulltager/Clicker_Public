namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ItemDropIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Rat _ Rat Hair")] Rat_RatHair = 1,
		[Identifier("Rat _ Rat Tail")] Rat_RatTail = 2,
		[Identifier("Rat - Bronze Coins")] RatBronzeCoins = 3,
		[Identifier("Rock _ Stone")] Rock_Stone = 4,
		[Identifier("Rock _ Pebbles")] Rock_Pebbles = 5,
		[Identifier("Rock _ Flint")] Rock_Flint = 6,
	}

	public static class ItemDropIdentifierExtension
	{
		public static ItemDropRecord GetRecord(this ItemDropIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ItemDropModel.GetRecord(identifier, editableRecord);
		}
	}
}
