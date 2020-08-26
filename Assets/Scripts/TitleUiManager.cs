using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUiManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void ToggleSound(bool sound)
    {
        WordManager.sound = sound;
    }

    public void ToggleNsfw(bool nsfw)
    {
        WordManager.nsfw = nsfw;
    }
}
