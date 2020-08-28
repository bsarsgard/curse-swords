using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Security.Cryptography;
using UnityEditor;

public class WordManager: MonoBehaviour
{
    public static bool sound = true;
    public static bool nsfw = false;

    public List<Word> words;
    public Transform wordCanvas;

    public WordSpawner wordSpawner;
    public Text killsText;
    public Text goldText;
    public Text streakText;

    public Sprite chestClosed;
    public Sprite chestOpen;
    public Sprite chestClutter1;
    public List<GameObject> chests;
    public List<GameObject> chestClutter;

    public Sprite attackSlash1;
    public Sprite attackSlash2;
    public Sprite attackSlash3;
    public Sprite attackKill;
    public Sprite attackBurn;
    public List<GameObject> effects;
    public List<GameObject> tips;
    public Dictionary<Tuple<string, Vector3>, float> futureTips;
    public GameObject tipPrefab;
    private float effectsFade = 1f;
    private float tipsFade = 0.1f;

    private bool showedSwordTip = false;
    private bool showedCurseTip = false;
    private bool showedSwordPickupTip = false;

    public AudioClip music1;
    public AudioClip audioHit1;
    public AudioClip audioHit2;
    public AudioClip audioDie;
    public AudioClip audioSpell;
    public AudioClip audioTreasure;
    public AudioClip audioSword;
    public AudioClip audioSteal;
    public AudioClip audioMonster;
    public AudioClip audioError;
    public AudioClip audioType;

    public TextAsset cursesFile;
    public TextAsset cleansFile;
    public TextAsset phrasesFile;

    private string[] curses;
    private string[] cleans;
    private string[] phrases;

    private Word activeWord = null;
    private SpellWord activeSpell = null;
    private BaddieWord activeBaddie = null;
    private int kills = 0;
    private int gold = 10;
    private int streak = 0;
    private int maxStreak = 0;

    private int treasureChance = 30;

