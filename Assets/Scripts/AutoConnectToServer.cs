using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutoConnectToServer : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (!NetworkManager.singleton.isNetworkActive)
        {
            NetworkManager.singleton.StartClient();
        }
	}
}
