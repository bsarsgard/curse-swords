using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWord : Word
{
    // TODO: count down a timer to disappear
    public TreasureWord(string word, WordDisplay display, int streak): base(word, display, Word.CharacterClass.TreasureCoin) {
        gold = 1;
        timed = true;
        this.timer = 10f;
        if (Random.Range(streak, 100) > 60)
        {
            SetCharacterClass(Word.CharacterClass.TreasureSword);
            this.timer = 5f;
        }
    }
    
    public override bool StealingTreasure()
    {
        return false;
    }

    /*
    public override void RandomWord(List<Word> words)
    {
        SetWord(WordGenerator.GetRandomClean(words));
    }
    */
}
