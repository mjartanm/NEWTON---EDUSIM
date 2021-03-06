﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Entities;
using Assets.Scripts.Utils;

public class Persistance : MonoBehaviour
{
    [SerializeField]
    private GameObject _batteryPrefab;
    [SerializeField]
    private GameObject _resistorPrefab;
    [SerializeField]
    private GameObject _ampermeterPrefab;
    [SerializeField]
    private GameObject _analogSwitchPrefab;
    [SerializeField]
    private GameObject _capacitorPrefab;
    [SerializeField]
    private GameObject _inductorPrefab;
    [SerializeField]
    private GameObject _lampPrefab;
    [SerializeField]
    private GameObject _nodePrefab;
    [SerializeField]
    private GameObject _voltmeterPrefab;
    [SerializeField]
    private GameObject _linePrefab;
    [SerializeField]
    private GameObject _ledDiodePrefab;
    [SerializeField]
    private GameObject _transistorNPNPrefab;
    [SerializeField]
    private GameObject _transistorPNPPrefab;
    [SerializeField]
    private GameObject _zenerDiodePrefab;
    [SerializeField]
    private GameObject _potentiometerPrefab;

    private string _lastFileName;

    public string LastFileName
    {
        get { return _lastFileName; }
        private set { _lastFileName = value != null ? value.Trim() : null; }
    }

    public void Save(string fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            LastFileName = fileName;
        }
        else if (string.IsNullOrEmpty(LastFileName))
        {
            throw new ArgumentNullException("fileName");
        }
        
        GUICircuitComponent[] components = FindObjectsOfType<GUICircuitComponent>();
        Line[] lines = FindObjectsOfType<Line>();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(LastFileName, FileMode.OpenOrCreate);
        List<SimulationElement> elementsToSerialize = new List<SimulationElement>();
        List<LineEntity> linesToSerialize = new List<LineEntity>();

        foreach (GUICircuitComponent component in components)
        {
            if (!component.CompareTag("ActiveItem"))
            {
                continue;
            }
            SimulationElement element = component.Entity;
            if (element != null)
            {
                elementsToSerialize.Add(element);
            }
        }

        foreach (Line line in lines)
        {
            linesToSerialize.Add(new LineEntity
            {
                StartConnectorId = line.Begin.GetComponentInChildren<Connector>().GetInstanceID(),
                EndConnectorId = line.End.GetComponentInChildren<Connector>().GetInstanceID(),
                LineType = line.TypeOfLine
            });
        }

        SerializationPackage package = new SerializationPackage
        {
            LineEntities = linesToSerialize,
            SimulationElements = elementsToSerialize
        };


