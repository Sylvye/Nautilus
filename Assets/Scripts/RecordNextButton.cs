using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RecordNextButton : MonoBehaviour
{
    public TMP_Dropdown typeDropdown;
    public TMP_InputField angleField;
    public TMP_InputField distanceField;
    public TMP_InputField areaField;

    public void GoNext()
    {
        Vector2 pos;
        pos.x = float.Parse(RecordCreator.main.xField.text);
        pos.y = float.Parse(RecordCreator.main.yField.text);
        string label = RecordCreator.main.labelField.text;
        Color color = Color.HSVToRGB(RecordCreator.main.colorSlider.value, 1, 1);

        switch (typeDropdown.value)
        {
            case 0:
                RecordCreator.main.pointCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.pointCreator;
                RecordCreator.main.previewRecord = MapCameraController.CreatePointRecord(label, color, pos * 10);
                break;
            case 1:
                RecordCreator.main.dirCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.dirCreator;
                RecordCreator.main.previewRecord = MapCameraController.CreateDirectionRecord(label, color, pos * 10, angleField.text.Equals("") ? 0 : float.Parse(angleField.text)); // default angle 0
                break;
            case 2:
                RecordCreator.main.distCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.distCreator;
                RecordCreator.main.previewRecord = MapCameraController.CreateDistanceRecord(label, color, pos * 10, distanceField.text.Equals("") ? 10 : float.Parse(distanceField.text)); // default radius 10
                break;
            case 3:
                RecordCreator.main.areaCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.areaCreator;
                RecordCreator.main.previewRecord = MapCameraController.CreateAreaRecord(label, color, pos * 10, areaField.text.Equals("") ? 10 : float.Parse(areaField.text)); // default radius 10
                break;
            default:
                Debug.LogWarning("Invalid RecordType in switch statement");
                break;
        }

        RecordCreator.main.initializer.SetActive(false);
        RecordCreator.main.beacon.SetActive(false);
    }
}
