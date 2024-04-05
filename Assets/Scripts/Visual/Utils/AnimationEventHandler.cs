using OrionGames;
using System;
using UnityEngine;

public abstract class AnimationEventHandler<T> : MonoBehaviour where T : struct, IConvertible
{
    public event Action<T> animationEvent;

    private void OnEvent_Animation(string eventData)
    {
        T eventType = eventData.GetIdentifierEnum<T>();

        if (animationEvent != null)
            animationEvent(eventType);
    }
}