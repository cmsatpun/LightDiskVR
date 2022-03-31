using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<Weapon> activeWeapons = new List<Weapon>(2); //stores the left hand weapon at index 0 and the right hand weapon at index 1
    public List<Weapon> weaponList = new List<Weapon>(4); //a list of all weapons in this player's inventory

    public string playerName;

    const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";

    public List<GameObject> weaponSelectScreens = new List<GameObject>(2);
    public List<WeaponSelectUiController> selectScripts = new List<WeaponSelectUiController>(2);
    public List<Weapon>weaponSwapList = new List<Weapon>();
    public bool[] isSelectMenuEnabled = {false, false};

    /*public HandActual leftHA;
    public HandActual rightHA;


    private void Update() {
        if(leftHA =)
    }*/

    private void Start() {
        //activateWeapons();
        playerName = generateRandomName();
        selectScripts.Add(weaponSelectScreens[0].GetComponent<WeaponSelectUiController>());
        selectScripts.Add(weaponSelectScreens[0].GetComponent<WeaponSelectUiController>());
        Debug.Log("-1 % 3" + mod(-1, 3));
        //activateWeapons
    }

    private int mod(int num1, int num2)
    {
        if(num1 < 0)
            num1 += num2;
        return num1 % num2;
    }

    public void ToggleSelectUI(Hand h)
    {
        if (isSelectMenuEnabled[(int) h])
        {
            closeSelectUI(h, false);
        }
        //makes sure you can only have one UI open at a time or else bad things might happen
        else if (!isSelectMenuEnabled[((int) h + 1) % 2])
        {
            openSelectUI(h);
        }
    }

    public void openSelectUI(Hand h)
    {
        weaponSwapList = getSwapWeapons(h);
        isSelectMenuEnabled[(int) h] = true;
        weaponSelectScreens[(int) h].SetActive(true);
    }

    //seperate because letting go of the weapon will also close the select UI
    //set replace to true to attempt to replace the weapon, false to not
    public void closeSelectUI(Hand h, bool tryReplace)
    {
        //swap active weapon if held
        if (tryReplace && activeWeapons[(int) h].isHeld) 
        {
            Debug.Log("Amogus");
            //swapActiveWeapon(activeWeapons[(int) h], selectScripts[(int) h].selectedWeapon);
           // swapActiveWeapon(activeWeapons[(int) h], weaponSwapList[(int) h][1]);
        }
            
        //and close menu
        isSelectMenuEnabled[(int) h] = false;
        weaponSelectScreens[(int) h].SetActive(false);
        if(weaponSwapList.Count > 0) 
            weaponSwapList.Clear();
    }

    public void addWeapon(Weapon weapon)
    {
        weaponList.Add(weapon);
    }


    public void activateWeapons()
    {
        setActiveWeapon(weaponList[0], Hand.LEFT);
        setActiveWeapon(weaponList[1], Hand.RIGHT);
    }


    public void setActiveWeapon(Weapon weapon, Hand h)
    {
        Debug.Log((int) h);
        Debug.Log(activeWeapons);
        //Debug.Log(activeWeapons[0]);
        activeWeapons[(int) h] = weapon;
        weapon.setHand(h);
        weapon.EnableWeapon(weaponSelectScreens[(int) h].transform);
        //weapon.playerName = playerName;
    }

    public void CallGrabEventOnActiveWeapon(int h)
    {
        activeWeapons[h].OnGrabFunction(h);
    }

    public void CallReleaseEventOnActiveWeapon(int h)
    {
        activeWeapons[h].OnReleaseFunction(h);
    }

    //returns the active weapon in the given hand
    public Weapon getActiveWeapon(Hand h)
    {
        return activeWeapons[(int) h];
    }

    //make w2 the active weapon of the hand for which w1 was previously the active weapon
    //returns if the swap was successful
    public bool swapActiveWeapon(Weapon replaceWeapon, Weapon newWeapon)
    {
        if(weaponList.Contains(newWeapon) && activeWeapons.Contains(replaceWeapon))
        {
            setActiveWeapon(newWeapon, (Hand) activeWeapons.IndexOf(replaceWeapon));
            //activeWeapons[activeWeapons.IndexOf(replaceWeapon)] = newWeapon;
            return true;
        }
        return false;
    }

    //places weapon w into the given hand
    //returns if the swap was successful
   /* public bool swapActiveWeapon(Weapon w, Hand h)
    {
        if(weaponList.Contains(w))
        {
            setActiveWeapon()
            return true;
        }    
        return false;
    }*/

    public List<Weapon> getSwapWeapons(Hand h)
    {
        if(weaponSwapList.Count > 0)
            return weaponSwapList;
        List<Weapon> swapableWeapons = weaponList;
        foreach(Weapon w in activeWeapons) {
            swapableWeapons.Remove(w);
        }
        swapableWeapons.Add(activeWeapons[(int) h]);
        return cycleWeaponList(h, -1, swapableWeapons);
    }

    public List<Weapon> cycleWeaponList(Hand h, int dir)
    {
        return cycleWeaponList(h, dir, getSwapWeapons(h));
    }
    //for swapping UI
    public List<Weapon> cycleWeaponList(Hand h, int dir, List<Weapon> cycleList)
    {
        Debug.Log("Cycle Count " + cycleList.Count);
        List<Weapon> newSwapList = new List<Weapon>(3);
        for(int i = 0; i < cycleList.Count; i++)
        {
            Debug.Log("i " + i + " dir " + dir + " i + dir mod 3 " + mod((i + dir),3));
            newSwapList.Add(cycleList[mod((i - dir),3)]);
        }
        selectScripts[(int) h].UpdateWeaponDisplay(newSwapList);
        weaponSwapList = newSwapList;
        return newSwapList;
    }

    public string generateRandomName()
    {
        string s = "";
        for(int i=0; i<10; i++)
        {
            s += glyphs[Random.Range(0, glyphs.Length)];
        }
        return s;

    }



}
