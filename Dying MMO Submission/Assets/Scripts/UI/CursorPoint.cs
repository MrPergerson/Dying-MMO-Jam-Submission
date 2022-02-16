using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CursorPoint : MonoBehaviour
{
    PlayerControls controls;
    GameObject icon;
    private float iconDisplayTime = .5f;
    private IEnumerator hideCursorIcon;
    private Tween iconAnimation;

    private void Awake()
    {
        controls = new PlayerControls();
        icon = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        icon.SetActive(false);
        controls.Main.CursorPrimaryClick.performed += DisplayIconOnGround;
        hideCursorIcon = HideCursorIcon(iconDisplayTime);

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void DisplayIconOnGround(InputAction.CallbackContext context)
    {

        if (icon.activeInHierarchy)
        {
            StopCoroutine(hideCursorIcon);
            DOTween.Complete(iconAnimation);
        }

        // can I get this from the player code some how? 
        var mousePosition = controls.Main.CursorPosition.ReadValue<Vector2>();
        var ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }

        icon.SetActive(true);

        transform.localScale = new Vector3(2f, 2f, 2f); // this is a little finicky
        iconAnimation = transform.DOScale(new Vector3(1, 1, 1), iconDisplayTime);

        hideCursorIcon = HideCursorIcon(iconDisplayTime); // for some reason, this is needed here or icon will never go away
        StartCoroutine(hideCursorIcon);
    }

    IEnumerator HideCursorIcon(float time)
    {

        yield return new WaitForSeconds(time);
        icon.SetActive(false);
    }
}
