using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    public GameObject wordPrefab;
    public Transform wordCanvas;

    public WordDisplay SpawnTreasure(Vector3 position)
    {
        GameObject wordObject = Instantiate(wordPrefab, position, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObject.GetComponent<WordDisplay>();

        return wordDisplay;
    }

    public WordDisplay SpawnGoodie()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 6f);

        GameObject wordObject = Instantiate(wordPrefab, randomPosition, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObject.GetComponent<WordDisplay>();

        return wordDisplay;
    }

    public WordDisplay SpawnBaddie()
    {
        Vector3 randomPosition = new Vector3(-3.26f, -3.1f);

        GameObject wordObject = Instantiate(wordPrefab, randomPosition, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObject.GetComponent<WordDisplay>();

        return wordDisplay;
    }

    public WordDisplay SpawnSpell()
    {
        Vector3 randomPosition = new Vector3(3.8f, -3f);

        GameObject wordObject = Instantiate(wordPrefab, randomPosition, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObject.GetComponent<WordDisplay>();

        return wordDisplay;
    }
}
