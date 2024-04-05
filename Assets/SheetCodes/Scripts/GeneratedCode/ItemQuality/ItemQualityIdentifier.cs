namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ItemQualityIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Horrendous")] Horrendous = 1,
		[Identifier("Terrible")] Terrible = 2,
		[Identifier("Bad")] Bad = 3,
		[Identifier("Fair")] Fair = 4,
		[Identifier("Average")] Average = 5,
		[Identifier("Decent")] Decent = 6,
		[Identifier("Good")] Good = 7,
		[Identifier("Excellent")] Excellent = 8,
		[Identifier("Masterwork")] Masterwork = 9,
	}

	public static class ItemQualityIdentifierExtension
	{
		public static ItemQualityRecord GetRecord(this ItemQualityIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ItemQualityModel.GetRecord(identifier, editableRecord);
		}
	}
}
