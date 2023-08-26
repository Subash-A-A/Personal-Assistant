using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject chatPanel;
    [SerializeField] Transform content;
    [SerializeField]List<string> messages;

    [SerializeField] GameObject UserMessage;
    [SerializeField] GameObject AIMessage;

    private void Start()
    {
        HideChat();
    }

    public void ShowHideChat()
    {
        chatPanel.SetActive(!chatPanel.activeSelf);
    }

    public void ShowChat()
    {
        chatPanel.SetActive(true);
    }

    public void HideChat()
    {
        chatPanel.SetActive(false);
    }

    public void AppedUserMessage(string message)
    {
        messages.Add(message);
        GameObject text = Instantiate(UserMessage, content);
        text.GetComponent<TMP_Text>().text = "[You]\n" + message;
    }
    public void AppedAIMessage(string message)
    {
        messages.Add(message);
        GameObject text = Instantiate(AIMessage, content);
        text.GetComponent<TMP_Text>().text = "[AI]\n" + message;
    }
}
