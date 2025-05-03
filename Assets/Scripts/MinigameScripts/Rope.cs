using UnityEngine;
using UnityEngine.UI;

public class Rope : MonoBehaviour
{
    public Knot knotA;
    public Knot knotB;
    public bool isCut;
    //[SerializeField] private Sprite[] sprites = new Sprite[2];

    private BoxCollider2D col;
    private Image image;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        image = GetComponent<Image>();
        image.color = Color.white;
    }

    public void Cut()
    {
        if (isCut) { return; }
        isCut = true;
        col.enabled = false;
        image.color = Color.red;
    }
}
