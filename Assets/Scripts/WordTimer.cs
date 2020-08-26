using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour
{
    public WordManager wordManager;
    private float wordDelay = 15f;
    private float minDelay = 3f;
    private float nextWordTime = 0f;

    private void Update()
    {
        if (Time.time >= nextWordTime)
        {
            wordManager.AddGoodie();
            nextWordTime = Time.time + wordDelay;
            wordDelay *= .9f;

            if (wordDelay < minDelay)
            {
                wordDelay = minDelay * 5f;
                minDelay *= .8f;
            }
        }
    }
}
