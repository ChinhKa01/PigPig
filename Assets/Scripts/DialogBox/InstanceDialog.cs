using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceDialog : MonoBehaviour
{
    public Dialog dialog;

    private void Start()
    {
        Invoke("StartConversation", 0.5f);
    }

    private void StartConversation()
    {
        FindObjectOfType<DialogController>().StartDialog(dialog);
    }
}
