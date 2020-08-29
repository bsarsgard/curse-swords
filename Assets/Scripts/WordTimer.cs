using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTimer : MonoBehaviour
{
    public WordManager wordManager;
    private int wave;
    private float wordDelay;
    private float nextWordTime;

    private float waveTime = 60f;
    private float waveDelay;
    private float maxDelay = 15f;
    private float minDelay = 3f;
    private float nextValue = 0.85f;
    //private float nextDelay = 0.9f;

    private void Start()
    {
        wave = 1;
        wordDelay = maxDelay;
        waveDelay = waveTime;
        nextWordTime = 0f;
    }

    private void Update()
    {
        waveDelay -= Time.deltaTime;
        //Debug.Log(waveDelay);
        if (Time.time >= nextWordTime)
        {
            // add a word
            wordManager.AddGoodie();
            // reset time to next word 
            nextWordTime = Time.time + wordDelay;
            // shorten time to next word
            wordDelay = Math.Max(wordDelay * (float)Math.Pow(nextValue, (float)wave),
                (minDelay - (float)wave / 10f));
            Debug.Log("delay: " + wordDelay);

            //if (wordDelay < minDelay)
            if (waveDelay <= 0)
            {
                // new wave
                //Debug.Log("Wave: " + wave.ToString() + " delay: " + wordDelay);
                waveDelay = waveTime;
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
