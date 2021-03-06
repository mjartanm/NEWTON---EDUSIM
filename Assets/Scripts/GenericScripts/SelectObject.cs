﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SelectObject : MonoBehaviour, IPointerClickHandler
{   
    // Global variable to allow only one selected item.
    public GameObject SelectionBox;
    public static List<GameObject> SelectedObjects = new List<GameObject>();
    public static List<GameObject> SelectedLines = new List<GameObject>();
    private EditObjectProperties _script;

    //util class for work with toolbar buttons 
    private ToolbarButtonUtils _tbu = new ToolbarButtonUtils();

    // Initialization: Making object and SelectionBox the same size.
    void Start ()
    {
        SelectionBox.transform.position = this.transform.position;
        SelectionBox.transform.localScale = this.transform.localScale;
        GameObject propertiesContainer = GameObject.Find("PropertiesWindowContainer");
        _script = propertiesContainer.GetComponent<EditObjectProperties>();
    }

    // Add a new item to the selection
    public static void AddItemToSelection(GameObject go)
    {
        SelectedObjects.Add(go);
        go.transform.FindChild("SelectionBox").GetComponent<SpriteRenderer>().enabled = true;

        // Call the script from component that fills the Properties Window
        if (SelectedObjects.Count == 1)
        {
            GUICircuitComponent componentScript = SelectedObjects[0].GetComponent<GUICircuitComponent>();
            componentScript.GetProperties();
        }
    }

    // Add a list of new items to the selection
    public static void AddItemsToSelection(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            SelectedObjects.Add(gameObject);
            gameObject.transform.FindChild("SelectionBox").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // Add a list of new lines to the selection
    public static void AddLinesToSelection(List<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            SelectedLines.Add(gameObject);
            gameObject.GetComponent<Line>().MarkAsSelected();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle || this.gameObject.tag == "ToolboxItem" || this.gameObject.tag == "Node" || this.gameObject.tag == "ToolboxItemActive" || this.gameObject.tag == "Untagged")
        {
            return;
        }

        // Deselect selected item first.
        if (!SelectedObjects.Contains(this.gameObject))
        {
            DeselectObject();
        }

        // Deselect line
        if (!SelectedObjects.Contains(this.gameObject))
        {
            GameObject line = GameObject.Find("Line(Clone)");
            if (line != null)
            {
                DeselectLine();
            }
        }

        // Select new object. 
        if (!SelectedObjects.Contains(this.gameObject))
        {
            SelectedObjects.Add(this.gameObject);
            SelectionBox.GetComponent<SpriteRenderer>().enabled = true;
        }

        _tbu.EnableToolbarButtons();

        // Clear the Properties Window
        _script.Clear();
       
        // Call the script from component that fills the Properties Window
        if (SelectedObjects.Count == 1)
        {
            GUICircuitComponent componentScript = SelectedObjects[0].GetComponent<GUICircuitComponent>();
            componentScript.GetProperties();
        }
    }

    // Deselect object(s)
    public void DeselectObject()
    {        
        //deselect component
        if (SelectedObjects.Count != 0)
        {
            foreach (GameObject objectSelected in SelectedObjects)
            {
                objectSelected.transform.FindChild("SelectionBox").GetComponent<SpriteRenderer>().enabled = false;
            }
            SelectedObjects.Clear();
            _script.Clear();
        }
    }

    // Deselect Line(s)
    public void DeselectLine()
    {
        // Deselect line
        if (SelectedLines.Count != 0)
        {
            foreach (GameObject linesSelected in SelectedLines)
            {
                linesSelected.GetComponent<Line>().UnmarkAsSelected();
            }
            SelectedLines.Clear();
        }
    }
}
