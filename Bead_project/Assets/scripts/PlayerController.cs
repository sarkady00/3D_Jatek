using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    public float gravityScale;

    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;
    public GameObject playerModel;

    private Vector3 moveDirection;
        
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        float yStore = moveDirection.y; // elmentjük az y irányú komponenst még a normalizálás előtt
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore; // visszaadjuk neki azt az értéket
        if (controller.isGrounded) // ha a öldön vagyunk akkor tudunk ugrani
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime); // visszaessen a földre

        controller.Move(moveDirection * Time.deltaTime); // végre hajtuk a mozgást

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) // ha nem arra nézünk mint amerre a karakter
        {
            transform.rotation = Quaternion.Euler(0, pivot.rotation.eulerAngles.y, 0); // ha valamerre nézünk, és elindulunk akkor a karakter irányba áll és megindul
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }
}
