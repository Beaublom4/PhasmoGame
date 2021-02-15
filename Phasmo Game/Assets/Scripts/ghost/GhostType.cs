using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghost", menuName = "ScriptableObjects/TypeOfGhosts", order = 1)]
public class GhostType : ScriptableObject
{
    [Header("Ghost basic values")]
    public string ghostName;
    [Header("Ghost speed values")]
    public float walkingspeed;
    public float HuntingSpeed;
    [Header("Ghost event values")]
    [Range(0, 10)]
    public int agressionLevel;
    [Range(0, 10)]
    public int shyLevel;
    [Header("Ghost events")]
    public bool window;
    public bool door;
    public bool sound;
    public bool walkingInHallWay;
    public bool objectMove;
    public bool lights;
    public bool alarmsOn;
    public bool playerLevetate;
    [Header("Ghost clues")]
    public bool eMF;
    public bool SpiritBox;
    public bool weWeBord;
    public bool soundWaves;
    public bool lightsActivity;
    public bool TempCold;
    public bool inMirror;
    public bool typeMachine;
    public bool filtherCam;
    public bool ghostCamDistord;
}
