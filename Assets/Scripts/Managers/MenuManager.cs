using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class MenuManager : MonoBehaviour
{
    private Animator animator;

    private bool saveFileExists;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private Button  continueButton,
                    newButton,
                    loadButton;

//    private SaveFile saveFile;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // check for save files
        saveFileExists = true;
    }

    public void ActivateContinueAndLoadButtons()
    {
        continueButton.interactable = saveFileExists;
        loadButton.interactable = saveFileExists;

        if (saveFileExists)
            eventSystem.SetSelectedGameObject(continueButton.gameObject);
        else
            eventSystem.SetSelectedGameObject(newButton.gameObject);
    }

    public void QuitPressed()
    {
        animator.SetTrigger("Quit");
    }
    private void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    public void BackPressed()
    {
        animator.SetTrigger("Back");
    }

    public void SettingsPressed()
    {
        animator.SetTrigger("Settings");
    }

    public void CreditsPressed()
    {
        animator.SetTrigger("Credits");
    }

    public void NewPressed()
    {
        //  saveFile = SaveFile.New();
        Open();
    }

    public void LoadPressed()
    {
        animator.SetTrigger("Load");
    }

    public void Open()
    {
        animator.SetTrigger("Start");
    }

    private void StartGame()
    {

    }

    [MenuItem("Player Prefs/Clear")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
