using System.Collections;
using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    public new RectTransform transform => base.transform as RectTransform;
    public static DamageIndicatorManager instance { private set; get; }

    [SerializeField] private DamageIndicator damageIndicatorPrefab;
    [SerializeField] private RectTransform damageIndicatorContainer;

    private void Awake()
    {
        instance = this;    
    }

    public void AddDamageIndicator(Vector3 worldPosition, int damage, int critLevel)
    {
        DamageIndicator instance = GameObject.Instantiate(damageIndicatorPrefab, damageIndicatorContainer);
        instance.Initialize(worldPosition, damage, critLevel);
    }    
}