
using System.Collections;
using UnityEngine;

public class WorldResourceHitFlashEffect : MonoBehaviour
{
    private const string PROPERTY_FLASH_COLOR = "flashColor";

    [SerializeField] private float hitEffectDuration;
    [SerializeField] private Color hitEffectColor;
    [SerializeField] private AnimationCurve hitEffectCurve;

    private WorldResource worldResource;
    private Material material => worldResource.material;
    private Coroutine routineHitEffect;

    private void Awake()
    {
        worldResource = GetComponentInParent<WorldResource>();
        worldResource.onHit += OnEvent_Hit;
    }

    private void OnEvent_Hit()
    {
        if (routineHitEffect != null)
            StopCoroutine(routineHitEffect);

        routineHitEffect = StartCoroutine(Routine_HitEffect());
    }

    private IEnumerator Routine_HitEffect()
    {
        float timeInState = Time.deltaTime;
        while (timeInState <= hitEffectDuration)
        {
            float timeFactor = timeInState / hitEffectDuration;
            Color color = hitEffectColor;
            color.a *= hitEffectCurve.Evaluate(timeFactor);
            material.SetColor(PROPERTY_FLASH_COLOR, color);

            yield return null;
            timeInState += Time.deltaTime;
        }

        material.SetColor(PROPERTY_FLASH_COLOR, Color.clear);
        routineHitEffect = null;
    }

    private void OnDestroy()
    {
        worldResource.onHit -= OnEvent_Hit;
    }
}