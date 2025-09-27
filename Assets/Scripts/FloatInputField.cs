using System.Text;
using TMPro;
using UnityEngine;

public class FloatInputField : MonoBehaviour
{
    private TMP_InputField input;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    public void ValidateContent()
    {
        input.text = SanitizeForFloatParse(input.text);
        RecordCreator.main.beacon.transform.position = new(float.Parse(RecordCreator.main.xField.text) * 10, float.Parse(RecordCreator.main.yField.text) * 10, -30);
    }

    public static string SanitizeForFloatParse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "0"; // Default to zero if input is empty or null

        // Remove all characters except digits, decimal point, negative sign, and exponent
        StringBuilder sb = new StringBuilder();
        bool decimalPointFound = false;
        bool exponentFound = false;

        input = input.Trim();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else if (c == '.' && !decimalPointFound)
            {
                sb.Append(c);
                decimalPointFound = true;
            }
            else if ((c == 'e' || c == 'E') && !exponentFound)
            {
                sb.Append(c);
                exponentFound = true;
                decimalPointFound = false; // Reset decimal for exponent part
            }
            else if ((c == '-' || c == '+') && (i == 0 || input[i - 1] == 'e' || input[i - 1] == 'E'))
            {
                sb.Append(c); // Allow negative/positive signs at start or after exponent
            }
            // All other characters are ignored
        }

        string sanitized = sb.ToString();

        // Ensure something valid remains
        if (string.IsNullOrEmpty(sanitized) || sanitized == "." || sanitized == "-" || sanitized == "+" || sanitized == "e" || sanitized == "E" || sanitized.EndsWith("e"))
        {
            return "0";
        }

        return sanitized;
    }
}
