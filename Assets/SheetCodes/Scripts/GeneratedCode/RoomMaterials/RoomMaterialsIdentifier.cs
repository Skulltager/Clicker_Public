namespace SheetCodes
{
	//Generated code, do not edit!

	public enum RoomMaterialsIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Outside")] Outside = 1,
		[Identifier("Desert")] Desert = 2,
		[Identifier("Underground")] Underground = 3,
		[Identifier("Space")] Space = 4,
		[Identifier("Underwater")] Underwater = 5,
		[Identifier("Mars")] Mars = 6,
	}

	public static class RoomMaterialsIdentifierExtension
	{
		public static RoomMaterialsRecord GetRecord(this RoomMaterialsIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.RoomMaterialsModel.GetRecord(identifier, editableRecord);
		}
	}
}
