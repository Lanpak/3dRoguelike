using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScaler : MonoBehaviour
{
    public int scalingFramesLeft = 0;

    public int maxSize;
    public int minSize;


    public bool hasFinishedScaling = false;
    // Use this for initialization
    void Start()
    {
        scalingFramesLeft = Random.Range(minSize,maxSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFinishedScaling)
        {
            scalingFramesLeft = Random.Range(minSize, maxSize);

            hasFinishedScaling = false;
        }

        if (scalingFramesLeft > 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 2, Time.deltaTime * 10);
            scalingFramesLeft--;
        }



    }
}
