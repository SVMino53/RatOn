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
    bool left;
    bool right;
    bool select;


    public General_scr generalScr;

    public List<GameObject> menuOptions;
    public int rows;
    public int columns;
    public Vector3 selectScale;
    public Color selectButtonColor;
    public Color selectTextColor;

    public List<MenuActions_scr.Actions> actions;

    public float nextOptionDelay = 0.5f;
    float nextOptionTime = 0.0f;

    public List<GameObject> activateObjs;
    public List<GameObject> deactivateObjs;
    public List<GameObject> enableObjs;
    public List<GameObject> disableObjs;

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

        controls.Gameplay.Left.performed += ctx => left = true;
        controls.Gameplay.Left.canceled += ctx => left = false;

        controls.Gameplay.Right.performed += ctx => right  = true;
        controls.Gameplay.Right.canceled += ctx => right  = false;

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

        SelectButton(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (up || Input.GetKey(KeyCode.UpArrow))
        {
            if (nextOptionTime <= 0.0f)
            {
                DeselectButton(index);

                index -= columns;
                if (index < 0)
                {
                    index = menuOptions.Count + index;
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

                index += columns;
                if (index >= menuOptions.Count)
                {
                    index %= columns;
                }

                SelectButton(index);

                nextOptionTime = nextOptionDelay;
            }

            nextOptionTime -= Time.deltaTime;
        }
        else if (left || Input.GetKey(KeyCode.LeftArrow))
        {
            if (nextOptionTime <= 0.0f)
            {
                DeselectButton(index);

                index -= 1;
                if (index % columns == columns - 1 || index % columns == -1)
                {
                    index += columns;
                }

                SelectButton(index);

                nextOptionTime = nextOptionDelay;
            }

            nextOptionTime -= Time.deltaTime;
        }
        else if (right || Input.GetKey(KeyCode.RightArrow))
        {
            if (nextOptionTime <= 0.0f)
            {
                DeselectButton(index);

                index += 1;
                if (index % columns == 0)
                {
                    index -= columns;
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

        if (select || Input.GetKeyDown(KeyCode.Return))
        {
            if (actions[index] == MenuActions_scr.Actions.ContinueGameFromPause)
            {
                MenuActions_scr.DoAction(actions[index], activateObjs, deactivateObjs, enableObjs, disableObjs, generalScr);
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
