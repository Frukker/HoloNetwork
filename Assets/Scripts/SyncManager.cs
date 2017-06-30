using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncManager : NetworkBehaviour
{
    [SyncVar] Vector3 sPosition;
    [SyncVar] Quaternion sRotation;
    [SyncVar] Vector3 sScale;

	void Update () {
	    if (isServer)
	    {
	        sPosition = transform.parent.position;
	        sRotation = transform.parent.localRotation;
	        sScale = transform.parent.localScale;
	    }
	    else
	    {
            transform.parent.position = sPosition;
            transform.parent.localRotation = sRotation;
            transform.parent.localScale = sScale;
        }
    }
}
