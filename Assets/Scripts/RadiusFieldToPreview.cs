using TMPro;
using UnityEngine;

public class RadiusFieldToPreview : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void UpdatePreviewRadius()
    {
        if (RecordCreator.main.previewRecord.TryGetComponent(out RadiusRecord rr))
        {
            rr.SetRadius(float.Parse(inputField.text));
        }
    }
}
