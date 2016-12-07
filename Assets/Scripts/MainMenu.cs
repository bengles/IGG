using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
	}
	
    public void Demo1Press()
    {
        GlobalData.Instance.currentLevel = 3;
        GlobalData.Instance.currentInventory.Add(new Item(-1, ItemCategory.Staff));
        GlobalData.Instance.currentInventory.Add(new Item(-1, ItemCategory.Bomb));
        GlobalData.Instance.currentInventory.Add(new Item(-1, ItemCategory.Potion));
        SceneManager.LoadScene(1);
    }

    public void Demo2Press()
    {
        GlobalData.Instance.currentLevel = 4;
        GlobalData.Instance.currentInventory.Add(new Item(-1, ItemCategory.Staff));
        GlobalData.Instance.currentInventory.Add(new Item(-1, ItemCategory.Bomb));
        GlobalData.Instance.currentInventory.Add(new Item(-1, ItemCategory.Potion));
        GlobalData.Instance.currentInventory.Add(new Item(3, ItemCategory.Ingredient));
        GlobalData.Instance.currentInventory.Add(new Item(4, ItemCategory.Ingredient));
        GlobalData.Instance.currentInventory.Add(new Item(7, ItemCategory.Ingredient));
        //add more items
        SceneManager.LoadScene(1);
    }
}
