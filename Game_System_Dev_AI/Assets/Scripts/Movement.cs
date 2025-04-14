using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    //Health stuff
    public int maxHealth = 100;
    public int currentHealth;

    //public int enemyCount = 6;

    //public HealthBar healthBar;

    //Movement stuff

    public float moveSpeed = 20;
    public float rotationSpeed = 4;
    float runningSpeed;
    public float vaxis, haxis;
    public bool isJumping, isJumpingAlt, isGrounded = false;
    Vector3 movement;

    //Clamping stuff
    private float xMin = -0.5f, xMax = 5f;
    private float timeValue = 0.0f;

    void Start()
    {
        Debug.Log("Initialized: (" + this.name + ")");

        currentHealth = maxHealth;
        //healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        // Compute the sin position.
        float xValue = Mathf.Sin(timeValue * 5.0f);

        // Now compute the Clamp value.
        float xPos = Mathf.Clamp(xValue, xMin, xMax);
    }

    void FixedUpdate()
    {
        /*  Controller Mappings */
        vaxis = Input.GetAxis("Vertical");
        haxis = Input.GetAxis("Horizontal");
        isJumping = Input.GetButton("Jump");
        isJumpingAlt = Input.GetKey(KeyCode.Joystick1Button0);

        //Simplified...
        runningSpeed = vaxis;


        if (isGrounded)
        {
            movement = new Vector3(0, 0f, runningSpeed * 8);        // Multiplier of 8 seems to work well with Rigidbody Mass of 1.
            movement = transform.TransformDirection(movement);      // transform correction A.K.A. "Move the way we are facing"
        }
        else
        {
            movement *= 0.70f;                                      // Dampen the movement vector while mid-air
        }

        GetComponent<Rigidbody>().AddForce(movement * moveSpeed);   // Movement Force


        if ((isJumping || isJumpingAlt) && isGrounded)
        {
            Debug.Log(this.ToString() + " isJumping = " + isJumping);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 150);
        }



        if ((Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && !isJumping && isGrounded)
        {
            if (Input.GetAxis("Vertical") >= 0)
                transform.Rotate(new Vector3(0, haxis * rotationSpeed, 0));
            else
                transform.Rotate(new Vector3(0, -haxis * rotationSpeed, 0));

        }

        //If you run out of health, go to the game over screen.
        if (currentHealth <= 0)
        {
            GameOver();
        }



        //if (enemyCount <= 0) 
        //{
            //victory();
        //}
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        //Colliding with enemies deals damage
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);

            TakeDamage(100);

            //enemyCount = enemyCount - 1;
        }

        //Colliding with the victory point sends you to the victory screen.
        if (collision.gameObject.CompareTag("VictoryPoint"))
        {
            Debug.Log("Contacted VictoryPoint");
            victory();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        //If this is only in OnCollisionEnter it can't figure out ramps.
        //Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    //Lets the player jump
    void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //healthBar.SetHealth(currentHealth);
    }

    //Sends you to either the game over or victory screens
    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void victory()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}