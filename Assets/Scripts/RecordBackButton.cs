using UnityEngine;

public class RecordBackButton : MonoBehaviour
{
    public void GoBack()
    {
        RecordCreator.main.activeMenu.SetActive(false);
        RecordCreator.main.initializer.SetActive(true);
    }
}
