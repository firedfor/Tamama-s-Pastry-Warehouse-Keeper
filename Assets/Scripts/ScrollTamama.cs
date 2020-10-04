using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public bool[] AllOfCookieEaten;                      // 쿠키를 전부 다 먹었는지 알 수 있게 하기 위한 변수입니다.
    public sbyte[] TempCookieEaten;                    // 쿠키를 전부 다 먹었는지 확인하기 위한 임시 변수입니다.
    public bool[, ] IsCookieEaten;                            // 쿠키를 먹었는지 먹지 않았는지 체크하는 변수입니다.
    public float ScrollTimer;                                              // 시간을 재주어 타마마가 너무 빠르게 움직이지 않도록 조절해주는 것을 도와주는 변수입니다.
    public float TimeLimit;                                     // 타마마가 이동할 간격을 띄워주는 것을 도와줄 변수입니다.
    public sbyte CookieCount;                           // 쿠키를 몇개 먹었는지 실시간으로 세어줄 변수입니다

    // Start is called before the first frame update
    void Start()
    {
        XSpeed = 3f;
        YSpeed = 2.5f;
        XCount = 11;
        YCount = 1;     
        TamamaVoiceCount = 0;
        TamamaVoiceCycle = new System.Random();
        TempCookieEaten = new sbyte[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        AllOfCookieEaten = new bool[11] { false, false, false, false, false, false, false, false, false, false, false };
        IsCookieEaten = new bool[8, 11];
        ScrollTimer = 0;
        TimeLimit = 0f;

        manager = this.gameObject.AddComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ScrollTimer += Time.deltaTime;

        if (manager.GameStart == true && Input.GetKeyDown(KeyCode.W) && YCount <= 6 && manager.GameOver == false && ScrollTimer > TimeLimit)
        {
            ScrollTimer = 0;                                                                                      // 타이머 변수를 초기화 시켜줍니다

            XTemp = this.gameObject.transform.position.x;                           // 현재 타마마의 x좌표를 temp에 담습니다
            YTemp = this.gameObject.transform.position.y + YSpeed;             // 현재 타마마의 y좌표에 YSpeed를 더해줍니다.
            transform.position = new Vector3(XTemp, YTemp, 0);                  // 구해낸 X, Y 좌표를 타마마의 좌표에 대입합니다
            transform.localScale = new Vector3(0.8f, 0.8f, 1);                          // 이는 Scale_X 좌표에 -1을 곱하면 좌우반전이 되는 것에서 착안하였습니다. 타마마가 오른쪽으로 갈 시 좌우반전 됩니다.

            ++YCount;                                                                                       // 내부 변수 내 Y 좌표에 1을 더합니다

            if (manager.CookieArray[YCount, XCount].gameObject.activeSelf == true && XCount <= 11)                                   //  실제로 활성화 된 쿠키인지 체크합니다
            {
                IsCookieEaten[YCount, XCount] = true;                                                                                                                       // 각각 쿠키를 먹었는지 확인하는 변수에 true를 할당합니다
                Invoke("SetActiveFalse", 0.15f);                                                                                                                                    // 쿠키를 0.15초 뒤에 비활성화 시켜줍니다
                iTween.ShakePosition(manager.CookieArray[YCount, XCount], iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.15f));            // 흔들림 트위닝 효과를 줍니다
                iTween.FadeTo(manager.CookieArray[YCount, XCount], 0, 0.15f);                                                                                   // 쿠키를 페이드 아웃 시켜줍니다                
            }

            BreakDownCheck();
        }

        else if (manager.GameStart == true && Input.GetKeyDown(KeyCode.A) && XCount >= 2 && manager.GameOver == false && ScrollTimer > TimeLimit)
        {
            ScrollTimer = 0;

            XTemp = this.gameObject.transform.position.x - XSpeed;
            YTemp = this.gameObject.transform.position.y;
            transform.position = new Vector3(XTemp, YTemp, 0);
            transform.localScale = new Vector3(0.8f, 0.8f, 1);

            XCount--;
            if (XCount <= 11 && manager.CookieArray[YCount, XCount].gameObject.activeSelf == true)
            {
                IsCookieEaten[YCount, XCount] = true;
                Invoke("SetActiveFalse", 0.15f);
                iTween.ShakePosition(manager.CookieArray[YCount, XCount], iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.15f));
                iTween.FadeTo(manager.CookieArray[YCount, XCount], 0, 0.15f);                
            }

            BreakDownCheck();
        }

        else if (manager.GameStart == true && Input.GetKeyDown(KeyCode.S) && YCount >= 2 && manager.GameOver == false && ScrollTimer > TimeLimit)
        {
            ScrollTimer = 0;

            XTemp = this.gameObject.transform.position.x;
            YTemp = this.gameObject.transform.position.y - YSpeed;
            transform.position = new Vector3(XTemp, YTemp, 0);
            transform.localScale = new Vector3(0.8f, 0.8f, 1);

            --YCount;
            if (manager.CookieArray[YCount, XCount].gameObject.activeSelf == true && XCount <= 11)
            {
                IsCookieEaten[YCount, XCount] = true;
                Invoke("SetActiveFalse", 0.15f);
                iTween.ShakePosition(manager.CookieArray[YCount, XCount], iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.15f));
                iTween.FadeTo(manager.CookieArray[YCount, XCount], 0, 0.15f);                
            }

            BreakDownCheck();
        }

        else if (manager.GameStart == true && Input.GetKeyDown(KeyCode.D) && XCount <= 10 && manager.GameOver == false && ScrollTimer > TimeLimit)
        {
            ScrollTimer = 0;

            XTemp = this.gameObject.transform.position.x + XSpeed;
            YTemp = this.gameObject.transform.position.y;
            transform.position = new Vector3(XTemp, YTemp, 0);
            transform.localScale = new Vector3(-0.8f, 0.8f, 1);

            XCount++;
            if (XCount <= 11 && manager.CookieArray[YCount, XCount].gameObject.activeSelf == true)
            {
                IsCookieEaten[YCount, XCount] = true;
                Invoke("SetActiveFalse", 0.15f);
                iTween.ShakePosition(manager.CookieArray[YCount, XCount], iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.15f));
                iTween.FadeTo(manager.CookieArray[YCount, XCount], 0, 0.15f);
            }

            BreakDownCheck();
        }
    }

    void BreakDownCheck()       // 과자가 불균형하여 무너지는지 판단하여주는 함수입니다.
    {
        if( XCount == manager.BombTempX && YCount == manager.BombTempY)
        {
            manager.BombExplosion();
        }

        if( IsCookieEaten[YCount, XCount] == true )
        {

            if(manager.CookieArray[YCount, XCount].gameObject.activeSelf == true)
            {
                TempCookieEaten[YCount]++;
            }                

            for (int x = 1; x <= 10; x++)
            {
                if(TempCookieEaten[x] == 10)
                {
                    AllOfCookieEaten[x] = true;
                    //Invoke("CookieReload", 0.1f);
                }
            }

            if(YCount != 7)
            {
                if (AllOfCookieEaten[YCount + 1] == false && AllOfCookieEaten[YCount] == true)
                {
                    if(YCount + 1 == manager.BombTempY && TempCookieEaten[YCount + 1] == 9)
                    {
                        manager.BeforeBreakCookies();
                    }
                    else
                    {
                        manager.GameOverBreakDown();
                    }
                    
                }
                else if(AllOfCookieEaten[YCount + 1] == false && TempCookieEaten[YCount] > 0)
                {
                    manager.BeforeBreakCookies();
                }
            }
        
        }
    }

    void SetActiveFalse()       // 쿠키를 사라지게 만드는 것을 도와줄 함수입니다.
    {
        manager.CookieArray[YCount, XCount].gameObject.SetActive(false);
    }

    /*public void CookieReload()
    {
        manager.CookieReload(ReloadTempY);
    }
    */

}
