using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    UIDocument uiDocument;

    private Button newGameButton;
    private Button loadGameButton;
    private Button settingsButton;
    private Button quitButton;

    private void OnEnable()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        uiDocument = GetComponent<UIDocument>();

        var root = uiDocument.rootVisualElement;

        newGameButton = root.Q<Button>("ButtonNewGame");
        loadGameButton = root.Q<Button>("ButtonLoadGame");
        settingsButton = root.Q<Button>("ButtonSettings");
        quitButton = root.Q<Button>("ButtonQuit");

        newGameButton.clicked += OnNewGameClicked;
        loadGameButton.clicked += OnLoadGameClicked;
        settingsButton.clicked += OnSettingsClicked;
        quitButton.clicked += OnQuitClicked;
    }

    private void OnNewGameClicked()
    {
        throw new NotImplementedException();
    }

    private void OnLoadGameClicked()
    {
        gameManager.StartGame();
        uiDocument.enabled = false;
    }

    private void OnSettingsClicked()
    {
        throw new NotImplementedException();
    }

    private void OnQuitClicked()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
