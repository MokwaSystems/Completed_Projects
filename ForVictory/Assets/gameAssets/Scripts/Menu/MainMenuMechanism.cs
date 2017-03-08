using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuMechanism : MonoBehaviour 
{
	public Canvas mainCanvas;
	public Image DangerousString;
	public GameObject[] ButtonsCanvas = new GameObject[ 19 ]; 
	//ublic GameObject TrackObject1;
	//public GameObject TrackObject2;
	//public Vector3 NextCameraPosition = Vector3.zero;
	//public Vector3 NextTrackPosition1 = Vector3.zero;
	//public Vector3 NextTrackPosition2 = Vector3.zero;
	public Image Dimmek;
	public int Number = 0;
	public string ColorName = "";
    public Image LOGO;
    public Image AboutScreen;

	private Rect normalizedMenuArea;
	private Rect rectMainCanvas;
	private int lenButtonCanvas = 0;
	private bool firstMenu = true;

	// Use this for initialization
	void Start () 
	{
		lenButtonCanvas = ButtonsCanvas.Length;

		/*rectMainCanvas = mainCanvas.GetComponent< RectTransform >().rect;
		normalizedMenuArea = new Rect( rectMainCanvas.x * Screen.width - ( rectMainCanvas.width * 0.5f ), rectMainCanvas.y * Screen.height - ( rectMainCanvas.height * 0.5f ), rectMainCanvas.width, rectMainCanvas.height );
		*/
}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetKeyDown( KeyCode.Escape ) && !firstMenu )
		{
			firstMenu = true;
			StartCoroutine( BackMenu() );
		}

	}

	public void ClickExitGame()
	{
		Application.Quit();
	}

	public void ClickNewGame()
	{
		StartCoroutine( NewGameMechanism() );
	}

	public void ClickPlay()
	{
		string Name;

		Name = ButtonsCanvas[ 3 ].GetComponent< InputField >().text;

		if( Name == "" )
		{
			Name = "Player1";
		}

		if( Number == 0 )
		{
			Number = Random.Range( 1, 7 );
		}

		if( ColorName == "" )
		{
			ColorName = "Orange";
		}

		//Debug.Log( "Name = " + Name + ", Number = " + Number + ", Color = " + ColorName );
		PlayerPrefs.SetString( "Name", Name );
		PlayerPrefs.SetString( "ColorName", ColorName );
		PlayerPrefs.SetInt( "Number", Number );
		StartCoroutine( WaitForStart() );
	}

    public void ClickHelpButton()
    {
        StartCoroutine( HelpMechanism() );
    }

	IEnumerator WaitForStart()
	{
		Color tColor = Dimmek.color;
		while( tColor.a < 1.0f )
		{
			tColor.a += Time.deltaTime;
			Dimmek.color = tColor;
			yield return 0;
		}
		
		DangerousString.enabled = true;
		
		yield return new WaitForSeconds( 5 );
		
		Application.LoadLevel( 1 );
	}

	IEnumerator BackMenu()
	{
		Color tColor = Dimmek.color;
		while( tColor.a < 1.0f )
		{
			tColor.a += Time.deltaTime;
			Dimmek.color = tColor;
			yield return 0;
		}
        AboutScreen.enabled = false;

        for ( int i = 2; i < lenButtonCanvas; i++ )
		{
			ButtonsCanvas[ i ].SetActive( false );
			yield return 0;
		}

        ButtonsCanvas[0].SetActive(true);
        ButtonsCanvas[1].SetActive(true);
        ButtonsCanvas[19].SetActive(true);
        LOGO.enabled = true;

        // DIM-LIGHT
        while ( tColor.a > 0.0f )
		{
			tColor.a -= Time.deltaTime;
			Dimmek.color = tColor;
			yield return 0;
		}
		yield return 0;
	}

	IEnumerator NewGameMechanism()
	{
		//Debug.Log( "Wchodze do NewGameMechanism" );
		//mainCanvas.enabled = false;

		// DIM
		Color tColor = Dimmek.color;
		while( tColor.a < 1.0f )
		{
			tColor.a += Time.deltaTime;
			Dimmek.color = tColor;
			yield return 0;
		}
        ButtonsCanvas[19].SetActive(false);
        LOGO.enabled = false;
		ButtonsCanvas[ 0 ].SetActive( false );
		ButtonsCanvas[ 1 ].SetActive( false );
        

		for( int i = 2; i < lenButtonCanvas; i++ )
		{
			ButtonsCanvas[ i ].SetActive( true );
			yield return 0;
		}
        ButtonsCanvas[19].SetActive(false);

        // DIM-LIGHT
        while ( tColor.a > 0.0f )
		{
			tColor.a -= Time.deltaTime;
			Dimmek.color = tColor;
			yield return 0;
		}
		firstMenu = false;
		yield return 0;
	}	

    IEnumerator HelpMechanism()
    {
        Color tColor = Dimmek.color;
        while (tColor.a < 1.0f)
        {
            tColor.a += Time.deltaTime;
            Dimmek.color = tColor;
            yield return 0;
        }
        ButtonsCanvas[19].SetActive(false);
        LOGO.enabled = false;
        ButtonsCanvas[0].SetActive(false);
        ButtonsCanvas[1].SetActive(false);


        for (int i = 2; i < 19; i++)
        {
            ButtonsCanvas[i].SetActive(false);
            yield return 0;
        }
        AboutScreen.enabled = true;
        LOGO.enabled = true;
        // DIM-LIGHT
        while (tColor.a > 0.0f)
        {
            tColor.a -= Time.deltaTime;
            Dimmek.color = tColor;
            yield return 0;
        }
        firstMenu = false;
        yield return 0;
    }
}
