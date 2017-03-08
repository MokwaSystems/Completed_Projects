using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour 
{
	public string Name = "";
	public int Number = 0;

	public bool canController = false;
	public bool PlayerController = false;

	private float Timer = 0.0f;
	private float factorSpeed = 1.0f;
	private float tempSpeed = 0.0f;
	private float TimerSlow = 0.0f;
	private float Speed = 0.0f;
	private bool SpeedChecker = false;
	private bool EndRaceForMe = false;

	// Simple IA_Vars
	private bool IA_UpArrow = true;
	private float IA_TimerToSlow = 0.0f;
	//private float IA_RangeTimerToSlow = 5.0f;
	private int IA_SlowStart;
	private float IA_TimeWaitToStart = 0.0f;
	private float IA_factorSpeedChanger = 1.0f;
	private int IA_WhatIsMyStage = 1;
	private int IA_ModeCharacter = 0; // 1 = Szybki, 2 = Stały, przyspieszający (delikatnie), 3 = Wolny
	private float IA_SpeedLimterAllStages = 30.0f;
	private float IA_TimerSpeedVelocity = 0.0f; // czas przyspieszenia
	private float IA_TSV_SLOW_RandomA = 0.0f;
	private float IA_TSV_SLOW_RandomB = 0.0f;
	private float IA_TSV_FASTER_RandomA = 0.0f;
	private float IA_TSV_FASTER_RandomB = 0.0f;
    private bool IA_ChangeStateController = false;
    private int[] IA_StagesPosition = new int[3];
    private bool StageTwoStart = false;
    private bool StageThreeStart = false;
	
	// Stage #1 Vars
	private float IA_SO_SpeedVelocity = 0.0f;
    private float IA_LikelyDistance = 0.0f;
    private bool IA_StartLoopStage = false;
    private Transform IA_TransformPlayerCar;
    private bool IA_SlowNow = false;
    private bool IA_NotUpArrow = false;

	// Use this for initialization
	void Start () 
	{
        IA_StagesPosition[0] = 200;
        IA_StagesPosition[1] = 400;
        IA_StagesPosition[2] = 600;
		//IA_RangeTimerToSlow = Random.Range( 2.0f, 16.0f ); // Czas do zwolnienia
		//IA_SlowStart = Random.Range( 0, 2 ); // Czy mam ruszyć wolno?
		//IA_TimeWaitToStart = Random.Range( 0.0f, 1.1f ); // Modyfikator startu

		// Etap #1

		// Losowanie Mode
		IA_ModeCharacter = Random.Range( 1, 4 );

		// Jeśli Mode jest ustawiony na czyli ten który się nie angażuje i jest tylko limitowany prędkością oraz mechanizmem zwalaniania i przyspieszania
		if( IA_ModeCharacter == 1 )
		{
            IA_SlowStart = Random.Range(0, 2);
            IA_TimeWaitToStart = Random.Range( 0.2f, 0.4f );

			// Tutaj nie musimy ustawiać czasu przyspieszenia. Wystarczy, że ustawimy SpeedLimitera
			IA_SpeedLimterAllStages = Random.Range ( 40.0f, 45.0f );

			// Ustwiamy również zakres późniejszego czasu zwalniania.
			IA_TSV_SLOW_RandomA = Random.Range( 1.0f, 3.0f );
			IA_TSV_SLOW_RandomB = Random.Range( 4.0f, 6.0f );

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(5.0f, 8.0f);
            IA_TSV_FASTER_RandomB = Random.Range(8.0f, 10.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range( 1.2f, 2.1f );
		}
		else if( IA_ModeCharacter == 2 ) 
		{
			IA_SlowStart = Random.Range( 0, 2 );
			IA_TimeWaitToStart = Random.Range( 0.0f, 0.1f );

			// Losujemy czas przyspieszenia - jednak będzie to trwało krócej, ze względu na poziom 2
			IA_TimerSpeedVelocity = Random.Range( 6.0f, 11.0f );

			// Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit jest mniejszy.
			IA_SpeedLimterAllStages = Random.Range ( 40.0f, 45.0f );

			// Ustwiamy również zakres późniejszego czasu zwalniania.
			IA_TSV_SLOW_RandomA = Random.Range( 1.0f, 2.0f );
			IA_TSV_SLOW_RandomB = Random.Range( 2.0f, 3.0f );

			// Jak i ustawiamy czas późniejszego przyspieszania
			IA_TSV_FASTER_RandomA = Random.Range( 6.0f, 9.0f );
			IA_TSV_FASTER_RandomB = Random.Range( 9.0f, 13.0f );

			// Ustawiamy współczynnik poprawki przyspieszania;
			IA_factorSpeedChanger = Random.Range( 2.0f, 2.4f );
		}
		else if( IA_ModeCharacter == 3 ) 
		{
			IA_SlowStart = 0;

			// Losujemy czas przyspieszenia. Tutaj parametry są większe;
			IA_TimerSpeedVelocity = Random.Range( 8.0f, 13.0f );

			// Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit ma najwęższy zakres i wysokie progi.
			IA_SpeedLimterAllStages = Random.Range ( 43.0f, 50.0f );

			// Ustwiamy również zakres późniejszego czasu zwalniania.
			IA_TSV_SLOW_RandomA = Random.Range( 0.0f, 1.0f );
			IA_TSV_SLOW_RandomB = Random.Range( 1.0f, 2.0f );

			// Jak i ustawiamy czas późniejszego przyspieszania
			IA_TSV_FASTER_RandomA = Random.Range( 9.0f, 14.0f );
			IA_TSV_FASTER_RandomB = Random.Range( 14.0f, 18.0f );

			// Ustawiamy współczynnik poprawki przyspieszania;
			IA_factorSpeedChanger = Random.Range( 2.0f, 3.0f );
		}
        IA_TransformPlayerCar = Camera.main.GetComponent<GameEngine>().TransformPlayerCar;
        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
        Debug.Log("------------> START <----------------");
    }

    void CheckerAndChangerMode()
    {
        // Jeśli Mode jest ustawiony na wolny (3) ustaw Wolny Start i zakres opóźnionego startu
        if (IA_ModeCharacter == 1)
        {
            // Losujemy czas przyspieszenia
            IA_TimerSpeedVelocity = Random.Range(1.0f, 5.0f);

            // Tutaj nie musimy ustawiać czasu przyspieszenia. Wystarczy, że ustawimy SpeedLimitera
            IA_SpeedLimterAllStages = Random.Range(40.0f, 46.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(4.0f, 6.0f);
            IA_TSV_SLOW_RandomB = Random.Range(5.0f, 8.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            // W tym wypadku ustawiamy zmienne na czas zwalniania aby nie przesadzić z zwalnianiem.
            IA_TSV_FASTER_RandomA = IA_TSV_SLOW_RandomA;
            IA_TSV_FASTER_RandomB = IA_TSV_SLOW_RandomB;

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(1.5f, 2.1f);
        }
        else if (IA_ModeCharacter == 2) // czy Mode jest Stały, przyspieszający. Tutaj pozwól losować czy wolny start ma być czy też nie i jego zakres.
        {
            IA_LikelyDistance = Random.Range(2.0f, 6.0f);
            IA_SpeedLimterAllStages = 45.0f;
            IA_StartLoopStage = true;
        }
        else if (IA_ModeCharacter == 3) // Mode == szybki więc tutaj nie ma opóźnionego startu
        {
            IA_LikelyDistance = Random.Range(0.5f, 2.0f);
            IA_SpeedLimterAllStages = 45.0f;
            IA_StartLoopStage = true;
        }

        /*
        if( StageTwoStart )
        {
            Debug.Log("Three START!");
            StageThreeStart = true;
        }
        StageTwoStart = true;
        */
    }

    public void MarkStats( string N, int C )
	{
		Name = N;
		Number = C;
	}

    // ZMIENNE PRÓBNE
    private float TimeRelaseKey = 0.0f;
    // Update is called once per frame
    void Update () 
	{
        // Pobieram tranform samochodu gracza
       // IA_TransformPlayerCar = Camera.main.GetComponent<GameEngine>().TransformPlayerCar;

        // Pobieram stan wyscigu - czy juz wystartowal badz sie zakonczyl
        canController = Camera.main.GetComponent< GameEngine >().STARTRACE;
		if( canController && !EndRaceForMe )
		{
			if( gameObject.transform.position.y > 640.0f )
			{
                Debug.Log("Oznaczam == " + Name);
				Camera.main.GetComponent< GameEngine >().MarkWinners( gameObject );
				EndRaceForMe = true;
			}

            if (PlayerController)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (TimeRelaseKey > 0.1f)
                    {
                        Debug.Log("Time To Relase Key jest mniejszy od 0.1f");
                        if (Speed > 0.0f)
                        {
                            Speed -= 0.1f - TimeRelaseKey;
                        }
                    }
                    else
                    {
                        if (TimeRelaseKey != 0)
                            Speed += Mathf.Abs(1.0f - TimeRelaseKey);
                    }
                    TimeRelaseKey = 0.0f;

                }

                // Działa gdy nie trzymamy przycisku, a gdy trzymamy to nie dziala czyli okej
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    TimeRelaseKey += Time.deltaTime;

                    if (Speed > 0.0f)
                    {
                        Speed -= TimeRelaseKey;
                    }
                }

                Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (Speed * Time.deltaTime));
                gameObject.transform.position = tempPos;

            }
            else // IA_PART
			{
                // Losowanie startu - kto wystartuje wolno. 
                // To dobry moment na losowanie etapu #1, bądź kontrolę tego etapu
                if ( IA_SlowStart == 1 )
				{
					IA_UpArrow = false;
					IA_TimeWaitToStart -= Time.deltaTime;
					if( IA_TimeWaitToStart < 0.0f )
					{
						IA_UpArrow = true;
						IA_SlowStart = -1; // Wyjście z warunku.
					}
				}

                if (IA_StartLoopStage)
                {
                    if (IA_ModeCharacter == 2)
                    {
                        if ((IA_TransformPlayerCar.position.y - gameObject.transform.position.y) > IA_LikelyDistance)// && !IA_SlowNow)
                        {
                            // zwieksz predkosc
                            factorSpeed = (factorSpeed + 1.5f * Time.deltaTime); // * Time.deltaTime;
                        }
                        else
                        {
                            // zmniejsz predkosc
                            factorSpeed = (factorSpeed - 1.0f * Time.deltaTime); // * Time.deltaTime;
                        }
                    }

                    if( IA_ModeCharacter == 3 )
                    {
                        if( ( gameObject.transform.position.y - IA_TransformPlayerCar.position.y  ) > IA_LikelyDistance)// && !IA_SlowNow)
                        {
                            // zmniejsz predkosc
                            factorSpeed = (factorSpeed - 1.5f * Time.deltaTime); // * Time.deltaTime;
                        }
                        else
                        {
                            // zwieksz predkosc
                            factorSpeed = (factorSpeed + 1.5f * Time.deltaTime); // * Time.deltaTime;
                        }
                    }

                    if(gameObject.transform.position.y >= IA_StagesPosition[2] && IA_SlowNow )
                    {
                        // zmniejsz predkosc
                        //factorSpeed = ( (factorSpeed-- ) * Time.deltaTime); // * Time.deltaTime;
                        //factorSpeed = (factorSpeed - 2.0f * Time.deltaTime); // * Time.deltaTime;
                        //IA_TimerToSlow = IA_TimerSpeedVelocity + 1.0f;
                        IA_NotUpArrow = true;
                        Debug.Log( Name + " ZWALNIAM! 2");
                    }
                }

                /*
                if( gameObject.transform.position.y >= IA_StagesPosition[2] && !StageThreeStart )
                {
                    Debug.Log("ZWALNIAM 1");
                    IA_SlowNow = true;
                    StageThreeStart = true;
                }
                */

                if (gameObject.transform.position.y >= IA_StagesPosition[0] && !StageTwoStart )
                {
                    Debug.Log( Name + " --> ETAP 2 --> Zmiana stanu po = " + IA_StagesPosition[0] + " --> pierwszy odcinek za nami");
                    // 1 = Szybki, 2 = Stały, przyspieszający (delikatnie), 3 = Wolny
                    // W ZMIANACH PRZEPROWADZONYCH W TYCH WARUNKACH - NALEŻY PAMIĘTAC O ZMIANACH STANÓW !!!
                    if ( IA_ModeCharacter == 3 ) // jeśli mój stan to 1 = szybki
                    {
                        // trzeba wylosować stałą prędkość - lub zwolnienie. Samo to czy to, też musi być losowane
                        IA_ModeCharacter = 3; // Random.Range(2, 4);
                        CheckerAndChangerMode();
                        StageTwoStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if( IA_ModeCharacter == 2 ) // jeśli mój stan to 2 = stały, przyspieszający
                    {
                        // trzeba wylosować stałą prędkość lub przyspieszenie.
                        IA_ModeCharacter = Random.Range(2, 4);
                        CheckerAndChangerMode();
                        StageTwoStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if( IA_ModeCharacter == 1 ) // jeśli mój stan to 3 = wolny
                    {
                        // Trzeba nakreślić przyspieszenie. Wartość przyspieszenia należy wylosować
                        IA_ModeCharacter = 1;
                        CheckerAndChangerMode();
                        StageTwoStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }

                    
                }

                if (gameObject.transform.position.y >= IA_StagesPosition[1] && !StageThreeStart )
                {
                    Debug.Log(Name + " --> ETAP 3 --> Zmiana stanu po = " + IA_StagesPosition[0] + " --> pierwszy odcinek za nami");
                    IA_SlowNow = true;
                    // 1 = Szybki, 2 = Stały, przyspieszający (delikatnie), 3 = Wolny
                    // W ZMIANACH PRZEPROWADZONYCH W TYCH WARUNKACH - NALEŻY PAMIĘTAC O ZMIANACH STANÓW !!!
                    if (IA_ModeCharacter == 3) // jeśli mój stan to 1 = szybki
                    {
                        // trzeba wylosować stałą prędkość - lub zwolnienie. Samo to czy to, też musi być losowane
                        IA_ModeCharacter = 3;
                        CheckerAndChangerMode();
                        StageThreeStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if (IA_ModeCharacter == 2) // jeśli mój stan to 2 = stały, przyspieszający
                    {
                        // trzeba wylosować stałą prędkość lub przyspieszenie.
                        IA_ModeCharacter = Random.Range(2, 4);
                        CheckerAndChangerMode();
                        StageThreeStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if (IA_ModeCharacter == 1) // jeśli mój stan to 3 = wolny
                    {
                        // Trzeba nakreślić przyspieszenie. Wartość przyspieszenia należy wylosować
                        IA_ModeCharacter = 1;
                        CheckerAndChangerMode();
                        StageThreeStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                }

                    // "Symulacja" puszczania gazu. Zmienna IA_factorySpeedChanger - jako modyfikator trudności
                    // Ta część kodu mogłaby zostać. Ze względu na jej losowość. Jednak trzeba podkręcić trudność.
                    // Warto też zwrócić uwagę na to, że tutaj cały czas jest powiększany modyfikator. 
                    // Czyli trzeba pilnować aby nie zaszedł za daleko. 
                    IA_TimerToSlow += Time.deltaTime;
				if( IA_TimerToSlow > IA_TimerSpeedVelocity && IA_ModeCharacter == 1 && !IA_NotUpArrow )
				{
					IA_UpArrow = !IA_UpArrow;

					if( !IA_UpArrow ) // Czas zwalniania
					{
						IA_TimerSpeedVelocity = Random.Range( IA_TSV_SLOW_RandomA, IA_TSV_SLOW_RandomB );
					}
					else // Czas przyspieszania z nową poprawką na prędkość
					{
						//IA_factorSpeedChanger = 1.025f;
						IA_TimerSpeedVelocity = Random.Range( IA_TSV_SLOW_RandomA, IA_TSV_SLOW_RandomB );
					}
					IA_TimerToSlow = 0.0f;

				}
                else if( IA_NotUpArrow )
                {
                    IA_UpArrow = false;
                    Debug.Log("ZWALNIAM TERAZ NOW!");
                }

				// Ten warunek jest potrzebny ponieważ załącza się za każdym wciśnięciem gazu - czyli pojazd przyspiesza
				// z prędkości jaką akutalnie posiada. A nie hamuje i na nowo przypisuje prędkość.
				if( IA_UpArrow )
				{

					Speed = IA_factorSpeedChanger;
				}

				// Tutaj główna symulacja wciśnięcia gazu. Input.GetKey - jak u Playera. 
				// Modyfikowany jest tutaj kolejny współczynnik odpowiedzialny za przyspieszanie.
				// Tutaj też jest ustalany Limit Prędkości 
				if( IA_UpArrow )
				{
					if( !SpeedChecker )
					{
						Speed = IA_factorSpeedChanger;
						//Debug.Log( "Speed = " + Speed );
					}
					
					Timer += Time.deltaTime;
					if( Timer > 0.5f && factorSpeed <= IA_SpeedLimterAllStages )
					{
						factorSpeed++;
						//Speed += factorSpeed * Time.deltaTime;
;
						Timer = 0.0f;
					}
					tempSpeed = Speed * factorSpeed * Time.deltaTime;
					Vector2 tempPos = new Vector2( gameObject.transform.position.x, gameObject.transform.position.y + ( Speed * factorSpeed * Time.deltaTime ) );
					gameObject.transform.position = tempPos;
					
				}
				else
				{
					// Ten warunek {else} działa gdy odpuszczamy gaz
					// Tutaj samochód zwalnia delikatnie. 

					SpeedChecker = true;
					
					if( tempSpeed <= 0 )
					{
						SpeedChecker = false;
					}
					
					if( tempSpeed > 0 )
					{
						TimerSlow += Time.deltaTime;
						
						if( TimerSlow > 0.5f )
						{
                            if (!IA_NotUpArrow)
                            {
                                factorSpeed -= 1.0f * Time.deltaTime;
                            }
                            else
                            {
                                factorSpeed -= 2.5f * Time.deltaTime;
                            }
							tempSpeed -= Time.deltaTime;
							TimerSlow = 0.0f;
						}
						
						Vector2 tempPos = new Vector2( gameObject.transform.position.x, gameObject.transform.position.y + ( 1.0f * tempSpeed ) );
						gameObject.transform.position = tempPos;
					}
				}
			}
		}

		if( EndRaceForMe )
		{
			SpeedChecker = true;
			
			if( tempSpeed <= 0 )
			{
				SpeedChecker = false;
			}

			if( tempSpeed > 0 )
			{
				TimerSlow += Time.deltaTime;
				
				if( TimerSlow > 0.2f )
				{
					factorSpeed -= 1.0f;
					tempSpeed = tempSpeed / 2;
					TimerSlow = 0.0f;
				}
				
				Vector2 tempPos = new Vector2( gameObject.transform.position.x, gameObject.transform.position.y + ( 1.0f * tempSpeed ) );
				gameObject.transform.position = tempPos;
			}
		}
	
	}
}