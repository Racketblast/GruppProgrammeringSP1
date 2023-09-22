using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpForce = 600f;
    [SerializeField] private AudioClip trampolineSound;

    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource = GetComponent<AudioSource>();
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            audioSource.PlayOneShot(trampolineSound, 0.5f);
            GetComponent<Animator>().SetTrigger("Jump");
        }
    }
}
