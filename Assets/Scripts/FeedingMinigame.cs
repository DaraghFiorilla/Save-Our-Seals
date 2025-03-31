using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FeedingMinigame : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private Transform canvas;
    [SerializeField] private Vector2 spawnPos;
    private GameObject fishObj;

    public void SpawnFish()
    {
        if (fishObj != null)
        {
            Destroy(fishObj);
        }
        fishObj = Instantiate(fishPrefab, canvas);
        fishObj.transform.localPosition = spawnPos;
    }
}
