using UnityEngine;

public class TriggerColliderCollection : MonoBehaviour
{
    public readonly EventVariable<TriggerColliderCollection, bool> anyInteractors;
    public readonly EventList<TriggerCollider> triggerColliders;
    private readonly EventList<InteractorCount> interactorCounts;

    private TriggerColliderCollection()
    {
        interactorCounts = new EventList<InteractorCount>();
        anyInteractors = new EventVariable<TriggerColliderCollection, bool>(this, false);
        triggerColliders = new EventList<TriggerCollider>();
    }

    private void Awake()
    {
        triggerColliders.onAdd += OnAdd_TriggerCollider;
        triggerColliders.onRemove += OnRemove_TriggerCollider;
    }

    private void OnAdd_TriggerCollider(TriggerCollider item)
    {
        item.onTriggerEnter += OnEvent_TriggerEnter;
        item.onTriggerExit += OnEvent_TriggerExit;
    }

    private void OnRemove_TriggerCollider(TriggerCollider item)
    {
        item.onTriggerEnter -= OnEvent_TriggerEnter;
        item.onTriggerExit -= OnEvent_TriggerExit;
    }

    private void OnEvent_TriggerEnter(Interactor interactor)
    {
        InteractorCount interactorCount = interactorCounts.Find(i => i.interactor == interactor);
        if (interactorCount == null)
        {
            interactorCount = new InteractorCount(interactor);
            interactorCounts.Add(interactorCount);
            anyInteractors.value = true;
        }
        interactorCount.interactorCount.value++;

    }

    private void OnEvent_TriggerExit(Interactor interactor)
    {
        InteractorCount interactorCount = interactorCounts.Find(i => i.interactor == interactor);
        interactorCount.interactorCount.value--;

        if (interactorCount.interactorCount.value == 0)
        {
            interactorCounts.Remove(interactorCount);

            if (interactorCounts.Count == 0)
                anyInteractors.value = false;
        }
    }
}