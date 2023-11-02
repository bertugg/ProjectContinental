using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public PlayerInteraction interactor;

    public string interactionMessage
    {
        get => _interactionMessage;
    } 
    
    [SerializeField] private string _interactionMessage;
    [SerializeField] private UnityEvent interaction;

    public void Interact(PlayerInteraction interactor)
    {
        this.interactor = interactor;
        interaction.Invoke();
    }
}
