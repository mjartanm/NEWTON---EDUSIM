﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Deselect : MonoBehaviour, IPointerClickHandler
{
    // When left mouse button is pressed...
    public void OnPointerClick(PointerEventData eventData)
    {
        if (SelectObject.SelectedObject != null)
        {
            SelectObject.SelectedObject.transform.FindChild("SelectionBox").GetComponent<SpriteRenderer>().enabled = false;
            SelectObject.SelectedObject = null;
            EditObjectProperties.Clear();
        }
    }
}
