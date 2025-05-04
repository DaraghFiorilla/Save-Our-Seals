using UnityEngine;

public class TrashScript : MonoBehaviour
{
    public bool collected;
    public int myID;

    //private Rigidbody2D rb;
    private Sea_GameManager gameManager;

    private void Awake()
    {
        if (IsAlreadyCollected())
        {
            Destroy(gameObject);
            return;
        }
        //rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<Sea_GameManager>();
        gameManager.UpdateTrashNo(true);
    }

    public void BoatTriggered()
    {
        collected = true;
        SaveAsCollected();
        gameManager.UpdateTrashNo(false);
        Destroy(gameObject);
    }

    private void SaveAsCollected()
    {
        PlayerPrefs.SetInt(GetTrashKey(), 1);
        PlayerPrefs.Save();
    }


    private bool IsAlreadyCollected()
    {
        return PlayerPrefs.GetInt(GetTrashKey(), 0) == 1;
    }

    private string GetTrashKey()
    {
        return "TrashCollected_" + myID;
    }
}
