using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [SerializeField] Acount acount;

    public float rotateSpeed;
    public Transform spawnPos, rotateTrans;

    public GameObject[] characters;

    bool rotate;
    float mousePos1, mousePos2;

    public void SpawnAccountCharacter()
    {
        rotateTrans.rotation = Quaternion.Euler(0, 180, 0);
        Instantiate(characters[acount.character], spawnPos.position, rotateTrans.rotation, rotateTrans);
    }
    public void ChangeCharacter(Character character)
    {
        if (!character.unlocked)
            return;

        acount.character = character.ID;

        foreach(Transform child in rotateTrans)
        {
            Destroy(child.gameObject);
        }
        rotateTrans.rotation = Quaternion.Euler(0, 180, 0);
        Instantiate(characters[acount.character], spawnPos.position, rotateTrans.rotation, rotateTrans);
    }
    private void Update()
    {
        if (rotate == true)
        {
            mousePos1 = Input.mousePosition.x;
            float dist = mousePos1 - mousePos2;
            rotateTrans.Rotate(new Vector3(0, dist * rotateSpeed * Time.deltaTime, 0));

            mousePos2 = mousePos1;
        }
    }
    public void Rotate(bool _rotate)
    {
        mousePos1 = Input.mousePosition.x;
        mousePos2 = mousePos1;
        rotate = _rotate;
    }
    public void RemoveCharacter()
    {
        Destroy(rotateTrans.GetChild(0).gameObject);
    }
}
