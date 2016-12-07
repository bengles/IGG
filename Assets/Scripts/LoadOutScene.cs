using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        itemWidth = 139.5f;
        itemHeight = 128.3f;
        canvasScale = GameObject.Find("Canvas").transform.localScale.x;
        background = GameObject.Find("Background");
        GlobalData.Instance.AddTestItems();
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
            pos.x += 32f * canvasScale + itemWidth * col * canvasScale;
            pos.y += 347f * canvasScale;
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
        for (int col = 0; col < bombList.Count; col++)
        {
            Vector3 pos = background.transform.position;
            pos.x += 32f * canvasScale + itemWidth * col * canvasScale;
            pos.y += 210f * canvasScale;
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
         
            }
        }
        
        for (int col = 0; col < potionList.Count; col++)
        {
            Vector3 pos = background.transform.position;
            pos.x += 32f * canvasScale + itemWidth * col * canvasScale;
            pos.y += 70f * canvasScale;
            string resourcePath = "Prefabs/UI/" + potionList[col].name.Replace(" ", string.Empty);
            GameObject potion = (GameObject)Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, background.transform);
            potions.Add(new UIItem(potionList[col], potion.GetComponent<Toggle>()));
            
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

    public void Transition()
    {
        GlobalData.Instance.equippedStaffs.Clear();
        foreach(UIItem i in staffs)
        {
            if (i.toggle.isOn)
                GlobalData.Instance.equippedStaffs.Add(i.item);
        }
        GlobalData.Instance.equippedBombs.Clear();
        foreach (UIItem i in bombs)
        {
            if (i.toggle.isOn)
                GlobalData.Instance.equippedBombs.Add(i.item);
        }
        GlobalData.Instance.equippedPotions.Clear();
        foreach (UIItem i in potions)
        {
            if (i.toggle.isOn)
                GlobalData.Instance.equippedPotions.Add(i.item);
        }

        SceneManager.LoadScene(GlobalData.Instance.currentLevel);
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
        if (potionsToggled > GlobalData.Instance.nrPotionSlots)
            clearPotionToggles();
    }

    private void clearStaffToggles()
    {
        foreach (UIItem i in staffs)
        {
            Debug.Log("Clearing staff toggle");
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
            Debug.Log("Clearing potion toggle");
            i.toggle.isOn = false;
        }
    }

}
