using UnityEngine;
using System.Collections;

public class Seal_SeaBehaviour : MonoBehaviour
{
    public float maxTimeBetweenMoves;
    public float minTimeBetweenMoves;
    public float currentTimeBetweenMoves;
    public float timer;
    public float speed;
    public bool moving;
    public float radiusSize;
    public bool collided;

    private Rigidbody2D rb;
    private Sea_GameManager gameManager;
    [SerializeField] private Sprite[] sprites = new Sprite[2];
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetMoveTime();
        moving = false;
        gameManager = FindAnyObjectByType<Sea_GameManager>();
        gameManager.UpdateSealNo(true);
        spriteRenderer= GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    private void Update()
    {
        if (!collided)
        {
            if (timer < currentTimeBetweenMoves && !moving) { timer += Time.deltaTime; }
            else if (!moving && timer >= currentTimeBetweenMoves) { StartCoroutine(MoveSeal()); }
        }
    }

    void ResetMoveTime()
    {
        currentTimeBetweenMoves = Random.Range(minTimeBetweenMoves, maxTimeBetweenMoves);
        radiusSize = Random.Range(4.0f, 6.0f);
    }

    private IEnumerator MoveSeal()
    {
        Debug.Log("Starting move seal");
        spriteRenderer.sprite = sprites[1];
        timer = 0;
        moving = true;
        Vector2 moveDir = Random.insideUnitCircle.normalized * radiusSize;
        float moveTime = 3f;
        if (moveDir.x > 0) { spriteRenderer.flipX = true; }
        
        while (moveTime > 0)
        {
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
        collided = true;
        gameManager.UpdateSealNo(false);
        Destroy(gameObject);
    }
}
