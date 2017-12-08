using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f; //length of ray cast from camera


    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal"); //raw axis values are either -1, 0, 1.
        float v = Input.GetAxisRaw("Vertical"); //no values inbetween so char will snap to max speed

        //Move(h, v);
        //Turning();
        //Animating(h, v);
    }


    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        //because of how vectors work, moving in both directions (diagonally)
        //will result in a value of 1.4 therefore a faster speed than usual (1)
        //we should normalize this:
        movement = movement.normalized * speed * Time.deltaTime; //move per second not per frame

        playerRigidbody.MovePosition(transform.position + movement);
    }



    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast (camRay, out floorHit,　camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;

        anim.SetBool("IsWalking", walking);
    }


}
