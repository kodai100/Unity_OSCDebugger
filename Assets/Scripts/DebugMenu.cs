using PrefsGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : DebugMenuBase {

    GUIUtil.Folds _fieldFolds = new GUIUtil.Folds();

    protected override string WindowName { get { return ""; } }

    // Use this for initialization
    void Start () {


        _fieldFolds.Add("Server OSC Settings", () =>
        {
            OSCManager.Instance.DebugMenuGUIServer();
            
        });

        _fieldFolds.Add("Client OSC Setting", () => {
            OSCManager.Instance.DebugMenuGUIClient();
        });


        _fieldFolds.Add("Help", () => {
            OSCManager.Instance.DebugMenuGUIHelp();
        });

    }

    protected override void OnGUIInternal()
    {
        _fieldFolds.OnGUI();

        if (GUILayout.Button("Save")) Prefs.Save();

    }


}
