using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTamama : MonoBehaviour
{
    public byte TamamaVoiceCount;           // 타마마의 울음소리를 일정 간격 후에 출력하는 것을 도와주는 변수입니다.
    System.Random TamamaVoiceCycle;     // 타마마의 울음소리 간격을 설정하는 변수입니다
    GameManager manager;        // 게임 매니저의 요소값을 빌려오기 위한 객체를 생성합니다
    public float XSpeed;                // 타마마가 x좌표로 이동할 때 속도를 정하는 변수입니다
    public float YSpeed;                   // 타마마가 y좌표로 이동할 때 속도를 정하는 변수입니다
    public float XTemp;                     // 타마마의 현재 X 좌표를 임시로 따오는 변수입니다
    public float YTemp;                     // 타마마의 현재 Y 좌표를 임시로 따오는 변수입니다.
    public sbyte XCount;                     // 타마마가 x좌표로 몇 칸 이동했는지 알기 위한 변수입니다.
    public sbyte YCount;                     // 타마마가 y좌표로 몇 칸 이동했는지 알기 위한 변수입니다.
    public bool[] UnbalancedCookie;                 // 쿠키가 위에 있는데도 불구하고 밑의 쿠키를 먹었는 지 알 수 있는 변수입니다.
    public bool[] AllOfCookieEaten;                      // 쿠키를 전부 다 먹었는지 알 수 있게 하기 위한 변수입니다.
    public sbyte[] TempCookieEaten;                    // 쿠키를 전부 다 먹었는지 확인하기 위한 임시 변수입니다.

    // Start is called before the first frame update
    void Start()
    {
        XSpeed = 3f;
        YSpeed = 2.5f;
        XCount = 8;
        YCount = 1;
        TamamaVoiceCount = 0;
        TamamaVoiceCycle = new System.Random();
        TempCookieEaten = new sbyte[6] { 0, 0, 0, 0, 0, 0 };
        AllOfCookieEaten = new bool[6] { false, false, false, false, false, false };
        UnbalancedCookie = new bool[6] { false, false, false, false, false, false };

        manager = this.gameObject.AddComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.GameStart == true && Input.GetKeyDown(KeyCode.W) && YCount <= 4 && manager.GameOver == false)
        {
            XTemp = this.gameObject.transform.position.x;                           // 현재 타마마의 x좌표를 temp에 담습니다
            YTemp = this.gameObject.transform.position.y + YSpeed;             // 현재 타마마의 y좌표에 YSpeed를 더해줍니다.
            transform.position = new Vector3(XTemp, YTemp, 0);                  // 구해낸 X, Y 좌표를 타마마의 좌표에 대입합니다
            transform.localScale = new Vector3(0.8f, 0.8f, 1);                          // 이는 Scale_X 좌표에 -1을 곱하면 좌우반전이 되는 것에서 착안하였습니다. 타마마가 오른쪽으로 갈 시 좌우반전 됩니다.

            ++YCount;
            if (manager.CookieArray[YCount, XCount].gameObject.activeSelf == true && XCount <= 8)
            {
                manager.CookieArray[YCount, XCount].gameObject.SetActive(false);
            }

            BreakDownCheck();
        }

        else if (manager.GameStart == true && Input.GetKeyDown(KeyCode.A) && XCount >= 2 && manager.GameOver == false)
        {
            XTemp = this.gameObject.transform.position.x - XSpeed;
            YTemp = this.gameObject.transform.position.y;
            transform.position = new Vector3(XTemp, YTemp, 0);
            transform.localScale = new Vector3(0.8f, 0.8f, 1);

            XCount--;
            if (XCount != 8 && manager.CookieArray[YCount, XCount].gameObject.activeSelf == true)
            {
                manager.CookieArray[YCount, XCount].gameObject.SetActive(false);
            }

            BreakDownCheck();
        }

        else if (manager.GameStart == true && Input.GetKeyDown(KeyCode.S) && YCount >= 2 && manager.GameOver == false)
        {
            XTemp = this.gameObject.transform.position.x;
            YTemp = this.gameObject.transform.position.y - YSpeed;
            transform.position = new Vector3(XTemp, YTemp, 0);
            transform.localScale = new Vector3(0.8f, 0.8f, 1);

            --YCount;
            if (manager.CookieArray[YCount, XCount].gameObject.activeSelf == true && XCount <= 7)
            {
                manager.CookieArray[YCount, XCount].gameObject.SetActive(false);
            }

            BreakDownCheck();
        }

        else if (manager.GameStart == true && Input.GetKeyDown(KeyCode.D) && XCount <= 7 && manager.GameOver == false)
        {
            XTemp = this.gameObject.transform.position.x + XSpeed;
            YTemp = this.gameObject.transform.position.y;
            transform.position = new Vector3(XTemp, YTemp, 0);
            transform.localScale = new Vector3(-0.8f, 0.8f, 1);

            XCount++;
            if (XCount != 8 && manager.CookieArray[YCount, XCount].gameObject.activeSelf == true)
            {
                manager.CookieArray[YCount, XCount].gameObject.SetActive(false);
            }

            BreakDownCheck();
        }



    }

    //void OnTriggerEnter2D(Collider2D target)
    //{
    //    if (target.gameObject.tag == "Cookie" && target.gameObject.activeSelf == true)
    //    {
    //        Debug.Log("Collider");
    //        target.gameObject.SetActive(false);
    //    }

    //}

    void BreakDownCheck()       // 과자가 불균형하여 무너지는지 판단하여주는 함수입니다.
    {
        if( manager.CookieArray[YCount, XCount].gameObject.activeSelf == false)
        {
            foreach (int y in manager.range(manager.CookieArray_Y))
                    {
                        foreach (int x in manager.range(manager.CookieArray_X))
                        {
                            if (manager.CookieArray[y, x].gameObject.activeSelf == false)
                            {
                                TempCookieEaten[y]++;
                            }
                        }
            }

                    for (int x = 1; x <= 5; x++)
                    {
                        if(TempCookieEaten[x] == 7)
                        {
                            AllOfCookieEaten[x] = true;
                        }
                    }

                    if(YCount != 5)
                    {
                        if (AllOfCookieEaten[YCount + 1] == false && AllOfCookieEaten[YCount] == true)
                        {
                            manager.GameOverBreakDown();
                        }
                        else if(AllOfCookieEaten[YCount + 1] == false && TempCookieEaten[YCount] > 0)
                        {
                            manager.BeforeBreakCookies();
                        }
                    }
        
                    TempCookieEaten = new sbyte [6] { 0, 0, 0, 0, 0, 0 };
        }
    }

}
