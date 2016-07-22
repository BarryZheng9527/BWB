using UnityEngine;
using FairyGUI;
using System.Collections;

public class GameLoader : MonoBehaviour {

    static public GameLoader instance;

    GameLoader()
    {
        instance = this;
    }

    void Awake()
    {
        Screen.SetResolution(360, 640, false);
        ConfigManager.Instance.PreloadXml();
        GUIManager.Instance.Init();
    }

    void Start() {
        GRoot.inst.SetContentScaleFactor(1280, 720, UIContentScaler.ScreenMatchMode.MatchHeight);
        GUIManager.Instance.OpenLogin();
    }
}
