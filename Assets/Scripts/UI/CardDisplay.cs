using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Animator animator;
    public Button btn;
    public Text Coint;
    private void Start()
    {
        Coint.text = card.cointAmount.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        btn.onClick.AddListener(() =>
        {
            btn.enabled = false;
            FindObjectOfType<ButtonController>().State(card.id);
            animator.SetTrigger(Cards.Open.ToString());
        });
    }
}
