﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject EndUI;
    public GameObject StateUI;
    public GameObject ResultUI;
    //public GameObject HitPanel;

    public Text QuizText;

    public GameObject CountText;
    public GameObject StageStateText;
    public GameObject GameStateText;

    public GameObject effSuccess;
    public GameObject effFail;
    public Transform effSpawn;


    public enum Gamestate
    {
        GamePlaying, GamePause, GameOver, GameClear        
    }

    public Gamestate gamestate;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        WaveManager.instance.InitWave();
        gamestate = Gamestate.GamePlaying;
        Time.timeScale = 1;
        StateUI.SetActive(true);
        // State UI 활성화
    }
   
    // 결과 버튼 메소드
    IEnumerator coResultButtonsOn()
    {
        yield return new WaitForSeconds(2.5f);
        ResultUI.SetActive(true);
    }

    // 다시하기 버튼 메소드
    IEnumerator coMenuButtonsOn()
    {
        yield return new WaitForSeconds(10f);
        EndUI.SetActive(true);
        ResultUI.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void MenuButtonsOn()
    {
        StartCoroutine(coMenuButtonsOn());
    }

    // 스테이지 클리어 메소드
    public void GameClear()
    {
        ItemManager.instance.itemSpawnStop = true;
        EnemyDestroy();
        ItemDestroy();
        DataSave.instance.data.AddGold(1);
        GameStateText.SetActive(true);
        GameStateText.GetComponent<TextMeshPro>().text = "GameClear";
        GameStateText.GetComponent<Animator>().SetTrigger("GameClear");
        if (SceneManager.GetActiveScene().name == "EnglishScene")
        {
            SliderController.instance.WaitSlider();
        }
        gamestate = Gamestate.GameClear;
        StartCoroutine("coResultButtonsOn");
        StateUI.SetActive(false);
    }

    // 게임 오버 메소드
    public void GameOver()
    {
        ItemManager.instance.itemSpawnStop = true;
        EnemyDestroy();
        ItemDestroy();
        GameStateText.SetActive(true);
        GameStateText.GetComponent<TextMeshPro>().text = "GameOver";
        GameStateText.GetComponent<Animator>().SetTrigger("GameOver");
        if (SceneManager.GetActiveScene().name == "EnglishScene")
        {
            SliderController.instance.WaitSlider();
        }
        StartCoroutine("coResultButtonsOn");
        gamestate = Gamestate.GameOver;
        StateUI.SetActive(false);
    }

    // 게임 일시정지 메소드
    public void GamePause()
    {
        EndUI.SetActive(true);
        gamestate = Gamestate.GamePause;
        Time.timeScale = 0;
    }

    public void GamePauseFin()
    {
        EndUI.SetActive(false);
        gamestate = Gamestate.GamePlaying;
        Time.timeScale = 1;
    }

    // 다음 레벨 메소드
    public void NextLevel()     
    {
        EnemyDestroy();
        WaveManager.instance.StartWave();
        // WaveDelay를 true로 주어 3초간 딜레이를 준다.
                
    }
    
    public void GameClose()
    {
        Application.Quit();
    }

    // 모든 적을 파괴하는 메소드
    public void EnemyDestroy()
    {
        GameObject[] allCube = GameObject.FindGameObjectsWithTag("enemy");
        // allCube에 "enemy" 태그를 가지는 오브젝트 전부 넣어줌.
        foreach (GameObject i in allCube)
        {
            Destroy(i.gameObject);
            // 나머지 적들 파괴
        }
    }

    public void ItemDestroy()
    {
        GameObject[] allHP = GameObject.FindGameObjectsWithTag("HP");
        // allCube에 "enemy" 태그를 가지는 오브젝트 전부 넣어줌.
        foreach (GameObject i in allHP)
        {
            Destroy(i.gameObject);      // 나머지 적들 파괴
        }
        GameObject[] allTimer = GameObject.FindGameObjectsWithTag("Timer");
        // allCube에 "enemy" 태그를 가지는 오브젝트 전부 넣어줌.
        foreach (GameObject i in allTimer)
        {
            Destroy(i.gameObject);        // 나머지 적들 파괴
        }
    }

    public void SuccessEffect()     // 성공 이펙트
    {
        StageStateText.SetActive(true);
        StageStateText.GetComponent<TextMeshPro>().text = "Success";
        StageStateText.GetComponent<Animator>().SetTrigger("Success");
    }

    public void FailEffect()        // 실패 이펙트
    {
        StageStateText.SetActive(true);
        StageStateText.GetComponent<TextMeshPro>().text = "Fail";
        StageStateText.GetComponent<Animator>().SetTrigger("Fail");
    }

    private void OnDestroy()
    {
        instance = null;
    }
}