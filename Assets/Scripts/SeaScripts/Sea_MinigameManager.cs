using UnityEngine;

public class Sea_MinigameManager : MonoBehaviour
{
    public bool minigameActive;
    private Sea_GameManager gameManager;
    [SerializeField] private GameObject ropeMinigamePrefab;
    private GameObject activeMinigameObj;
    private Seal_SeaBehaviour activeSeal;

    public void CallRopeMinigame(Seal_SeaBehaviour seal)
    {
        if (!minigameActive)
        {
            activeSeal = seal;
            minigameActive = true;
            gameManager.paused = true;
            activeMinigameObj = Instantiate(ropeMinigamePrefab);
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
        }
    }
}
