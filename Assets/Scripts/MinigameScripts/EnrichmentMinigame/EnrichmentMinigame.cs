using UnityEngine;
using UnityEngine.UI;

public class EnrichmentMinigame : MonoBehaviour
{
    [SerializeField] private GameObject iceCubePrefab;
    [SerializeField] private GameObject[] iceCubeObjs;
    [SerializeField] private GameObject[] freezeObjs;
    [SerializeField] private Vector2 blockSpawnPos;
    public GameObject tutorialArrow;
    private Freezer freezer;
    [HideInInspector] public Vector3[] corners = new Vector3[4];
    private RectTransform targetRect;
    public int slotsFilled;
    [SerializeField] private GameObject successWindow;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private Vector2 spawnPos;
    private Sanc_FeedAndEnrich feedEnrichManager;

    private void Awake()
    {
        freezer = FindFirstObjectByType<Freezer>();
        gameObject.transform.SetAsLastSibling();
        targetRect = GameObject.Find("IceBox").GetComponent<RectTransform>();
        feedEnrichManager = FindFirstObjectByType<Sanc_FeedAndEnrich>();
    }

    private void Start()
    {
        // calculate rect transform corners
        targetRect.GetWorldCorners(corners);
        SetStartingPos();
    }

    private void SetStartingPos()
    {
        // Calculate boundaries
        float widthAndHeight = iceCubePrefab.GetComponent<RectTransform>().rect.width; // hardcoded because for some reason this code stopped working ->//iceCubePrefab.GetComponent<RectTransform>().rect.width;
        float minX = corners[0].x + widthAndHeight / 2;
        float maxX = corners[2].x - widthAndHeight / 2;
        float minY = corners[0].y + widthAndHeight / 2;
        float maxY = corners[1].y - widthAndHeight / 2;
        foreach (GameObject cube in iceCubeObjs)
        {
            cube.transform.localPosition = CalcPos(minX, maxX, minY, maxY);
        }
        foreach (GameObject obj in freezeObjs)
        {
            float wAndH = obj.GetComponent<RectTransform>().rect.width;
            float fMinX = corners[0].x + wAndH / 2;
            float fMaxX = corners[2].x - wAndH / 2;
            float fMinY = corners[0].y + wAndH / 2;
            float fMaxY = corners[1].y - wAndH / 2;
            obj.transform.localPosition = CalcPos(fMinX, fMaxX, fMinY, fMaxY);
        }
        // spawn 1 fish / toy / seaweed etc
    }

    public Vector3 CalcPos(float minX, float maxX, float minY, float maxY)
    {
        Vector3 randomWorldPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
        // convert world to local position for UI
        Vector3 localPos = targetRect.InverseTransformPoint(randomWorldPos);
        return localPos;
    }

    public void DestroySelf() // referenced by button
    {
        Destroy(gameObject);
    }

    public void FillSlot()
    {
        slotsFilled++;
        if (slotsFilled >= 3)
        {
            UnlockFreezer();
        }
    }

    public void UnlockFreezer()
    {
        // play another click type noise
        transform.Find("IceBox/GrayOutBox").gameObject.SetActive(true);
        freezer.transform.Find("GrayOutBox").gameObject.SetActive(false);
        tutorialArrow.SetActive(true);
    }

    public void SpawnEndWindow()
    {
        successWindow.SetActive(true);
        quitButton.SetActive(false);
    }

    public void EndMinigame()
    {
        feedEnrichManager.SpawnEnrich();
        Destroy(gameObject);
    }
}
