using UnityEngine;
using System.Collections;

public class TargetScriptCarController : MonoBehaviour
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
    private int[] IA_StagesPosition = new int[2];
    private bool StageTwoStart = false;
    private bool StageThreeStart = false;

    // Stage #1 Vars
    private float IA_SO_SpeedVelocity = 0.0f;

    // Use this for initialization
    void Start()
    {
        IA_StagesPosition[0] = 200;
        IA_StagesPosition[1] = 400;
        //IA_RangeTimerToSlow = Random.Range( 2.0f, 16.0f ); // Czas do zwolnienia
        //IA_SlowStart = Random.Range( 0, 2 ); // Czy mam ruszyć wolno?
        //IA_TimeWaitToStart = Random.Range( 0.0f, 1.1f ); // Modyfikator startu

        // Etap #1

        /* TODO:
		 * Czy w stanie szybkim i pośrednim ma być zwalnianie. Ogólnie czy zwalnane ma się odbywać (przez odpuszczenie gazu)
		 * Co ze współczynnikami (dwoma!) przyspieszenia - i kiedy jest modyfikowany
		 * 
		 */

        // Losowanie Mode
        IA_ModeCharacter = Random.Range(1, 4);

        // Jeśli Mode jest ustawiony na wolny (3) ustaw Wolny Start i zakres opóźnionego startu
        if (IA_ModeCharacter == 3)
        {
            IA_SlowStart = 1;
            IA_TimeWaitToStart = Random.Range(0.2f, 1.0f);

            // Tutaj nie musimy ustawiać czasu przyspieszenia. Wystarczy, że ustawimy SpeedLimitera
            IA_SpeedLimterAllStages = Random.Range(28.0f, 35.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(2.0f, 4.0f);
            IA_TSV_SLOW_RandomB = Random.Range(5.0f, 7.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            // W tym wypadku ustawiamy zmienne na czas zwalniania aby nie przesadzić z zwalnianiem.
            IA_TSV_FASTER_RandomA = Random.Range(4.0f, 6.0f);
            IA_TSV_FASTER_RandomB = Random.Range(7.0f, 9.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(1.2f, 1.9f);
        }
        else if (IA_ModeCharacter == 2) // czy Mode jest Stały, przyspieszający. Tutaj pozwól losować czy wolny start ma być czy też nie i jego zakres.
        {
            IA_SlowStart = Random.Range(0, 2);
            IA_TimeWaitToStart = Random.Range(0.0f, 0.8f);

            // Losujemy czas przyspieszenia - jednak będzie to trwało krócej, ze względu na poziom 2
            IA_TimerSpeedVelocity = Random.Range(2.0f, 4.0f);

            // Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit jest mniejszy.
            IA_SpeedLimterAllStages = Random.Range(30.0f, 35.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(2.0f, 4.0f);
            IA_TSV_SLOW_RandomB = Random.Range(3.0f, 4.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(2.0f, 4.0f);
            IA_TSV_FASTER_RandomB = Random.Range(4.0f, 8.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(0.9f, 1.8f);
        }
        else if (IA_ModeCharacter == 1) // Mode == szybki więc tutaj nie ma opóźnionego startu
        {
            IA_SlowStart = 0;

            // Losujemy czas przyspieszenia. Tutaj parametry są większe;
            IA_TimerSpeedVelocity = Random.Range(2.0f, 5.0f);

            // Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit ma najwęższy zakres i wysokie progi.
            IA_SpeedLimterAllStages = Random.Range(33.0f, 40.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(1.0f, 3.0f);
            IA_TSV_SLOW_RandomB = Random.Range(3.0f, 6.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(4.0f, 6.0f);
            IA_TSV_FASTER_RandomB = Random.Range(6.0f, 9.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(1.5f, 2.1f);
        }
        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
        Debug.Log("------------> START <----------------");
    }

    void CheckerAndChangerModeSTAGE_TWO()
    {
        // Jeśli Mode jest ustawiony na wolny (3) ustaw Wolny Start i zakres opóźnionego startu
        if (IA_ModeCharacter == 3)
        {
            // Losujemy czas przyspieszenia
            IA_TimerSpeedVelocity = Random.Range(1.0f, 2.0f);

            // Tutaj nie musimy ustawiać czasu przyspieszenia. Wystarczy, że ustawimy SpeedLimitera
            IA_SpeedLimterAllStages = Random.Range(24.0f, 31.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(4.0f, 6.0f);
            IA_TSV_SLOW_RandomB = Random.Range(5.0f, 8.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            // W tym wypadku ustawiamy zmienne na czas zwalniania aby nie przesadzić z zwalnianiem.
            IA_TSV_FASTER_RandomA = IA_TSV_SLOW_RandomA;
            IA_TSV_FASTER_RandomB = IA_TSV_SLOW_RandomB;

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(0.8f, 1.0f);
        }
        else if (IA_ModeCharacter == 2) // czy Mode jest Stały, przyspieszający. Tutaj pozwól losować czy wolny start ma być czy też nie i jego zakres.
        {
            // Losujemy czas przyspieszenia - jednak będzie to trwało krócej, ze względu na poziom 2
            IA_TimerSpeedVelocity = Random.Range(0.0f, 3.0f);

            // Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit jest mniejszy.
            IA_SpeedLimterAllStages = Random.Range(28.0f, 33.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(0.0f, 4.0f);
            IA_TSV_SLOW_RandomB = Random.Range(0.0f, 4.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(0.0f, 4.0f);
            IA_TSV_FASTER_RandomB = Random.Range(0.0f, 9.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(0.9f, 2.0f);
        }
        else if (IA_ModeCharacter == 1) // Mode == szybki więc tutaj nie ma opóźnionego startu
        {
            // Losujemy czas przyspieszenia. Tutaj parametry są większe;
            IA_TimerSpeedVelocity = Random.Range(3.0f, 5.0f);

            // Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit ma najwęższy zakres i wysokie progi.
            IA_SpeedLimterAllStages = Random.Range(32.0f, 39.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(2.0f, 4.0f);
            IA_TSV_SLOW_RandomB = Random.Range(3.0f, 6.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(2.0f, 4.0f);
            IA_TSV_FASTER_RandomB = Random.Range(3.0f, 6.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(1.2f, 1.9f);
        }

    }

    void CheckerAndChangerModeSTAGE_THREE()
    {
        // Jeśli Mode jest ustawiony na wolny (3) ustaw Wolny Start i zakres opóźnionego startu
        if (IA_ModeCharacter == 3)
        {
            // Losujemy czas przyspieszenia
            IA_TimerSpeedVelocity = Random.Range(0.0f, 2.0f);

            // Tutaj nie musimy ustawiać czasu przyspieszenia. Wystarczy, że ustawimy SpeedLimitera
            IA_SpeedLimterAllStages = Random.Range(22.0f, 30.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(4.0f, 6.0f);
            IA_TSV_SLOW_RandomB = Random.Range(6.0f, 7.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            // W tym wypadku ustawiamy zmienne na czas zwalniania aby nie przesadzić z zwalnianiem.
            IA_TSV_FASTER_RandomA = Random.Range(0.0f, 3.0f);
            IA_TSV_FASTER_RandomB = Random.Range(0.0f, 7.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(0.8f, 1.0f);
        }
        else if (IA_ModeCharacter == 2) // czy Mode jest Stały, przyspieszający. Tutaj pozwól losować czy wolny start ma być czy też nie i jego zakres.
        {
            // Losujemy czas przyspieszenia - jednak będzie to trwało krócej, ze względu na poziom 2
            IA_TimerSpeedVelocity = Random.Range(0.0f, 3.0f);

            // Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit jest mniejszy.
            IA_SpeedLimterAllStages = Random.Range(28.0f, 33.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(0.0f, 4.0f);
            IA_TSV_SLOW_RandomB = Random.Range(0.0f, 4.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(0.0f, 4.0f);
            IA_TSV_FASTER_RandomB = Random.Range(0.0f, 9.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(0.0f, 2.0f);
        }
        else if (IA_ModeCharacter == 1) // Mode == szybki więc tutaj nie ma opóźnionego startu
        {
            // Losujemy czas przyspieszenia. Tutaj parametry są większe;
            IA_TimerSpeedVelocity = Random.Range(2.0f, 3.0f);

            // Losujemy wartość Speed Limitera czyli limitu prędkości. Tutaj limit ma najwęższy zakres i wysokie progi.
            IA_SpeedLimterAllStages = Random.Range(35.0f, 42.0f);

            // Ustwiamy również zakres późniejszego czasu zwalniania.
            IA_TSV_SLOW_RandomA = Random.Range(1.0f, 2.0f);
            IA_TSV_SLOW_RandomB = Random.Range(2.0f, 4.0f);

            // Jak i ustawiamy czas późniejszego przyspieszania
            IA_TSV_FASTER_RandomA = Random.Range(2.0f, 3.5f);
            IA_TSV_FASTER_RandomB = Random.Range(4.0f, 5.0f);

            // Ustawiamy współczynnik poprawki przyspieszania;
            IA_factorSpeedChanger = Random.Range(1.4f, 1.8f);
        }

    }

    public void MarkStats(string N, int C)
    {
        Name = N;
        Number = C;
    }

    // Update is called once per frame
    void Update()
    {
        canController = Camera.main.GetComponent<GameEngine>().STARTRACE;
        if (canController && !EndRaceForMe)
        {
            if (gameObject.transform.position.y > 640.0f)
            {
                Camera.main.GetComponent<GameEngine>().MarkWinners(gameObject);
                EndRaceForMe = true;
            }

            if (PlayerController)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Speed = 1.0f;
                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (!SpeedChecker)
                    {
                        Speed = Input.GetAxis("Vertical");
                        //Debug.Log( "Speed = " + Speed );
                    }

                    Timer += Time.deltaTime;
                    if (Timer > 0.5f && factorSpeed <= 30.0f)
                    {
                        factorSpeed++;
                        //Speed += factorSpeed * Time.deltaTime;
                        //Debug.Log( "ALL Speed = " + Speed * Time.deltaTime * factorSpeed  );
                        Timer = 0.0f;
                    }
                    tempSpeed = Speed * factorSpeed * Time.deltaTime;
                    Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (Speed * factorSpeed * Time.deltaTime));
                    gameObject.transform.position = tempPos;

                }
                else
                {
                    SpeedChecker = true;

                    if (tempSpeed <= 0)
                    {
                        SpeedChecker = false;
                    }



                    if (tempSpeed > 0)
                    {
                        TimerSlow += Time.deltaTime;

                        if (TimerSlow > 0.5f)
                        {
                            factorSpeed -= 1.0f;
                            tempSpeed -= Time.deltaTime;
                            TimerSlow = 0.0f;
                        }

                        Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (1.0f * tempSpeed));
                        gameObject.transform.position = tempPos;
                    }
                }
            }
            else // IA_PART
            {
                // Losowanie startu - kto wystartuje wolno. 
                // To dobry moment na losowanie etapu #1, bądź kontrolę tego etapu
                if (IA_SlowStart == 1)
                {
                    IA_UpArrow = false;
                    IA_TimeWaitToStart -= Time.deltaTime;
                    if (IA_TimeWaitToStart < 0.0f)
                    {
                        IA_UpArrow = true;
                        IA_SlowStart = -1; // Wyjście z warunku.
                    }
                }

                if (gameObject.transform.position.y >= IA_StagesPosition[0] && !StageTwoStart)
                {
                    Debug.Log(Name + " --> ETAP 2 --> Zmiana stanu po = " + IA_StagesPosition[0] + " --> pierwszy odcinek za nami");
                    // 1 = Szybki, 2 = Stały, przyspieszający (delikatnie), 3 = Wolny
                    // W ZMIANACH PRZEPROWADZONYCH W TYCH WARUNKACH - NALEŻY PAMIĘTAC O ZMIANACH STANÓW !!!
                    if (IA_ModeCharacter == 1) // jeśli mój stan to 1 = szybki
                    {
                        // trzeba wylosować stałą prędkość - lub zwolnienie. Samo to czy to, też musi być losowane
                        IA_ModeCharacter = Random.Range(2, 4);
                        CheckerAndChangerModeSTAGE_TWO();
                        StageTwoStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if (IA_ModeCharacter == 2) // jeśli mój stan to 2 = stały, przyspieszający
                    {
                        // trzeba wylosować stałą prędkość lub przyspieszenie.
                        IA_ModeCharacter = Random.Range(1, 3);
                        CheckerAndChangerModeSTAGE_TWO();
                        StageTwoStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if (IA_ModeCharacter == 3) // jeśli mój stan to 3 = wolny
                    {
                        // Trzeba nakreślić przyspieszenie. Wartość przyspieszenia należy wylosować
                        IA_ModeCharacter = 1;
                        CheckerAndChangerModeSTAGE_TWO();
                        StageTwoStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }


                }

                if (gameObject.transform.position.y >= IA_StagesPosition[1] && !StageThreeStart)
                {
                    Debug.Log(Name + " --> ETAP 3 --> Zmiana stanu po = " + IA_StagesPosition[0] + " --> pierwszy odcinek za nami");
                    // 1 = Szybki, 2 = Stały, przyspieszający (delikatnie), 3 = Wolny
                    // W ZMIANACH PRZEPROWADZONYCH W TYCH WARUNKACH - NALEŻY PAMIĘTAC O ZMIANACH STANÓW !!!
                    if (IA_ModeCharacter == 1) // jeśli mój stan to 1 = szybki
                    {
                        // trzeba wylosować stałą prędkość - lub zwolnienie. Samo to czy to, też musi być losowane
                        IA_ModeCharacter = 3;
                        CheckerAndChangerModeSTAGE_THREE();
                        StageThreeStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if (IA_ModeCharacter == 2) // jeśli mój stan to 2 = stały, przyspieszający
                    {
                        // trzeba wylosować stałą prędkość lub przyspieszenie.
                        IA_ModeCharacter = 3;
                        CheckerAndChangerModeSTAGE_THREE();
                        StageThreeStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                    else if (IA_ModeCharacter == 3) // jeśli mój stan to 3 = wolny
                    {
                        // Trzeba nakreślić przyspieszenie. Wartość przyspieszenia należy wylosować
                        IA_ModeCharacter = 1;
                        CheckerAndChangerModeSTAGE_THREE();
                        StageThreeStart = true;
                        Debug.Log("Moja nazwa = " + Name + ", Moj akutalny stan to = " + IA_ModeCharacter);
                    }
                }

                // "Symulacja" puszczania gazu. Zmienna IA_factorySpeedChanger - jako modyfikator trudności
                // Ta część kodu mogłaby zostać. Ze względu na jej losowość. Jednak trzeba podkręcić trudność.
                // Warto też zwrócić uwagę na to, że tutaj cały czas jest powiększany modyfikator. 
                // Czyli trzeba pilnować aby nie zaszedł za daleko. 
                IA_TimerToSlow += Time.deltaTime;
                if (IA_TimerToSlow > IA_TimerSpeedVelocity)
                {
                    IA_UpArrow = !IA_UpArrow;

                    if (!IA_UpArrow) // Czas zwalniania
                    {
                        IA_TimerSpeedVelocity = Random.Range(IA_TSV_SLOW_RandomA, IA_TSV_SLOW_RandomB);
                    }
                    else // Czas przyspieszania z nową poprawką na prędkość
                    {
                        //IA_factorSpeedChanger = 1.025f;
                        IA_TimerSpeedVelocity = Random.Range(IA_TSV_SLOW_RandomA, IA_TSV_SLOW_RandomB);
                    }
                    IA_TimerToSlow = 0.0f;

                }

                // Ten warunek jest potrzebny ponieważ załącza się za każdym wciśnięciem gazu - czyli pojazd przyspiesza
                // z prędkości jaką akutalnie posiada. A nie hamuje i na nowo przypisuje prędkość.
                if (IA_UpArrow)
                {

                    Speed = IA_factorSpeedChanger;
                }

                // Tutaj główna symulacja wciśnięcia gazu. Input.GetKey - jak u Playera. 
                // Modyfikowany jest tutaj kolejny współczynnik odpowiedzialny za przyspieszanie.
                // Tutaj też jest ustalany Limit Prędkości 
                if (IA_UpArrow)
                {
                    if (!SpeedChecker)
                    {
                        Speed = IA_factorSpeedChanger;
                        //Debug.Log( "Speed = " + Speed );
                    }

                    Timer += Time.deltaTime;
                    if (Timer > 0.5f && factorSpeed <= IA_SpeedLimterAllStages)
                    {
                        factorSpeed++;
                        //Speed += factorSpeed * Time.deltaTime;
                        ;
                        Timer = 0.0f;
                    }
                    tempSpeed = Speed * factorSpeed * Time.deltaTime;
                    Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (Speed * factorSpeed * Time.deltaTime));
                    gameObject.transform.position = tempPos;

                }
                else
                {
                    // Ten warunek {else} działa gdy odpuszczamy gaz
                    // Tutaj samochód zwalnia delikatnie. 

                    SpeedChecker = true;

                    if (tempSpeed <= 0)
                    {
                        SpeedChecker = false;
                    }

                    if (tempSpeed > 0)
                    {
                        TimerSlow += Time.deltaTime;

                        if (TimerSlow > 0.5f)
                        {
                            factorSpeed -= 1.0f * Time.deltaTime;
                            tempSpeed -= Time.deltaTime;
                            TimerSlow = 0.0f;
                        }

                        Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (1.0f * tempSpeed));
                        gameObject.transform.position = tempPos;
                    }
                }
            }
        }

        if (EndRaceForMe)
        {
            SpeedChecker = true;

            if (tempSpeed <= 0)
            {
                SpeedChecker = false;
            }

            if (tempSpeed > 0)
            {
                TimerSlow += Time.deltaTime;

                if (TimerSlow > 0.2f)
                {
                    factorSpeed -= 1.0f;
                    tempSpeed = tempSpeed / 2;
                    TimerSlow = 0.0f;
                }

                Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (1.0f * tempSpeed));
                gameObject.transform.position = tempPos;
            }
        }

    }
}
