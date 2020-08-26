using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BaddieWord : Word
{
    public bool on = false;
    private float bounce = 0f;
    private float bounceStep = 0.002f;

    public BaddieWord(string word, WordDisplay display): base(word, display, Word.CharacterClass.Monster)
    {
        power = 2f;
    }
    
    public override bool StealingTreasure()
    {
        return false;
    }

    public override bool WordDead()
    {
        return false;
    }

    public void SetOn(bool on)
    {
        this.on = on;
        if (on)
        {
            display.spriteRenderer.color = Color.green;
        }
        else
        {
            display.spriteRenderer.color = Color.white;
        }
    }

    public override bool IsActive()
    {
        return !on;
    }

    public override void Update()
    {
        base.Update();
        if (on)
        {
            bounce += bounceStep * Time.deltaTime;
            if (bounce > 0.0005f)
            {
                bounce = 0.0005f;
                bounceStep = -bounceStep;
            } else if (bounce < -0.0005f) {
                bounce = -0.0005f;
                bounceStep = -bounceStep;
            }
            display.transform.Translate(0f, bounceStep, 0);
        }
        else
        {
            bounce = 0f;
        }
    }

    public override void SetWord(string word)
    {
        base.SetWord(word);
        display.SetSprite(characterClass);
        display.transform.position = new Vector3(-3.26f, -3.1f);
    }
}
