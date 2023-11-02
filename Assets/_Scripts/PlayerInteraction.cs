using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public bool isInteracting;
    
    private Interactable _interaction;
    private UIManager _ui;

    private void Start()
    {
        _ui = GameManager.Instance.ui;
    }

    private void Update()
    {
        if(isInteracting)
            return;
        
        // Show interaction on UI
        _interaction = GetInteractableObject();
        if (_interaction != null)
            _ui.ShowInteractionMessage(_interaction.interactionMessage);
        else
            _ui.HideInteractionMessage();
    }

    public Interactable GetInteractableObject()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (var collided in colliderArray)
        {
            if (collided.TryGetComponent(out Interactable interactable))
            {
                return interactable;
            }
        }

        return null;
    }

    public void Interact()
    {
        if (!isInteracting && _interaction)
        {
            _interaction.Interact(this);
        }
    }
}
