using UnityEngine;
using System.Collections;

public class WhatIsMyNumber : MonoBehaviour 
{
	public bool ChooseNumber = true;
	public int myNumber = 0;
	public string myColorName = "";
	// Use this for initialization
	void Start () {
	
	}

	public void MyNumberIs()
	{
		if( ChooseNumber )
		{
			Camera.main.GetComponent< MainMenuMechanism >().Number = myNumber;
		}
		else
		{
			Camera.main.GetComponent< MainMenuMechanism >().ColorName = myColorName;
		}
	}
}
