using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionMessage;
    

    private void Awake() => GameManager.Instance.ui = this;

    public void ShowGameOverPanel() => gameoverPanel.SetActive(true);

    public void ShowInteractionMessage(string message = "Interact")
    {
        interactionMessage.text = message;
        interactionPanel.SetActive(true);
    }

    public void HideInteractionMessage()
    {
        interactionPanel.SetActive(false);
    }
}