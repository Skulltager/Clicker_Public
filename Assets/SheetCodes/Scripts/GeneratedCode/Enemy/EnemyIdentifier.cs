namespace SheetCodes
{
	//Generated code, do not edit!

	public enum EnemyIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Rat")] Rat = 1,
	}

	public static class EnemyIdentifierExtension
	{
		public static EnemyRecord GetRecord(this EnemyIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.EnemyModel.GetRecord(identifier, editableRecord);
		}
	}
}
