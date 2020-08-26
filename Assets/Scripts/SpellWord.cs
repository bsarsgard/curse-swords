using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellWord : Word
{
    private int words;
    public int charge = 0;

    public SpellWord(string word, WordDisplay display): base(word, display, Word.CharacterClass.Spell)
    {
        words = word.Split(' ').Length;
        display.SetActive(false);
    }

    public override bool IsActive()
    {
        return charge >= words;
    }
    
    public override bool StealingTreasure()
    {
        return false;
    }

    /*
    public override void RandomWord(List<Word> words)
    {
        SetWord(WordGenerator.GetRandomPhrase(words));
    }
    */

    public override bool WordDead()
    {
        return false;
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void SetWord(string word)
    {
        base.SetWord(word);
        words = word.Split(' ').Length;
        charge = 0;
        display.SetActive(false);
    }

    public void AddCharge()
    {
        charge += 1;

        if (IsActive())
        {
            display.SetWord(word);
        }
        else
        {
            string[] parts = word.Split(' ');
            string text = "<color=white>";
            for (int i = 0; i < parts.Length; i++)
            {
                text += parts[i] + " ";
                if (i == charge - 1)
                {
                    text += "</color>";
                }
            }
            display.SetWord(text);
        }
        display.SetActive(IsActive(), ((float)charge / (float)words));
    }

    public string GetNextWord()
    {
        return word.Split(' ')[charge];
    }

}
