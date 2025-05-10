using UnityEngine;
using System.Collections;

public class Sea_HealthySeal : MonoBehaviour
{
    public float maxTimeBetweenMoves;
    public float minTimeBetweenMoves;
    public float speed;
    public bool moving;
    private float currentTimeBetweenMoves;
    private float timer;
    private float radiusSize;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private Sea_GameManager gameManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<Sea_GameManager>();
        spriteRenderer.sprite = sprites[0];
        ResetMoveTime();
        moving = false;
    }

    private void Update()
    {
        if (!gameManager.paused)
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
        spriteRenderer.sprite = sprites[1];
        timer = 0;
        moving = true;
        Vector2 moveDir = Random.insideUnitCircle * radiusSize;
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

        spriteRenderer.sprite = sprites[0];
        spriteRenderer.flipX = false;
        ResetMoveTime();
        moving = false;
        yield return null;
    }
}
