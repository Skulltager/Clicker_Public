using System.Collections.Generic;
using UnityEngine;

public class MiniMapMarkerManager : MonoBehaviour
{
    private const string PROPERTY_ICON = "mainTex";
    [SerializeField] private MiniMapMarker miniMapMarkerPrefab;
    [SerializeField] private Transform miniMapMarkersContainer;
    [SerializeField] private Material spriteMaterialBase;

    private readonly List<MiniMapMarker> miniMapMarkerInstances;
    private readonly Dictionary<Texture, Material> spriteMaterials;

    private MiniMapMarkerManager()
    {
        miniMapMarkerInstances = new List<MiniMapMarker>();
        spriteMaterials = new Dictionary<Texture, Material>();
    }

    public void AddMiniMapMarker(Transform followTransform, Texture texture)
    {
        Material material;
        if(!spriteMaterials.TryGetValue(texture, out material))
        {
            material = new Material(spriteMaterialBase);
            spriteMaterials.Add(texture, material);
        }
        material.SetTexture(PROPERTY_ICON, texture);

        MiniMapMarker instance = GameObject.Instantiate(miniMapMarkerPrefab, miniMapMarkersContainer);
        instance.transform.localPosition = Vector3.zero;
        instance.data = new MiniMapMarkerData(followTransform, material);
        miniMapMarkerInstances.Add(instance);
    }

    public void RemoveMiniMapMarker(Transform followTransform)
    {
        MiniMapMarker instance = miniMapMarkerInstances.Find(i => i.data.followTransform == followTransform);
        GameObject.Destroy(instance.gameObject);
        miniMapMarkerInstances.Remove(instance);
    }

    public void UpdatePositions(Camera camera, float sizeMultiplier)
    {
        foreach (MiniMapMarker instance in miniMapMarkerInstances)
            instance.UpdatePosition(camera, sizeMultiplier);
    }

    private void OnDestroy()
    {
        foreach (Material material in spriteMaterials.Values)
            GameObject.Destroy(material);
    }
}