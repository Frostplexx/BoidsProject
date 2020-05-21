using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreVal = 4;
    [SerializeField]
    public static bool won = false;
    Text score;
    void Start()
    {
        score = GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Keys: " + scoreVal + "/5";

        if (scoreVal >= 5) {
            won = true; 
        }
    }
}
