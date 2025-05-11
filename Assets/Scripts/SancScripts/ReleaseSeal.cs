using UnityEngine;

public class ReleaseSeal : MonoBehaviour
{
    public Seal_SancBehaviour releasedSeal; // seal assigns itself here
    public GameObject[] imageObjs; // seal sets the right image as active

    public void StartRelease()
    {
        releasedSeal.ReleaseSeal();
    }
}
