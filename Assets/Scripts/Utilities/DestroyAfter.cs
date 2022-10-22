using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    float timer = 0;
    [SerializeField] float timeUntilDestruction = 1f;
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;        
        if (timer >= timeUntilDestruction) 
            Destroy(gameObject);
    }
}
