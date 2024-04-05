using SheetCodes;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private SphereCollider groundCheckCollider;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private ItemPickupLocation itemPickupLocation;
    [SerializeField] private Texture miniMapIconTexture;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float maxCameraUpwardAngle;
    [SerializeField] private float maxCameraDownwardAngle;

    [Header("Movement")]
    [SerializeField] private float groundRaycastDistance;
    [SerializeField] private float groundMaxAngle;

    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundMaxMovementSpeed;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airMaxMovementSpeed;
    [SerializeField] private float jumpForce;

    [Header("Attack")]
    [SerializeField] private float attackDistance;

    private readonly EventVariable<PlayerController, Point> mapPoint;

    private bool jump;
    private float lastPhysicsTime;
    private PlayerInventory playerInventory;

    private PlayerController()
    {
        mapPoint = new EventVariable<PlayerController, Point>(this, default);
    }

    private void Awake()
    {
        playerInventory = new PlayerInventory();
        itemPickupLocation.inventory = playerInventory.inventory;
        SetMapPoint();
        MapChunk mapChunk = mapGenerator.GetMapChunk(mapPoint.value);
        mapChunk.GetChunkRoom(mapPoint.value).playersInRoom.value++;

        mapPoint.onValueChange += OnValueChanged_MapPoint;
        MiniMapVisualizer.instance.AddMiniMapMarker(cameraTransform, miniMapIconTexture);
    }

    private void Start()
    {
        IngameScreenManager.instance.SetHudData(playerInventory);
    }

    private void SetMapPoint()
    {
        int xPosition = Mathf.RoundToInt(transform.position.x / 2);
        int yPosition = Mathf.RoundToInt(transform.position.z / 2);
        mapPoint.value = new Point(xPosition, yPosition);
    }

    private void OnValueChanged_MapPoint(Point oldValue, Point newValue)
    {
        MapChunk mapChunk = mapGenerator.GetMapChunk(newValue);
        ChunkRoom room = mapChunk.GetChunkRoom(newValue);
        room.playersInRoom.value++;

        mapChunk = mapGenerator.GetMapChunk(oldValue);
        room = mapChunk.GetChunkRoom(oldValue);
        room.playersInRoom.value--;

    }

    private bool TryGetClickable(out Clickable clickable, out RaycastHit raycastHit)
    {
        int environmentLayer = LayerMask.NameToLayer("Environment");
        int layerMask = 1 << LayerMask.NameToLayer("Clickables");
        layerMask += 1 << environmentLayer;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, attackDistance, layerMask, QueryTriggerInteraction.Collide);
        Array.Sort(hits, delegate (RaycastHit hit1, RaycastHit hit2) { return hit1.distance.CompareTo(hit2.distance); });
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer == environmentLayer)
                break;

            if (!hit.collider.TryGetComponent(out clickable))
                continue;

            if (!clickable.CheckHitObject(hit.textureCoord))
                continue;

            raycastHit = hit;
            return true;
        }

        clickable = default;
        raycastHit = default;
        return false;
    }

    private void Update()
    {
        Update_CameraRotation();
        Update_Jump();
        Update_Clickable();
        Update_OpenCraftingMenu();
        Update_OpenInventoryMenu();
        Update_OpenWorldMap();
        SetMapPoint();
    }

    private void Update_OpenCraftingMenu()
    {
        if (!Input.GetKeyDown(KeyCode.F))
            return;

        IngameScreenManager.instance.ShowHideScreen_CraftingMenu(playerInventory.crafter);
    }

    private void Update_OpenInventoryMenu()
    {
        if (!Input.GetKeyDown(KeyCode.I))
            return;

        IngameScreenManager.instance.ShowHideScreen_PlayerInventoryDisplay(playerInventory);
    }

    private void Update_OpenWorldMap()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
            return;

        IngameScreenManager.instance.ShowHideScreen_WorldMapDisplay();
    }

    private void Update_Clickable()
    {
        Clickable clickable;
        RaycastHit hit;
        if (TryGetClickable(out clickable, out hit))
        {
            if (clickable.interactable is WorldResource worldResource)
                MonsterIndicator.instance.data = worldResource;
            else
                MonsterIndicator.instance.data = null;

            Update_Interact(clickable, hit);
        }
        else
        {
            MonsterIndicator.instance.data = null;
        }
    }

    private void Update_Interact(Clickable clickable, RaycastHit hit)
    {
        if (CursorController.instance.cursorVisibleCount.value > 0)
            return;

        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        Vector3 direction = (hit.point - cameraTransform.position).normalized;
        ParticleController.PlayParticleInstance(ParticleIdentifier.HitParticle, hit.point - direction * 0.05f);

        clickable.interactable.Interact(playerInventory, itemPickupLocation);
    }

    private void FixedUpdate()
    {
        lastPhysicsTime = Time.time;
        bool isAirBorn = IsAirBorn();
        float maxMovementSpeed = isAirBorn ? airMaxMovementSpeed : groundMaxMovementSpeed;
        float acceleration = isAirBorn ? airAcceleration : groundAcceleration;
        FixedUpdate_Jump(isAirBorn);
        FixedUpdate_Velocity(maxMovementSpeed, acceleration);
        playerInventory.crafter.CraftTick();
    }

    private void FixedUpdate_Velocity(float maxMovementSpeed, float acceleration)
    {
        Vector3 desiredVelocity = Vector3.zero;

        if (!playerInventory.inventory.overWeight.value && CursorController.instance.cursorVisibleCount.value == 0)
        {
            if (Input.GetKey(KeyCode.W))
                desiredVelocity += transform.forward;
            if (Input.GetKey(KeyCode.S))
                desiredVelocity -= transform.forward;

            if (Input.GetKey(KeyCode.D))
                desiredVelocity += transform.right;
            if (Input.GetKey(KeyCode.A))
                desiredVelocity -= transform.right;

            if(desiredVelocity.sqrMagnitude > 0)
                desiredVelocity.Normalize();

            desiredVelocity *= maxMovementSpeed;
        }

        float moveDelta = acceleration * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
        Vector3 newVelocity = Vector3.MoveTowards(horizontalVelocity, desiredVelocity, moveDelta);
        newVelocity.y = rigidBody.velocity.y;
        rigidBody.velocity = newVelocity;
    }

    private void Update_CameraRotation()
    {
        if (CursorController.instance.cursorVisibleCount.value == 0)
        {
            float xDifference = Input.GetAxis("Mouse X") * mouseSensitivity;
            float yDifference = Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.Rotate(0, xDifference, 0);
            Vector3 cameraAngles = cameraTransform.localRotation.eulerAngles;
            cameraAngles = GetFixedEulerAngles(cameraAngles);
            cameraAngles.x -= yDifference;
            cameraAngles.x = Mathf.Clamp(cameraAngles.x, maxCameraDownwardAngle, maxCameraUpwardAngle);
            cameraTransform.localRotation = Quaternion.Euler(cameraAngles);
        }

        float physicsTimeDifference = Time.time - lastPhysicsTime;
        Vector3 interpolationDistance = transform.worldToLocalMatrix * (rigidBody.velocity * physicsTimeDifference);
        cameraTransform.localPosition = interpolationDistance;

        Camera.main.transform.rotation = cameraTransform.rotation;
        Camera.main.transform.position = cameraTransform.position;
    }

    private void Update_Jump()
    {
        if (CursorController.instance.cursorVisibleCount.value > 0)
            return;

        if (playerInventory.inventory.overWeight.value)
            return;

        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        jump = true;
    }

    private void FixedUpdate_Jump(bool isAirBorn)
    {
        if (!jump)
            return;

        jump = false;

        if (isAirBorn)
            return;

        Vector3 velocity = rigidBody.velocity;
        velocity.y += jumpForce;
        rigidBody.velocity = velocity;
    }

    private bool IsAirBorn()
    {
        float scale = groundCheckCollider.transform.lossyScale.y;
        RaycastHit raycastHit;
        Vector3 feetPosition = groundCheckCollider.transform.position;
        feetPosition.y += groundCheckCollider.contactOffset * 2;

        float distance = groundRaycastDistance + groundCheckCollider.contactOffset * 2;
        int layerMask = 1 << LayerMask.NameToLayer("Floor");
        if (!Physics.SphereCast(feetPosition, groundCheckCollider.radius * scale, Vector3.down * distance, out raycastHit, groundRaycastDistance, layerMask))
            return true;

        float verticalMagnitudeSquared = Mathf.Sqrt(raycastHit.normal.x * raycastHit.normal.x + raycastHit.normal.z * raycastHit.normal.z);
        float slope = Mathf.Atan2(verticalMagnitudeSquared, raycastHit.normal.y) * Mathf.Rad2Deg;

        if (slope > groundMaxAngle)
            return true;

        return false;
    }

    private Vector3 GetFixedEulerAngles(Vector3 angle)
    {
        if (angle.x > 180)
            angle.x -= 360;

        if (angle.y != 180)
            return angle;

        if (angle.z != 180)
            return angle;

        if (angle.x > 0)
            angle.x = 180 - angle.x;
        else
            angle.x = -180 - angle.x;

        angle.y = 0;
        angle.z = 0;
        return angle;
    }

    private void OnDestroy()
    {
        mapPoint.onValueChange -= OnValueChanged_MapPoint;

        if (mapGenerator == null)
            return;

        MapChunk mapChunk = mapGenerator.GetMapChunk(mapPoint.value);
        mapChunk.GetChunkRoom(mapPoint.value).playersInRoom.value--;
    }
}
