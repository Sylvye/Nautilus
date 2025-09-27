using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderToInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateInputField()
    {
        inputField.text = slider.value * 360 + "";
    }
}
