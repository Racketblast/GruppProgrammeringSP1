using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashCooldown;
    [SerializeField] private AudioClip dashSound;

    private AudioSource audioSource;
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;

    void Update()
    {

        audioSource = GetComponent<AudioSource>();
        if ((Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) || (Input.GetKeyDown(KeyCode.RightShift) && !isDashing))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        // Spelarens ing�ngshastighet
        Vector2 originalVelocity = GetComponent<Rigidbody2D>().velocity;

        // Standard dash �t h�ger
        Vector2 dashDirection = transform.right;

        // �ndrad dash �t v�nster
        if (GetComponent<PlayerMovement>().playerDirection == 2)
        {
            dashDirection = -dashDirection;
        }

        // Totalkraft efter dash
        GetComponent<Rigidbody2D>().AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        //ljud
        audioSource.PlayOneShot(dashSound, 0.5f);

        // F�rhindrar annan r�relse under dashen (dashen funkar inte utan detta??)
        GetComponent<PlayerMovement>().enabled = false;

        // Dash l�ngden (�ndras i Unity under Player i Script)
        yield return new WaitForSeconds(dashDuration);

        // �terst�ll 
        GetComponent<Rigidbody2D>().velocity = originalVelocity;
        GetComponent<PlayerMovement>().enabled = true;

        // G�r s� att spelaren f�r en cooldown innan den kan anv�nda dashen igen
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
}
