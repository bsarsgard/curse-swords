using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsUiManager : MonoBehaviour
{
    public SpriteRenderer screenRenderer;
    public Sprite screen1;
    public Sprite screen2;
    public Sprite screen3;
    public Sprite screen4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.inputString.Length > 0 || Input.GetMouseButtonUp(0))
        {
            if (screenRenderer.sprite == screen1)
            {
                screenRenderer.sprite = screen2;
            }
            else if (screenRenderer.sprite == screen2)
            {
                screenRenderer.sprite = screen3;
            }
            else if (screenRenderer.sprite == screen3)
            {
                screenRenderer.sprite = screen4;
            } else
            {
                SceneManager.LoadScene("TitleScreen");
            }
        }
        
    }
}
