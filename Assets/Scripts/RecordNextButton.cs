using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class RecordNextButton : MonoBehaviour
{
    public TMP_Dropdown typeDropdown;

    public void GoNext()
    {
        Record.RecordType type = (Record.RecordType)typeDropdown.value+1; // +1 for None
        switch (type)
        {
            case Record.RecordType.Point:
                RecordCreator.main.pointCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.pointCreator;
                break;
            case Record.RecordType.Direction:
                RecordCreator.main.dirCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.dirCreator;
                break;
            case Record.RecordType.Distance:
                RecordCreator.main.distCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.distCreator;
                break;
            case Record.RecordType.Area:
                RecordCreator.main.areaCreator.SetActive(true);
                RecordCreator.main.activeMenu = RecordCreator.main.areaCreator;
                break;
            default:
                Debug.LogWarning("Invalid RecordType " + type + " in switch statement");
                break;
        }

        RecordCreator.main.initializer.SetActive(false);
    }

    
}
