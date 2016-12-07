using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CraftingScene : MonoBehaviour
{
    public GameObject statusText;

    private GameObject background;
    private float ingredientWidth;
    private float ingredientHeight;
    private float canvasScale;
    private Vector2 topLeftCorner;
    private Toggle staffToggle;
    private Toggle bombToggle;
    private Toggle potionToggle;
    private List<UIItem> ingredients; //list to keep track of which items are toggled
    private GameObject currentStatusText;
    private GameObject newCraftWindow;

    void Start()
    {
        ingredientWidth = 138.6f;
        ingredientHeight = 128.3f;
        canvasScale = GameObject.Find("Canvas").transform.localScale.x;
        background = GameObject.Find("Background");
        topLeftCorner = new Vector2(-516f * canvasScale, 361.8f * canvasScale);
        ingredients = new List<UIItem>(6*6);
        currentStatusText = null;

        GlobalData.Instance.AddTestItems();
        populateStandardItems();
        populateIngredients(GlobalData.Instance.GetIngredients());
    }

    private void populateStandardItems()
    {
        //spawn stick, bomb, potion
        Vector3 staffPos = background.transform.position;
        staffPos.x -= 780f * canvasScale;
        staffPos.y += 298.6f * canvasScale;
        GameObject staff = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Stick"), staffPos, Quaternion.identity, background.transform);
        staffToggle = staff.GetComponent<Toggle>();
        staff.AddComponent<EventTrigger>();
        EventTrigger.Entry staffEnterEvent = new EventTrigger.Entry();
        staffEnterEvent.eventID = EventTriggerType.PointerEnter;
        staffEnterEvent.callback.AddListener((data) => { StaffPointerEnter((PointerEventData)data); });
        EventTrigger.Entry staffExitEvent = new EventTrigger.Entry();
        staffExitEvent.eventID = EventTriggerType.PointerExit;
        staffExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
        staff.GetComponent<EventTrigger>().triggers.Add(staffEnterEvent);
        staff.GetComponent<EventTrigger>().triggers.Add(staffExitEvent);
        
        Vector3 bombPos = background.transform.position;
        bombPos.x -= 780f * canvasScale;
        //bombPos.y += 0;
        GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Bomb"), bombPos, Quaternion.identity, background.transform);
        bombToggle = bomb.GetComponent<Toggle>();
        bomb.AddComponent<EventTrigger>();
        EventTrigger.Entry bombEnterEvent = new EventTrigger.Entry();
        bombEnterEvent.eventID = EventTriggerType.PointerEnter;
        bombEnterEvent.callback.AddListener((data) => { BombPointerEnter((PointerEventData)data); });
        EventTrigger.Entry bombExitEvent = new EventTrigger.Entry();
        bombExitEvent.eventID = EventTriggerType.PointerExit;
        bombExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
        bomb.GetComponent<EventTrigger>().triggers.Add(bombEnterEvent);
        bomb.GetComponent<EventTrigger>().triggers.Add(bombExitEvent);

        Vector3 potionPos = background.transform.position;
        potionPos.x -= 780f * canvasScale;
        potionPos.y -= 300f * canvasScale;
        GameObject potion = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Potion"), potionPos, Quaternion.identity, background.transform);
        potionToggle = potion.GetComponent<Toggle>();
        potion.AddComponent<EventTrigger>();
        EventTrigger.Entry potionEnterEvent = new EventTrigger.Entry();
        potionEnterEvent.eventID = EventTriggerType.PointerEnter;
        potionEnterEvent.callback.AddListener((data) => { PotionPointerEnter((PointerEventData)data); });
        EventTrigger.Entry potionExitEvent = new EventTrigger.Entry();
        potionExitEvent.eventID = EventTriggerType.PointerExit;
        potionExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
        potion.GetComponent<EventTrigger>().triggers.Add(potionEnterEvent);
        potion.GetComponent<EventTrigger>().triggers.Add(potionExitEvent);
    }

    private void populateIngredients(List<Item> ingredientList) {

        //spawn ingredient-type items on ui
        Debug.Log("Drawing " + ingredientList.Count + " ingredients.");
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 6; col++)
            {
                int index = 6 * row + col;
                if (index >= ingredientList.Count)
                {
                    //gone through all ingredients
                    goto PostLoop;
                }
                //put item on ui
                Vector3 pos = background.transform.position;
                Vector2 itemPos = getItemPosition(row, col);
                pos.x += itemPos.x;
                pos.y += itemPos.y;
                string resourcePath = "Prefabs/UI/" + ingredientList[index].name.Replace(" ", string.Empty);
                GameObject ingredient = (GameObject)Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, background.transform);
                ingredients.Add(new UIItem(ingredientList[index], ingredient.GetComponent<Toggle>()));
                ingredient.AddComponent<EventTrigger>();
                EventTrigger.Entry ingredientEnterEvent = new EventTrigger.Entry();
                ingredientEnterEvent.eventID = EventTriggerType.PointerEnter;
                ingredientEnterEvent.callback.AddListener((data) => { IngredientPointerEnter((PointerEventData)data); });
                EventTrigger.Entry ingredientExitEvent = new EventTrigger.Entry();
                ingredientExitEvent.eventID = EventTriggerType.PointerExit;
                ingredientExitEvent.callback.AddListener((data) => { PointerExit((PointerEventData)data); });
                ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientEnterEvent);
                ingredient.GetComponent<EventTrigger>().triggers.Add(ingredientExitEvent);
            }
        }
    PostLoop:

        Debug.Log("all items on UI.");   
    }

    private Vector2 getItemPosition(int row, int col)
    {
        return new Vector2(topLeftCorner.x + col * ingredientWidth * canvasScale, topLeftCorner.y + -row * ingredientWidth * canvasScale);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("Pointer enter!");
    }

    private void CraftItem(Item item)
    {
        if (item == null)
        {
            //display
            Debug.Log("Crafting null");
            return;
        }

        newCraftWindow = (GameObject)Instantiate(Resources.Load("Prefabs/UI/NewCraftBackground"), this.transform.position, Quaternion.identity, this.transform);

        //spawn ingredient
        Vector3 itemPos = newCraftWindow.transform.position;
        itemPos.x -= 319f * canvasScale;
        itemPos.y += 135.3f * canvasScale;
        string resourcePath = "Prefabs/UI/" + item.name.Replace(" ", string.Empty);
        //string resourcePath = "Prefabs/UI/MammothBone";
        GameObject ingredient = (GameObject)Instantiate(Resources.Load(resourcePath), itemPos, Quaternion.identity, newCraftWindow.transform);
        
        //spawn flavortext x y w h 85.5 -4.4 548.2 
        Vector3 textPos = newCraftWindow.transform.position;
        textPos.x += 85.6f * canvasScale;
        textPos.y -= 4.6f * canvasScale;
        GameObject flavorText = (GameObject)Instantiate(Resources.Load("Prefabs/UI/FlavorText"), textPos, Quaternion.identity, newCraftWindow.transform);
        flavorText.GetComponent<Text>().text = item.flavorText;

        Debug.Log("Crafting " + item.name);

        foreach(UIItem i in ingredients)
        {
            i.toggle.isOn = false;
        }
        staffToggle.isOn = false;
        bombToggle.isOn = false;
        potionToggle.isOn = false;

        if (!GlobalData.Instance.HasItemID(item.index, item.cat))
        {
            GlobalData.Instance.currentInventory.Add(item);
        }
    }

    public void craft()
    {
        HashSet<int> itemIDs = GetSelectedItemIDs(GlobalData.Instance.GetIngredients());
        //huuuge look-up table consisting of all possible crafts
        //first make sure only 1 non-ingredient is selected
        bool s = staffToggle.isOn; bool b = bombToggle.isOn; bool p = potionToggle.isOn;
        if (!s && !s && !p)
            CraftItem(null);
        if ((s && b) || (s && p) || (b && p))
            CraftItem(null);
        //find out which item is crafted
        if (s)
        {
            
        }
        if (b)
        {
            if (itemIDs.Contains(3) && itemIDs.Count == 1)
            {
                CraftItem(new Item(0, ItemCategory.Bomb));
            }
        }
        if (p)
        {
            if (itemIDs.Contains(3) && itemIDs.Count == 1) 
            {
                CraftItem(new Item(0, ItemCategory.Potion)); //Chilled Drink
            }
            if (itemIDs.Contains(7) && itemIDs.Count == 1)
            {
                CraftItem(new Item(4, ItemCategory.Potion)); // sniffle enhancer
            }
            if (itemIDs.Contains(5) && itemIDs.Count == 1)
            {
                CraftItem(new Item(1, ItemCategory.Potion)); //Curry sauce
            }
            if (itemIDs.Contains(3) && itemIDs.Contains(8) && itemIDs.Count == 2)
            {
                CraftItem(new Item(5, ItemCategory.Potion)); //Mushroom proof lotion
            }
        }
        CraftItem(null);
    }

    private HashSet<int> GetSelectedItemIDs(List<Item> items)
    {
        HashSet<int> itemIDs = new HashSet<int>();
        for (int i = 0; i < items.Count; i++)
        {
            if (ingredients[i] != null && ingredients[i].toggle.isOn)
            {
                itemIDs.Add(items[i].index);
            }
        }
        return itemIDs;
    }

    public void StaffPointerEnter(PointerEventData data)
    {
        Destroy(currentStatusText);
        //spawn status-text
        Vector3 pos = staffToggle.gameObject.transform.position;
        pos.x += 77.5f * canvasScale;
        pos.y -= 106f * canvasScale;
        currentStatusText = (GameObject)Instantiate(statusText, pos, Quaternion.identity, staffToggle.gameObject.transform);
        currentStatusText.GetComponentInChildren<Text>().text = "Staff";
        Debug.Log("Pointer enter");
    }

    public void BombPointerEnter(PointerEventData data)
    {
        Destroy(currentStatusText);
        //spawn status-text
        Vector3 pos = bombToggle.gameObject.transform.position;
        pos.x += 77.5f * canvasScale;
        pos.y -= 106f * canvasScale;
        currentStatusText = (GameObject)Instantiate(statusText, pos, Quaternion.identity, bombToggle.gameObject.transform);
        currentStatusText.GetComponentInChildren<Text>().text = "Bomb";
        Debug.Log("Pointer enter");
    }

    public void PotionPointerEnter(PointerEventData data)
    {
        Destroy(currentStatusText);
        //spawn status-text
        Vector3 pos = potionToggle.gameObject.transform.position;
        pos.x += 77.5f * canvasScale;
        pos.y -= 106f * canvasScale;
        currentStatusText = (GameObject)Instantiate(statusText, pos, Quaternion.identity, potionToggle.gameObject.transform);
        currentStatusText.GetComponentInChildren<Text>().text = "Potion";
        Debug.Log("Pointer enter");
    }

    public void IngredientPointerEnter(PointerEventData data)
    {
        Toggle toggle = null;
        foreach (GameObject g in data.hovered)
        {
            if (g.tag == "Ingredient")
            {
                toggle = g.GetComponentInParent<Toggle>();
                break;
            }
        }
        if (toggle == null)
        {
            Debug.Log("Couldnt find toggle of mouseover.");
            return;
        }
        //spawn status-text
        Vector3 pos = toggle.gameObject.transform.position;
        pos.x += 77.5f * canvasScale;
        pos.y -= 106f * canvasScale;
        currentStatusText = (GameObject)Instantiate(statusText, pos, Quaternion.identity, toggle.gameObject.transform);
        currentStatusText.GetComponentInChildren<Text>().text = "Potion";
        currentStatusText.GetComponent<RectTransform>().SetAsLastSibling();
        currentStatusText.transform.parent.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        //find which item
        Item item = null;
        foreach(UIItem i in ingredients)
        {
            if (i.toggle == toggle)
            {
                item = i.item;
            }
        }
        if (item == null)
        {
            Debug.Log("For some reason failed to find item.");
            return;
        }
        Text textField = currentStatusText.GetComponentInChildren<Text>();
        textField.text = item.name;
        if (item.name.Length > 5)
        {
            textField.fontSize -= 7;
        }
        if (item.name.Length > 7)
        {
            textField.fontSize -= 5;
        }
        if (item.name.Length > 12)
        {
            textField.fontSize -= 3;
        }
    }

    public void PointerExit(PointerEventData data)
    {
        Destroy(currentStatusText);
    }

    public void PointerDown()
    {
        Destroy(newCraftWindow);
    }

    public void Exit()
    {
        SceneManager.LoadScene(1);
    }
}
