using UnityEngine;

public class TrashScript : MonoBehaviour
{
    public bool collected;
    public int myID;
    [SerializeField] private Sprite[] trashSprites = new Sprite[2];
    [SerializeField] private float animTime;

    private SpriteRenderer spriteRenderer;
    private Sea_GameManager gameManager;
    private float timer;

    private void Awake()
    {
        if (IsAlreadyCollected())
        {
            Destroy(gameObject);
            return;
        }
        //rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<Sea_GameManager>();
        gameManager.UpdateTrashNo(true);
    }

    private void Update()
    {
        if (timer <= animTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            SwitchSprite();
        }
    }

    private void SwitchSprite()
    {
        if (spriteRenderer.sprite == trashSprites[0]) { spriteRenderer.sprite = trashSprites[1]; }
        else { spriteRenderer.sprite = trashSprites[0]; }
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
        return "TrashCollected_" + myID + "_";
    }
}
