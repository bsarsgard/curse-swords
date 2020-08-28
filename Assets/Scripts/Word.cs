using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[System.Serializable]
public class Word
{
    public string word;
    public CharacterClass characterClass;
    public float power = 0f;
    public int gold = 0;
    protected float fallSpeed = 0f;
    protected float fallDelay = 0f;
    protected float hitPoints = 1f;
    protected bool timed = false;
    protected float timer = 0f;

    private int typeIndex;

    public enum CharacterClass
    {
        Fighter,
        Thief,
        Wizard,

        Monster,

        TreasureCoin,
        TreasureSword,

        Spell,
    }

    public readonly WordDisplay display;

    public Word(string word, WordDisplay display, CharacterClass characterClass)
    {
        this.word = word;
        this.display = display;
        this.display.SetWord(word);
        typeIndex = 0;
        SetCharacterClass(characterClass);
    }

    public virtual void Update()
    {
        if (fallDelay > 0)
        {
            fallDelay -= Time.deltaTime;
            if (fallDelay < 0)
            {
                fallDelay = 0;
            }
        } else
        {
            display.transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        }
        if (timed)
        {
            timer -= Time.deltaTime;
        }
    }

    public void SetCharacterClass(CharacterClass characterClass) {
        this.characterClass = characterClass;
        this.display.SetSprite(characterClass);
    }

    public virtual bool IsActive()
    {
        return true;
    }

    public char GetNextLetter()
    {
        if (typeIndex < word.Length)
        {
            return word[typeIndex];
        } else
        {
            return Char.MinValue;
        }
    }

    public void TypeLetter()
    {
        typeIndex++;
        display.RemoveLetter();
    }

    public void Kill()
    {
        Destroy();
    }

    public bool WordTyped(BaddieWord baddie)
    {
        bool wordTyped = typeIndex >= word.Length;
        if (wordTyped)
        {
            fallDelay = 2f;
            if (baddie == null)
            {
                hitPoints--;
            }
            else
            {
                hitPoints -= (1 + baddie.power);
            }
            if (WordDead())
            {
                Destroy();
            }
        }

        return wordTyped;
    }

    public virtual bool WordDead()
    {
        return hitPoints <= 0 || (this.timed && this.timer <= 0);
    }

    public virtual bool StealingTreasure()
    {
        return display.transform.position.y < -0.5f;
    }

    public virtual void SetWord(string word)
    {
        this.word = word;
        Reset();
    }

    public virtual void Reset()
    {
        typeIndex = 0;

        try
        {
            display.SetWord(word);
        } catch
        {
            // do nothing
        }
    }

    public void Destroy()
    {
        display.RemoveWord();
    }
}
