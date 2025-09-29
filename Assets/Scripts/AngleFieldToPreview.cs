using TMPro;
using UnityEngine;

public class AngleFieldToPreview : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void UpdatePreviewAngle()
    {
        if (RecordCreator.main.previewRecord != null && RecordCreator.main.previewRecord.TryGetComponent(out DirectionRecord dr))
        {
            dr.SetAngle(float.Parse(inputField.text));
        }
    }
}
