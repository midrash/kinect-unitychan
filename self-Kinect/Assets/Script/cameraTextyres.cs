using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTextyres : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        KinectManager manager = KinectManager.Instance;
        if (manager && manager.IsInitialized())
        {
            Texture2D depthTexture = manager.GetUsersLblTex2D();
            Texture2D colorTexture = manager.GetUsersClrTex2D();
            // do something with the textures
        }
    }
}
