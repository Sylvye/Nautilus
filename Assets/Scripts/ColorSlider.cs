using UnityEngine;
using UnityEngine.UI;

public class ColorSlider : MonoBehaviour
{
    public Slider slider;
    public Image background;
    public Image fill;
    public Image handle;

    public void UpdateColor()
    {
        float hue = slider.value;

        background.color = Color.HSVToRGB(hue, 1, 0.8f);
        fill.color = Color.HSVToRGB(hue, 1, 0.8f);
        handle.color = Color.HSVToRGB(hue, 1, 1);
    }
}
