using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifetime : MonoBehaviour {

    
	// Use this for initialization
	void OnEnable () {
        StartCoroutine(watetime());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator watetime()
    {
        yield return new WaitForSeconds(5.0f);
        GameObject.Find("text_bubble").transform.GetChild(0).gameObject.SetActive(false);
    }
}
