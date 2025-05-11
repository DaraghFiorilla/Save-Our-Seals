using UnityEngine;

public class GameEndButtons : MonoBehaviour
{
    private Sanc_GameManager gameManager;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<Sanc_GameManager>();
    }

    public void ExitGame()
    {
        gameManager.EndGame();
        gameManager.SaveAndExit();
    }

    public void ContinueGame()
    {
        gameManager.EndGame();
        Destroy(gameObject);
    }

    public void VisitSRI()
    {
        Application.OpenURL("https://www.sealrescueireland.org/");
    }
}
