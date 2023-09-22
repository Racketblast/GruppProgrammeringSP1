using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject textPopUp;
    [SerializeField] private AudioClip questGiverVoice;

    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource = GetComponent<AudioSource>();
        if (textPopUp != null && other.CompareTag("Player"))
        {
            textPopUp.SetActive(true);
            audioSource.PlayOneShot(questGiverVoice, 0.5f);
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
