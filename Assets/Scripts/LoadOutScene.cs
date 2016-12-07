﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadOutScene : MonoBehaviour {
    private GameObject background;
    private float itemWidth;
    private float itemHeight;
    private float canvasScale;
    private List<UIItem> staffs;
    private List<UIItem> bombs;
    private List<UIItem> potions;

    void Start()
    {
        itemWidth = 138.6f;
        itemHeight = 128.3f;
        canvasScale = GameObject.Find("Canvas").transform.localScale.x;
        background = GameObject.Find("Background");
        Debug.Log("before adding items to list, size: " + GlobalData.Instance.currentInventory.Count);
        GlobalData.Instance.AddTestItems();
        Debug.Log("after adding items to list, size: " + GlobalData.Instance.currentInventory.Count);
        staffs = new List<UIItem>();
        bombs = new List<UIItem>();
        potions = new List<UIItem>();
        PopulateStaffs();
        PopulateBombs();
        PopulatePotions();
    }

    private void PopulateStaffs()
    {
        List<Item> staffList = new List<Item>();
        foreach (Item item in GlobalData.Instance.currentInventory)
        {
            if (item.cat == ItemCategory.Staff)
            {
                staffList.Add(item);
            }
        }
        for(int col = 0; col < staffList.Count; col++)
        {
            Vector3 pos = background.transform.position;
            pos.x -= 220.3f * canvasScale - itemWidth * col * canvasScale;
            pos.y += 350f * canvasScale;
            string resourcePath = "Prefabs/UI/" + staffList[col].name.Replace(" ", string.Empty);
            GameObject staff = (GameObject)Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, background.transform);
            staffs.Add(new UIItem(staffList[col], staff.GetComponent<Toggle>()));
            /* for status text
            staff.AddComponent<EventTrigger>();
            EventTrigger.Entry ingredientEnterEvent = new EventTrigger.Entry();
            ingredientEnterEvent.eventID = EventTriggerType.PointerEnter;
            ingredientEnterEvent.callback.AddListener((data) => { IngredientPointerEnter((PointerEventData)data); });
            EventTrigger.Entry ingredientExitEvent = new EventTrigger.Entry();
            ingredientExitEvent.eventID = EventTriggerType.PointerExit;
            ingredientExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
            ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientEnterEvent);
            ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientExitEvent);
            */
        }
    }

    private void PopulateBombs()
    {
        List<Item> bombList = new List<Item>();
        foreach (Item item in GlobalData.Instance.currentInventory)
        {
            if (item.cat == ItemCategory.Bomb)
            {
                bombList.Add(item);
            }
        }
        Debug.Log("Nr Items: " + bombList.Count);
        for (int col = 0; col < bombList.Count; col++)
        {
            Vector3 pos = background.transform.position;
            pos.x -= 220.3f * canvasScale - itemWidth * col * canvasScale;
            pos.y += 150f * canvasScale;
            string resourcePath = "Prefabs/UI/" + bombList[col].name.Replace(" ", string.Empty);
            GameObject staff = (GameObject)Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, background.transform);
            bombs.Add(new UIItem(bombList[col], staff.GetComponent<Toggle>()));
            /* for status text
            staff.AddComponent<EventTrigger>();
            EventTrigger.Entry ingredientEnterEvent = new EventTrigger.Entry();
            ingredientEnterEvent.eventID = EventTriggerType.PointerEnter;
            ingredientEnterEvent.callback.AddListener((data) => { IngredientPointerEnter((PointerEventData)data); });
            EventTrigger.Entry ingredientExitEvent = new EventTrigger.Entry();
            ingredientExitEvent.eventID = EventTriggerType.PointerExit;
            ingredientExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
            ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientEnterEvent);
            ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientExitEvent);
            */
        }
    }

    private void PopulatePotions()
    {
        List<Item> potionList = new List<Item>();
        foreach (Item item in GlobalData.Instance.currentInventory)
        {
            if (item.cat == ItemCategory.Potion)
            {
                potionList.Add(item);
                Debug.Log("Found potion: " + item.name);
            }
        }
        Debug.Log("There were " + GlobalData.Instance.currentInventory.Count + "items");
        for (int col = 0; col < potionList.Count; col++)
        {
            Vector3 pos = background.transform.position;
            pos.x -= 220.3f * canvasScale - itemWidth * col * canvasScale;
            pos.y += 50f * canvasScale;
            string resourcePath = "Prefabs/UI/" + potionList[col].name.Replace(" ", string.Empty);
            GameObject staff = (GameObject)Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, background.transform);
            staffs.Add(new UIItem(potionList[col], staff.GetComponent<Toggle>()));
            Debug.Log("Creating potion: " + potionList[col].name);
            /* for status text
            staff.AddComponent<EventTrigger>();
            EventTrigger.Entry ingredientEnterEvent = new EventTrigger.Entry();
            ingredientEnterEvent.eventID = EventTriggerType.PointerEnter;
            ingredientEnterEvent.callback.AddListener((data) => { IngredientPointerEnter((PointerEventData)data); });
            EventTrigger.Entry ingredientExitEvent = new EventTrigger.Entry();
            ingredientExitEvent.eventID = EventTriggerType.PointerExit;
            ingredientExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
            ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientEnterEvent);
            ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientExitEvent);
            */
        }
    }

    private void Transition()
    {
        //Update GlobalData
        //Scenemanager.Go
    }

    void Update()
    {
        int staffsToggled = 0;
        foreach (UIItem i in staffs)
        {
            if (i.toggle.isOn)
                staffsToggled++;
        }
        if (staffsToggled > GlobalData.Instance.nrStaffSlots)
        {
            clearStaffToggles();
        }
        int bombsToggled = 0;
        foreach (UIItem i in bombs)
        {
            if (i.toggle.isOn)
                bombsToggled++;
        }
        if (bombsToggled > GlobalData.Instance.nrBombSlots)
            clearBombToggles();
        int potionsToggled = 0;
        foreach (UIItem i in potions)
        {
            if (i.toggle.isOn)
                potionsToggled++;
        }
        if (bombsToggled > GlobalData.Instance.nrPotionSlots)
            clearPotionToggles();
    }

    private void clearStaffToggles()
    {
        foreach (UIItem i in staffs)
        {
            i.toggle.isOn = false;
        }
    }
    private void clearBombToggles()
    {
        foreach (UIItem i in bombs)
        {
            i.toggle.isOn = false;
        }
    }
    private void clearPotionToggles()
    {
        foreach (UIItem i in potions)
        {
            i.toggle.isOn = false;
        }
    }

}
