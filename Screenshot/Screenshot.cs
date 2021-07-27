using System;
using System.IO;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif


/// <summary>
/// Simple drag and drop screenshot script that will work with new or old input system.
/// </summary>
public class Screenshot : MonoBehaviour
{
    [SerializeField] string path;
    [SerializeField, Range(1, 4)] int size = 1;
    [SerializeField] int maxImagesWithSameName = 10;
#if ENABLE_INPUT_SYSTEM 
    [SerializeField]
    InputAction action = new InputAction("Take Screenshot", InputActionType.Button, "<Keyboard>/F8");
#else
    [SerializeField] KeyCode key = KeyCode.F8;
#endif

    void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            path = Application.dataPath + "/Screenshots/";
        }
    }

    private void TakeScreenshot()
    {
        if (!Directory.Exists(path))
        {
            Debug.Log($"<color=red>{path} does NOT. Could not take screenshot.</color>");
            return;
        }

        var imageName = $"{DateTime.Now.ToString("MMddyyyy-hhmmsstt")}_";
        for (int i = 0; i < maxImagesWithSameName; i++)
        {
            var filePath = Path.Combine(path, $"{imageName}{i + 1:00}.png");
            if (!File.Exists(filePath))
            {
                Debug.Log($"<color=green>Screenshot captured to {filePath}</color>");
                ScreenCapture.CaptureScreenshot(filePath, size);
                return;
            }
        }

        Debug.Log($"<color=red>Could not take screenshot! ({imageName}xx.png) Already {maxImagesWithSameName} images with the same name!</color>");
    }


#if ENABLE_INPUT_SYSTEM
    void OnEnable()
    {
        action.performed += TakeScreenshot;
        action.Enable();
    }

    void OnDisable()
    {
        action.performed -= TakeScreenshot;
        action.Disable();
    }

    private void TakeScreenshot(InputAction.CallbackContext obj)
    {
        TakeScreenshot();
    }

#else

    private void Update()
    {
        if (Input.GetKeyDown(key))
            TakeScreenshot();
    }

#endif

}
