using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldToSlider : MonoBehaviour
{
    public Slider slider;
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void UpdateSlider()
    {
        slider.value = float.Parse(inputField.text) / 360f;
    }
}
