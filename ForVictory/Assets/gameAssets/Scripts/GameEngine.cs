using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour 
{
    [HideInInspector]
    public Transform TransformPlayerCar;
	public GameObject myPanel;
	public bool STARTRACE = false;
	public SpriteRenderer LampCount;
	public GameObject[] CarsTab = new GameObject[ 6 ];
	public Sprite[] NumbersTab = new Sprite[ 6 ];
	public UnityEngine.UI.Text [] TextTab = new UnityEngine.UI.Text[ 6 ];
	public Sprite[] LampTab = new Sprite[ 5 ];
	public GameObject[] Winners = new GameObject[ 3 ];
	public GUITexture GUITex;
	public Vector3[] PodiumPos = new Vector3[ 3 ];
	public Vector3 CameraPodiumPos;
	public SpriteRenderer PodiumGO;
	public SpriteRenderer YouWinnerStringSprite;
	public GameObject FireWorks1;
	public GameObject FireWorks2;
	private float ss;

	private bool GamePaused = false;
	private int CountWinners = 0;
	private int Number;
	private string Name;
	private string ColorName;
	private int PlayerCarIndex = 0;
	private int lenCarsTab = 0;
	private int lenTextTab = 0;
	private int NumberNumberCar;
	private int IndexNumberCar;
	private bool AllTabsClear = false;
    private bool Couting = false;
    private bool MarkTransformPlayerCar = false;

	// Use this for initialization
	void Awake () 
	{
		Time.timeScale = 1.0f;
		Number = PlayerPrefs.GetInt( "Number" );
		Name = PlayerPrefs.GetString( "Name" );
		ColorName = PlayerPrefs.GetString( "ColorName" );
		Cursor.visible = false;

		lenCarsTab = CarsTab.Length;
		lenTextTab = TextTab.Length;

		FindPlayerCars();
		MarkOtherCars();
		StartCoroutine( StartCount() );
	}

	IEnumerator Wait( int Sec )
	{
		yield return new WaitForSeconds( Sec );
	}

	IEnumerator WinnerChampion()
	{
		Color tColor = GUITex.color;
		while( tColor.a < 1.0f )
		{
			tColor.a += Time.deltaTime;
			GUITex.color = tColor;
			yield return 0;
		}
		STARTRACE = false;

		Camera.main.transform.parent = Winners[ 0 ].transform.parent;
		yield return new WaitForSeconds( 2 );
		for( int i = 0; i < 3; i++ )
		{
			Winners[ i ].transform.position = PodiumPos[ i ];
			Winners[ i ].transform.Rotate( new Vector3( 0.0f, 0.0f, 140.0f ) );
			yield return 0;
		}
		Camera.main.transform.position = CameraPodiumPos;
		PodiumGO.enabled = true;
		yield return 0;

		if( Winners[ 0 ].GetComponent< CarController >().Name == Name )
		{
			YouWinnerStringSprite.enabled = true;
		}

		FireWorks1.SetActive( true );
		FireWorks2.SetActive( true );

		while( tColor.a > 0.0f )
		{
			tColor.a -= Time.deltaTime;
			GUITex.color = tColor;
			yield return 0;
		}
	}

	IEnumerator DimnessScreen( bool D )
	{
		Color tColor = GUITex.color;

		if( D )
		{
			while( tColor.a < 1.0f )
			{
				tColor.a += Time.deltaTime;
				GUITex.color = tColor;
				yield return 0;
			}
		}
		else
		{
			while( tColor.a > 0.0f )
			{
				tColor.a -= Time.deltaTime;
				GUITex.color = tColor;
				yield return 0;
			}
		}	
	}

	public void MarkWinners( GameObject GO )
	{
		if( CountWinners < 3 )
		{
			Winners[ CountWinners ] = GO;
			CountWinners++;
		}

		if( CountWinners == 3 )
		{
			STARTRACE = false;
			StartCoroutine( WinnerChampion() );
		}

	}

	IEnumerator StartCount()
	{
        Couting = true;
		Color tempColor = LampCount.color;
		tempColor.a = 0;
		LampCount.color = tempColor;
		LampCount.enabled = true;
		while( tempColor.a < 1.0f )
		{
			tempColor.a += Time.deltaTime;
			LampCount.color = tempColor;
			yield return 0;
		}
		yield return new WaitForSeconds( 1 );

		LampCount.sprite = LampTab[ 1 ];
		yield return new WaitForSeconds( 1 );
		LampCount.sprite = LampTab[ 2 ];
		yield return new WaitForSeconds( 1 );
		LampCount.sprite = LampTab[ 3 ];
		yield return new WaitForSeconds( 1 );
		ClearTextTab();
		STARTRACE = true;
		LampCount.sprite = LampTab[ 4 ];
        Couting = false;
        yield return new WaitForSeconds( 1 );
		Destroy( LampCount.gameObject );
		yield return 0;
	}

	void MarkOtherCars()
	{
		for( int i = 0; i < lenCarsTab; i++ )
		{
			if( i != PlayerCarIndex )
			{
				// Name (on asphalt) 
				string tName = ( "Player " + ( i + 1 ).ToString() );
				TextTab[ i ].text = tName;
				// Number choose for player (on car)
				CarsTab[ i ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber( i );
				int k = i;
				if( k == Number )
				{
					k = PlayerCarIndex + 1;
				}
				CarsTab[ i ].GetComponent< CarController >().MarkStats( tName, k );
			}
		}
	}

	// Znajdowanie samochodu gracza
	void FindPlayerCars()
	{
		if( ColorName == "Blue" )
		{
            TransformPlayerCar = CarsTab[0].transform;
			// Name (on asphalt)
			TextTab[ 0 ].text = Name;
			// Number choose for player (on car)
			CarsTab[ 0 ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber();
			CarsTab[ 0 ].GetComponent< CarController >().PlayerController = true;
			CarsTab[ 0 ].GetComponent< CarController >().MarkStats( Name, Number );
			CarsTab[ 0 ].tag = "Player";
			CarsTab[ 0 ].AddComponent< BoxCollider2D >().isTrigger = true;
			CarsTab[ 0 ].AddComponent< Rigidbody2D >().isKinematic = true;
			Camera.main.transform.parent = CarsTab[ 0 ].transform;
            MarkTransformPlayerCar = true;
            PlayerCarIndex = 0;
		}
		else if( ColorName == "Yellow" )
		{
            TransformPlayerCar = CarsTab[1].transform;
            TextTab[ 1 ].text = Name;
			CarsTab[ 1 ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber();
			CarsTab[ 1 ].GetComponent< CarController >().PlayerController = true;
			CarsTab[ 1 ].GetComponent< CarController >().MarkStats( Name, Number );
			CarsTab[ 1 ].tag = "Player";
			CarsTab[ 1 ].AddComponent< BoxCollider2D >().isTrigger = true;
			CarsTab[ 1 ].AddComponent< Rigidbody2D >().isKinematic = true;
			Camera.main.transform.parent = CarsTab[ 1 ].transform;
            MarkTransformPlayerCar = true;
            PlayerCarIndex = 1;
		}
		else if( ColorName == "Green" )
		{
            TransformPlayerCar = CarsTab[2].transform;
            TextTab[ 2 ].text = Name;
			CarsTab[ 2 ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber();
			CarsTab[ 2 ].GetComponent< CarController >().PlayerController = true;
			CarsTab[ 2 ].GetComponent< CarController >().MarkStats( Name, Number );
			CarsTab[ 2 ].tag = "Player";
			CarsTab[ 2 ].AddComponent< BoxCollider2D >().isTrigger = true;
			CarsTab[ 2 ].AddComponent< Rigidbody2D >().isKinematic = true;
			Camera.main.transform.parent = CarsTab[ 2 ].transform;
            MarkTransformPlayerCar = true;
            PlayerCarIndex = 2;
		}
		else if( ColorName == "White" )
		{
            TransformPlayerCar = CarsTab[3].transform;
            TextTab[ 3 ].text = Name;
			CarsTab[ 3 ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber();
			CarsTab[ 3 ].GetComponent< CarController >().PlayerController = true;
			CarsTab[ 3 ].GetComponent< CarController >().MarkStats( Name, Number );
			CarsTab[ 3 ].tag = "Player";
			CarsTab[ 3 ].AddComponent< BoxCollider2D >().isTrigger = true;
			CarsTab[ 3 ].AddComponent< Rigidbody2D >().isKinematic = true;
			Camera.main.transform.parent = CarsTab[ 3 ].transform;
            MarkTransformPlayerCar = true;
            PlayerCarIndex = 3;
		}
		else if( ColorName == "Brown" )
		{
            TransformPlayerCar = CarsTab[4].transform;
            TextTab[ 4 ].text = Name;
			CarsTab[ 4 ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber();
			CarsTab[ 4 ].GetComponent< CarController >().PlayerController = true;
			CarsTab[ 4 ].GetComponent< CarController >().MarkStats( Name, Number );
			CarsTab[ 4 ].tag = "Player";
			CarsTab[ 4 ].AddComponent< BoxCollider2D >().isTrigger = true;
			CarsTab[ 4 ].AddComponent< Rigidbody2D >().isKinematic = true;
			Camera.main.transform.parent = CarsTab[ 4 ].transform;
            MarkTransformPlayerCar = true;
            PlayerCarIndex = 4;
		}
		else if( ColorName == "Orange" )
		{
            TransformPlayerCar = CarsTab[5].transform;
            TextTab[ 5 ].text = Name;
			CarsTab[ 5 ].transform.FindChild( "numcar" ).GetComponent< SpriteRenderer >().sprite = ParserNumber();
			CarsTab[ 5 ].GetComponent< CarController >().PlayerController = true;
			CarsTab[ 5 ].GetComponent< CarController >().MarkStats( Name, Number );
			CarsTab[ 5 ].tag = "Player";
			CarsTab[ 5 ].AddComponent< BoxCollider2D >().isTrigger = true;
			CarsTab[ 5 ].AddComponent< Rigidbody2D >().isKinematic = true;
			Camera.main.transform.parent = CarsTab[ 5 ].transform;
            MarkTransformPlayerCar = true;
            PlayerCarIndex = 5;
		}

		NumberNumberCar = PlayerCarIndex + 1;
		IndexNumberCar = Number - 1;
	}

	void ClearTextTab()
	{
		for( int i = 0; i < lenTextTab; i++ )
		{
			Destroy( TextTab[ i ].gameObject );
		}
		AllTabsClear = true;
	}

	Sprite ParserNumber()
	{
		if( Number == 1 )
		{
			return NumbersTab[ 0 ];
		}
		else if( Number == 2 )
		{
			return NumbersTab[ 1 ];
		}
		else if( Number == 3 )
		{
			return NumbersTab[ 2 ];
		}
		else if( Number == 4 )
		{
			return NumbersTab[ 3 ];
		}
		else if( Number == 5 )
		{
			return NumbersTab[ 4 ];
		}
		else if( Number == 6 )
		{
			return NumbersTab[ 5 ];
		}

		return NumbersTab[ 0 ];
	}

	Sprite ParserNumber( int n )
	{
		int k = n;

		if( k == IndexNumberCar )
		{
			return NumbersTab[ NumberNumberCar - 1 ];
		}
		else
		{
			return NumbersTab[ k ];
		}
	}

	public void buttonResumeGame()
	{
		Cursor.visible = false;
		GamePaused = false;
		Time.timeScale = 1;
		myPanel.SetActive( false );
	}

	public void buttonExitGame()
	{
		Application.Quit();
	}

	IEnumerator RestartLevel()
	{
		if( !AllTabsClear )
		{
			ClearTextTab();
		}

		GUITex.enabled = true;
		Color tColor = GUITex.color;
		while( tColor.a < 1.0f )
		{
			tColor.a += 0.02f;
			GUITex.color = tColor;
			yield return 0;
		}
		LoadLevelForRestart();
		yield return 0;

	}

	void LoadLevelForRestart()
	{
		StopAllCoroutines();
		Application.LoadLevel( 1 );
	}

	public void buttonRestartGame()
	{
		StartCoroutine( RestartLevel() );
	}

	// Update is called once per frame
	void Update () 
	{
        if( MarkTransformPlayerCar )
        {
            TransformPlayerCar = CarsTab[PlayerCarIndex].transform;
        }

		if( Input.GetKeyDown( KeyCode.Escape ) && !GamePaused )
		{
			Cursor.visible = true;
			STARTRACE = false;
			GamePaused = true;
			Time.timeScale = 0;
			myPanel.SetActive( true );
		}
		else if( Input.GetKeyDown( KeyCode.Escape ) && GamePaused )
		{
			Cursor.visible = false;
            if( Couting )
            {
                STARTRACE = false;
            }
            else
            {
                STARTRACE = true;
            }
			GamePaused = false;
			Time.timeScale = 1;
			myPanel.SetActive( false );
		}
	}
}
