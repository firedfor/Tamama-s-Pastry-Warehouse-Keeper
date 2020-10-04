using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static ScrollTamama tamama;          // 타마마 객체를 관리하기 위한 변수입니다.

    public static GameManager manager;              // 게임 매니저 객체를 관리하기 위한 변수입니다.

    public static Sound sound;                          // Sound 객체를 관리하기 위한 변수입니다.

    public GameObject Tamama;
    public GameObject[,] CookieArray;   // 쿠키들의 관리를 효과적이고 체계적으로 만들어 줄 쿠키 게임 오브젝트들의 배열입니다
    public byte CookieArray_X;                   // 쿠키 게임 오브젝트 배열의 X좌표 입니다
    public byte CookieArray_Y;                   // 쿠키 게임 오브젝트 배열의 Y좌표 입니다
    public Sprite[] CookiesSpriteArray;     // 쿠키 스프라이트 텍스쳐들을 담을 배열입니다
    public byte CookiesSpriteArrayNum;      // 쿠키 스프라이트 텍스쳐 배열의 인덱스 요소 개수입니다.
    private SpriteRenderer SpriteRenderer;          // 쿠키 스프라이트 텍스쳐 파일을 게임 오브젝트 내에 입히는 것을 도와주는 변수입니다.
    public int BombTempY;                                   // 쿠키 좌표 내에 폭탄을 설치할 Y 좌표를 임시로 담아두는 변수입니다.
    public int BombTempX;                                   // 쿠키 좌표 내에 폭탄을 설치할 X 좌표를 임시로 담아두는 변수입니다.
    public UnityEngine.Vector3 WhereBomb;                            // 폭탄의 좌표를 기록하는 변수입니다         
    private GameObject ExplosionParticle;                                // 폭발 파티클을 저장하는 변수입니다.
    System.Random r;        // 쿠키 이미지나 음악 등을 랜덤으로 불러올 시 사용하는 변수입니다

    public bool GameStart = true;             // 게임을 시작하였는지 하지 않았는지 판단하는 변수입니다. 게임 메인 화면에서 마우스 왼쪽 버튼을 클릭해 활성화 할 수 있습니다.
    public bool GameOver = false;                // 게임이 끝났는지 판단하는 변수입니다. 타마마가 과자 밑에 깔릴 시 활성화 됩니다.

    UnityEngine.Vector2 TempBombPosition;               // 폭발 할 때 어느 방향으로 힘을 가할 지 결정하는데 도움을 주는 변수입니다

    void Start()
    {
        tamama = this.gameObject.GetComponent<ScrollTamama>();
        sound = this.gameObject.GetComponent<Sound>();

        manager = this;

        CookieArray_X = 10;
        CookieArray_Y = 7;
        CookiesSpriteArrayNum = 43;
        r = new System.Random();

        CookieArray = new GameObject[CookieArray_Y + 1, CookieArray_X + 1];
        CookiesSpriteArray = new Sprite[CookiesSpriteArrayNum + 1];

        Tamama = GameObject.Find("Tamama");
        AllocateCookies();

        CookiesSpriteArray = Resources.LoadAll<Sprite>("Sprite/Cookies");       // 스프라이트 배열 내에 쿠키 이미지들을 담는 과정입니다

        ExplosionParticle = Resources.Load("Sprite/BigExplosion") as GameObject;

        ShakeCookie();
        BombPlace();
        EnableCookies();
    }


    void Update()
    {

    }

    public int[] range(int n)  // 파이썬의 range 함수를 모방한 함수입니다. 만약 range(2)를 선언할 시 a[2] = { 1, 2 } 를 반환합니다.
    {
        int[] a = new int[n];
        for (int i = 1; i <= n; i++)
        {
            a[i - 1] = i;
        }

        return a;
    }

    public void AllocateCookies() {
        foreach (int y in range(CookieArray_Y))     // 게임 오브젝트 배열 내에 쿠키들의 객체를 담는 과정입니다
        {
            foreach (int x in range(CookieArray_X))
            {
                CookieArray[y, x] = GameObject.Find("Cookies" + y.ToString() + "-" + x.ToString());
            }
        }
    }

    public void EnableCookies()             // 쿠키들을 모두 활성화 시키는 함수입니다.
    {
        foreach (int y in range(CookieArray_Y))
        {
            foreach (int x in range(CookieArray_X))
            {
                CookieArray[y, x].gameObject.SetActive(true);
                tamama.IsCookieEaten[y, x] = false;
            }
        }
    }

    public void ShakeCookie()
    {
        foreach (int y in range(CookieArray_Y))     // 게임 오브젝트 스프라이트 내에 쿠키들의 이미지를 담는 과정입니다
        {
            foreach (int x in range(CookieArray_X))
            {
                SpriteRenderer = CookieArray[y, x].GetComponent<SpriteRenderer>();
                SpriteRenderer.sprite = CookiesSpriteArray[r.Next(0, 43)];
            }
        }
    }

    public void BeforeBreakCookies()        // 쿠키에 흔들림 효과를 줄 함수입니다
    {

        for (int y = tamama.YCount + 1; y <= CookieArray_Y; y++)
        {
            foreach (int x in range(CookieArray_X))
            {
                if (CookieArray[y, x].gameObject.activeSelf == true)
                {
                    iTween.ShakePosition(CookieArray[y, x], iTween.Hash("x", 0.5, "y", 0.5, "time", 0.5f));
                    iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.05, "y", 0.05, "time", 0.5f));
                }

            }
        }
    }

    public void GameOverBreakDown()     // 쿠키를 균형적으로 먹지 않아 무너질 시 발생하는 이벤트 입니다.
    {
        GameOver = true;
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.5, "y", 0.5, "time", 0.75f));
        foreach (int y in range(CookieArray_Y))
        {
            foreach (int x in range(CookieArray_X))
            {
                CookieArray[y, x].gameObject.GetComponent<Rigidbody2D>().simulated = true;
            }
        }

        Tamama.gameObject.GetComponent<Animator>().SetBool("GameOver", true);
        Tamama.gameObject.GetComponent<Rigidbody2D>().simulated = true;

    }

    public void BombPlace()                             // 폭탄을 배치하는 함수입니다.
    {
        BombTempY = r.Next(1, CookieArray_Y);
        BombTempX = r.Next(1, CookieArray_X);

        SpriteRenderer = CookieArray[BombTempY, BombTempX].GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = Resources.Load<Sprite>("Sprite/Bomb");
                   
    }

    public void BombExplosion()     // 폭탄이 터지는 것을 구현할 함수입니다
    {
        //sound.PlayAudio("explosion");

        WhereBomb = CookieArray[BombTempY, BombTempX].transform.position;
        ExplosionParticle = (GameObject)Instantiate(ExplosionParticle, WhereBomb, UnityEngine.Quaternion.identity);
        Destroy(ExplosionParticle, 1);

        GameOver = true;
        CookieArray[BombTempY, BombTempX].SetActive(false);
        foreach (int y in range(CookieArray_Y))
        {
            foreach (int x in range(CookieArray_X))
            {
                TempBombPosition = CookieArray[y, x].transform.position - CookieArray[BombTempY, BombTempX].transform.position;
                CookieArray[y, x].gameObject.GetComponent<Rigidbody2D>().simulated = true;
                CookieArray[y, x].gameObject.GetComponent<Rigidbody2D>().AddForce(TempBombPosition * 1.2f , ForceMode2D.Impulse);
            }
        }

        Tamama.gameObject.GetComponent<Animator>().SetBool("GameOver", true);
        Tamama.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.5, "y", 0.5, "time", 0.75f));
    }


    /*public void CookieReload(byte Ycount)              // 쿠키를 다 먹었을 시 쿠키를 다시 재호출 해주는 함수입니다
    {
         Debug.Log(Ycount);       
        
        tamama.AllOfCookieEaten[Ycount] = false;
        tamama.TempCookieEaten[Ycount] = 0;

        foreach (int x in range(CookieArray_X))
        {
            CookieArray[Ycount, x].gameObject.SetActive(true);
            tamama.IsCookieEaten[Ycount, x] = false;
        }
    }
    */
}