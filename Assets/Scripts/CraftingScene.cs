using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CraftingScene : MonoBehaviour
{

    private GameObject background;
    private float ingredientWidth;
    private float ingredientHeight;
    private float canvasScale;
    private Vector2 topLeftCorner;
    private Toggle staffToggle;
    private Toggle bombToggle;
    private Toggle potionToggle;
    private List<Toggle> ingredientToggles; //list to keep track of which items are toggled

    void Start()
    {
        ingredientWidth = 138.6f;
        ingredientHeight = 128.3f;
        canvasScale = GameObject.Find("Canvas").transform.localScale.x;
        background = GameObject.Find("Background");
        topLeftCorner = new Vector2(-516f * canvasScale, 361.8f * canvasScale);
        ingredientToggles = new List<Toggle>(6*6);
        for (int i = 0; i < 6*6; i++)
        {
            ingredientToggles.Add(null);
        }

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
        GameObject stick = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Stick"), staffPos, Quaternion.identity, background.transform);
        staffToggle = stick.GetComponent<Toggle>();
        
        Vector3 bombPos = background.transform.position;
        bombPos.x -= 780f * canvasScale;
        //bombPos.y += 0;
        GameObject bomb = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Bomb"), bombPos, Quaternion.identity, background.transform);
        bombToggle = bomb.GetComponent<Toggle>();
        
        Vector3 potionPos = background.transform.position;
        potionPos.x -= 780f * canvasScale;
        potionPos.y -= 300f * canvasScale;
        GameObject potion = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Potion"), potionPos, Quaternion.identity, background.transform);
        potionToggle = potion.GetComponent<Toggle>();
    }

    private void populateIngredients(List<Item> ingredients) {

        //spawn ingredient-type items on ui
        Debug.Log("Drawing " + ingredients.Count + " ingredients.");
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 6; col++)
            {
                int index = 6 * row + col;
                if (index >= ingredients.Count)
                {
                    //gone through all ingredients
                    goto PostLoop;
                }
                //put item on ui
                Vector3 pos = background.transform.position;
                pos.x += topLeftCorner.x  + col * ingredientWidth * canvasScale;
                pos.y += topLeftCorner.y - row * ingredientHeight * canvasScale;
                string resourcePath = "Prefabs/UI/" + ingredients[index].name.Replace(" ", string.Empty);
                GameObject ingredient = (GameObject)Instantiate(Resources.Load(resourcePath), pos, Quaternion.identity, background.transform);
                ingredientToggles[index] = ingredient.GetComponent<Toggle>();
            }
        }
    PostLoop:

        Debug.Log("all items on UI.");   
    }

    private void CraftItem(Item item)
    {
        if (item == null)
        {
            //display 
            return;
        }
        
        //Display item crafted


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

        }
        if (p)
        {
            if (itemIDs.Contains(3) && itemIDs.Count == 1) 
            {
                CraftItem(new Item(0, ItemCategory.Potion)); //Chilled Drink
            }
        }
        CraftItem(null);
    }

    private HashSet<int> GetSelectedItemIDs(List<Item> items)
    {
        HashSet<int> itemIDs = new HashSet<int>();
        for (int i = 0; i < items.Count; i++)
        {
            if (ingredientToggles[i] != null && ingredientToggles[i].isOn)
            {
                itemIDs.Add(items[i].index);
            }
        }
        return itemIDs;
    }

    public void Exit()
    {
        SceneManager.LoadScene(1);
    }
}
