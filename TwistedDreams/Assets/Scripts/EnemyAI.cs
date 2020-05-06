using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float lookRadius = 10f;
    Transform target; 
    NavMeshAgent agent;
    public GameObject bala;
    public float bullet_speed;
    private Transform bullet_spawn;
    private Som gunshot_sound;
    public ParticleSystem muzzleFlash;
    public float attack_speed = 0.33f;
    public float cooldown = 0f;
    private TimeManager timemanager;

    // Start is called before the first frame update
    void Start()
    { 
        bullet_spawn = GameObject.Find("BulletSpawn").transform;
        gunshot_sound = GameObject.FindObjectOfType<AudioManager>().getSom("shotgun_gunshot");
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        timemanager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = Mathf.Clamp(cooldown - Time.deltaTime, 0f, 1f / attack_speed);

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                Dispara();
                FaceTarget();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }


    void Dispara()
    {
        
        RaycastHit disparo;
        if (cooldown == 0f)
        {
            muzzleFlash.Play();
            gunshot_sound.source.Play();
            //Instantiate projétil
            GameObject projetil = Instantiate(bala, bullet_spawn.position, Quaternion.Euler(0f, -90f,0f));
            projetil.AddComponent<Projétil>();
            projetil.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bullet_speed);
            //projetil.AddComponent<Rewindable>();
            //projetil.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * bullet_speed / timemanager.slowdownFactor);
            //timemanager.SlowMotion();


            cooldown = 1f / attack_speed;



        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
