﻿using UnityEngine;
using UnityEngine.EventSystems;

public class Line : MonoBehaviour, IPointerClickHandler
{
    
    public GameObject Begin;
    public GameObject End = null;
    public Vector3 EndPos;
    private LineRenderer _line;
    public Vector3 StartPos;
    private bool _addedCollider = false;
    private Vector3 _oldStartPos;
    private Vector3 _oldEndPos;
    private BoxCollider2D _col1;
    private BoxCollider2D _col2;
    public Vector3 MiddlePos;
    public string TypeOfLine = "RightBreak";
    private string _oldTypeOfLine;

    void Awake()
    {
        UnmarkAsSelected();
    }

    // Update is called once per frame
    void Update()
    {  
        StartPos.x = Begin.transform.position.x;
        StartPos.y = Begin.transform.position.y;
        StartPos.z = 0;

        //when i correctly connect two connectors
        if (End != null)
        {
            EndPos.x = End.transform.position.x;
            EndPos.y = End.transform.position.y;
            EndPos.z = 0;          

            //when space was 1 times pressed - right break line
            if (TypeOfLine == "RightBreak")
            {
                _line = GetComponent<LineRenderer>();
                _line.numPositions = 3;
                _line.numCornerVertices = 1;

                _line.SetPosition(0, new Vector3(StartPos.x, StartPos.y, 0));
                _line.SetPosition(1, new Vector3(EndPos.x, StartPos.y, 0));
                _line.SetPosition(2, new Vector3(EndPos.x, EndPos.y, 0));

                MiddlePos = new Vector3(EndPos.x, StartPos.y, 0);
            }

            //when space was 2 times pressed - left break line
            else if (TypeOfLine == "LeftBreak")
            {
                _line = GetComponent<LineRenderer>();
                _line.numPositions = 3;
                _line.numCornerVertices = 1;

                _line.SetPosition(0, new Vector3(EndPos.x, EndPos.y, 0));
                _line.SetPosition(1, new Vector3(StartPos.x, EndPos.y, 0));
                _line.SetPosition(2, new Vector3(StartPos.x, StartPos.y, 0));

                MiddlePos = new Vector3(StartPos.x, EndPos.y, 0);
            }
            

            //added dynamic collider in case that moving electric components or changing type of line
            if (!_addedCollider)
            {
                _oldStartPos = StartPos;
                _oldEndPos = EndPos;
                _oldTypeOfLine = TypeOfLine;
                AddCollidersToLine();
                _addedCollider = true;
            }
        }

        //when dragging the line
        else
        {
            _line = GetComponent<LineRenderer>();
            _line.numPositions = 2;
            _line.SetPosition(0, StartPos);
            _line.SetPosition(1, EndPos);
        }

        //destroy old colliders in case that moving electric components or changing type of line
        if (_addedCollider && (_oldStartPos != StartPos || _oldEndPos != EndPos || _oldTypeOfLine != TypeOfLine))
        {
            for (int i = 0; i < _line.transform.childCount; i++)
            {
                Destroy(_line.transform.GetChild(i).gameObject);
            }
            _addedCollider = false;
        }   
    }

    private void AddCollidersToLine()
    {

        _col1 = new GameObject("Collider").AddComponent<BoxCollider2D>();
        _col2 = new GameObject("Collider").AddComponent<BoxCollider2D>();

        // Colliders is added as child object of line
        _col1.transform.parent = _line.transform;
        _col2.transform.parent = _line.transform;

        //get distance between connectors and break line position
        float lineLegth1 = Vector2.Distance(StartPos, MiddlePos);
        float lineLegth2 = Vector2.Distance(MiddlePos, EndPos);

        // size of colliders width and length
        if (TypeOfLine == "RightBreak")
        {               
            _col1.size = new Vector2(lineLegth1, 0.3f);
            _col2.size = new Vector2(0.3f, lineLegth2);
        }

        else if (TypeOfLine == "LeftBreak")
        {
            _col1.size = new Vector2(0.3f, lineLegth1);
            _col2.size = new Vector2(lineLegth2, 0.3f);                               
        }

        Vector2 midPoint1 = (StartPos + MiddlePos) / 2;
        Vector2 midPoint2 = (MiddlePos + EndPos) / 2;

        // setting position of colliders object
        _col1.transform.position = midPoint1;
        _col2.transform.position = midPoint2;

        //set to ActiveItem
        _col1.gameObject.layer = 8;
        _col2.gameObject.layer = 8;
    }

    // Mark this line as Selected
    public void MarkAsSelected()
    {
        GetComponent<LineRenderer>().startColor = Color.red;
        GetComponent<LineRenderer>().endColor = Color.red;
    }

    // Unmark this line as Selected
    public void UnmarkAsSelected()
    {
        GetComponent<LineRenderer>().startColor = Color.black;
        GetComponent<LineRenderer>().endColor = Color.black;
    }

    // When the line is duplicated its coliders are duplicated as well - no need to create new ones
    public void KeepColiders()
    {
        _addedCollider = true;
    }

    //select line with mouse click and change color of selected line
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        //deselect item
        GameObject item = GameObject.Find("Canvas");
        item.GetComponent<SelectObject>().DeselectObject();

        //deselect previous selected lines
        if (!SelectObject.SelectedLines.Contains(this.gameObject))
        {
            item.GetComponent<SelectObject>().DeselectLine();
        }

        SelectObject.SelectedLines.Add(this.gameObject);
        MarkAsSelected();
    }

    public static void TransformLines()
    {
        GameObject[] lines;
        lines = GameObject.FindGameObjectsWithTag("ActiveLine");
        foreach (GameObject l in lines)
        {
            l.transform.position = new Vector2((l.GetComponent<Line>().Begin.transform.position.x + l.GetComponent<Line>().End.transform.position.x) / 2,
                    (l.GetComponent<Line>().Begin.transform.position.y + l.GetComponent<Line>().End.transform.position.y) / 2);
        }
    }
}