using UnityEngine;
using System.Collections;

public class CollisionFireworks : MonoBehaviour {

	public bool AllFireworksStart = false;
	public GameObject Fireworks;
	public GameObject Fireworks2;

	void OnTriggerEnter2D(Collider2D Col)
	{
		if( Col.tag == "Player" )
		{
			if( AllFireworksStart )
			{
				Fireworks.SetActive( true );
				Fireworks2.SetActive( true );
			}
			else
			{
				Fireworks.SetActive( true );
			}
		}

	}

	IEnumerator TurnOffFireworks()
	{
		yield return new WaitForSeconds( 3 );
		if( AllFireworksStart )
		{
			Fireworks.SetActive( false );
			Fireworks2.SetActive( false );
		}
		else
		{
			Fireworks.SetActive( false );
		}
		yield return 0;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
