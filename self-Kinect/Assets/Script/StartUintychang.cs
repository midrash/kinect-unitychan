using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUintychang : MonoBehaviour {

    // bool startUnity = false;
    public Text text_bubble;  
    // Use this for initialization
    void Start () {
       // transform.GetChild(0).gameObject.SetActive(false);
        
    }
	
	// Update is called once per frame
	void Update () {
        KinectManager kinectManager = KinectManager.Instance;

        //Debug.Log(kinectManager.isUser);

        if (kinectManager.isUser == true)
        {
                transform.GetChild(0).gameObject.SetActive(true); // 유니티 활성화
        }

        else if (kinectManager.isUser == false)
        {
            transform.GetChild(0).gameObject.SetActive(false); // 유니티 비활성화
        }
    }
    
}
