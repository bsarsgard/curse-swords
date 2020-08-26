using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWord : Word
{
    private SpellWord spell;

    public SwordWord(string word, WordDisplay display, int streak, SpellWord spell): base(word, display, Word.CharacterClass.TreasureSword) {
        timed = true;
        this.timer = 5f;
        this.spell = spell;
    }
    
    public override bool StealingTreasure()
    {
        return false;
    }

    /*
    public override void RandomWord(List<Word> words)
    {
        SetWord(spell.GetNextWord());
    }
    */
}
