using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class RecordNextButton : MonoBehaviour
{
    public TMP_InputField labelField;
    public Slider colorSlider;
    public TMP_Dropdown typeDropdown;

    public void GoNext()
    {
        string dropdownText = typeDropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        Record.RecordType type = StringToType(dropdownText);
        switch (type)
        {
            case Record.RecordType.Point:
                RecordCreator.main.pointCreator.SetActive(true);
                break;
            case Record.RecordType.Direction:
                RecordCreator.main.dirCreator.SetActive(true);
                break;
            case Record.RecordType.Distance:
                RecordCreator.main.distCreator.SetActive(true);
                break;
            case Record.RecordType.Area:
                RecordCreator.main.areaCreator.SetActive(true);
                break;
            default:
                Debug.LogWarning("Invalid RecordType in switch statement");
                break;
        }

        RecordCreator.main.initializer.SetActive(false);
    }

    private Record.RecordType StringToType(string type) // keep, but for create button
    {
        return (Record.RecordType)Enum.Parse(typeof(Record.RecordType), type);
    }
}
