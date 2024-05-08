using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialScreen1;
    public GameObject TutorialScreen2;
    public GameObject TutorialScreen3;
    public GameObject TutorialScreen4;
    public XRIDefaultInputActions customInputs;
    private InputAction menuButton;

    private void Awake() {
        customInputs = new XRIDefaultInputActions();
        menuButton = customInputs.Custom.Menu;
    }

    private void OnEnable() {
        customInputs.Enable();
        menuButton.Enable();
        menuButton.performed += Menu;

    }

    private void OnDisable() {
        customInputs.Disable();
        menuButton.Disable();
    }

    public void GoToTutorialScreen2()
    {
        TutorialScreen1.SetActive(false);
        TutorialScreen2.SetActive(true);
    }
    public void GoToTutorialScreen3()
    {
        TutorialScreen2.SetActive(false);
        TutorialScreen3.SetActive(true);
    }
    public void GoToTutorialScreen4()
    {
        TutorialScreen3.SetActive(false);
        TutorialScreen4.SetActive(true);
    }

    private void Menu(InputAction.CallbackContext callbackContext)
    {
    }
}
