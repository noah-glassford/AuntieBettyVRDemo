using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InteractionInstigator : MonoBehaviour
{
    private List<Interactable> m_NearbyInteractables = new List<Interactable>();
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


    public bool HasNearbyInteractables()
    {
        return m_NearbyInteractables.Count != 0;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) { 

        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            m_NearbyInteractables.Add(interactable);
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            m_NearbyInteractables.Remove(interactable);
        }
    }

     private void Menu(InputAction.CallbackContext callbackContext)
    {
           
         if (HasNearbyInteractables())
        {
            Debug.Log("tab");
            //Ideally, we'd want to find the best possible interaction (ex: by distance & orientation).
            m_NearbyInteractables[0].DoInteraction();
        }
    }

}