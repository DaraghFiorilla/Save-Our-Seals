using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sanc_FeedAndEnrich : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject enrichPrefab;
    [SerializeField] private Transform canvas;
    [SerializeField] private Vector2 fishSpawnPos;
    [SerializeField] private Vector2 enrichSpawnPos;
    private GameObject fishObj;
    private GameObject enrichObj;
    public bool enrichObjActive;

    public void SpawnFish()
    {
        if (fishObj != null) { Destroy(fishObj); }
        fishObj = Instantiate(fishPrefab, canvas);
        fishObj.transform.localPosition = fishSpawnPos;
    }

    public void SpawnEnrich()
    {
        if (!enrichObjActive)
        {
            enrichObjActive = true;
            if (enrichObj != null) { Destroy(enrichObj);}
            enrichObj = Instantiate(enrichObj, canvas);
            enrichObj.transform.localPosition = enrichSpawnPos;
        }
    }
}
