using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject ironBox;

    private Animator anim;
    private bool hasPlayedAnimation = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayedAnimation)
        {
            ironBox.SetActive(false);
            hasPlayedAnimation = true;
            anim.SetTrigger("Move");
        }
    }
}
