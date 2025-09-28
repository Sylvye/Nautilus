using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordCreateButton : MonoBehaviour
{
    public void CreateRecord()
    {
        Vector2 pos;
        pos.x = float.Parse(RecordCreator.main.xField.text);
        pos.y = float.Parse(RecordCreator.main.yField.text);
        string label = RecordCreator.main.labelField.text;
        Color color = Color.HSVToRGB(RecordCreator.main.colorSlider.value, 1, 1);

        switch (RecordCreator.main.typeDropdown.value)
        {
            case 0:
                MapCameraController.CreatePointRecord(label, color, pos * 10);
                break;
            case 1:
                float angle = RecordCreator.main.dirCreator.transform.Find("Angle Slider").GetComponent<Slider>().value;
                MapCameraController.CreateDirectionRecord(label, color, pos * 10, angle * 360);
                break;
            case 2:
                float radius = float.Parse(RecordCreator.main.distCreator.transform.Find("Distance Field").GetComponent<TMP_InputField>().text);
                MapCameraController.CreateDistanceRecord(label, color, pos * 10, radius * 10);
                break;
            case 3:
                float radius2 = float.Parse(RecordCreator.main.areaCreator.transform.Find("Radius Field").GetComponent<TMP_InputField>().text);
                MapCameraController.CreateAreaRecord(label, color, pos * 10, radius2 * 10);
                break;
            default:
                Debug.Log("Something went terribly wrong");
                break;
        }

        RecordCreator.ExitRecordCreator();
    }
}
