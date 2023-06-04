using UnityEngine;
using TMPro;

public class SignPostBehavior : MonoBehaviour
{
    public TMP_Text textObject;

    void Awake()
    {
        textObject.GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            textObject.GetComponent<Renderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            textObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
