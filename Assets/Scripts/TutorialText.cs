using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTutText : MonoBehaviour
{
    [SerializeField] private GameObject textPopUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (textPopUp != null && other.CompareTag("Player"))
        {
            textPopUp.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (textPopUp != null && other.CompareTag("Player"))
        {
            textPopUp.SetActive(false);
        }
    }
}
