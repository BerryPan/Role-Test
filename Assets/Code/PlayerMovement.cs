using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Vector3 TotalRotate;
    Animator anim;
    Rigidbody playerRigidBody;
    public HealthDemo HD;
    bool shift;
    float enablemove = 0.2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        shift = (Input.GetKey(KeyCode.LeftShift)|Input.GetKey(KeyCode.RightShift));
        if (enablemove>0)
        {
            enablemove-=Time.deltaTime;
            Move(h, v);
            Animating(h, v);
        }
        Turing();
    }

    private void Move(float h,float v)
    {
        shift = (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift));
        movement.Set(h, 0, v);
        //Quaternion newRot = Quaternion.Euler(TotalRotate);
        //movement=newRot*movement;
        movement = playerRigidBody.transform.TransformVector(movement);
        if (shift && HD.currentPower > 0)
            movement = movement.normalized * speed * Time.deltaTime * 2.0f;
            //movement = movement.normalized * 2.0f * speed;
        else
            movement = movement.normalized * speed * Time.deltaTime;
            //movement = movement.normalized * speed;
        playerRigidBody.MovePosition(transform.position + movement);
        //playerRigidBody.AddForce(movement);
    }

    void Animating(float h,float v)
    {
        bool walking = (h!=0)|(v!=0);
        anim.SetBool("IsWalking",walking);
        if (!walking)
        {
            if (HD.currentPower < HD.maxSP)
                HD.currentPower += Time.deltaTime * 2;
            return;
        }

        if (shift)
        {
            if (HD.currentPower > 0)
            {
                HD.currentPower -= Time.deltaTime * 2;
                anim.SetFloat("Speed", 2.0f);
            }
            else
            {
                anim.SetFloat("Speed", 1.0f);
            }
        }
        else
        {
            anim.SetFloat("Speed", 1.0f);
            if (HD.currentPower < 10f)
                HD.currentPower += Time.deltaTime;
        }
    }

    void Turing()
    {
        TotalRotate+=new Vector3(0,Input.GetAxis("Mouse X"),0);
        Quaternion newRot = Quaternion.Euler(TotalRotate);
        playerRigidBody.MoveRotation(newRot);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 0) enablemove = 0.5f;
    }
}
