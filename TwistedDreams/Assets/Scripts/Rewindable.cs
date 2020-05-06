using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewindable : MonoBehaviour
{
    private float rewindLength = 5f;
    public bool isRewinding = false;
    private Rigidbody rb;
    private List<StateInTime> statesInTime;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        statesInTime = new List<StateInTime>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    void Rewind()
    {
        if (statesInTime.Count > 0)
        {
            StateInTime stateinTime = statesInTime[0];
            transform.position = stateinTime.position;
            transform.rotation = stateinTime.rotation;
            statesInTime.RemoveAt(0);

        }
        else
        {
            StopRewind();
        }
    }
    void Record()
    {
        if (statesInTime.Count > Mathf.Round(rewindLength / Time.fixedDeltaTime)){
            statesInTime.RemoveAt(statesInTime.Count -1);
        }
        statesInTime.Insert(0, new StateInTime(transform.position, transform.rotation));
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
        //FindObjectOfType<TimeManager>().StopSlowMotion();
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        if (gameObject.tag.CompareTo("Projétil") == 0)
        {
            
            rb.AddForce(GameObject.FindGameObjectWithTag("John").transform.forward * gameObject.GetComponent<Projétil>().bullet_speed);
        }
    }

}