        binaryFormatter.Serialize(fileStream, package);
        fileStream.Close();
    }

    public void Load(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException("fileName");
        }

        LastFileName = fileName;
        ClearScene();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(LastFileName, FileMode.Open, FileAccess.Read);

        object o = binaryFormatter.Deserialize(fileStream);
        SerializationPackage package = (SerializationPackage) o;

        foreach (SimulationElement simulationElement in package.SimulationElements)
        {
            Type entityType = simulationElement.GetType();
            if (entityType == typeof(BatteryEntity))
            {
                BatteryEntity concreteEntity = simulationElement as BatteryEntity;
                GameObject concreteGameObject = InstantiateGameObject(_batteryPrefab);
                concreteGameObject.GetComponent<GUIBattery>().Awake();
                concreteGameObject.GetComponent<GUIBattery>().Entity = concreteEntity;
            }
            else if (entityType == typeof(ResistorEntity))
            {
                ResistorEntity concreteEntity = simulationElement as ResistorEntity;
                GameObject concreteGameObject = InstantiateGameObject(_resistorPrefab);
                concreteGameObject.GetComponent<GUIResistor>().Awake();
                concreteGameObject.GetComponent<GUIResistor>().Entity = concreteEntity;
            }
            else if (entityType == typeof(VoltmeterEntity))
            {
                VoltmeterEntity concreteEntity = simulationElement as VoltmeterEntity;
                GameObject concreteGameObject = InstantiateGameObject(_voltmeterPrefab);
                concreteGameObject.GetComponent<GUIVoltmeter>().Awake();
                concreteGameObject.GetComponent<GUIVoltmeter>().Entity = concreteEntity;
            }
            else if (entityType == typeof(AmpermeterEntity))
            {
                AmpermeterEntity concreteEntity = simulationElement as AmpermeterEntity;
                GameObject concreteGameObject = InstantiateGameObject(_ampermeterPrefab);
                concreteGameObject.GetComponent<GUIAmpermeter>().Awake();
                concreteGameObject.GetComponent<GUIAmpermeter>().Entity = concreteEntity;
            }
            else if (entityType == typeof(AnalogSwitchEntity))
            {
                AnalogSwitchEntity concreteEntity = simulationElement as AnalogSwitchEntity;
                GameObject concreteGameObject = InstantiateGameObject(_analogSwitchPrefab);
                concreteGameObject.GetComponent<GUIAnalogSwitch>().Awake();
                concreteGameObject.GetComponent<GUIAnalogSwitch>().Entity = concreteEntity;
            }
            else if (entityType == typeof(CapacitorEntity))
            {
                CapacitorEntity concreteEntity = simulationElement as CapacitorEntity;
                GameObject concreteGameObject = InstantiateGameObject(_capacitorPrefab);
                concreteGameObject.GetComponent<GUICapacitor>().Awake();
                concreteGameObject.GetComponent<GUICapacitor>().Entity = concreteEntity;
            }
            else if (entityType == typeof(InductorEntity))
            {
                InductorEntity concreteEntity = simulationElement as InductorEntity;
                GameObject concreteGameObject = InstantiateGameObject(_inductorPrefab);
                concreteGameObject.GetComponent<GUIInductor>().Awake();
                concreteGameObject.GetComponent<GUIInductor>().Entity = concreteEntity;
            }
            else if (entityType == typeof(LampEntity))
            {
                LampEntity concreteEntity = simulationElement as LampEntity;
                GameObject concreteGameObject = InstantiateGameObject(_lampPrefab);
                concreteGameObject.GetComponent<GUILamp>().Awake();
                concreteGameObject.GetComponent<GUILamp>().Entity = concreteEntity;
            }
            else if (entityType == typeof(NodeEntity))
            {
                NodeEntity concreteEntity = simulationElement as NodeEntity;
                GameObject concreteGameObject = InstantiateGameObject(_nodePrefab);
                concreteGameObject.GetComponent<GUINode>().Awake();
                concreteGameObject.GetComponent<GUINode>().Entity = concreteEntity;
            }
            else if (entityType == typeof(LedDiodeEntity))
            {
                LedDiodeEntity concreteEntity = simulationElement as LedDiodeEntity;
                GameObject concreteGameObject = InstantiateGameObject(_ledDiodePrefab);
                concreteGameObject.GetComponent<GUILedDiode>().Awake();
                concreteGameObject.GetComponent<GUILedDiode>().Entity = concreteEntity;
            }
            else if (entityType == typeof(TransistorNPNEntity))
            {
                TransistorNPNEntity concreteEntity = simulationElement as TransistorNPNEntity;
                GameObject concreteGameObject = InstantiateGameObject(_transistorNPNPrefab);
                concreteGameObject.GetComponent<GUITransistorNPN>().Awake();
                concreteGameObject.GetComponent<GUITransistorNPN>().Entity = concreteEntity;
            }
            else if (entityType == typeof(TransistorPNPEntity))
            {
                TransistorPNPEntity concreteEntity = simulationElement as TransistorPNPEntity;
                GameObject concreteGameObject = InstantiateGameObject(_transistorPNPPrefab);
                concreteGameObject.GetComponent<GUITransistorPNP>().Awake();
                concreteGameObject.GetComponent<GUITransistorPNP>().Entity = concreteEntity;
            }
            else if (entityType == typeof(ZenerDiodeEntity))
            {
                ZenerDiodeEntity concreteEntity = simulationElement as ZenerDiodeEntity;
                GameObject concreteGameObject = InstantiateGameObject(_zenerDiodePrefab);
                concreteGameObject.GetComponent<GUIZenerDiode>().Awake();
                concreteGameObject.GetComponent<GUIZenerDiode>().Entity = concreteEntity;
            }
            else if (entityType == typeof(PotentiometerEntity))
            {
                PotentiometerEntity concreteEntity = simulationElement as PotentiometerEntity;
                GameObject concreteGameObject = InstantiateGameObject(_potentiometerPrefab);
                concreteGameObject.GetComponent<GUIPotentiometer>().Awake();
                concreteGameObject.GetComponent<GUIPotentiometer>().Entity = concreteEntity;
            }
            
        }

        List<Connector> connectors = FindObjectsOfType<Connector>().ToList();
        connectors =
            connectors.Where(
                x => x.transform.parent != null && (x.transform.parent.CompareTag("ActiveItem") || x.transform.parent.CompareTag("ActiveNode"))).ToList();

        foreach (LineEntity packageLineEntity in package.LineEntities)
        {
            Connector start = connectors.Find(x => x.TemporaryId == packageLineEntity.StartConnectorId);
            Connector end = connectors.Find(x => x.TemporaryId == packageLineEntity.EndConnectorId);

            GameObject linePrefab = Instantiate(_linePrefab);
            Line line = linePrefab.AddComponent<Line>();
            line.Begin = start.gameObject;
            line.End = end.gameObject;
            line.TypeOfLine = packageLineEntity.LineType;
            line.EndPos = end.transform.position;
            line.StartPos = start.transform.position;

            linePrefab.tag = "ActiveLine";
            linePrefab.transform.position = new Vector2((line.Begin.transform.position.x + line.EndPos.x) / 2,
                    (line.Begin.transform.position.y + line.EndPos.y) / 2);

            end.GetComponent<Connectable>().AddConnected(start.gameObject);
            start.GetComponent<Connectable>().AddConnected(end.gameObject);

            start.ConnectedConnectors.Add(end);
            end.ConnectedConnectors.Add(start);
        }

        fileStream.Close();
    }

    void Awake()
    {
        FileBrowserHandler.Instance.PersistanceScript = this;
    }

    public void NewProject()
    {
        LastFileName = null;
        ClearScene();
    }

    private void ClearScene()
    {
        FindObjectOfType<MultiSelect>().DoDeselect();
        List<GameObject> elementGameObjects = FindObjectsOfType<GUICircuitComponent>().ToList()
            .Where(x => x.CompareTag("ActiveItem"))
            .Select(x => x.gameObject).ToList();

        List<GameObject> lineGameObjects = FindObjectsOfType<Line>().ToList()
            .Select(x => x.gameObject).ToList();

        foreach (GameObject elementGameObject in elementGameObjects)
        {
            Destroy(elementGameObject);
        }
        foreach (GameObject lineGameObject in lineGameObjects)
        {
            Destroy(lineGameObject);
        }
    }

    private static GameObject InstantiateGameObject(GameObject gameObject)
    {
        GameObject activeGameObject = Instantiate(gameObject);
        activeGameObject.tag = "ActiveItem";
        activeGameObject.layer = 8; //Name of 8th layer is ActiveItem
        activeGameObject.transform.localScale = new Vector3(1, 1, 0);
        activeGameObject.GetComponent<SpriteRenderer>().enabled = true;
        activeGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "ActiveItem";
        for (int i = 0; i < activeGameObject.transform.childCount; i++)
        {
            activeGameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingLayerName = "ActiveItem";
            activeGameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            activeGameObject.transform.GetChild(i).gameObject.layer = 8;
        }
        activeGameObject.transform.FindChild("SelectionBox").GetComponent<SpriteRenderer>().enabled = false;
        return activeGameObject;
    }

    [Serializable]
    private class SerializationPackage
    {
        public List<SimulationElement> SimulationElements { get; set; }
        public List<LineEntity> LineEntities { get; set; }
    }
}
