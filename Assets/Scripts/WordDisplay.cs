using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour
{
    public Text text;
    public SpriteRenderer spriteRenderer;

    public Sprite thief1;
    public Sprite thief2;
    public Sprite thief3;
    public Sprite fighter1;
    public Sprite fighter2;
    public Sprite fighter3;
    public Sprite wizard1;
    public Sprite wizard2;
    public Sprite wizard3;

    public Sprite monster1;
    public Sprite monster2;
    public Sprite monster3;
    public Sprite monster4;
    public Sprite monster5;
    public Sprite monster6;
    public Sprite monster7;
    public Sprite monster8;

    public Sprite treasureCoin;
    public Sprite treasureSword;

    public Sprite attackSlash1;

    private Color activeColor = new Color(255f, 255f, 0f, 1f);
    private Color inactiveColor = Color.gray;
    private Color selectedColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetWord(string word)
    {
        text.text = word;
        text.color = activeColor;
    }

    public void SetSprite(Word.CharacterClass characterClass)
    {
        var rnd = new System.Random();
        switch (characterClass)
        {
            case Word.CharacterClass.Fighter:
                spriteRenderer.sprite = (new Sprite[] {fighter1, fighter2, fighter3})[rnd.Next(0, 3)];
                break;
            case Word.CharacterClass.Thief:
                spriteRenderer.sprite = (new Sprite[] {thief1, thief2, thief3})[rnd.Next(0, 3)];
                break;
            case Word.CharacterClass.Wizard:
                spriteRenderer.sprite = (new Sprite[] {wizard1, wizard2, wizard3})[rnd.Next(0, 3)];
                break;
            case Word.CharacterClass.Monster:
                spriteRenderer.sprite = (new Sprite[] {monster1, monster2, monster3, monster4, monster5, monster6, monster7, monster8})[rnd.Next(0, 8)];
                //spriteRenderer.transform.position = new Vector3(spriteRenderer.transform.position.x - 2f, spriteRenderer.transform.position.y + 0.5f);
                break;
            case Word.CharacterClass.TreasureCoin:
                spriteRenderer.sprite = treasureCoin;
                break;
            case Word.CharacterClass.TreasureSword:
                spriteRenderer.sprite = treasureSword;
                break;
            case Word.CharacterClass.Spell:
                spriteRenderer.sprite = treasureSword;
                spriteRenderer.transform.position = new Vector3(spriteRenderer.transform.position.x - 4f, spriteRenderer.transform.position.y + 0.5f);
                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
                break;
        }
    }

    public void SetActive(bool active, float percent)
    {
        if (active)
        {
            text.color = activeColor;
        } else
        {
            //text.color = inactiveColor + ((Color.white - inactiveColor) * percent);
            text.color = inactiveColor;
        }
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            text.color = activeColor;
        } else
        {
            text.color = Color.gray;
        }
    }

    public void RemoveLetter()
    {
        if (text != null)
        {
            text.text = text.text.Remove(0, 1);
            text.color = selectedColor;
        }
    }

    public void RemoveWord()
    {
        try
        {
            Destroy(gameObject);
        } catch { }
    }
}
