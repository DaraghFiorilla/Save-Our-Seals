using UnityEngine;
using UnityEngine.UI;

public class Knot : MonoBehaviour
{
    public enum KnotType {Knot, KnotFrayed, KnotTight }
    public KnotType myKnotType;

    private Image myImage;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }

    public void AssignSprite()
    {
        if (myImage == null) { Debug.Log("Knot " + gameObject.name + " is unassigned :("); }
        switch (myKnotType)
        {
            case KnotType.Knot:
                {
                    myImage.color = Color.white;
                    break;
                }
            case KnotType.KnotFrayed:
                {
                    myImage.color = Color.green;
                    break;
                }
            case KnotType.KnotTight:
                {
                    myImage.color = Color.red;
                    break;
                }
            default:
                {
                    Debug.Log("PANIC");
                    break;
                }
        }
    }
}
