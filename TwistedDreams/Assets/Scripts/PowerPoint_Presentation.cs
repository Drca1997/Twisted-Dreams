using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPoint_Presentation : MonoBehaviour
{
    public GameObject tela;
    private DialogSystem dialogSystem;
    private string slide2_trigger;
    private string slide3_trigger;
    private string slide4_trigger;
    public Sprite[] slides;
    public GameObject time_lapse;
    public GameObject C;
    private Animator animator;
    [Tooltip("Quantidade de tempo que 'One Hour Later' fica no ecrã")]
    public float time_on_screen;
    public TextAsset final;

    private void Awake()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        slide2_trigger = "As you already now, you are dreaming.";
        slide3_trigger = "And bad news for you, because it's actually a nightmare.";
        slide4_trigger = "You see, you had a terrible car accident...";
        animator = C.GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        
        if (dialogSystem.is_active())
        {
            if (dialogSystem.GetCurrentLine().Contains(slide2_trigger))
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[0];
                C.GetComponent<Head_Animations>().Do_NewSlide_Anim();
            }
            else if (dialogSystem.GetCurrentLine().Contains(slide3_trigger))
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[1];
                animator.SetTrigger("Novo_Slide");
            }
            else if (dialogSystem.GetCurrentLine().Contains(slide4_trigger))
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[2];
                animator.SetTrigger("Novo_Slide");
            }
            
        }
        if (dialogSystem.Is_Dialog_Finished())
        {
            time_lapse.GetComponent<SpriteRenderer>().enabled = true;
            time_on_screen -= Time.deltaTime;
            animator.ResetTrigger("Novo_Slide");
        }
        if (time_on_screen <= 0)
        {
            time_lapse.GetComponent<SpriteRenderer>().enabled = false;
            tela.GetComponent<SpriteRenderer>().sprite = slides[3];
            animator.SetTrigger("Novo_Slide");
            dialogSystem.ReStart(final, false);
            dialogSystem.ActivateDialog(false);
        }
       
    }
}
