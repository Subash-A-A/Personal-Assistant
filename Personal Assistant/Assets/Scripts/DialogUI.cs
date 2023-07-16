using TMPro;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogText;

    public string FormatDialog(string prompt, string reply)
    {
        string dialog = "You: " + prompt + "\n" + "Yuki: " + reply;
        return dialog;
    }

    public void SetDialog(string dialog, bool isError = false)
    {
        dialogText.color = isError ? Color.red : Color.white;
        dialogText.text = dialog;
    }
}
