using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Public Variables
    [Header("Player Settings")]

    [SerializeField]
    public int maxCardLength = 4;
    public int maxHealth = 20;
    public int speed = 0;

    [Header("Player Stats")]
    public int currentCardLength = 0;
    public int currentHealth = 0;
}
