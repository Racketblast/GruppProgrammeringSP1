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

        // Spelarens ingångshastighet
        Vector2 originalVelocity = GetComponent<Rigidbody2D>().velocity;

        // Standard dash åt höger
        Vector2 dashDirection = transform.right;

        // Ändrad dash åt vänster
        if (GetComponent<PlayerMovement>().playerDirection == 2)
        {
            dashDirection = -dashDirection;
        }

        // Totalkraft efter dash
        GetComponent<Rigidbody2D>().AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        //ljud
        audioSource.PlayOneShot(dashSound, 0.5f);

        // Förhindrar annan rörelse under dashen (dashen funkar inte utan detta??)
        GetComponent<PlayerMovement>().enabled = false;

        // Dash längden (ändras i Unity under Player i Script)
        yield return new WaitForSeconds(dashDuration);

        // Återställ 
        GetComponent<Rigidbody2D>().velocity = originalVelocity;
        GetComponent<PlayerMovement>().enabled = true;

        // Gör så att spelaren får en cooldown innan den kan använda dashen igen
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
}
