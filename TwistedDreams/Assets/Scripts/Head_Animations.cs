using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Animations : MonoBehaviour
{
    private Animator animator;
    private float time;
    private float next_rand_anim_timestamp;
    [Tooltip("Quanto tempo minimo entre animações aleatórias")]
    public float min_time_between_rand_anims;
    [Tooltip("Quanto tempo máximo entre animações aleatórias")]
    public float max_time_between_rand_anims;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        time = 0;
        next_rand_anim_timestamp = 0;
    }


    private void Update()
    {

        time += Time.deltaTime;
        if ((time >= next_rand_anim_timestamp)  && Can_Do_Random_Animation())
        {
            Do_Random_Animation();
            time = 0;
            next_rand_anim_timestamp = Random.Range(min_time_between_rand_anims, max_time_between_rand_anims);
            
        }
    }


    public bool Can_Do_Random_Animation() //verifica se esta a decorrer alguma animaçao. Se sim, não efetua uma nova
    {
        //float length = animator.GetCurrentAnimatorStateInfo(0).length;
        //string name = animator.n

        return true;
    }

    public void Do_Random_Animation()
    {
        int num = Random.Range(1,8);
       
       
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookDown");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Novo_Slide");
        animator.ResetTrigger("Close_Eyes");
        animator.SetInteger("Random_Int", num);
        animator.SetTrigger("Random_Trigger");
        Debug.Log("Random Anim: " + num);
    }

    public void Do_Horizontal_Headshake()
    {
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookDown");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Novo_Slide");
        animator.ResetTrigger("Close_Eyes");
        animator.ResetTrigger("Random_Trigger");
        animator.SetInteger("Random_Int", 0);
        animator.SetTrigger("X_Shake");
    }

    public void Do_Vertical_Headshake()
    {
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookDown");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Novo_Slide");
        animator.ResetTrigger("Close_Eyes");
        animator.ResetTrigger("Random_Trigger");
        animator.SetInteger("Random_Int", 0);
        animator.SetTrigger("Y_Shake");
    }

    public void Do_Embarassed_Anim()
    {
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("LookDown");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Novo_Slide");
        animator.ResetTrigger("Close_Eyes");
        animator.ResetTrigger("Random_Trigger");
        animator.SetInteger("Random_Int", 0);
        animator.SetTrigger("Embarassed");
    }

    public void Do_LookUp_Anim()
    {
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookDown");
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("Close_Eyes");
        animator.ResetTrigger("Novo_Slide");
        animator.SetInteger("Random_Int", 0);
        animator.ResetTrigger("Random_Trigger");
        animator.SetTrigger("LookUp");
    }

    public void Do_LookDown_Anim()
    {
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("Random_Trigger");
        animator.ResetTrigger("Close_Eyes");
        animator.ResetTrigger("Novo_Slide");
        animator.SetInteger("Random_Int", 0);
        animator.SetTrigger("LookDown");
    }

    public void Do_NewSlide_Anim()
    {
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("Random_Trigger");
        animator.ResetTrigger("Close_Eyes");
        animator.SetInteger("Random_Int", 0);
        animator.SetTrigger("Novo_Slide");

    }

    public void Close_Eyes_Anim()
    {
        Debug.Log("Close Eyes");
        animator.ResetTrigger("X_Shake");
        animator.ResetTrigger("Embarassed");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("Y_Shake");
        animator.ResetTrigger("Random_Trigger");
        animator.ResetTrigger("Novo_Slide");
        animator.SetInteger("Random_Int", 0);
        animator.SetTrigger("Close_Eyes");
        StartCoroutine(DelayedLoad());
    }

    IEnumerator DelayedLoad()
    {
        Debug.Log("Delaying...");
        //Wait until clip finish playing
        yield return new WaitForSeconds(2); //duraçao da animaçao de fechar os olhos
    }
}
