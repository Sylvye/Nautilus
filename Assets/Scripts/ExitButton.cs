using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public GameObject obj;

    public void DisableObject()
    {
        obj.SetActive(false);
    }
}
