using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    public void OutOfFuel(GameObject objToDisable)
    {
        objToDisable.SetActive(false);
        SwitchScene(0);
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
