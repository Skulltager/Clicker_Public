namespace SheetCodes
{
	//Generated code, do not edit!

	public enum RoomIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Start Room")] StartRoom = 2,
		[Identifier("Rat Cave")] RatCave = 3,
		[Identifier("Rocky Tomb")] RockyTomb = 4,
		[Identifier("Hallway")] Hallway = 5,
		[Identifier("Lava Area")] LavaArea = 6,
		[Identifier("Underwater")] Underwater = 7,
	}

	public static class RoomIdentifierExtension
	{
		public static RoomRecord GetRecord(this RoomIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.RoomModel.GetRecord(identifier, editableRecord);
		}
	}
}
