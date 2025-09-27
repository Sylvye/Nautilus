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
        Color dim = Color.HSVToRGB(hue, 1, 0.8f);
        Color full = Color.HSVToRGB(hue, 1, 1);

        background.color = dim;
        fill.color = dim;
        handle.color = full;
        RecordCreator.main.beacon.GetComponent<SpriteRenderer>().color = full;
    }
}
