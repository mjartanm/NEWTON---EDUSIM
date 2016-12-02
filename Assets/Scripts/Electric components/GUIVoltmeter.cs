﻿using UnityEngine;
using ClassLibrarySharpCircuit;
using System;
using System.Collections.Generic;

public class GUIVoltmeter : GUICircuitComponent
{
    public Circuit.Lead[] DllConnectors;
    public Resistor MyComponent;


    public override void GetProperties()
    {
        GameObject propertiesContainer = GameObject.Find("PropertiesWindowContainer");
        EditObjectProperties script = propertiesContainer.GetComponent<EditObjectProperties>();

        script.AddResult("VoltagePropertyLabel", MyComponent.getVoltageDelta().ToString(), "V");
    }

    // Use this for initialization
    public void Start()
    {
        if (CompareTag("ActiveItem"))
        {
            Debug.Log("activeItem inserted");
            DllConnectors = new Circuit.Lead[2];
            MyComponent = GUICircuit.sim.Create<Resistor>();
            DllConnectors[0] = MyComponent.leadIn;
            DllConnectors[1] = MyComponent.leadOut;
            MyComponent.resistance = Double.PositiveInfinity;

            Connectors[0] = transform.FindChild("Connector1").GetComponent<Connector>();
            Connectors[1] = transform.FindChild("Connector2").GetComponent<Connector>();
            Connectors[0].SetConnectedConnectors();
            Connectors[1].SetConnectedConnectors();
            Connectors[0].AssignComponent(this);
            Connectors[1].AssignComponent(this);
            Connectors[0].SetDllConnector(DllConnectors[0]);
            Connectors[1].SetDllConnector(DllConnectors[1]);
        }
    }
}
