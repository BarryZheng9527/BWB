using UnityEngine;
using FairyGUI;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class ConfigManager
{
    static private ConfigManager instance = null;
    private XmlDocument _XmlReader = new XmlDocument();

    public static ConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConfigManager();
            }
            return instance;
        }
    }

    public void PreloadXml()
    {
        LoadConfig("language");
        LoadConfig("name");
        LoadConfig("level");
        LoadConfig("equip");
        LoadConfig("remould");
        LoadConfig("skill");
    }

    private void LoadConfig(string szConfigName)
    {
        string xml = Resources.Load("Config/" + szConfigName).ToString();
        _XmlReader.LoadXml(xml);
        XmlNode root = _XmlReader.SelectSingleNode("Root");
        if (szConfigName == "language")
        {
            LanguageConfig.Instance.ReadXml(root);
        }
        else if (szConfigName == "name")
        {
            NameConfig.Instance.ReadXml(root);
        }
        else if (szConfigName == "level")
        {
            LevelConfig.Instance.ReadXml(root);
        }
        else if (szConfigName == "equip")
        {
            EquipConfig.Instance.ReadXml(root);
        }
        else if (szConfigName == "remould")
        {
            RemouldConfig.Instance.ReadXml(root);
        }
        else if (szConfigName == "skill")
        {
            SkillConfig.Instance.ReadXml(root);
        }
    }
}