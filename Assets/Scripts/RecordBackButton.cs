using UnityEngine;

public class RecordBackButton : MonoBehaviour
{
    public void GoBack()
    {
        RecordCreator.main.pointCreator.SetActive(false);
        RecordCreator.main.dirCreator.SetActive(false);
        RecordCreator.main.distCreator.SetActive(false);
        RecordCreator.main.areaCreator.SetActive(false);
        RecordCreator.main.initializer.SetActive(true);
    }
}
