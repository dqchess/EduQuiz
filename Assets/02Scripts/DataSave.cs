﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//public class UserInfo
//{
//    private int id;
//    private string name;
//    private int password;
//}

//public class LevelInfo
//{
//    public string summary; //0:국어, 1:수학, 2:영어
//    public int grade;
//    public int season;
//    public int level;

//    public LevelInfo(string summary, int grade, int season, int level)
//    {
//        this.summary = summary;
//        this.grade = grade;
//        this.season = season;
//        this.level = level;
//    }
//}

public class Data
{
    public int gold = 0;
    public int nowWeapon = 0;
    public int[] availableWeapon = new int[25]; //0 : 사용불가, 1 : 사용가능
    public int[] availableAnimal = new int[25]; //0 : 사용불가, 1 : 사용가능, 2 : 사용됨
    public enum ItemType
    {
        Weapon, Animal
    }

    //기본 생성자, logo씬 awake시(캐릭터자체에 달려있는 DataSave awake시) db에 있는 정보 받아오기
    public Data()
    {
        var inventoryData = DataService.Instance.GetData<Table.Data>(1);

        this.gold = inventoryData.gold;
        this.nowWeapon = inventoryData.now_weapon;
        Debug.Log("최초골드:" + this.gold);
        Debug.Log("현재 웨폰:" + this.nowWeapon);
        for (int i=0; i < inventoryData.available_weapon.Length; i++)
        {
            this.availableWeapon[i] = int.Parse(inventoryData.available_weapon[i].ToString());
            if (availableWeapon[i] == 1)
                Debug.Log("이용가능한 웨폰번호 : " + (i));
        }
        for(int i = 0; i < inventoryData.available_animal.Length; i++)
        {
            this.availableAnimal[i] = int.Parse(inventoryData.available_animal[i].ToString());
            if (availableAnimal[i] == 1)
                Debug.Log("이용가능한 동물번호 : " + (i));
        }
    }
    
    public void AddGold(int getGold)
    {
        var inventoryData = DataService.Instance.GetData<Table.Data>(1);
        gold += getGold;
        inventoryData.gold = gold;
        int result = DataService.Instance.UpdateData<Table.Data>(inventoryData);
        Debug.Log("gold : " + gold);
    }

    public void Charge(int nowGold, int chargeGold, ItemType itemType, int itemNum)
    {
        if (nowGold < chargeGold) {
            StoreManager.instance.SetNotice();
        }
        else {
            switch (itemType)
            {
                case ItemType.Weapon:
                    ChargeWeapon(chargeGold, itemNum);
                    break;
                case ItemType.Animal:
                    ChargeAnimal(chargeGold, itemNum);
                    break;
            }
        }
    }
    void ChargeWeapon(int chargeGold, int weaponNum)
    {
        var inventoryData = DataService.Instance.GetData<Table.Data>(1);

        //now_weapon수정하기
        inventoryData.now_weapon = weaponNum;

        if (availableWeapon[weaponNum] != 1)
        {
            {//available_weapon문자열 수정하기
                string tmpWeaponList = inventoryData.available_weapon;
                char[] phraseAsChars = tmpWeaponList.ToCharArray();
                if (phraseAsChars[weaponNum] == '0')
                    phraseAsChars[weaponNum] = '1';
                else
                    phraseAsChars[weaponNum] = '0';
                inventoryData.available_weapon = new string(phraseAsChars);
            }

            AddGold(chargeGold * -1);
            Debug.Log(chargeGold + "원에 " + weaponNum + "무기 구입");
            availableWeapon[weaponNum] = 1;
        }
        else
        {
            Debug.Log("이미 가지고있는 무기입니다");
        }
        Debug.Log("현재 무기 변경됨" + weaponNum + "무기");

        ChangeNowWeaponData(weaponNum);
        //테이블 값 수정하기
        int result = DataService.Instance.UpdateData<Table.Data>(inventoryData);
    }
    public void ChangeNowWeaponData(int weaponNum)
    {
        nowWeapon = weaponNum;
    }

    void ChargeAnimal(int chargeGold, int animalNum)
    {
        var inventoryData = DataService.Instance.GetData<Table.Data>(1);

        //now_weapon수정하기
        
        if (availableAnimal[animalNum] == 0)
        {
            {//available_weapon문자열 수정하기
                string tmpAnimalList = inventoryData.available_animal;
                char[] phraseAsChars = tmpAnimalList.ToCharArray();
                if (phraseAsChars[animalNum] == '0')
                    phraseAsChars[animalNum] = '1';
                else
                    phraseAsChars[animalNum] = '0';
                inventoryData.available_weapon = new string(phraseAsChars);
            }

            AddGold(chargeGold * -1);

            Debug.Log(chargeGold + "원에 " + animalNum + "동물 구입");
            availableWeapon[animalNum] = 1;
        }
        else
        {
            Debug.Log("이미 가지고있는 동물입니다");
        }
        int result = DataService.Instance.UpdateData<Table.Data>(inventoryData);
        availableAnimal[animalNum] = 1;
    }
    
}


public class DataSave : MonoBehaviour
{
    public static DataSave instance;
    public Data data;

    public int grade;
    public float maxScore;
    public float score;

    //public UserInfo userInfo;
    //public LevelInfo basicLevelInfo;
    //public LevelInfo mahtLevelInfo;
    //public LevelInfo englishLevelInfo;


    private void Awake()
    {
        instance = this;
        data = new Data();
    }

    private void Start()
    {
        
    }
    
    
    private void OnDestroy()
    {
        instance = null;
    }
}
