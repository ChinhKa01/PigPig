using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject[] listBtn;
    public GameObject Claim;

    private void Start()
    {
        Claim.SetActive(false);
    }

    public void State(int id)
    {
        StartCoroutine(SetState(id));
    }

    IEnumerator SetState(int id)
    {
        foreach(GameObject btn in listBtn)
        {
            if(btn.GetComponent<CardDisplay>().card.id != id)
            {
                btn.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(2f);
        Claim.SetActive(true);
        Claim.GetComponent<Button>().onClick.AddListener(() =>
        {
            Claim.SetActive(false);
            gameObject.SetActive(false);
        });
    }
}
