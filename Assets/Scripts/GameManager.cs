﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager manager; 

    public GameObject[,] CookieArray;   // 쿠키들의 관리를 효과적이고 체계적으로 만들어 줄 쿠키 게임 오브젝트들의 배열입니다
    public byte CookieArray_X;                   // 쿠키 게임 오브젝트 배열의 X좌표 입니다
    public byte CookieArray_Y;                   // 쿠키 게임 오브젝트 배열의 Y좌표 입니다
    public Sprite[] CookiesSpriteArray;     // 쿠키 스프라이트 텍스쳐들을 담을 배열입니다
    public byte CookiesSpriteArrayNum;      // 쿠키 스프라이트 텍스쳐 배열의 인덱스 요소 개수입니다.
    private SpriteRenderer SpriteRenderer;          // 쿠키 스프라이트 텍스쳐 파일을 게임 오브젝트 내에 입히는 것을 도와주는 변수입니다.
    System.Random r;        // 쿠키 이미지나 음악 등을 랜덤으로 불러올 시 사용하는 변수입니다

    public bool GameStart = true;             // 게임을 시작하였는지 하지 않았는지 판단하는 변수입니다. 게임 메인 화면에서 마우스 왼쪽 버튼을 클릭해 활성화 할 수 있습니다.
    public bool GameOver = false;                // 게임이 끝났는지 판단하는 변수입니다. 타마마가 과자 밑에 깔릴 시 활성화 됩니다.

    void Start()
    {

        manager = this;

        CookieArray_X = 7;
        CookieArray_Y = 5;
        CookiesSpriteArrayNum = 43;
        r = new System.Random();

        CookieArray = new GameObject[CookieArray_Y + 1, CookieArray_X + 1];
        CookiesSpriteArray = new Sprite[CookiesSpriteArrayNum + 1];

        foreach (int y in range(CookieArray_Y))     // 게임 오브젝트 배열 내에 쿠키들의 객체를 담는 과정입니다
        {
            foreach (int x in range(CookieArray_X))
            {
                CookieArray[y, x] = GameObject.Find("Cookies" + y.ToString() + "-" +  x.ToString());
            }
        }

        CookiesSpriteArray = Resources.LoadAll<Sprite>("Sprite/Cookies");       // 스프라이트 배열 내에 쿠키 이미지들을 담는 과정입니다


        foreach (int y in range(CookieArray_Y))     // 게임 오브젝트 스프라이트 내에 쿠키들의 이미지를 담는 과정입니다
        {
            foreach (int x in range(CookieArray_X))
            {
                SpriteRenderer = CookieArray[y, x].GetComponent<SpriteRenderer>();
                SpriteRenderer.sprite = CookiesSpriteArray[r.Next(0, 42)];
            }
        }
        EnableCookies();
    }


    void Update()
    {
        
    }

    public int[] range(int n)  // 파이썬의 range 함수를 모방한 함수입니다. 만약 range(2)를 선언할 시 a[2] = { 1, 2 } 를 반환합니다.
    {
        int[] a = new int[n];
        for( int i = 1; i <= n; i++)
        {
            a[i - 1] = i;
        }

        return a;
    }

    public void EnableCookies()
    {
        foreach (int y in range(5)) 
        {
            foreach (int x in range(7))
            {
                CookieArray[y, x].gameObject.SetActive(true);
            }
        }
    }
}