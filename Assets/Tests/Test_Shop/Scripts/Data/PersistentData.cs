using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour, IPersistentData
{
    public PlayerData PlayerData { get; set; }
}
