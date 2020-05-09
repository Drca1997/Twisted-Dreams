using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    public float gravity = -12;

    public float walkSpeed = 2;
    private float velocityY;

    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.05f;
    float speedSmoothVelocity;
    float currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //se ela estiver a interagir com algo ela não se mexe
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|interact")) input = Vector2.zero;

        Vector2 inputDirection = input.normalized;


        //roda a Sarah na direção correta
        if (inputDirection != Vector2.zero)
        {
            //não rodar instantaneamente na direção indicada
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            //transform.eulerAngles.y - rotação atual
            //turnSmoothTime - tempo que Mathf.SmoothDampAngle demora a ir da rotação atual para a target
            //turnSmoothVelocity - não alteramos, apenas damos à função como ref
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        }


        //Mover a Sarah

        //Não meter instantaneamente na velocidade máxima
        float targetSpeed = walkSpeed * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        //calcular a velocidade vertical
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up*velocityY;
        controller.Move(velocity * Time.deltaTime);

        //atualizar as velocidades de acordo com o estado do controller
        if (controller.isGrounded) velocityY = 0;
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;


        //atualizar animator
        if (inputDirection != Vector2.zero && currentSpeed!=0)
        {
            animator.SetBool("isWalking", true);
            animator.SetInteger("time_wasted", 0);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetInteger("time_wasted", animator.GetInteger("time_wasted") + 1);
        }


        if (Input.GetKeyDown(KeyCode.E)) //Interact
        {
            animator.ResetTrigger("Dab");
            animator.ResetTrigger("T");
            animator.SetTrigger("Interact");
            animator.SetInteger("time_wasted", 0);
        }
        //Temporario
        else if (Input.GetKeyDown(KeyCode.Z)) //Dab
        {
            animator.ResetTrigger("Interact");
            animator.ResetTrigger("T");
            animator.SetTrigger("Dab");
        }
        else if (Input.GetKeyDown(KeyCode.X)) //T-pose
        {
            animator.ResetTrigger("Interact");
            animator.ResetTrigger("Dab");
            animator.SetTrigger("T");
        }
    }
}
