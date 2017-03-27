﻿using Assets.Scripts.Localization;
using UnityEngine;
using UnityEngine.UI;
// Component is meant to be attatched to root element of the whole GUI (canvas)
public class Localization : MonoBehaviour
{
    private int _languageCycle;
    // instantiate Resource reader and set text elements text from resrouce file
    void Start()
    {
        ResourceReader.SetLanguage(Application.systemLanguage);
        SetElementTexts();
        if (Application.systemLanguage == SystemLanguage.Slovak)
        {
            _languageCycle = _languageCycle + 1;
        }
    }
    // used for testing purposes - cycles language between Slovak and English
    public void CycleLanguage()
    {
        ChangeLanguage(_languageCycle++%2 == 0 ? SystemLanguage.Slovak : SystemLanguage.English);
    }
    // create a new instance of resource reader and set text elements text from resrouce file
    public void ChangeLanguage(SystemLanguage language)
    {
        ResourceReader.SetLanguage(language);
        SetElementTexts();
    }
    // cycles trough all child elements and sets their texts according to current language
    private void SetElementTexts()
    {
        FindObjectOfType<MultiSelect>().DoDeselect();
        foreach (Text textComponent in GetComponentsInChildren<Text>(true))
        {
            textComponent.text = ResourceReader.Instance.GetResource(textComponent.name);
        }
    }
}