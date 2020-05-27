using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_scr : MonoBehaviour
{
    ControllerInput controls;

    bool start;
    bool impulseStart = true;
    public bool changeState = false;


    public enum GameState
    {
        START_MENU,
        GAME,
        PAUSE_MENU,
        GAME_OVER
    }

    public GameState curGameState;

    public List<GameObject> gameObjectsActivate;
    public List<GameObject> pauseObjectsActivate;
    public List<GameObject> gameObjectsEnable;
    public List<GameObject> pauseObjectsEnable;

    public List<GameObject> gameoverObjectsActivate;
    public List<GameObject> gameoverObjectsEnable;


    private void Awake()
    {
        controls = new ControllerInput();

        controls.Gameplay.Menu.performed += ctx => start = true;
        controls.Gameplay.Menu.canceled += ctx => start = false;
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
                    curGameState = GameState.PAUSE_MENU;
                    changeState = true;
                    break;

                case GameState.PAUSE_MENU:
                    curGameState = GameState.GAME;
                    changeState = true;
                    break;
            }

            impulseStart = false;
        }
        else if (!start && !impulseStart)
        {
            impulseStart = true;
        }

        if (curGameState == GameState.GAME_OVER && changeState)
        {
            for (int i = 0; i < gameObjectsActivate.Count; i++)
            {
                gameObjectsActivate[i].SetActive(false);
            }
            for (int i = 0; i < gameoverObjectsActivate.Count; i++)
            {
                gameoverObjectsActivate[i].SetActive(true);
            }
            for (int i = 0; i < gameObjectsEnable.Count; i++)
            {
                gameObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
            }
            for (int i = 0; i < gameoverObjectsEnable.Count; i++)
            {
                gameoverObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = true;
            }
            changeState = false;
        }
        else if (curGameState == GameState.GAME && changeState)
        {
            for (int i = 0; i < gameObjectsActivate.Count; i++)
            {
                gameObjectsActivate[i].SetActive(true);
            }
            for (int i = 0; i < pauseObjectsActivate.Count; i++)
            {
                pauseObjectsActivate[i].SetActive(false);
            }
            for (int i = 0; i < gameoverObjectsActivate.Count; i++)
            {
                gameoverObjectsActivate[i].SetActive(false);
            }
            for (int i = 0; i < gameObjectsEnable.Count; i++)
            {
                gameObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = true;
            }
            for (int i = 0; i < pauseObjectsEnable.Count; i++)
            {
                pauseObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
            }
            for (int i = 0; i < gameoverObjectsEnable.Count; i++)
            {
                gameoverObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
            }
            changeState = false;
        }
        else if (curGameState == GameState.PAUSE_MENU && changeState)
        {
            for (int i = 0; i < gameObjectsActivate.Count; i++)
            {
                gameObjectsActivate[i].SetActive(false);
            }
            for (int i = 0; i < pauseObjectsActivate.Count; i++)
            {
                pauseObjectsActivate[i].SetActive(true);
            }
            for (int i = 0; i < gameoverObjectsActivate.Count; i++)
            {
                gameoverObjectsActivate[i].SetActive(false);
            }
            for (int i = 0; i < gameObjectsEnable.Count; i++)
            {
                gameObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
            }
            for (int i = 0; i < pauseObjectsEnable.Count; i++)
            {
                pauseObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = true;
            }
            for (int i = 0; i < gameoverObjectsEnable.Count; i++)
            {
                gameoverObjectsEnable[i].GetComponent<MonoBehaviour>().enabled = false;
            }
            changeState = false;
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
