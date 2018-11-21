using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;
using PrefsGUI;
using System.Net;

public class OSCManager : SingletonMonoBehaviour<OSCManager>
{

    public PrefsInt port = new PrefsInt("Port", 7000);

    public PrefsString destinationIP = new PrefsString("Destination IP", "127.0.0.1");
    public PrefsInt destinationPort = new PrefsInt("Destination Port", 7000);

    private Queue queue;

    void Start()
    {
        queue = new Queue();
        queue = Queue.Synchronized(queue);

        OSCHandler.Instance.InitServer("Server", port);
        OSCHandler.Instance.InitClient("Client", IPAddress.Parse(destinationIP), destinationPort);
        
        OSCHandler.Instance.PacketReceivedEvent += OnPacketReceived;
    }

    void OnPacketReceived(OSCServer server, OSCPacket packet)
    {
        queue.Enqueue(packet);
    }

    void Update()
    {
        

        while (0 < queue.Count)
        {
            // 受信時に初期化
            str = "";

            OSCPacket packet = queue.Dequeue() as OSCPacket;
            if (packet.IsBundle())
            {
                
                // OSCBundleの場合バンドル展開
                OSCBundle bundle = packet as OSCBundle;
                foreach (OSCMessage msg in bundle.Data)
                {
                    string a = "Bundle - " + msg.Address;

                    foreach(var d in msg.Data)
                    {
                        a += "," + d.ToString();
                        str += msg.Address + " - " + d.ToString() + "\n";
                    }

                }
            }
            else
            {
                // OSCMessageの場合はそのまま変換
                OSCMessage msg = packet as OSCMessage;

                string a = msg.Address;

                foreach (var d in msg.Data)
                {
                    a += "," + d.ToString();
                    str += msg.Address + " - " + d.ToString() + "\n";
                }
            }
        }
    }


    string str = "";

    public void DebugMenuGUIServer()
    {
        port.OnGUI();

        GUILayout.Space(2);

        GUILayout.Label("Latest Message (Bundle)");
        GUILayout.TextArea(str);
    }

    string s = "";

    public void DebugMenuGUIClient()
    {
        destinationIP.OnGUI();
        destinationPort.OnGUI();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Message (/message)");
        s = GUILayout.TextField(s);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Send"))
        {
            OSCHandler.Instance.SendMessageToClient("Client", "/message", s);
        }
    }

    public void DebugMenuGUIHelp()
    {
        GUILayout.Label(
            "If you change any OSC network settings, please press save and restart this app.");
    }
}