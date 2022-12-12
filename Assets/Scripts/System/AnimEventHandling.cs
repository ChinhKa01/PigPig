using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHandling : MonoBehaviour
{
    public void Destroy() => Destroy(gameObject);

    public void SetActiveOff() {
        gameObject.SetActive(false);
    }

    public void SetActiveOn()
    {
        gameObject.SetActive(true);
    }

    public void DisableAnim() => GetComponent<Animator>().enabled = false;
}
