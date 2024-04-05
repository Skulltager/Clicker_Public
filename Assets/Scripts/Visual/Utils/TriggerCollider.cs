using System;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    private TriggerColliderCollection colliderCollection;
    public event Action<Interactor> onTriggerEnter;
    public event Action<Interactor> onTriggerExit;

    private void Awake()
    {
        colliderCollection = GetComponentInParent<TriggerColliderCollection>();
    }

    private void OnEnable()
    {
        colliderCollection.triggerColliders.Add(this);
    }

    private void OnDisable()
    {
        colliderCollection.triggerColliders.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactor interactor;
        if (!other.TryGetComponent(out interactor))
            return;

        if (onTriggerEnter != null)
            onTriggerEnter(interactor);
    }

    private void OnTriggerExit(Collider other)
    {
        Interactor interactor;
        if (!other.TryGetComponent(out interactor))
            return;

        if (onTriggerExit != null)
            onTriggerExit(interactor);
    }
}