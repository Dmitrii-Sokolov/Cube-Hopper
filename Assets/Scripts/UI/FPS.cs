using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    [SerializeField]
    private Text FPSText;

    private float Timer = 0f;
    private int Counter = 0;

    private void Update()
    {
        Timer += Time.deltaTime;
        Counter++;
        if (Timer > 1)
        {
            FPSText.text = Counter.ToString();
            Timer = Timer % 1;
            Counter = 0;
        }
    }
}
