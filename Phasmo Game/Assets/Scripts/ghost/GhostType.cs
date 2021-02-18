using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghost", menuName = "ScriptableObjects/TypeOfGhosts", order = 1)]
public class GhostType : ScriptableObject
{
    [Header("Ghost basic values")]
    public string ghostName;
    public float windowtime;
    public float randomEventTime;
    [Header("Ghost speed values")]
    public float walkingspeed;
    public float HuntingSpeed;
    [Header("Ghost event values")]
    [Range(0, 10)]
    public int agressionLevel;
    [Range(0, 10)]
    public int shyLevel;
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
