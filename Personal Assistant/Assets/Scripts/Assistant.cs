using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Assistant : MonoBehaviour
{
    [SerializeField] private int speakerID = 20;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public IEnumerator GetAudioFile()
    {
        Debug.Log("Getting Audio File!");
        string audioFilePath = Path.Combine(Application.streamingAssetsPath, "voice.wav");

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + audioFilePath, AudioType.WAV);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
            audioClip.name = "voice";
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log(request.error);
        }
    }
    
    public int GetSpeakerID()
    {
        return speakerID;
    }
}
