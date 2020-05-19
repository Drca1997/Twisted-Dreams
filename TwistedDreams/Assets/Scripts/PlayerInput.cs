using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    public GameObject Canvas;
    private Vector3 movimento;
    private Vector3 salto;
    private Rigidbody rg;
    private bool can_jump;
    [SerializeField]
    private float impulsao;
    [SerializeField]
    private float velocidade;
    // Start is called before the first frame update
    private Som bonk;
    private bool bonked;
    private Scene cena;
    public float gravity = -12;

    public float walkSpeed = 2;
    private float velocityY;

    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.05f;
    float speedSmoothVelocity;
    private Animator animator;
    private CharacterController controller;
    float currentSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cena = SceneManager.GetActiveScene();
        rg = gameObject.GetComponent<Rigidbody>();
        salto = new Vector3(0.0f, impulsao, 0.0f);
        can_jump = true;
        bonk = FindObjectOfType<AudioManager>().getSom("Bonk");
        bonked = false;
    }

    public void Update()
    {
        // Jump
        if (Input.GetKeyDown("space") && can_jump && Canvas.GetComponent<DialogSystem>().getMovable())
            rg.AddForce(salto, ForceMode.Impulse);

        // Skip writing effect on dialog
        if (Input.GetKeyDown(KeyCode.Q) && !Canvas.GetComponent<DialogSystem>().is_in_independent() && !Canvas.GetComponent<DialogSystem>().getLogStatus() && !Canvas.GetComponent<DialogSystem>().getFinished()) {
            Canvas.GetComponent<DialogSystem>().finishText(true);
        }
        // Enable auto continue on dialog
        if (Input.GetKeyDown(KeyCode.X) && !Canvas.GetComponent<DialogSystem>().getLogStatus())
            Canvas.GetComponent<DialogSystem>().changeAutoDialog();

        // Disable writting effect on dialog
        //if (Input.GetKeyDown(KeyCode.Z) && !Canvas.GetComponent<DialogSystem>().getLogStatus())
        //{
        //    Canvas.GetComponent<DialogSystem>().changeWrittingEffect();
        //    if(!Canvas.GetComponent<DialogSystem>().getFinished())
        //        Canvas.GetComponent<DialogSystem>().finishText(true);
        //}

        // Enable/Disable Log
        if (Input.GetKeyDown(KeyCode.L))
        {
            Canvas.GetComponent<DialogSystem>().switchLog();
        }
    }

// Update is called once per frame
    void FixedUpdate()
    {
        //if (Canvas.GetComponent<DialogSystem>().getMovable())
        //{
            //movimento.x = -Input.GetAxis("Horizontal");
            //movimento.z = -Input.GetAxis("Vertical");
            Vector2 input = new Vector2(-Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Vertical"));

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
            Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);

            //atualizar as velocidades de acordo com o estado do controller
            if (controller.isGrounded) velocityY = 0;
            currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;


            //atualizar animator
            if (inputDirection != Vector2.zero && currentSpeed != 0)
            {
                animator.SetBool("isWalking", true);
                animator.SetInteger("time_wasted", 0);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetInteger("time_wasted", animator.GetInteger("time_wasted") + 1);
            }

        //}
        if (Input.GetKeyDown(KeyCode.E)) //Interact
        {
            animator.ResetTrigger("Dab");
            animator.ResetTrigger("T");
            animator.SetTrigger("Interact");
            animator.SetInteger("time_wasted", 0);
        }
        //movimento = Vector3.ClampMagnitude(movimento, 1);
            //rg.MovePosition(transform.position + movimento * velocidade * Time.fixedDeltaTime);
        //}
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            can_jump = true;
        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            bonk.source.Play();
            if (cena.name == "Quente_Frio" && !Canvas.GetComponent<DialogSystem>().is_active())
            {
                if (bonked)
                {
                    Canvas.GetComponent<DialogSystem>().independentDialog("???", "Hehe");
                }
                else
                {
                    bonked = true;
                    Canvas.GetComponent<DialogSystem>().independentDialog("???", "Wow, would you look at that? The mighty and incredibly stubborn Sarah banging her head against the wall!");
                }
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            can_jump = false;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
        }
    }

    public float GetVelocidade()
    {
        return velocidade;
    }

    public void SetVelocidade(float nova_velocidade)
    {
        velocidade = nova_velocidade;
    }
}
