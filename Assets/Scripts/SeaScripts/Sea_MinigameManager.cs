using UnityEngine;

public class Sea_MinigameManager : MonoBehaviour
{
    public bool minigameActive;
    private Sea_GameManager gameManager;
    [SerializeField] private GameObject ropeMinigamePrefab;
    private GameObject activeMinigameObj;
    private Seal_SeaBehaviour activeSeal;
    private Canvas canvas;

    private void Awake()
    {
        gameManager = GetComponent<Sea_GameManager>();
        canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
    }

    public void CallRopeMinigame(Seal_SeaBehaviour seal)
    {
        if (!minigameActive)
        {
            Debug.Log("Calling rope minigame");
            activeSeal = seal;
            minigameActive = true;
            gameManager.paused = true;
            activeMinigameObj = Instantiate(ropeMinigamePrefab, canvas.transform);
            activeMinigameObj.transform.SetAsLastSibling();
        }
    }

    public void FinishRopeMinigame()
    {
        if (minigameActive)
        {
            Destroy(activeMinigameObj);
            activeMinigameObj = null;
            gameManager.paused = false;
            activeSeal.MinigameCompleted();
            activeSeal = null;
            minigameActive = false;
        }
    }
}
