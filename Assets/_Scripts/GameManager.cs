using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameStates
    {
        STARTED,
        PAUSED,
        WON,
        LOST
    }

    public UIManager ui;
    public FirstPersonController player;
    public StarterAssetsInputs inputs;
    public CameraManager mainCamera;
    private GameStates _currentState;

    public GameStates CurrentState
    {
        get => _currentState;
        set
        {
            if(_currentState == value)
                return;
            
            _currentState = value;
            switch (_currentState)
            {
                case GameStates.LOST:
                    ui.ShowGameOverPanel();
                    break;
                default:
                    break;
            }
        }
    }

    void Update()
    {
        if (CurrentState == GameStates.LOST)
        {
            if (inputs.jump)
            {
                RestartStage();
            }
        }
    }

    void RestartStage()
    {
        DissolveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart
        CurrentState = GameStates.STARTED;
    }
    
    void DissolveGame()
    {
        Destroy(player.gameObject);
    }
}
