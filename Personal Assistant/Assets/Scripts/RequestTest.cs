using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEditor.Rendering;

public class RequestTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private string lang = "en";

    private Assistant assistant;
    private Coroutine postCoroutine = null;
    private DialogUI dialog;

    [System.Serializable]
    public class Data
    {
        public string message;
        public int speakerID;
        public string srcLang;
        public string streamingAssetsPath;
    }
    private void Awake()
    {
        assistant = FindObjectOfType<Assistant>();
        dialog = GetComponent<DialogUI>();
    }

    public void Request()
    {
        /*
        if(input.text == null || input.text == "")
        {
            Debug.Log("Please Enter some Prompt");
            return;
        }
        */

        Debug.Log("Submit!");
        dialog.SetDialog("Yuki is Thinking...");
        postCoroutine = StartCoroutine(PostAPIRequest());
    }

    IEnumerator PostAPIRequest()
    {
        //Animation
        assistant.StartThinking();

        // Create a Data object and populate it with the desired values
        Data data = new Data();
        data.message = input.text;
        data.speakerID = assistant.GetSpeakerID();
        data.srcLang = lang;
        data.streamingAssetsPath = Application.streamingAssetsPath;

        // Convert the Data object to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Convert the JSON string to a byte array
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Create and configure the request
        UnityWebRequest request = new UnityWebRequest("http://localhost:5000/speak", "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Recieved Data!");
            
            assistant.StopThinking();
            string expression = request.GetResponseHeader("Expression");
            string prompt = request.GetResponseHeader("Prompt");
            string reply = request.GetResponseHeader("Reply");

            Debug.Log("Prompt: " + prompt);
            Debug.Log("Reply: " + reply);

            string dialogText = dialog.FormatDialog(prompt, reply);
            dialog.SetDialog(dialogText);

            assistant.SetExpressionString(expression);
            StartCoroutine(assistant.GetAudioFile());
        }
        else
        {
            Debug.Log(request.error);
            dialog.SetDialog(request.error, true);
        }
    }
}