    private void Start()
    {
        futureTips = new Dictionary<Tuple<string, Vector3>, float>();

        // parse text files
        if (nsfw)
        {
            curses = cursesFile.text.ToLower().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
        else
        {
            curses = cleansFile.text.ToLower().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
        cleans = cleansFile.text.ToLower().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        phrases = phrasesFile.text.ToLower().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        curses = curses.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
        cleans = cleans.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
        phrases = phrases.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = music1;
        audio.loop = true;
        if (sound)
        {
            audio.volume = 0.5f;
        }
        else
        {
            audio.mute = true;
        }
        audio.Play();

        AddBaddies();
        AddSpell();
        AddChests(gold);
        AddTip("Type the words as they come down to defend your dungeon!", new Vector3(0, 3f, 0));
        AddFutureTip("Power up your next attack by adding a Monster word", new Vector3(-1.5f, -2.5f, 0), 10f);
    }

    private void Update()
    {
        foreach (Word word in words)
        {
            word.Update();
            if (word.StealingTreasure())
            {
                gold--;
                RemoveChestTreasure(1);
                PlayClip(audioSteal);
                if (word == activeWord)
                {
                    activeWord = null;
                }
                word.Destroy();
                words.Remove(word);
                break;
            }
            else if (word.WordDead())
            {
                if (word == activeWord)
                {
                    activeWord = null;
                }
                kills++;
                word.Destroy();
                words.Remove(word);
                break;
            }
            else
            {
            }
        }

        killsText.text = kills.ToString();
        goldText.text = gold.ToString();
        streakText.text = streak.ToString();

        // fade effects
        foreach (GameObject obj in effects)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer.color.a == 0)
            {
                effects.Remove(obj);
                Destroy(obj);
                break;
            }
            else
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, renderer.color.a - effectsFade * Time.deltaTime);
            }
        }
        foreach (GameObject obj in tips)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            Text text = obj.GetComponent<Text>();
            if (text.color.a == 0)
            {
                effects.Remove(obj);
                Destroy(obj);
                break;
            }
            else
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - tipsFade * Time.deltaTime);
            }
        }
        List<Tuple<string, Vector3>> keys = new List<Tuple<string, Vector3>>(futureTips.Keys);
        foreach (var key in keys)
        {
            if (futureTips[key] <= 0)
            {
                AddTip(key.Item1, key.Item2);
                futureTips.Remove(key);
                break;
            }
            else
            {
                futureTips[key] = futureTips[key] - Time.deltaTime;
            }
        }
    }

    public string GetRandomWord(string[] list, bool hardPass)
    {
        string randomWord = null;
        int tries = 0;
        while (randomWord == null && tries++ < 100) {
            int randomIndex = UnityEngine.Random.Range(0, list.Length);
            randomWord = list[randomIndex].Trim();
            if (words.Any(x => randomWord[0] == x.word[0]))
            {
                randomWord = null;
            }
        }

        if (randomWord == null && !hardPass)
        {
            // couldn't find a word but not a hard pass so just pick something
            int randomIndex = UnityEngine.Random.Range(0, list.Length);
            randomWord = list[randomIndex].Trim();
        }

        return randomWord;
    }

    public void AddChests(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject sprite = new GameObject("chest");
            SpriteRenderer renderer = sprite.AddComponent<SpriteRenderer>();
            renderer.sprite = chestClosed;
            sprite.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            float xpos = -4.5f + (10f / (float)count) * (float)i;
            sprite.transform.position = new Vector3(xpos, -1.29f);
            chests.Add(sprite);
        }
    }

    public void AddChestTreasure(int count)
    {
        for (int i = 0; i < count; i++)
        {
            bool opened = false;
            foreach (GameObject chest in chests)
            {
                SpriteRenderer chestRenderer = chest.GetComponent<SpriteRenderer>();
                if (chestRenderer.sprite == chestOpen)
                {
                    chestRenderer.sprite = chestClosed;
                    opened = true;
                    break;
                }
            }

            if (!opened)
            {
                GameObject sprite = new GameObject("chest clutter");
                SpriteRenderer renderer = sprite.AddComponent<SpriteRenderer>();
                renderer.sprite = chestClutter1;
                sprite.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                float xpos = UnityEngine.Random.Range(-5f, 5f);
                sprite.transform.position = new Vector3(xpos, -1.29f);
                chestClutter.Add(sprite);
            }
        }
    }

    public void RemoveChestTreasure(int count)
    {
        if (gold <= 0)
        {
            KillScreenUiManager.kills = kills;
            KillScreenUiManager.treasure = gold;
            KillScreenUiManager.streak = maxStreak;
            SceneManager.LoadScene("KillScreen");
        }
        for (int i = 0; i < count; i++)
        {
            if (chestClutter.Count > 0)
            {
                GameObject chest = chestClutter[0];
                Destroy(chest);
                chestClutter.RemoveAt(0);
            }
            else
            {
                foreach (GameObject sprite in chests)
                {
                    SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
                    if (renderer.sprite == chestClosed)
                    {
                        renderer.sprite = chestOpen;
                        break;
                    }
                }
            }
        }
    }

    public void AddTreasure(Vector3 position)
    {
        string theWord = GetRandomWord(cleans, true);
        if (theWord != null)
        {
            TreasureWord word = new TreasureWord(theWord, wordSpawner.SpawnTreasure(position), streak);

            if (word.characterClass == Word.CharacterClass.TreasureSword && !showedSwordTip)
            {
                showedSwordTip = true;
                AddTip("Collect Swords to unlock words in Curse Sword phrases", new Vector3(0, 0, 0));
            }
            words.Add(word);
        }
    }

    public void AddSpell()
    {
        activeSpell = new SpellWord(GetRandomWord(phrases, false), wordSpawner.SpawnSpell());
        words.Add(activeSpell);
    }

    public void AddBaddies()
    {
        words.Add(new BaddieWord(GetRandomWord(curses, false), wordSpawner.SpawnBaddie()));
    }

    public void AddGoodie()
    {
        string theWord = GetRandomWord(cleans, true);

        if (theWord != null) {
            var rnd = new System.Random();
            Word.CharacterClass characterClass = (Word.CharacterClass)rnd.Next(3);

            Word word = new GoodieWord(theWord, wordSpawner.SpawnGoodie(), characterClass);
            words.Add(word);
        }
    }

    private void AddEffect(Word word, Sprite sprite, Color color)
    {
        AddEffect(word, sprite, color, new Vector3(0, 0, 0));
    }

    private void PlayClip(AudioClip clip)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(clip);
    }

    private void AddEffect(Word word, Sprite sprite, Color color, Vector3 offset)
    {
        GameObject attackSprite = new GameObject("atack");
        SpriteRenderer renderer = attackSprite.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;
        attackSprite.transform.localScale = new Vector3(5f, 5f, 5f);
        attackSprite.transform.position = word.display.spriteRenderer.transform.position + offset;
        effects.Add(attackSprite);
    }

    private void AddFutureTip(String tip, Vector3 position, float delay)
    {
        Tuple<string, Vector3> key = new Tuple<string, Vector3>(tip, position);
        futureTips.Add(key, delay);
    }

    private void AddTip(String tip, Vector3 position)
    {
        GameObject obj = Instantiate(tipPrefab, position, Quaternion.identity, wordCanvas);
        Text text = obj.GetComponent<Text>();
        text.text = tip;
        //text.transform.localScale = new Vector3(5f, 5f, 5f);
        text.transform.position = position;
        tips.Add(obj);
    }

    private void AddStreak()
    {
        streak++;
        if (streak > maxStreak)
        {
            maxStreak = streak;
        }
    }

    private void CastSpell()
    {
        PlayClip(audioSpell);
        List<Word> wordsCopy = new List<Word>(words);
        foreach (Word word in wordsCopy)
        {
            if (word is GoodieWord)
            {
                AddEffect(word, attackBurn, Color.blue);
                words.Remove(word);
                word.Kill();
                KillGoodie((GoodieWord)word);
            }
        }
    }

    private void SetBaddie(BaddieWord baddie)
    {
        if (activeBaddie != null)
        {
            activeBaddie.SetOn(false);
        }

        activeBaddie = baddie;

        if (activeBaddie != null)
        {
            activeBaddie.SetOn(true);
        }
    }

    private void KillGoodie(GoodieWord word)
    {
        if (UnityEngine.Random.Range(streak, 100) > treasureChance)
        {
            AddTreasure(word.display.transform.position);
        }
    }

    public void TypeLetter(char letter)
    {
        if (activeWord == null)
        {
            // TODO: optimize (ordered? hash?)
            foreach(Word word in words)
            {
                if (word.IsActive() && word.GetNextLetter() == letter)
                {
                    PlayClip(audioType);
                    activeWord = word;
                    word.TypeLetter();
                    break;
                } 
            }
        }
        else
        {
            if (activeWord.GetNextLetter() == letter)
            {
                activeWord.TypeLetter();

                if (activeWord.WordTyped(activeBaddie))
                {
                    if (activeWord is BaddieWord)
                    {
                        AddStreak();
                        PlayClip(audioMonster);
                        SetBaddie((BaddieWord)activeWord);
                    }
                    else if (activeWord is SpellWord)
                    {
                        CastSpell();
                        activeWord.SetWord(GetRandomWord(phrases, false));
                    }
                    else if (activeWord is TreasureWord)
                    {
                        if (activeWord.characterClass == Word.CharacterClass.TreasureSword)
                        {
                            if (!showedSwordPickupTip)
                            {
                                showedSwordPickupTip = true;
                                AddTip("Collect more Swords to unlock the Curse", new Vector3(3.5f, -4.5f, 0));
                            }
                            PlayClip(audioSword);
                            if (activeSpell == null)
                            {
                                AddSpell();
                            }
                            else
                            {
                                activeSpell.AddCharge();
                                if (activeSpell.IsActive() && !showedCurseTip)
                                {
                                    showedCurseTip = true;
                                    AddTip("Type your Curse Sword phrase to kill everything!", new Vector3(3.5f, -4.5f, 0));
                                }
                            }
                        }
                        else if (activeWord.gold > 0)
                        {
                            PlayClip(audioTreasure);
                            AddChestTreasure(activeWord.gold);
                            gold += activeWord.gold;
                        }
                        words.Remove(activeWord);
                    }
                    else if (activeWord is GoodieWord)
                    {
                        if (activeBaddie != null)
                        {
                            // moster attack
                            PlayClip(audioHit2);
                            AddEffect(activeWord, attackSlash3, Color.red);
                            AddEffect(activeWord, activeBaddie.display.spriteRenderer.sprite, Color.white, new Vector3(-1f, 0, 0));
                            activeBaddie.SetWord(GetRandomWord(curses, false));
                            activeBaddie.Reset();
                            SetBaddie(null);
                        }
                        if (activeWord.WordDead())
                        {
                            AddStreak();
                            kills++;
                            words.Remove(activeWord);
                            if (activeWord is GoodieWord)
                            {
                                PlayClip(audioDie);
                                AddEffect(activeWord, attackKill, Color.red);
                                KillGoodie((GoodieWord)activeWord);
                            }
                        }
                        else
                        {
                            // hit but no kill
                            PlayClip(audioHit1);
                            AddEffect(activeWord, attackSlash1, Color.red);

                            string theWord = GetRandomWord(cleans, true);
                            if (theWord != null)
                            {
                                activeWord.SetWord(theWord);
                            } else {
                                AddStreak();
                                kills++;
                                words.Remove(activeWord);
                                if (activeWord is GoodieWord)
                                {
                                    PlayClip(audioDie);
                                    AddEffect(activeWord, attackKill, Color.red);
                                    KillGoodie((GoodieWord)activeWord);
                                }
                            }
                        }
                    }
                    activeWord = null;
                }
                else
                {
                    PlayClip(audioType);
                }
            }
            else
            {
                // Typo
                PlayClip(audioError);
                activeWord.Reset();
                activeWord = null;

                if (activeBaddie != null)
                {
                    activeBaddie.SetWord(GetRandomWord(curses, false));
                    activeBaddie.Reset();
                    SetBaddie(null);
                }
                streak = 0;
            }
        }
    }
}
