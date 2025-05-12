using UnityEngine;
using UnityEngine.UI;

public class Rope : MonoBehaviour
{
    public Knot knotA;
    public Knot knotB;
    public bool isCut;
    //[SerializeField] private Sprite[] sprites = new Sprite[2];

    //private BoxCollider2D col;
    private Image image;
    [SerializeField] private RopeMinigame minigameManager;

    private void Awake()
    {
        //col = GetComponent<BoxCollider2D>();
        image = GetComponent<Image>();
        ChangeSprite(false);
    }

    public void Cut()
    {
        if (isCut) { return; }
        isCut = true;
        //col.enabled = false;
        image.raycastTarget = false;
        ChangeSprite(true);
    }

    public void ChangeSprite(bool cut)
    {
        if (cut) { image.sprite = minigameManager.ropeSprites[1]; }
        else { image.sprite = minigameManager.ropeSprites[0]; }
    }

    public void ResetRope()
    {
        //col.enabled = true;
        image.raycastTarget = true;
        ChangeSprite(false);
        isCut = false;
    }
}
