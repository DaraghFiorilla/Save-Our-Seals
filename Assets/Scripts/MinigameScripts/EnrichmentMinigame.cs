using UnityEngine;

public class EnrichmentMinigame : MonoBehaviour
{
    private Sanc_GameManager gameManager;
    [SerializeField] private GameObject iceCubePrefab;
    [SerializeField] private int iceCubeCount;
    Vector3[] corners = new Vector3[4];
    private RectTransform targetRect;


    private void Awake()
    {
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
        gameObject.transform.SetAsLastSibling();
        targetRect = transform.GetChild(0).GetComponent<RectTransform>();
        // calculate rect transform corners
        targetRect.GetWorldCorners(corners);

        SpawnObjects();
    }

    private void SpawnObjects()
    {

        // Calculate boundaries
        float widthAndHeight = iceCubePrefab.GetComponent<RectTransform>().rect.width;
        float minX = corners[0].x + widthAndHeight / 2;
        float maxX = corners[2].x - widthAndHeight / 2;
        float minY = corners[0].y + widthAndHeight / 2;
        float maxY = corners[1].y - widthAndHeight / 2;
        for (int i = 0; i < iceCubeCount; i++)
        {
            GameObject g = Instantiate(iceCubePrefab, gameObject.transform.GetChild(0));
            Vector3 randomWorldPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
            // convert world to local position for UI
            Vector3 localPos = targetRect.InverseTransformPoint(randomWorldPos);
            g.transform.localPosition = localPos;
        }

        // spawn 1 fish / toy / seaweed etc
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
