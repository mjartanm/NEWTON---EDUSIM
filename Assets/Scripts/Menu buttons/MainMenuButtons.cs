
﻿using Assets.Scripts.Utils;
using Assets.Scripts.Hotkeys;
using System;
using UnityEngine;
using System.Collections;
 using System.IO;
 using Assets.Scripts.Localization;
 using ClassLibrarySharpCircuit;


public class MainMenuButtons : MonoBehaviour
{
    private const string _manualHotkey = "Manual";

    // Handle hotkeys
    public void Update()
    {
        bool check = HotkeyManager.Instance.CheckHotkey(_manualHotkey, KeyAction.Down);
        if (HotkeyManager.Instance.CheckHotkey(_manualHotkey, KeyAction.Down))
        {
            OpenManual();
        }
    }

    void Start()
    {
        //Initialization, program is paused
        PlayPauseButton("pause");
        GameObject toolbox = GameObject.Find("ToolboxButton");
        GameObject properties = GameObject.Find("PropertiesButton");
        GameObject objectExplorer = GameObject.Find("ObjectExplorerButton");
        GameObject debug = GameObject.Find("DebugButton");
        toolbox.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        properties.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        //objectExplorer.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        debug.GetComponent<UnityEngine.UI.Image>().color = Color.white;

        // Scroll up to first row
        GameObject aboutViewport = GameObject.Find("AboutViewport");
        aboutViewport.GetComponent<UnityEngine.UI.ScrollRect>().velocity = new Vector2(0f, -10000f);
    }

    // Function to handle play and pause buttons
    public void PlayPauseButton(string action)
    {
        // Find all components for play/pause
        GameObject playButton = GameObject.Find("PlayButton");
        GameObject pauseButton = GameObject.Find("PauseButton");
        GameObject checkboxPlay = GameObject.Find("PlayToggle");
        GameObject checkboxPause = GameObject.Find("PauseToggle");
        GameObject menuPlayButton = GameObject.Find("MenuPlayButton");
        GameObject menuPauseButton = GameObject.Find("MenuPauseButton");

        // Set GUI components for play/pause
        if (action == "play")
        {
            checkboxPlay.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
            checkboxPause.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            menuPlayButton.GetComponent<UnityEngine.UI.Image>().color = new Color32(200, 200, 200, 255);
            menuPauseButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            playButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            pauseButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        else
        {
            checkboxPlay.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            checkboxPause.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
            menuPlayButton.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            menuPauseButton.GetComponent<UnityEngine.UI.Image>().color = new Color32(200, 200, 200, 255);
            playButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            pauseButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
    }

    // Show or hide toolbox panel and mark menu button
    public void ShowToolbox(GameObject guiComponent)
    {
        GameObject checkbox = GameObject.Find("ToolboxToggle");
        GameObject button = GameObject.Find("ToolboxButton");

        if (guiComponent.activeSelf == false)
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
            button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            button.GetComponent<UnityEngine.UI.Image>().color = new Color32(200, 200, 200, 255);
        }

        ShowPanel(guiComponent);
    }

    // Show or hide properties panel and mark menu button
    public void ShowProperties(GameObject guiComponent)
    {
        GameObject checkbox = GameObject.Find("PropertiesToggle");
        GameObject button = GameObject.Find("PropertiesButton");

        if (guiComponent.activeSelf == false)
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
            button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            button.GetComponent<UnityEngine.UI.Image>().color = new Color32(200, 200, 200, 255);
        }

        ShowPanel(guiComponent);
    }

    // Show or hide object explorer panel and mark menu button
    public void ShowObjectExplorer(GameObject guiComponent)
    {
        GameObject checkbox = GameObject.Find("ObjectExplorerToggle");
        GameObject button = GameObject.Find("ObjectExplorerButton");

        if (guiComponent.activeSelf == false)
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
            button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            button.GetComponent<UnityEngine.UI.Image>().color = new Color32(200, 200, 200, 255);
        }

