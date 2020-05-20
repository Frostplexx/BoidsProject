using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyCollect : MonoBehaviour
{
    public GameObject scoreText;
    public int keys = 0;

    GameObject Key01;
    GameObject Key02;
    GameObject Key03;
    GameObject Key04;
    GameObject Key05;

    

    void OnTriggerEnter(Collider other)
    {
        keys++;
        scoreText.GetComponent<Text>().text = "Keys:" + keys;
        Destroy(gameObject);
    }

    int GenerateRan20()
    {
        int ran = Random.Range(0, 20);
        return ran;
    }
}
