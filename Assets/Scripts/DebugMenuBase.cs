using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DebugMenuBase : MonoBehaviour {

    Rect _windowRect = new Rect(0, 0, 300, 300);

    public void OnGUI()
    {
        _windowRect =
            GUIUtil.ResizableWindow(
                GetHashCode(), _windowRect, (id) =>
                {
                    OnGUIInternal();
                    GUI.DragWindow();
                },
        WindowName,
        GUILayout.MinWidth(MinWidth));
    }

    protected abstract void OnGUIInternal();
    protected virtual float MinWidth { get { return 300f; } }
    protected virtual string WindowName { get { return "Debug Window"; } }
}
