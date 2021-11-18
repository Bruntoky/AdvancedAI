using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CharacterMovement : MonoBehaviour
{

    // Start is called before the first frame update
    public float speed = 1f;// grounded movement speed
    public float jumpSpeed = 8.0f;//movement speed while in the air
    public float gravity = 20.0f;//speed at which you fall
    public Transform playerCameraParent;//camera linked to player
    public float lookSpeed = 2.0f;//speed the camera rotates
    public float lookXLimit = 60.0f;//maximum angle you can look to your sides
    private bool Jumping = false;//jumping animation bool
    private bool Sprinting = false;//sprinting animation bool
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;//direction your moving
    Vector2 rotation = Vector2.zero;//direction your looking

    public Animator anim;//reference to the animator
    private float curSpeedX ;
    private float curSpeedY ;
    private bool Attack;//attack aniamtion bool
    public BoxCollider Sword;//reference to swords collider
    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;//ensures facing angle is correct
        Sword.enabled = false;//turns off the swords collider
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Dash"))//if you press shift switch to sprinting
        {
            Sprinting = true;
        }
        if(Input.GetButtonUp("Dash"))//when you let go of sprint stop sprinting
        {
            Sprinting = false;
        }
        if(Input.GetMouseButtonDown(0))//when you left click start the attack animationa and turn on the collider
        {
            Attack = true;
            Sword.enabled = true;
            StartCoroutine (attackDelay());
            anim.SetBool("attack", Attack);

        }
       
      
        //sets animation values
        anim.SetFloat("vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));
        anim.SetBool("sprinting", Sprinting);
        if (characterController.isGrounded)
        {
            Jumping = false;
            anim.SetBool("jumping", Jumping);
            // If we are grounded, so recalculate move direction based on axes
            //set move speed
            if (Sprinting)
            {
                curSpeedX = canMove ? (speed * 2) * Input.GetAxis("Vertical") : 0;
                curSpeedY = canMove ? (speed * 2) * Input.GetAxis("Horizontal") : 0;
            }
            else
            {
                curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
                curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            }
            
            //sets movement direction
            moveDirection = (transform.forward * curSpeedX) + (transform.right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && (Sprinting == false))
            {
                Jumping = true;
                anim.SetBool("jumping", Jumping);
                moveDirection.y = jumpSpeed;//makes the character move up
                
                
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        // Camera rotation editing movement
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector3(0, rotation.y, 0);
        }
    }


    private IEnumerator attackDelay()//turns off the sword collider after the animation runs
    {
        
        yield return new WaitForSeconds(1.5f);
        Sword.enabled = false;
        Attack = false;//stop the attack animation
        anim.SetBool("attack", Attack);
    }
}