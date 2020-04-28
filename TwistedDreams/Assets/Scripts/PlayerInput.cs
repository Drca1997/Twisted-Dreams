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
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Q) && !Canvas.GetComponent<DialogSystem>().is_in_independent() && !Canvas.GetComponent<DialogSystem>().getLogStatus() && !Canvas.GetComponent<DialogSystem>().getautoDialog() && !Canvas.GetComponent<DialogSystem>().getFinished()) {
            Canvas.GetComponent<DialogSystem>().finishText(true);
        }
        // Enable auto continue on dialog
        if (Input.GetKeyDown(KeyCode.X) && !Canvas.GetComponent<DialogSystem>().getLogStatus())
            Canvas.GetComponent<DialogSystem>().changeAutoDialog();

        // Disable writting effect on dialog
        if (Input.GetKeyDown(KeyCode.Z) && !Canvas.GetComponent<DialogSystem>().getLogStatus())
        {
            Canvas.GetComponent<DialogSystem>().changeWrittingEffect();
            if(!Canvas.GetComponent<DialogSystem>().getFinished())
                Canvas.GetComponent<DialogSystem>().finishText(true);
        }

        // Enable/Disable Log
        if (Input.GetKeyDown(KeyCode.L))
        {
            Canvas.GetComponent<DialogSystem>().switchLog();
        }
    }

// Update is called once per frame
    void FixedUpdate()
    {
        if (Canvas.GetComponent<DialogSystem>().getMovable())
        {
            movimento.x = -Input.GetAxis("Horizontal");
            movimento.z = -Input.GetAxis("Vertical");
            movimento = Vector3.ClampMagnitude(movimento, 1);
            rg.MovePosition(transform.position + movimento * velocidade * Time.fixedDeltaTime);
        }
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
}
