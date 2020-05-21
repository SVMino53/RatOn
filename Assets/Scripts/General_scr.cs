using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_scr : MonoBehaviour
{
    ControllerInput controls;

    bool start;
    bool impulseStart = true;


    public enum GameState
    {
        START_MENU,
        GAME,
        PAUSE_MENU
    }

    public GameState curGameState;

    public List<GameObject> gameObjectsActivate;
    public List<GameObject> pauseObjectsActivate;
    public List<GameObject> gameObjectsEnable;
    public List<GameObject> pauseObjectsEnable;


    private void Awake()
    {
        controls = new ControllerInput();

        controls.Gameplay.Menu.performed += ctx => start = true;
        controls.Gameplay.Menu.canceled += ctx => start = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start && impulseStart || Input.GetKeyDown(KeyCode.Escape))
        {
            switch (curGameState)
            {
                case GameState.START_MENU:

                    Application.Quit();
                    break;

                case GameState.GAME:

                    for (int i = 0; i < gameObjectsActivate.Count; i++)
                    {
                        gameObjectsActivate[i].SetActive(false);
                    }
                    for (int i = 0; i < pauseObjectsActivate.Count; i++)
                    {
                        pauseObjectsActivate[i].SetActive(true);
                    }
                    for (int i = 0; i < gameObjectsEnable.Count; i++)
                    {
                        gameObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
                    }
                    for (int i = 0; i < pauseObjectsEnable.Count; i++)
                    {
                        pauseObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = true;
                    }

                    curGameState = GameState.PAUSE_MENU;

                    break;

                case GameState.PAUSE_MENU:

                    for (int i = 0; i < pauseObjectsActivate.Count; i++)
                    {
                        pauseObjectsActivate[i].SetActive(false);
                    }
                    for (int i = 0; i < gameObjectsActivate.Count; i++)
                    {
                        gameObjectsActivate[i].SetActive(true);
                    }
                    for (int i = 0; i < pauseObjectsEnable.Count; i++)
                    {
                        pauseObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
                    }
                    for (int i = 0; i < gameObjectsEnable.Count; i++)
                    {
                        gameObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = true;
                    }

                    curGameState = GameState.GAME;

                    break;
            }

            impulseStart = false;
        }
        else if (!start && !impulseStart)
        {
            impulseStart = true;
        }
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
