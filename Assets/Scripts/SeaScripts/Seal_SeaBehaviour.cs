using UnityEngine;
using System.Collections;

public class Seal_SeaBehaviour : MonoBehaviour
{
    public float maxTimeBetweenMoves;
    public float minTimeBetweenMoves;
    public float speed;
    public bool moving;
    public bool collected;
    private float radiusSize;
    private float currentTimeBetweenMoves;
    private float timer;

    [Header("Identification")]
    [Tooltip("ID must be unique to avoid sanc seal ID conflicts. ID needs to be below 100 for this")]
    public int myID;
    public int age;
    public string sealType;

    private Rigidbody2D rb;
    private Sea_GameManager gameManager;
    private Sea_MinigameManager minigameManager;
    [SerializeField] private Sprite[] sprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (IsAlreadyCollected())
        {
            Destroy(gameObject);
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        ResetMoveTime();
        moving = false;
        gameManager = FindFirstObjectByType<Sea_GameManager>();
        minigameManager = FindFirstObjectByType<Sea_MinigameManager>();
        gameManager.UpdateSealNo(true);
        spriteRenderer= GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    private void Update()
    {
        if (!gameManager.paused)
        {
            if (!collected)
            {
                if (timer < currentTimeBetweenMoves && !moving) { timer += Time.deltaTime; }
                else if (!moving && timer >= currentTimeBetweenMoves) { StartCoroutine(MoveSeal()); }
            }
        }
    }

    void ResetMoveTime()
    {
        currentTimeBetweenMoves = Random.Range(minTimeBetweenMoves, maxTimeBetweenMoves);
        radiusSize = Random.Range(4.0f, 6.0f);
    }

    private IEnumerator MoveSeal()
    {
        //Debug.Log("Starting move seal");
        spriteRenderer.sprite = sprites[1];
        timer = 0;
        moving = true;
        Vector2 moveDir = Random.insideUnitCircle.normalized * radiusSize;
        float moveTime = 3f;
        if (moveDir.x > 0) { spriteRenderer.flipX = true; }
        
        while (moveTime > 0)
        {
            while (gameManager.paused)
            {
                yield return null;
            }
            moveTime -= Time.deltaTime;
            rb.MovePosition((Vector2)transform.position + speed * moveDir * Time.deltaTime);
            yield return null;
        }

        // END COROUTINE
        spriteRenderer.sprite = sprites[0];
        spriteRenderer.flipX = false;
        ResetMoveTime();
        moving = false;
        yield return null;
    }

    public void BoatTriggered()
    {
        // when multiple minigames are incorporated, add function to choose randomly
        minigameManager.CallRopeMinigame(this);
    }

    public void MinigameCompleted()
    {
        collected = true;
        SaveAsCollected();
        gameManager.UpdateSealNo(false);
        Destroy(gameObject);
    }

    private void SaveAsCollected()
    {
        PlayerPrefs.SetInt(GetSealKey(), 1); // collected flag
        PlayerPrefs.SetString(GetSealKey("Type"), sealType);
        PlayerPrefs.SetInt(GetSealKey("Age"), age);
        PlayerPrefs.Save();
    }

    private bool IsAlreadyCollected()
    {
        return PlayerPrefs.GetInt(GetSealKey(), 0) == 1;
    }

    private string GetSealKey(string suffix = "")
    {
        return "SealCollected_" + myID + suffix;
    }
}
