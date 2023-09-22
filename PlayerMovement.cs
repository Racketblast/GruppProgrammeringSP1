using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 250f;
    [SerializeField] private float jumpForce = 300f; //Axel - �ndrade till 70 i Unity f�r att fungera med DJ och beh�lla samma jump h�jd
    [SerializeField] private float jumpForceDoubleJump = 5f; //Axel - separat jF f�r DJ, �ka eller minska f�r att justera andra hoppet
    [SerializeField] private float jumpHeight = 1f; //Axel - h�jden av andra hoppet, justeras i Unity p� Player
    [SerializeField] public int doubleJump; //Axel -
    [SerializeField] private float jumpCount = 1; //Axel -
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillCollor;
    [SerializeField] private Color greenHealth, yellowHealth, redHealth;
    [SerializeField] private TMP_Text appleText;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject appleParticles, dustParticles;

    private float horizontalValue;
    private float rayDistance = 0.25f;

    public bool isGrounded;
    private bool canMove = false;
    private bool isDead = false;

    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;
    private AudioSource audioSource;

    public int doubleJumpValue; //Axel -
    private int startingHealth = 5;
    private int currentHealth = 0;
    public int applesCollected = 0;
    public int playerDirection = 1; // riktningen 1 �r h�ger | 2 �r v�nster
    // Start is called before the first frame update
    void Start()
    {
        doubleJump = doubleJumpValue; //Axel -
        currentHealth = startingHealth;
        appleText.text = "" + applesCollected;
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Appear();
        Invoke("CanMoveAgain", 0.5f);//Appear animationen h�nder och d� vill jag att man ska st� stil
    }

    private void Appear()
    {
        anim.SetBool("Appearing", true);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }
        else if (horizontalValue > 0)
        {
            FlipSprite(false);
        }

        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true && isDead == false)
        {
            Jump();
        }



        //if (SceneManager.GetActiveScene().buildIndex > 2)
        //{
        //    doubleJump = doubleJumpValue +1;
        //}
        //else
        //{
        //    doubleJumpValue = 0;
        //}

        
        isGrounded = CheckIfGrounded();
        if (isGrounded == true)
        {
            doubleJump = doubleJumpValue;
        }
        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true/* || doubleJumpValue < 0*/)
        {
            Jump();
            rgbd.velocity = new Vector3(rgbd.velocity.x, 0, jumpForceDoubleJump); //Axel - justerar velocity under double jump samt vanligt hopp. Nollst�ller y axeln d� den annars applicerar jumpForceDoubleJump + jumpForce
            rgbd.AddForce(Vector3.up * jumpHeight, (ForceMode2D)ForceMode.Impulse); //Axel - ForceMode.Impulse applicerar forcen direkt
        }
        else if (Input.GetButtonDown("Jump") && doubleJump > 0)
        {
            Jump();
            doubleJump--;
            rgbd.velocity = new Vector3(rgbd.velocity.x, 0, jumpForceDoubleJump);
            rgbd.AddForce(Vector3.up * jumpHeight, (ForceMode2D)ForceMode.Impulse);
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.velocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.velocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());
        playerDirection = CheckIfFacingRight(playerDirection);  //Skickar in den tidigare riktningen till koden som kollar nya riktningen
    }



    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
            rgbd.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple")) 
        {
            Destroy(other.gameObject);
            applesCollected++;
            appleText.text = "" + applesCollected;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(pickupSound, 0.5f);
            Instantiate(appleParticles, other.transform.position, appleParticles.transform.localRotation);
        }
        if (other.CompareTag("Health"))
        {
            RestoreHealth(other.gameObject);
        }
    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }
    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));
        int randomValue = Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[randomValue], 0.5f);
        if (isGrounded == true)
        {
            Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
        }
        else
        {
            return;
        }

    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);
        //Debug.DrawLine(leftFoot.position, Vector2.down * rayDistance, Color.blue, 0.25f);
        //Debug.DrawLine(rightFoot.position, Vector2.down * rayDistance, Color.red, 0.25f);
        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage( int damageAmount)
    {
        anim.SetTrigger("IsHit");
        currentHealth -= damageAmount;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            canMove = false;// Nu kan man inte g�
            isDead = true;// Nu kan man inte hoppa n�r man �r d�d
            rgbd.Sleep();// Nu slutar gravitationen fungera
            rgbd.gravityScale = 0;// Nu stoppas man i luften
            anim.SetBool("Dissapearing", true);
            Invoke("Respawn",0.5f);// V�ntar en tid innan scenen reloadas
        }
    }

    public void TakeKnockback(float knockbackForce, float upwards)
    {
        if (currentHealth > 0)// Om man d�r av en enemy tar man ingen knockback
        {
            canMove = false;
            rgbd.AddForce(new Vector2(knockbackForce, upwards));
            Invoke("CanMoveAgain", 0.25f);
        }
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

    private void RestoreHealth(GameObject healthPickup)
    {
        if (currentHealth >= startingHealth)
        {
            return;
        }
        else
        {   
            int healthToRestore = healthPickup.GetComponent<HealthPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHealthBar();
            Destroy(healthPickup);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(pickupSound, 0.5f);

            if (currentHealth >= startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
        if (currentHealth > 3)
        {
            fillCollor.color = greenHealth;
            // 43CC40
        }
        else if (currentHealth > 1 && currentHealth <= 3)
        {
            fillCollor.color = yellowHealth;
            // F6F300
        }
        else
        {
            fillCollor.color = redHealth;
            // DB2424
        }
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Scenen reloadas n�r man d�r
    }

    private int CheckIfFacingRight(int lastDirection) // Kollar vilken riktning spelaren st�r i
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            return 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            return 2;
        }
        else
        {
            return lastDirection;
        }
    }
}
