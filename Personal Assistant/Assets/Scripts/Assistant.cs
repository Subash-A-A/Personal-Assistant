using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Assistant : MonoBehaviour
{
    [SerializeField] private int speakerID = 20;
    [SerializeField] private string expression = "Neutral";
    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
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

            // Expression
            SetExpression();
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

    public void StartThinking()
    {
        anim.SetBool("isThinking", true);
    }

    public void StopThinking()
    {
        anim.SetBool("isThinking", false);
    }

    public void SetExpressionString(string exp)
    {
        expression = exp;
    }

    public void SetExpression()
    {
        ResetExpression();
        
        if(expression == "Neutral")
        {
            return;
        }

        anim.SetLayerWeight(anim.GetLayerIndex("Eye Blinking"), 0f); // Stop Blinking while showing facial expressions
        anim.SetBool(expression, true);
    }

    public void ResetExpression()
    {
        anim.SetBool("Angry", false);
        anim.SetBool("Joy", false);
        anim.SetBool("Sorrow", false);
        anim.SetBool("Surprised", false);

        anim.SetLayerWeight(anim.GetLayerIndex("Eye Blinking"), 1f); // Blink
    }
}
