using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpd = 5f;
    public float dodgeForce = 40f;
    public bool facingRight = true;
    public GameObject dashPS;
    public Animator anim;

    private Vector3 playerInput;
    private Rigidbody2D rb;
    private float slidingSpd;
    private Vector3 slidingDir;
    private Vector3 lastDir;
    private float stamina = 100f;
    private float regenStamina = 1f;

    public STATE state = STATE.normal;
    public enum STATE
    {
        normal,
        dodging,
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        switch (state)
        {
            case STATE.normal:
                if (h != 0 || v != 0)
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Run", true);
                    lastDir = new Vector3(h, v);
                }
                else
                {
                    anim.SetBool("Run", false);
                    anim.SetBool("Idle", true);
                }
                playerInput = new Vector3(h,v);
                rb.velocity = playerInput * moveSpd;
                CheckForDodgeRoll();
                slidingDir = lastDir;
                break;
            case STATE.dodging:
                HandleDodgeRoll();
                break;
        }

        if (h > 0 && !facingRight) Flip();
        if (h < 0 && facingRight) Flip();

        if (stamina >= 100)
        {
            stamina = 100;
        }
        else
        {
            stamina += regenStamina;
        }
    }

    public void CheckForDodgeRoll()
    {
        if (Input.GetKey(KeyCode.Space) && stamina > 85 && slidingDir != Vector3.zero)
        {
            GameObject dash = Instantiate(dashPS,transform.position,Quaternion.identity);
            Destroy(dash, 2f);
            anim.SetBool("Roll", true);
            stamina -= 50;
            state = STATE.dodging;
            slidingSpd = 100f;
        }
    }

    public void HandleDodgeRoll()
    {
        rb.velocity = slidingDir * slidingSpd;
        slidingSpd -= slidingSpd * 100 * Time.deltaTime;
        if (slidingSpd < 0f)
        {
            anim.SetBool("Roll", false);
            state = STATE.normal;
        }
    }

    public Vector3 SetUpSlidingDir(float h,float v)
    {
        Vector3 dir;
        if (h == 0 && v == 0)//No Movement
        {
            dir = new Vector3(0, 0, 0);
        }
        else if (h > 0 && v == 0)//Right
        {
            dir = new Vector3(1, 0, 0);
        }
        else if (h < 0 && v == 0)//Left
        {
            dir = new Vector3(-1, 0, 0);
        }
        else if (h == 0 && v > 0)//Up
        {
            dir = new Vector3(0, 1, 0);
        }
        else if (h == 0 && v < 0)//Down
        {
            dir = new Vector3(0, -1, 0);
        }
        else if (h > 0 && v > 0)//Up Right
        {
            dir = new Vector3(1, 1, 0);
        }
        else if (h < 0 && v > 0)//Up Left
        {
            dir = new Vector3(-1, 1, 0);
        }
        else if (h > 0 && v < 0)//Down Right
        {
            dir = new Vector3(1, -1, 0);
        }
        else if (h < 0 && v < 0)//Down Left
        {
            dir = new Vector3(-1, -1, 0);
        }
        else
        {
            dir = new Vector3(0, 0, 0);
        }
        return dir;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
