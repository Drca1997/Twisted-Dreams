using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPoint_Presentation : MonoBehaviour
{
    public GameObject tela;
    private DialogSystem dialogSystem;
    private string slide1_trigger;
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
    private bool [] Anims_Done;
    private void Awake()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        slide1_trigger = "So, here we go. Since you don't remember s**t, I prepared a PowerPoint presentation to explain you the situation.";
        slide2_trigger = "As you already now, you are dreaming.";
        slide3_trigger = "And bad news for you, because it's actually a nightmare.";
        slide4_trigger = "You see, you had a terrible car accident...";
        animator = C.GetComponent<Animator>();
        dialogSystem.ActivateDialog(false);
        Anims_Done = new bool[5];
        for (int i=0; i< Anims_Done.Length; i++)
        {
            Anims_Done[i] = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (dialogSystem.is_active())
        {
            if (dialogSystem.GetCurrentLine().Contains(slide2_trigger) && !Anims_Done[0])
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[0];
                C.GetComponent<Head_Animations>().Do_NewSlide_Anim();
                Anims_Done[0] = true;
            }
            else if (dialogSystem.GetCurrentLine().Contains(slide2_trigger) && !Anims_Done[1])
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[1];
                C.GetComponent<Head_Animations>().Do_NewSlide_Anim();
                Anims_Done[1] = true;
            }
            else if (dialogSystem.GetCurrentLine().Contains(slide3_trigger) && !Anims_Done[2])
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[2];
                animator.SetTrigger("Novo_Slide");
                Anims_Done[2] = true;
            }
            else if (dialogSystem.GetCurrentLine().Contains(slide4_trigger) && !Anims_Done[3])
            {
                tela.GetComponent<SpriteRenderer>().sprite = slides[3];
                animator.SetTrigger("Novo_Slide");
                Anims_Done[3] = true;
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
            tela.GetComponent<SpriteRenderer>().sprite = slides[4];
            animator.SetTrigger("Novo_Slide");
            dialogSystem.ReStart(final, false);
            dialogSystem.ActivateDialog(false);
        }
       
    }
}
