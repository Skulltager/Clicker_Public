using UnityEngine;
using SheetCodes;
using System;
using System.Collections.Generic;
using MirzaBeig.ParticleSystems;

public class ParticleController : MonoBehaviour
{
    public static ParticleController instance { private set; get; }
    private readonly Dictionary<ParticleIdentifier, ParticleSystem[]> particleSystems;

    private ParticleController()
    {
        particleSystems = new Dictionary<ParticleIdentifier, ParticleSystem[]>();
    }

    private void Awake()
    {
        ParticleIdentifier[] identifiers = Enum.GetValues(typeof(ParticleIdentifier)) as ParticleIdentifier[];
        foreach(ParticleIdentifier identifier in identifiers)
        {
            if (identifier == ParticleIdentifier.None)
                continue;

            ParticleRecord record = identifier.GetRecord();
            ParticleSystem instance = GameObject.Instantiate(record.Prefab, transform);
            particleSystems.Add(identifier, instance.GetComponentsInChildren<ParticleSystem>());
        }
        instance = this;
    }

    public static void PlayParticleInstance(ParticleIdentifier identifier, Vector3 worldPosition)
    {
        if (instance == null)
            return;

        ParticleSystem[] particleSystemCollection = instance.particleSystems[identifier];
        particleSystemCollection[0].transform.position = worldPosition;
        foreach(ParticleSystem particleSystem in particleSystemCollection)
            particleSystem.Emit(1);
    }
}