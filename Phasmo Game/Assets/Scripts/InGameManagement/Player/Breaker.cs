using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Breaker : MonoBehaviour
{
    public string[] Puzzles;

    public GameObject countUp;
    public List<GameObject> CountUp_ButtonRandomized = new List<GameObject>();
    public int buttonNum;
    public void Use()
    {
        int randomNum = Random.Range(0, Puzzles.Length);
        Invoke(Puzzles[randomNum], 0);
    }
    void CountUp()
    {
        countUp.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach(Transform child in countUp.transform)
        {
            AddToList();
        }
        for (int i = 0; i < CountUp_ButtonRandomized.Count; i++)
        {
            CountUp_ButtonRandomized[i].GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();
        }
    }
    void AddToList()
    {
        int randomNum = Random.Range(0, countUp.transform.childCount);
        GameObject g = countUp.transform.GetChild(randomNum).gameObject;
        if (!CountUp_ButtonRandomized.Contains(g))
        {
            CountUp_ButtonRandomized.Add(g);
        }
        else
        {
            AddToList();
        }
    }
    public void ClickCountUp(TMP_Text text)
    {
        if (int.Parse(text.text) - 1 == buttonNum)
        {
            print("Good");
            buttonNum++;
            if(buttonNum == CountUp_ButtonRandomized.Count)
            {
                print("Finished");
            }
        }
        else
        {
            print("Wrong");
        }
    }
}
