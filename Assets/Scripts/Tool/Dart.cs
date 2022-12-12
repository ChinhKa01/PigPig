using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] private float speedRotate;
    //[SerializeField] private int _damage;
    public GameObject damagePopups;

    // Update is called once per frame
    void Update()
    {
        setActive();
        transform.Rotate(new Vector3(0, 0, speedRotate * Time.deltaTime));
    }

    public void setActive()
    {
        if (transform.position.x < SystemVariable.playerController.minCam.x - 1 || transform.position.x > SystemVariable.playerController.maxCam.x + 1
           || transform.position.y < SystemVariable.playerController.minCam.y - 1 || transform.position.y > SystemVariable.playerController.maxCam.y + 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FindObjectOfType<PlayerController>().RandomDamage();
            int dame = FindObjectOfType<PlayerController>()._damage;
            Instantiate(damagePopups, collision.gameObject.transform.position, Quaternion.identity);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>().TakeDame(dame);
            gameObject.SetActive(false);
            Instantiate(SystemVariable.gameController.EffectOfPlayer[0], transform.position, Quaternion.identity);
        }
    }
}
