using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class test : MonoBehaviour
{

    public interface IButton
    {
        void clicked();
    }

    public class RealButton : IButton
    {
        public void clicked()
        {
            Debug.Log("clicked");
        }
    }

    public class Pad
    {
        public IButton ibutton;

        public void clickButton()
        {
            ibutton.clicked();
        }
    }

    // Use this for initialization
    void Start()
    {
        Pad p = new Pad();
        p.ibutton = new RealButton();
        p.clickButton();
    }

    // Update is called once per frame
    void Update()
    {

    }
}