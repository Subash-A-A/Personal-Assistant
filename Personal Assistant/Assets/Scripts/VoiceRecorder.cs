using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class VoiceRecorder : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private GameObject recordingUI;

    private RequestTest requestTest;
    private Coroutine record = null;
    private bool isRecording = false;
    private DialogUI dialog;

    private void Awake()
    {
        requestTest = GetComponent<RequestTest>();
        dialog = GetComponent<DialogUI>();
    }

    private void Start()
    {
        recordingUI.SetActive(isRecording);
    }

    private void Update()
    {
        if ((Input.GetKeyDown(key) && !isRecording) || (Input.GetKeyUp(key) && isRecording))
        {
            if(record != null)
            {
                StopCoroutine(record);
            }

            record = StartCoroutine(MakeGetRequest());
        }

        if (isRecording)
        {
            dialog.SetDialog("Yuki is Listening!");
        }
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
            recordingUI.SetActive(isRecording);

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
