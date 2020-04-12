using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        rg = gameObject.GetComponent<Rigidbody>();
        salto = new Vector3(0.0f, impulsao, 0.0f);
        can_jump = true;
        bonk = FindObjectOfType<AudioManager>().getSom("Bonk");
       
    }

    public void Update()
    {
        if (Input.GetKeyDown("space") && can_jump)
        {
            rg.AddForce(salto, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Return) && !Canvas.GetComponent<DialogSystem>().running)
        {
            Canvas.GetComponent<DialogSystem>().active = true;
        }
    }

// Update is called once per frame
    void FixedUpdate()
    {
       
        movimento.x = -Input.GetAxis("Horizontal");
        movimento.z = -Input.GetAxis("Vertical");
        movimento = Vector3.ClampMagnitude(movimento, 1);
        rg.MovePosition(transform.position + movimento * velocidade * Time.fixedDeltaTime);


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
            //triggers dialog with Sarah-Camera
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            can_jump = false;
        }
    }

    public float GetVelocidade()
    {
        return velocidade;
    }
}
