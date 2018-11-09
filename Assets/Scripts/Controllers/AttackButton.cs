using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour {
    public InvManager manager;
    Button button;
    GameObject imageObject;
    Image image;
    GameObject imageObject2;
    Image image2;

    public Sprite hand;
    public Sprite sword;
    public Sprite pistol;
    public Sprite auto;

    Color alphaColor;

    Dictionary<WeaponAttackType, Sprite> spriteDict;

	void Start () {
        alphaColor = Color.white;
        alphaColor.a = 0.4f;

        manager = InvManager.instance;
        button = GetComponent<Button>();
        imageObject = transform.GetChild(0).gameObject;
        image = transform.GetChild(0).GetComponent<Image>();
        imageObject2 = transform.GetChild(1).gameObject;
        image2 = transform.GetChild(1).GetComponent<Image>();

        spriteDict = new Dictionary<WeaponAttackType, Sprite>();
        spriteDict.Add(WeaponAttackType.None, hand);
        spriteDict.Add(WeaponAttackType.Sword, sword);
        spriteDict.Add(WeaponAttackType.Pistol, pistol);
        spriteDict.Add(WeaponAttackType.Automative, auto);

       //UpdateImage();
        Activate(false);
    }
    public void UpdateImage()
    {

        Equipment weapon = manager.GetCurrentWeapon();
        if (weapon != null && weapon.attackType==WeaponAttackType.Pistol)
        {
            image.enabled = true;
            image2.enabled = false;
        }
        else
        {
            image2.enabled = true;
            image.enabled = false;
        }
        /*
        if (weapon != null)
        {
            image.sprite = spriteDict[weapon.attackType];
            //imageObject.SetActive(true);
        }
        else
        {
            image.sprite = spriteDict[0];
            //imageObject.SetActive(false);
        }
        //*/
    }
	
    public void Activate(bool act=true)
    {
        Equipment weapon = manager.GetCurrentWeapon();
        if (act)
        {
            button.interactable = true;
            imageObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            button.interactable = false;
            imageObject.GetComponent<Image>().color = alphaColor;
        }
    }
}
