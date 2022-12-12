using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public Text NPC, sentence;
    private Queue<string> sentences;
    public Animator animator;
    public GameObject buttonClose, buttonNext;

    void Start()
    {
        buttonClose.SetActive(false);
        buttonNext.SetActive(true);
        animator.SetBool(DialogAnim.Open.ToString(), true);
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog dialog)
    {
        sentences.Clear();
        NPC.text = dialog.name;

        foreach (string sentence in dialog.sentence)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 1)
        {
            buttonNext.SetActive(false);
            buttonClose.SetActive(true);
        }

        sentence.text = sentences.Dequeue();
        StartCoroutine(DisplayMode(sentence.text));
    }

    public void EndConversation()
    {
        animator.SetBool(DialogAnim.Open.ToString(), false);

        FindObjectOfType<PlayerController>().enabled = true;

        SystemVariable.gameController._state = stateOfGame.Play.ToString();
        buttonClose.SetActive(false);
    }

    IEnumerator DisplayMode(string sentenseCurrent)
    {
        sentence.text = null;
        foreach (char c in sentenseCurrent.ToCharArray())
        {
            sentence.text += c;
            yield return null;
        }
    }
}
