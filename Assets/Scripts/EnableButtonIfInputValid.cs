using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnableButtonIfInputValid : MonoBehaviour
{
    public Button button;
    public TMP_InputField field;

    public void ValidateLabel()
    {
        button.interactable = !string.IsNullOrWhiteSpace(field.text);
    }
}
