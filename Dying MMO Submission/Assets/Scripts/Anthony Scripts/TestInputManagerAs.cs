using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputManagerAs : MonoBehaviour
{
    private static TestInputManagerAs instance;

    private PlayerControls playerControls;

    private bool canClick;

    [Header("JSON")]
    [SerializeField] private TextAsset inkJSON;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError(this.gameObject + " Awake: More than one Test Input Manager detected");
        }
        instance = this;

        playerControls = new PlayerControls();
        playerControls.Main.Enable();
    }

    private void Start()
    {
        //playerControls.Main.CursorPrimaryClick.started += DialogueManagerAs.GetInstance().ContinueStory;
        //playerControls.Main.CursorPrimaryClick.performed += DialogueManagerAs.GetInstance().ContinueStory;
        //playerControls.Main.CursorPrimaryClick.canceled += DialogueManagerAs.GetInstance().ContinueStory;

        canClick = true;
    }

    private void Update()
    {
        if (canClick)
        {
            if (inkJSON != null && DialogueManagerAs.GetInstance() != null)
            {
                canClick = false;
                DialogueManagerAs.GetInstance().EnterDialogueMode(inkJSON);
            }
            else
            {
                Debug.LogError(this.gameObject + " Start: ink JSON file or Dialogue Manager is null");
            }
        }
    }
}
