using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    public Image weaponImg;
    public Image notSelect1;
    public Image notSelect2;

    void Start()
    {
        SelectWeapon();
        if(transform.childCount == 0)
        {
            weaponImg.gameObject.SetActive(false);
            notSelect1.gameObject.SetActive(false);
            notSelect2.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (transform.childCount > 1) {
            notSelect1.gameObject.SetActive(true);

            if (transform.GetChild(0).gameObject.activeInHierarchy == false)
            {
                notSelect1.sprite = transform.GetChild(0).GetComponent<Gun>().weaponImg;

                if (transform.childCount > 2)
                {
                    notSelect2.gameObject.SetActive(true);

                    if (transform.GetChild(2).gameObject.activeInHierarchy == false)
                    {
                        notSelect2.sprite = transform.GetChild(2).GetComponent<Gun>().weaponImg;
                    }
                    else if (transform.GetChild(1).gameObject.activeInHierarchy == false)
                    {
                        notSelect2.sprite = transform.GetChild(1).GetComponent<Gun>().weaponImg;
                    }
                    else if(transform.GetChild(0).gameObject.activeInHierarchy == false)
                    {
                        notSelect2.sprite = transform.GetChild(0).GetComponent<Gun>().weaponImg;
                    }
                     
                }
            }
            else if (transform.GetChild(1).gameObject.activeInHierarchy == false)
            {
                notSelect1.sprite = transform.GetChild(1).GetComponent<Gun>().weaponImg;
                if (transform.childCount > 2)
                {
                    notSelect2.gameObject.SetActive(true);

                    if (transform.GetChild(2).gameObject.activeInHierarchy == false)
                    {
                        notSelect2.sprite = transform.GetChild(2).GetComponent<Gun>().weaponImg;
                    }
                    else if (transform.GetChild(1).gameObject.activeInHierarchy == false)
                    {
                        notSelect2.sprite = transform.GetChild(1).GetComponent<Gun>().weaponImg;
                    }
                    else if (transform.GetChild(0).gameObject.activeInHierarchy == false)
                    {
                        notSelect2.sprite = transform.GetChild(0).GetComponent<Gun>().weaponImg;
                    }
                }
            }
        } else if(transform.childCount == 1)
        {
            weaponImg.gameObject.SetActive(true);
        }

        if (transform.childCount == 1)
        {
            selectedWeapon = -1;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        int previousSelected = selectedWeapon;

        // Scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeapon++;
            if (selectedWeapon >= transform.childCount)
                selectedWeapon = 0;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeapon--;
            if (selectedWeapon < 0)
                selectedWeapon = transform.childCount - 1;
        }

        // Number keys (1, 2, 3...)
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) selectedWeapon = 2;

        if (previousSelected != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}
