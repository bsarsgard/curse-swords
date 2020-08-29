using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureWord : Word
{
    public TreasureWord(string word, WordDisplay display, int swordChance): base(word, display, Word.CharacterClass.TreasureCoin) {
        gold = 1;
        timed = true;
        this.timer = 10f;
        if (Random.Range(0, 100) < swordChance)
        {
            display.spriteRenderer.color = Color.yellow;
            SetCharacterClass(Word.CharacterClass.TreasureSword);
            this.timer = 5f;
        }
    }
    
    public override bool StealingTreasure()
    {
        return false;
    }

    public override void Update()
    {
        base.Update();

        if (timer < 5f) {
            display.spriteRenderer.color = new Color(
                display.spriteRenderer.color.r,
                display.spriteRenderer.color.g,
                display.spriteRenderer.color.b,
                (timer / 5f));
        }
    }
}
