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
        if (cut) { image.color = Color.red; }
        else { image.color = Color.white; }
    }

    public void ResetRope()
    {
        //col.enabled = true;
        image.raycastTarget = true;
        ChangeSprite(false);
        isCut = false;
    }
}
