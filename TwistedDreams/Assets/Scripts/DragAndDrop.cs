using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public bool SelectedPiece;
    Vector3 RightPosition;
    public bool InRightPosition;
    private float distance;
    PuzzleDone parentScript;

    // Start is called before the first frame update
    void Start()
    {
        RightPosition = transform.position;
        transform.position = new Vector3(Random.Range(15f, 25f), Random.Range(-8f, 1f), 0);
       
    }


    void OnMouseDown()
    {
        if (!InRightPosition)
        {
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            SelectedPiece = true;
        }
      
    }

    void OnMouseUp()
    {
        SelectedPiece = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectedPiece)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = new Vector3(ray.GetPoint(distance).x,ray.GetPoint(distance).y,0);
            transform.position = rayPoint;

        }

        if (Vector3.Distance(transform.position, RightPosition) < 1)
        {
            transform.position = RightPosition;
            InRightPosition = true;
        }
    } 
}
