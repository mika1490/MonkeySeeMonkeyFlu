using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    private Camera camera;
    private Transform camTran;

	public float speed = 5.0F;
    public float jumpSpeed = 10.0f;

    public bool grounded;
    public LayerMask groundLayerMask;

    public Animator anim;
    private int groundedParamHash;

	// Use this for initialization
	void Start ()
	{
        camera = Camera.main;
        camTran = camera.transform;

        groundedParamHash = Animator.StringToHash("grounded");
	}

    private void Update()
    {
        grounded = Physics.CheckSphere(transform.position, 0.1f, groundLayerMask);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            Vector3 v = rb.velocity;
            v.y += jumpSpeed;
            rb.velocity = v;

        }
        anim.SetBool(groundedParamHash, grounded);
    }
    // Update is called once per physics frame
    void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

        // figure out the camera's forward direction on the flat floor plane
        Vector3 camForward = camTran.forward;
        camForward.y = 0.0f;
        camForward.Normalize();

        Quaternion rot = Quaternion.LookRotation(camForward);

        // figure out the velocity based on the inputs, relative to the camera direction
		Vector3 moveDirection = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        moveDirection.Normalize();
        Vector3 targetVelocity = rot * moveDirection * speed;

        
        targetVelocity.y = rb.velocity.y;
        if (targetVelocity.y < 0.0f)
        {
            targetVelocity.y += Time.deltaTime * Physics.gravity.y * 5.0f;
        }
        else if (targetVelocity.y > 0.0f)
        {
            if (Input.GetButton("Jump"))
            {
                targetVelocity.y += Time.deltaTime * Physics.gravity.y * 1f;

            }
            else
            {
                targetVelocity.y += Time.deltaTime * Physics.gravity.y * 3.0f;

            }
        }

        rb.velocity = targetVelocity;

        // rotate the monkey to face where it's going
        if (Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10.0f);
        }


	}
}
