using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VoiceRecorder : MonoBehaviour
{
    [SerializeField] private KeyCode recordKey = KeyCode.Space;
    [SerializeField] private Image innerCircle;

    private RequestTest requestTest;
    private Coroutine record = null;
    private bool isRecording = false;

    private void Awake()
    {
        requestTest = GetComponent<RequestTest>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(recordKey))
        {
            if(record != null)
            {
                StopCoroutine(record);
            }

            record = StartCoroutine(MakeGetRequest());
        }

        innerCircle.color = (isRecording) ? Color.red : Color.white;
    }

    IEnumerator MakeGetRequest()
    {
        string url = "http://localhost:5000/record"; 

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Request succeeded, you can process the response here
            Debug.Log("Response: " + request.downloadHandler.text);
            isRecording = !isRecording;

            if (!isRecording)
            {
                requestTest.Request();
            }
        }
        else
        {
            // Request failed, you can handle the error here
            Debug.LogError("Error: " + request.error);
        }
    }
}
