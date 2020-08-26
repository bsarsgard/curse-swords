using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillScreenUiManager : MonoBehaviour
{
    public static int kills = 0;
    public static int treasure = 0;
    public static int streak = 0;

    public Text killsLabel;
    public Text treasureLabel;
    public Text streakLabel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        killsLabel.text = kills.ToString();
        treasureLabel.text = treasure.ToString();
        streakLabel.text = streak.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
