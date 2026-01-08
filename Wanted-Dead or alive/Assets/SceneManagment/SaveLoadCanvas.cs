using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SaveLoadCanvas : MonoBehaviour
{
    [SerializeField] private MouseLook mouseLook;

    public GameObject SaveCanvas;

    public GameObject LoadCanvas;

    public GameObject ExitCanvas;

    private bool isSaveCanvasActive = false;

    private bool isLoadCanvasActive = false;

    private bool isExitCanvasActive = false;
    void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            ToggleSaveCanvas();
            Invoke(nameof(ToggleSaveCanvas), 1.5f);
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            ToggleLoadCanvas();
            Invoke(nameof(ToggleLoadCanvas), 1.5f);
        }
        if (Keyboard.current.f4Key.wasPressedThisFrame)
        {
            ToggleExitCanvas();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseLook.canMove = false;
        }
    }

    public void ToggleSaveCanvas()
    {
        isSaveCanvasActive = !isSaveCanvasActive;
        SaveCanvas.SetActive(isSaveCanvasActive);
    }
    public void ToggleLoadCanvas()
    {
        isLoadCanvasActive = !isLoadCanvasActive;
        LoadCanvas.SetActive(isLoadCanvasActive);
    }
    public void ToggleExitCanvas()
    {
        isExitCanvasActive = !isExitCanvasActive;
        ExitCanvas.SetActive(isExitCanvasActive);
    }
    public void ContinueGame()
    {
        isExitCanvasActive = isExitCanvasActive;
        ExitCanvas.SetActive(!isExitCanvasActive);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseLook.canMove = true;
    }
    public void MenuChange()
    {
        SceneManager.LoadScene("Menu Scene");
        Cursor.visible = true;
        mouseLook.canMove = false;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
