using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private void OnGUI()
	{
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }
        GUILayout.EndArea();
	}
	// Start is called before the first frame update
	public void StartButtons()
    {
        if (GUILayout.Button("Host"))
            NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Server"))
            NetworkManager.Singleton.StartServer();
        if(GUILayout.Button("Client"))
            NetworkManager.Singleton.StartClient();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Mode: " + mode);
    }

}
