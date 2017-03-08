using UnityEngine;
using System.Collections;

public class NewCarControllerScript : MonoBehaviour
{
    public string Name = "";
    public int Number = 0;

    public bool canController = false;
    public bool PlayerController = false;

    private float Timer = 0.0f;
    private float tempSpeed = 0.0f;
    private float TimerSlow = 0.0f;
    private float Speed = 0.0f;
    private bool SpeedChecker = false;
    private bool EndRaceForMe = false;

    // Stage #1 Vars
    private float IA_SO_SpeedVelocity = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    public void MarkStats(string N, int C)
    {
        Name = N;
        Number = C;
    }

    // ZMIENNE PRÓBNE
    private float TimeRelaseKey = 0.0f;


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
                if ( !Input.GetKey( KeyCode.UpArrow ) )
                {
                    TimeRelaseKey += Time.deltaTime;

                    if (Speed > 0.0f)
                    {
                        Speed -= TimeRelaseKey;
                    }
                }

                // To jest jednorazowe 
                if ( Input.GetKeyUp( KeyCode.UpArrow ) )
                {

                }
                //Debug.Log("CounterAll = " + CounterAll + " TimerDisKey = " + tempTimerDisKey );

                Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (Speed * Time.deltaTime));
                gameObject.transform.position = tempPos;

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
                    tempSpeed = tempSpeed / 2;
                    TimerSlow = 0.0f;
                }

                Vector2 tempPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (1.0f * tempSpeed));
                gameObject.transform.position = tempPos;
            }
        }

    }
}
