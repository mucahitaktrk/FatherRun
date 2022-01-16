using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool plusBar = false;
    public bool minusBar = false;
    public bool greenGate = false;
    public bool redGate = false;
    public bool finish = false;
    public bool x2 = false;
    public bool x4 = false;
    public bool x6 = false;
    public bool x8 = false;

    private GameManager gameManager;

    // Layer 6 = +
    // Layer 7 = -
    // Layer 8 = Green Gate
    // Layer 9 = Red Gate

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            plusBar = true;
            Destroy(other.transform.gameObject);
        }
        else if (other.gameObject.layer == 7)
        {
            minusBar = true;
            Destroy(other.transform.gameObject);   
        }
        else if (other.gameObject.layer == 8)
        {
            greenGate = true;
        }
        else if (other.gameObject.layer == 9)
        {
            redGate = true;
        }
        else if (other.gameObject.layer == 10)
        {
            finish = true;
        }
        
        else if (other.gameObject.layer == 15 )
        {
            x2 = true;
        }
        else if (other.gameObject.layer == 16)
        {
            x4 = true;
        }
        else if (other.gameObject.layer == 17)
        {
            x6 = true;
        }
        else if (other.gameObject.layer == 18)
        {
            x8 = true;
        }
        

    }
}
