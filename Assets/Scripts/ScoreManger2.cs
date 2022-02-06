using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManger2 : MonoBehaviour
{
   private Text lengthText = null;


    private void Awake()
    {
        if (lengthText == null) lengthText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lengthText.text = Snake2.length.ToString();   
    }
}
