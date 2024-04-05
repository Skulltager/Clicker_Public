namespace SheetCodes
{
	//Generated code, do not edit!

	public enum TokenIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Rock Token")] RockToken = 1,
		[Identifier("Rat Token")] RatToken = 2,
		[Identifier("Pickaxe Token")] PickaxeToken = 3,
		[Identifier("Axe Token")] AxeToken = 4,
		[Identifier("Sword Token")] SwordToken = 5,
	}

	public static class TokenIdentifierExtension
	{
		public static TokenRecord GetRecord(this TokenIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.TokenModel.GetRecord(identifier, editableRecord);
		}
	}
}
