using System.Collections;
using UnityEngine;

public class PrefabCollection : MonoBehaviour
{
    public static PrefabCollection instance { private set; get; }

    [SerializeField] private WorldResource_Monster monsterPrefab;
    public WorldResource_Monster MonsterPrefab => monsterPrefab;

    [SerializeField] private WorldResource_StaticObject staticObjectPrefab;
    public WorldResource_StaticObject StaticObjectPrefab => staticObjectPrefab;

    private void Awake()
    {
        instance = this;
    }
}