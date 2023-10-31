using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject gameoverPanel;

    private void Awake() => GameManager.Instance.ui = this;

    public void ShowGameOverPanel() => gameoverPanel.SetActive(true);
}