        ShowPanel(guiComponent);
    }

    // Show or hide debug log panel and mark menu button
    public void ShowDebug(GameObject guiComponent)
    {
        GameObject checkbox = GameObject.Find("DebugToggle");
        GameObject button = GameObject.Find("DebugButton");

        if (guiComponent.activeSelf == false)
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
            button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else
        {
            checkbox.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
            button.GetComponent<UnityEngine.UI.Image>().color = new Color32(200, 200, 200, 255);
        }

        ShowPanel(guiComponent);
    }

    // Hide or show panel
    public void ShowPanel(GameObject guiComponent)
    {
        if (guiComponent.activeSelf == false)
        {
            guiComponent.SetActive(true);
        }
        else
        {
            guiComponent.SetActive(false);
        }
    }

    public void NewProject()
    {
        FindObjectOfType<Persistance>().NewProject();
    }

    public void OpenExample(int exampleNumber)
    {
        string file = "ExampleCircuits/example" + exampleNumber + ".es";
        FindObjectOfType<Persistance>().Load(Path.Combine(Application.streamingAssetsPath, file));
        GameObject.Find("ExampleCircuitCanvas").GetComponent<Canvas>().enabled = false;
    }

    // Open basic explorer after buttonClick to Open project
    public void OpenProject()
    {
        FileBrowserHandler.Instance.LoadFile();
    }

    // Open basic explorer after buttonClick to Save project
    public void SaveProject()
    {
        FileBrowserHandler.Instance.SaveFile();
    }

    // Open basic explorer after buttonClick to Save As project
    public void SaveAsProject()
    {
        FileBrowserHandler.Instance.SaveAsFile();
    }

    // Open basic explorer after buttonClick to Export icon project
    public void SaveExport()
    {
        FileBrowserHandler.Instance.SaveExport();
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Show or hide given canvas
    public void ShowCanvas(GameObject guiComponent)
    {
        if (guiComponent.GetComponent<Canvas>().enabled == false)
        {
            guiComponent.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            guiComponent.GetComponent<Canvas>().enabled = false;
        }
    }

    // Circuit error 1
    public static void CircuitError(ICircuitElement element)
    {
        Whisp whisp = FindObjectOfType<Whisp>();            // tuna je ta chyba niekde,, bez tohto nevyhadzuje nikde chybu
        /*string path = element.ToString();
        int pos = path.LastIndexOf(".", StringComparison.Ordinal) + 1;
        string componentName = path.Substring(pos, path.Length - pos);

        string toFindInRes = path.Substring(pos, path.Length - pos);
        string resComponentName = ResourceReader.Instance.GetResource("ComponentText" + componentName);
        string resCircuitErrorMsg = ResourceReader.Instance.GetResource("CircuitErrorMSG1");
        resCircuitErrorMsg = resCircuitErrorMsg.Replace("{COMPONENTNAME}", resComponentName);

        string windowName = "ERRORMSG_" + componentName;

        if (GameObject.Find(windowName))
        {
            bool currentlyEnabled = GameObject.Find(windowName).GetComponent<Canvas>().enabled;
            if (currentlyEnabled == false)
            {
                currentlyEnabled = true;
                GameObject.Find(windowName).GetComponent<Canvas>().enabled = true;
            }
            else
            {
                currentlyEnabled = false;
                GameObject.Find(windowName).GetComponent<Canvas>().enabled = false;
            }
        }
        else
        {
            whisp.Say(ResourceReader.Instance.GetResource("CircuitErrorMissingErrorBox") + "{" + resComponentName + "}");
        }
        whisp.Say(resCircuitErrorMsg);*/
    }

    // Open the HTML Manual
    public void OpenManual()
    {
        string manualPath = "manual/index.html";
        Application.OpenURL(System.IO.Path.Combine(Application.streamingAssetsPath, manualPath));
    }
}