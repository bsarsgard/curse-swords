using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour
{
    public WordManager wordManager;
    private int wave = 0;
    private float wordDelay = 0f;
    private float nextWordTime = 0f;

    private float maxDelay = 15f;
    private float minDelay = 3f;
    private float nextValue = 0.9f;

    private void Start()
    {
        wave = 1;
        wordDelay = maxDelay;
        nextWordTime = 0f;
    }

    private void Update()
    {
        if (Time.time >= nextWordTime)
        {
            // add a word
            wordManager.AddGoodie();
            // reset time to next word
            nextWordTime = Time.time + (wordDelay / (float)wave);
            // shorten time to next word
            wordDelay *= nextValue;

            if (wordDelay < minDelay)
            {
                // new wave
                wave++;
                wordManager.NewWave(wave);
                // reset delay to baseline
                wordDelay = maxDelay;
                // bump min delay and increase rate
                //minDelay *= .8f;
                //nextValue *= .9f;
            }
        }
    }
}
