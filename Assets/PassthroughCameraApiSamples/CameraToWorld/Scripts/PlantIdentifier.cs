using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using TMPro;

public class PlantIdentifier : MonoBehaviour
{
    private const string APIKEY = "SJWuP5nJYnQA3R08YgB3EiPmFOWkKyQLi0APhBJAG2yTRu57oV";
    private const string API_URL = "https://api.plant.id/v3/identification?details=url,common_names";

    [SerializeField] private TextMeshProUGUI m_plantIDText;
    public Texture2D plantImageTexture { get; set; }

    public void test()
    {
        Debug.Log("test test");
    }
    public void CallIdentifyPlant()
    {
        if (plantImageTexture != null)
        {
            StartCoroutine(IdentifyPlant(plantImageTexture));
        }
        else
        {
            m_plantIDText.text = "No image found!";
        }
    }

    private IEnumerator IdentifyPlant(Texture2D plantTexture)
    {
        string base64Image = System.Convert.ToBase64String(plantTexture.EncodeToJPG());

        // Build JSON payload

        string jsonPayload = $@"{{ ""images"": [""{base64Image}""] }}";

        UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Api-Key", APIKEY);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Plant.id Response: " + responseText);
            string formatted = FormatResponse(responseText);
            StartCoroutine(TypeText(responseText));
        }
        else
        {
            Debug.LogError("API Error: " + request.error);
            m_plantIDText.text = $"Error: {request.error}";
        }
    }

    private string FormatResponse(string json)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("🌿 <b>Top Plant Matches</b>\n");

        int index = 0;
        while ((index = json.IndexOf("\"name\":\"", index)) != -1)
        {
            index += 8;
            string name = ExtractBetween(json, "\"name\":\"", "\"", ref index);
            string probabilityStr = ExtractBetween(json, "\"probability\":", ",", ref index);
            float probability = 0;
            float.TryParse(probabilityStr, out probability);

            string commonNames = ExtractBetween(json, "\"common_names\":[", "]", ref index).Replace("\"", "");
            string url = ExtractBetween(json, "\"url\":\"", "\"", ref index);

            sb.AppendLine($"• <b>{name}</b> ({Mathf.RoundToInt(probability * 100)}%)");

            if (!string.IsNullOrEmpty(commonNames))
                sb.AppendLine($"   → Also known as: <i>{commonNames}</i>");

            if (!string.IsNullOrEmpty(url))
                sb.AppendLine($"   🔗 <u>{url}</u>");

            sb.AppendLine(); // spacing
        }

        return sb.Length > 0 ? sb.ToString() : "No plant suggestions found.";
    }

    private string ExtractBetween(string text, string start, string end, ref int index)
    {
        int i1 = text.IndexOf(start, index);
        if (i1 == -1) return "";
        int i2 = text.IndexOf(end, i1 + start.Length);
        if (i2 == -1) return "";
        index = i2 + end.Length;
        return text.Substring(i1 + start.Length, i2 - i1 - start.Length);
    }

    private IEnumerator TypeText(string fullText, float delay = 0.02f)
    {
        m_plantIDText.text = "";

        foreach (char c in fullText)
        {
            m_plantIDText.text += c;

            // If the character is a comma, add a newline
            if (c == ',')
            {
                m_plantIDText.text += "\n";
            }

            yield return new WaitForSeconds(delay);
        }
    }



}
