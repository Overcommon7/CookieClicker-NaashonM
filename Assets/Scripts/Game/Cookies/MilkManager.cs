using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MilkManager : MonoBehaviour
{
    List<Transform> children = new List<Transform>();
    [SerializeField] SpriteRenderer multiplier;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] List<float> heights = new List<float>();
    [SerializeField] float minXValue, animationSpeed, riseSpeed;
    private float milkWidth = 0;
    public static short spriteIndexOffset = 0;
    void Start()
    {
        children = GetComponentsInChildren<Transform>().ToList();
        children.Remove(transform);
        children.Remove(multiplier.transform);
        if (children.Count > 1)
            milkWidth = children[1].position.x - children[0].position.x;
    }

    void AnimateMilk()
    {
        for (short i = 0; i < children.Count; i++)
        {
            if (children[i].position.x <= minXValue)
            {
                int index = i - 1;
                if (index < 0) index = children.Count - 1;
                children[i].position = new Vector3(children[index].position.x + milkWidth, children[i].position.y, children[i].position.z);
            }
                
            children[i].Translate(-animationSpeed * Time.deltaTime, 0, 0);
        }
    }

    void AdjustYHeight(short index)
    {
        multiplier.sprite = sprites[index];
        index -= spriteIndexOffset;
        foreach (var t in children)
        {
            if (children[0].position.y < heights[index])
            {
                t.Translate(0, riseSpeed * Time.deltaTime, 0);
                t.position = t.position.ClampY(t.position.y, heights[index]);
            }               
            else
            {
                t.Translate(0, -riseSpeed * Time.deltaTime, 0);
                t.position = t.position.ClampY(heights[index], t.position.y);
            }
               
        }
    }

    void Update()
    {
        AnimateMilk();
        AdjustYHeight((short)(PlayerManager.instance.runTimeValues.cookiePerClickMultiplier - 1 + spriteIndexOffset));
    }
}
