using UnityEngine;

public class DualJoystickPlayerController : MonoBehaviour
{
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
    public RightJoystick rightJoystick; // the game object containing the RightJoystick script
    public float moveSpeed = 6.0f; // movement speed of the player character
    public int rotationSpeed = 8; // rotation speed of the player character
    public Transform rotationTarget; // the game object that will rotate to face the input direction
    public Animator anim; // the animator controller of the player character
    private Vector3 leftJoystickInput; // holds the input of the Left Joystick
    private Vector3 rightJoystickInput; // hold the input of the Right Joystick
    private Rigidbody rigidBody; // rigid body component of the player character

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    void Start()
    {
        if (transform.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("A RigidBody component is required on this game object.");
        }
        else
        {
            rigidBody = transform.GetComponent<Rigidbody>();
        }

        if (leftJoystick == null)
        {
            Debug.LogError("The left joystick is not attached.");
        }

        if (rightJoystick == null)
        {
            Debug.LogError("The right joystick is not attached.");
        }

        if (rotationTarget == null)
        {
            Debug.LogError("The target rotation game object is not attached.");
        }
    }



    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        // get input from both joysticks
        leftJoystickInput = leftJoystick.GetInputDirection();
        rightJoystickInput = rightJoystick.GetInputDirection();

        float xMovementLeftJoystick = leftJoystickInput.x; // The horizontal movement from joystick 01
        float zMovementLeftJoystick = leftJoystickInput.y; // The vertical movement from joystick 01	

        float xMovementRightJoystick = rightJoystickInput.x; // The horizontal movement from joystick 02
        float zMovementRightJoystick = rightJoystickInput.y; // The vertical movement from joystick 02

        // if there is no input on the left joystick
        if (leftJoystickInput == Vector3.zero)
        {
            anim.SetBool("IsWalking", false);
        }
        // if there is no input on the right joystick
        if (rightJoystickInput == Vector3.zero)
        {
            //anim.SetBool("isAttacking", false);
        }

        // if there is  input from the left joystick
        if (leftJoystickInput != Vector3.zero)
        {
            // calculate the player's direction based on angle
            float tempAngle = Mathf.Atan2(zMovementLeftJoystick, xMovementLeftJoystick);

            if (anim != null)
            {
                anim.SetBool("IsWalking", true);
            }

            // move the player
            //rigidBody.transform.Translate(leftJoystickInput * Time.fixedDeltaTime);


            leftJoystickInput.Set(xMovementLeftJoystick, 0f, zMovementLeftJoystick);

            //because of how vectors work, moving in both directions (diagonally)
            //will result in a value of 1.4 therefore a faster speed than usual (1)
            //we should normalize this:
            leftJoystickInput = leftJoystickInput.normalized * moveSpeed * Time.deltaTime; //move per second not per frame

            rigidBody.MovePosition(transform.position + leftJoystickInput);


        }

        // if there is  input from the right joystick
        if (rightJoystickInput != Vector3.zero)
        {
            // calculate the player's direction based on angle
            float tempAngle = Mathf.Atan2(zMovementRightJoystick, xMovementRightJoystick);
            xMovementRightJoystick *= Mathf.Abs(Mathf.Cos(tempAngle));
            zMovementRightJoystick *= Mathf.Abs(Mathf.Sin(tempAngle));

            // rotate the player to face the direction of input 
            Vector3 temp = transform.position;
            temp.x += xMovementRightJoystick;
            temp.z += zMovementRightJoystick;
            Vector3 lookDirection = temp - transform.position;

            Vector3 dir = temp - transform.position;
            dir.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rigidBody.MoveRotation(newRotation);
        }

    }


}