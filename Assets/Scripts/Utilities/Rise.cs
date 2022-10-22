using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rise : MonoBehaviour
{
    [SerializeField] float speed; 
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0); 
    }
}
