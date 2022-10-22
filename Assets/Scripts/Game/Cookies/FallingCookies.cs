using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallingCookies
{
    public static void Update(Transform transform, ref FallingVariables values)
    {
        for (short i = 0; i < transform.childCount; i++)
        {
            var temp = transform.GetChild(i);
            if (temp.position.y <= values.minYValue) 
                temp.position = new Vector3(temp.position.x, values.maxYValue, temp.position.z);
            temp.Translate(0, -values.fallSpeed * Time.deltaTime, 0);
        }
            
    }
}
