using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVisual : DataDrivenBehaviour<RoomDoor>
{
    private const string ANIMATOR_PROPERTY_OPEN = "Open";

    [SerializeField] private Material materialPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private MeshRenderer meshRendererLeft;
    [SerializeField] private MeshRenderer meshRendererRight;
    [SerializeField] private TriggerColliderCollection triggerColliderCollection;
    [SerializeField] private AnimationEventHandler_RoomDoor animationEventHandler;
    [SerializeField] private GameObject doorOpenColliderPrefab;
    [SerializeField] private Transform doorOpenCollidersContainer;

    private readonly List<GameObject> doorOpenColliderInstances;

    private Material material;

    private DoorVisual()
    {
        doorOpenColliderInstances = new List<GameObject>();
    }

    private void Awake()
    {
        material = new Material(materialPrefab);
        meshRendererLeft.sharedMaterial = material;
        meshRendererRight.sharedMaterial = material;
    }

    protected override void OnValueChanged_Data(RoomDoor oldValue, RoomDoor newValue)
    {
        if (oldValue != null)
        {
            foreach(GameObject instance in doorOpenColliderInstances)
                GameObject.Destroy(instance);

            doorOpenColliderInstances.Clear();
            oldValue.firstRoom.roomColor.onValueChange -= OnValueChanged_FirstRoomColor;
            oldValue.secondRoom.roomColor.onValueChange -= OnValueChanged_SecondRoomColor;
            triggerColliderCollection.anyInteractors.onValueChange -= OnValueChanged_AnyInteractors;
            animationEventHandler.animationEvent -= OnEvent_Animation;
            oldValue.doorOpen.value = false;
        }

        if (newValue != null)
        {
            foreach(Point doorPoint in newValue.doorOpenCells)
            {
                GameObject instance = GameObject.Instantiate(doorOpenColliderPrefab, doorOpenCollidersContainer);
                instance.transform.position = new Vector3(doorPoint.xIndex * 2, 0, doorPoint.yIndex * 2);
                doorOpenColliderInstances.Add(instance);
            }
            newValue.firstRoom.roomColor.onValueChangeImmediate += OnValueChanged_FirstRoomColor;
            newValue.secondRoom.roomColor.onValueChangeImmediate += OnValueChanged_SecondRoomColor;
            triggerColliderCollection.anyInteractors.onValueChangeImmediate += OnValueChanged_AnyInteractors;
            animationEventHandler.animationEvent += OnEvent_Animation;
        }
    }

    private void OnEvent_Animation(AnimationEventType_RoomDoor eventType)
    {
        switch(eventType)
        {
            case AnimationEventType_RoomDoor.Close:
                data.doorOpen.value = false;
                break;
            case AnimationEventType_RoomDoor.Open:
                data.doorOpen.value = true;
                break;
        }
    }

    private void OnValueChanged_AnyInteractors(bool oldValue, bool newValue)
    {
        animator.SetBool(ANIMATOR_PROPERTY_OPEN, newValue);
    }

    private void OnValueChanged_FirstRoomColor(float oldValue, float newValue)
    {
        material.SetFloat("firstRoomColor", newValue);
    }

    private void OnValueChanged_SecondRoomColor(float oldValue, float newValue)
    {
        material.SetFloat("secondRoomColor", newValue);
    }

    private void OnDestroy()
    {
        GameObject.Destroy(material);
        triggerColliderCollection.anyInteractors.onValueChange -= OnValueChanged_AnyInteractors;
    }
}