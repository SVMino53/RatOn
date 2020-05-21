using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuUI_scr : MonoBehaviour
{
    ControllerInput controls;

    bool up;
    bool down;
    bool select;


    public List<GameObject> menuOptions;
    public Vector3 selectScale;
    public Color selectButtonColor;
    public Color selectTextColor;

    public List<MenuActions_scr.Actions> actions;

    public float nextOptionDelay = 0.5f;
    float nextOptionTime = 0.0f;

    public List<GameObject> activateObjs;
    public List<GameObject> deactivateObjs;

    Vector3 mainScale;
    Color mainButtonColor;
    Color mainTextColor;

    int index = 0;


    void SelectButton(int i)
    {
        menuOptions[i].transform.localScale = selectScale;
        menuOptions[i].GetComponent<Image>().color = selectButtonColor;
        menuOptions[i].GetComponentInChildren<Text>().color = selectTextColor;
    }

    void DeselectButton(int i)
    {
        menuOptions[i].transform.localScale = mainScale;
        menuOptions[i].GetComponent<Image>().color = mainButtonColor;
        menuOptions[i].GetComponentInChildren<Text>().color = mainTextColor;
    }

    void Awake()
    {
        controls = new ControllerInput();

        controls.Gameplay.Up.performed += ctx => up = true;
        controls.Gameplay.Up.canceled += ctx => up = false;

        controls.Gameplay.Down.performed += ctx => down  = true;
        controls.Gameplay.Down.canceled += ctx => down  = false;

        controls.Gameplay.Select.performed += ctx => select = true;
        controls.Gameplay.Select.canceled += ctx => select = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject tempObj = menuOptions[0];

        mainScale = tempObj.transform.localScale;
        mainButtonColor = tempObj.GetComponent<Image>().color;
        mainTextColor = tempObj.GetComponentInChildren<Text>().color;

        SelectButton(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (up || Input.GetKey(KeyCode.UpArrow))
        {
            if (nextOptionTime <= 0.0f)
            {
                DeselectButton(index);

                index--;
                if (index < 0)
                {
                    index = menuOptions.Count - 1;
                }

                SelectButton(index);

                nextOptionTime = nextOptionDelay;
            }

            nextOptionTime -= Time.deltaTime;
        }
        else if (down || Input.GetKey(KeyCode.DownArrow))
        {
            if (nextOptionTime <= 0.0f)
            {
                DeselectButton(index);

                index++;
                if (index >= menuOptions.Count)
                {
                    index = 0;
                }

                SelectButton(index);

                nextOptionTime = nextOptionDelay;
            }

            nextOptionTime -= Time.deltaTime;
        }
        else
        {
            nextOptionTime = 0.0f;
        }

        if (select)
        {
            if (index == 0)
            {
                MenuActions_scr.DoAction(actions[index], activateObjs, deactivateObjs);
            }
            else
            {
                MenuActions_scr.DoAction(actions[index]);
            }
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
