
using UnityEngine;

public class WorldResourceHitAnimationEffect : MonoBehaviour
{
    [SerializeField] private AnimatedScale animatedScale;

    private WorldResource worldResource;

    private void Awake()
    {
        worldResource = GetComponentInParent<WorldResource>();
        worldResource.onHit += OnEvent_Hit;
    }

    private void OnEvent_Hit()
    {
        animatedScale.Restart();
    }

    private void OnDestroy()
    {
        worldResource.onHit -= OnEvent_Hit;
    }
}