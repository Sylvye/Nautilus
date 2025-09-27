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
            case 1:
                MapCameraController.CreatePointRecord(label, color, pos);
                break;
            case 2:
                float angle = RecordCreator.main.dirCreator.transform.Find("Angle Slider").GetComponent<Slider>().value;
                MapCameraController.CreateDirectionRecord(label, color, pos, angle);
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }
}
