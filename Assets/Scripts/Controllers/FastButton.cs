using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastButton : MonoBehaviour
{
    InvManager manager;
    Button button;
    GameObject imageObject;
    Image image;

    public Sprite sword;
    public Sprite pistol;
    public Sprite auto;

    Dictionary<WeaponAttackType, Sprite> spriteDict;
    
    void Start ()
    {
        manager = InvManager.instance;
        button = GetComponent<Button>();
        imageObject = transform.GetChild(0).gameObject;
        image = transform.GetChild(0).GetComponent<Image>();

        spriteDict = new Dictionary<WeaponAttackType, Sprite>();
        //spriteDict.Add(WeaponAttackType.None, hand);
        spriteDict.Add(WeaponAttackType.Sword, sword);
        spriteDict.Add(WeaponAttackType.Pistol, pistol);
        spriteDict.Add(WeaponAttackType.Automative, auto);

        UpdateImage();

    }

    public void UpdateImage()
    {
        Item item = manager.GetFastSlotItem();
        if (item!=null)
        {
            Equipment weapon = null;
            if (item as Equipment)
            {
                weapon = item as Equipment;
            }
            if (weapon != null)
            {
                image.sprite = spriteDict[weapon.attackType];
                button.interactable = true;
                imageObject.SetActive(true);
            }
        }
        else
        {
            button.interactable = false;
            imageObject.SetActive(false);
        }
    }
}
