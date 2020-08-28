using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodieWord : Word
{
    private float rotation = 0f;
    private float rotationStep = 60f;

    public GoodieWord(string word, WordDisplay display, CharacterClass characterClass) : base(word, display, characterClass)
    {
        switch (characterClass)
        {
            case Word.CharacterClass.Fighter:
                fallSpeed = 0.3f;
                hitPoints = 3f;
                break;
            case Word.CharacterClass.Thief:
                fallSpeed = 0.5f;
                hitPoints = 1f;
                break;
            case Word.CharacterClass.Wizard:
                fallSpeed = 0.4f;
                hitPoints = 2f;
                break;
        }

        rotationStep *= fallSpeed;
    }

    public override void Update()
    {
        base.Update();

        if (fallDelay <= 0)
        {
            float thisRotation = rotationStep * Time.deltaTime;
            rotation += thisRotation;
            display.spriteRenderer.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);

            if (rotation > 8)
            {
                rotation = 8f;
                rotationStep = -rotationStep;
            }
            else if (rotation < -8)
            {
                rotation = -8f;
                rotationStep = -rotationStep;
            }
        }

        if (!this.WordDead())
        {
            float scale = 1f + ((hitPoints - 1f) / 3f);
            this.display.spriteRenderer.transform.localScale = new Vector3(200 * scale, 200 * scale, 200 * scale);
        }
    }
}
