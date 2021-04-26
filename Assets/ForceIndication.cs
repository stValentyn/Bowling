using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceIndication : MonoBehaviour
{
    public static float forceLevel;

    public GameObject IndicatorScreen;

    float rotationAngle;
    public Image indicator;
    const float FORCE_INDICATOR_FACTOR = 2f;

    // Start is called before the first frame update
    void Start()
    {
        forceLevel = indicator.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (ButtonS.IsPaused) return;

        float dy = Input.GetAxis("Vertical")
            * Time.deltaTime * FORCE_INDICATOR_FACTOR;
        if(forceLevel + dy < 1 && forceLevel + dy >= 0.1)
        {
            forceLevel += dy;
            indicator.fillAmount = forceLevel;
        }
    }
}
