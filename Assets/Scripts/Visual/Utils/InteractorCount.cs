using System.Collections;
using UnityEngine;

public class InteractorCount
{
    public readonly Interactor interactor;
    public readonly EventVariable<InteractorCount, int> interactorCount;

    public InteractorCount(Interactor interactor)
    {
        this.interactor = interactor;
        interactorCount = new EventVariable<InteractorCount, int>(this, 0);
    }
}