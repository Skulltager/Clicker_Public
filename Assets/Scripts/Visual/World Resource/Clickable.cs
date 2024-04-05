
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Interactable interactable { private set; get; }
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        interactable = GetComponentInParent<Interactable>();
    }

    public void Initialize(Material sharedMaterial)
    {
        meshRenderer.sharedMaterial = sharedMaterial;
    }

    public bool CheckHitObject(Vector2 texCoord)
    {
        return interactable.CheckHitObject(texCoord);
    }
}