using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 5f;

	private Rigidbody2D myBody;
	private Animator anim;

	public Transform groundCheckPosition;
	public LayerMask groundLayer;

	private bool isGrounded;
	private bool jumped;

	private float jumpPower = 12f;

    private int jumpCount;
    public int maxJumps = 2;
    public float doubleJumpPower = 14f;   

    void Awake() {
		myBody=GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
	}



	void Update () {
        
        CheckIfGrounded();
        PlayerJump();
    }

	void FixedUpdate() {
		PlayerWalk ();
	}

	void PlayerWalk() {

		float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");


        if (h > 0) {
			myBody.linearVelocity = new Vector2 (speed, myBody.linearVelocity.y);

			ChangeDirection (1);

		} else if (h < 0) {
			myBody.linearVelocity = new Vector2 (-speed, myBody.linearVelocity.y);

			ChangeDirection (-1);

		} else {
			myBody.linearVelocity = new Vector2 (0f, myBody.linearVelocity.y);
		}

		anim.SetInteger ("Speed", Mathf.Abs((int)myBody.linearVelocity.x));

	}

	void ChangeDirection(int direction) {
		Vector3 tempScale = transform.localScale;
		tempScale.x = direction;
		transform.localScale = tempScale;
	}

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.Raycast(groundCheckPosition.position, Vector2.down, 0.3f, groundLayer);

        if (isGrounded)
        {

            if (myBody.linearVelocity.y <= 0.1f)
            {
                jumpCount = 0;

                if (jumped)
                {
                    jumped = false;
                    anim.SetBool("Jump", false);
                }
            }
        }
        else if (jumpCount == 0)
        {

            jumpCount = 1;
        }
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumpCount = 1;
                jumped = true;
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, jumpPower);
                anim.SetBool("Jump", true);
            }
            else if (jumpCount < maxJumps)
            {
                jumpCount++;
                jumped = true;
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, doubleJumpPower);
                anim.SetBool("Jump", true);
            }
        }
    }

} // class

