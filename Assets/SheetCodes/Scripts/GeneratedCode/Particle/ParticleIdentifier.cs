namespace SheetCodes
{
	//Generated code, do not edit!

	public enum ParticleIdentifier
	{
		[Identifier("None")] None = 0,
		[Identifier("Hit Particle")] HitParticle = 1,
		[Identifier("Death Particle")] DeathParticle = 2,
		[Identifier("Rock Hit Particle")] RockHitParticle = 3,
	}

	public static class ParticleIdentifierExtension
	{
		public static ParticleRecord GetRecord(this ParticleIdentifier identifier, bool editableRecord = false)
		{
			return ModelManager.ParticleModel.GetRecord(identifier, editableRecord);
		}
	}
}
