using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CraftingScene : MonoBehaviour {
    public GameObject StickUI;
    public GameObject BombUI;
    public GameObject PotionUI;

    public GameObject WintermintUI;
    public GameObject MammothUI;

    public GameObject NewCraft;

    private bool wintermintSelected;
    private bool mammothSelected;
    private bool stickSelected;
    private bool bombSelected;
    void Start () {
        GlobalData.init();
        wintermintSelected = false;

        //spawn items on ui
        GameObject backgroundImage = GameObject.Find("BackgroundImage");
        for (int i = 0; i < GlobalData.currentInventory.Count; i++)
        {
            ItemInfo current = GlobalData.currentInventory[i];
            switch(current.getID())
            {
                case 4:
                    //wintermint
                    Vector3 p4 = backgroundImage.transform.position;
                    p4.x -= 162;
                    p4.y += 88;
                    GameObject w4 = (GameObject)Instantiate(WintermintUI, p4, Quaternion.identity, backgroundImage.transform);
                    Toggle t4 = w4.GetComponent<Toggle>();
                    t4.onValueChanged.AddListener(wintermintToggle);
                    break;
                case 5:
                    //mammoth bone
                    Vector3 p5 = backgroundImage.transform.position;
                    p5.x -= 105;
                    p5.y += 89;
                    GameObject w5 = (GameObject)Instantiate(MammothUI, p5, Quaternion.identity, backgroundImage.transform);
                    Toggle t5 = w5.GetComponent<Toggle>();
                    t5.onValueChanged.AddListener(mammothToggle);
                    break;
            }
        }

        //spawn equiping items
        Vector3 p1 = backgroundImage.transform.position;
        p1.x -= 282;
        p1.y += 85;
        GameObject o1 = (GameObject)Instantiate(StickUI, p1, Quaternion.identity, backgroundImage.transform);
        Toggle t1 = o1.GetComponent<Toggle>();
        t1.onValueChanged.AddListener(stickToggle);

        Vector3 p2 = backgroundImage.transform.position;
        p2.x -= 278;
        p2.y -= 2;
        GameObject o2 = (GameObject)Instantiate(BombUI, p2, Quaternion.identity, backgroundImage.transform);
        Toggle t2 = o2.GetComponent<Toggle>();
        t2.onValueChanged.AddListener(bombToggle);


    }

    public void stickToggle(bool toggle)
    {
        stickSelected = toggle;
    }

    public void bombToggle(bool toggle)
    {
        bombSelected = toggle;
    }

    public void wintermintToggle(bool toggle)
    {
        wintermintSelected = toggle;
    }

    public void mammothToggle(bool toggle)
    {
        mammothSelected = toggle;
    }

    public void craft()
    {
        Debug.Log("Craft called " + stickSelected + " " + mammothSelected + " " + !bombSelected + " " + !wintermintSelected);
        
        if (stickSelected && mammothSelected && !bombSelected && !wintermintSelected)
        {
            //mammoth
        }
        if (bombSelected && wintermintSelected && !stickSelected && !mammothSelected)
        {
            Debug.Log("asd");
            GameObject backgroundImage = GameObject.Find("BackgroundImage");
            Vector3 p1 = backgroundImage.transform.position;
            p1.x -= 25;
            //p1.y += 85;
            GameObject o1 = (GameObject)Instantiate(NewCraft, p1, Quaternion.identity, backgroundImage.transform);
            Destroy(o1, 5);
        }
    }
}
