using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircleCounter : MonoBehaviour
{
    public Slider slider;
    public string text;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { GetComponent<TextMeshProUGUI>().text = text + slider.value.ToString(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
