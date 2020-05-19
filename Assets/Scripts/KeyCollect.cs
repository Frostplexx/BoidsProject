using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyCollect : MonoBehaviour
{
    public GameObject scoreText;
    public int keys = 0;

    void OnTriggerEnter(Collider other)
    {
        keys++;
        scoreText.GetComponent<Text>().text = "Keys:" + keys;
        Destroy(gameObject);
    }
}
