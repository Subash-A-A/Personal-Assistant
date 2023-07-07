using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class RequestTest : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;

    private Assistant assistant;
    private Coroutine postCoroutine = null;

    [System.Serializable]
    public class Data
    {
        public string message;
        public int speakerID;
    }
    private void Awake()
    {
        assistant = FindObjectOfType<Assistant>();
    }

    private void Update()
    {

    }

    public void Request()
    {
        if(input.text == null || input.text == "")
        {
            Debug.Log("Please Enter some Prompt");
            return;
        }

        Debug.Log("Submit!");
        postCoroutine = StartCoroutine(PostAPIRequest());
    }

    IEnumerator PostAPIRequest()
    {
        // Create a Data object and populate it with the desired values
        Data data = new Data();
        data.message = input.text;
        data.speakerID = assistant.GetSpeakerID();

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
            string savePath = Path.Combine(Application.streamingAssetsPath, "voice.wav");
            File.WriteAllBytes(savePath, request.downloadHandler.data);
            Debug.Log("Voice file downloaded and saved at " + savePath);

            StartCoroutine(assistant.GetAudioFile());
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}
