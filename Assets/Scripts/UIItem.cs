using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIItem {
    public Item item;
    public Toggle toggle;

    public UIItem(Item item, Toggle toggle)
    {
        this.item = item;
        this.toggle = toggle;
    }
}
