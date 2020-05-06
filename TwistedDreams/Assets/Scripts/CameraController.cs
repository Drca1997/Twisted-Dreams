using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Referência ao objeto Player, para a câmara o seguir")]
    public GameObject player;

    private Vector3 offset;

    [Tooltip("Valor da interpolação da nova posição.")]
    [SerializeField][Range(0f, 1f)] private float interpolation = 0.1f;



    // Complemento da interpolação
    private float comp_interpolation;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        comp_interpolation = 1 - interpolation;

    }

    // Update is called once per frame
    void LateUpdate()
    {
       
       transform.position = (transform.position * comp_interpolation) + ((player.transform.position + offset) * interpolation);
    }

    public void Move(Vector3 destino){
        transform.position = (transform.position * comp_interpolation) + ((destino + offset));
    }

    public void Rotate(float angulo)
    {
        Quaternion target = Quaternion.Euler(angulo, angulo, angulo);
        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * comp_interpolation);
        
    }
}
