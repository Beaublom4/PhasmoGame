using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public static MenuSwitcher Instance;
	[SerializeField] GameObject[] menuPanels;

    private void Awake()
    {
        Instance = this;
    }
    public void SwitchPanel(string name)
    {
        foreach(GameObject g in menuPanels)
        {
            g.SetActive(false);
            if(g.GetComponent<MenuPanel>().panelName == name)
            {
                g.SetActive(true);
            }
        }
    }
}