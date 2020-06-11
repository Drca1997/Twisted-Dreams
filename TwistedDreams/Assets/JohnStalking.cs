using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JohnStalking : MonoBehaviour
{

    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;
    [SerializeField]
    private float time;
    [SerializeField]
    

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        time = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {

            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                time -= Time.deltaTime;
                    
            }
            FaceTarget();
        }
        if (time <= 0f)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<JohnLevel>().LoadNextScene("Paper");
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (FindObjectOfType<JohnLevel>().GetCurrentState() >= 2)
            {
                time -= Time.deltaTime;
            }
            
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.CompareTo("Player") == 0 && FindObjectOfType<JohnLevel>().GetCurrentState() >= 2){ 

            time -= Time.deltaTime;

        }
    }
}
