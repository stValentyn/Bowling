using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArrow : MonoBehaviour
{
    public static float rotationAngle;

    const float MAX_ROTATION_ANGLE = 10f;
    const float ROTATION_SPEED_FACTOR = 20f;

    private AudioSource squik;  // sound source
    private bool arrowHold;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngle = 0;
        squik = GetComponent<AudioSource>();  // getting point
        arrowHold = false;
    }

    // ������� ������ �� ������� ������� �������
    void Update()
    {
        if (ButtonS.IsPaused) return;

        float dx = Input.GetAxis("Horizontal")
            * Time.deltaTime
            * ROTATION_SPEED_FACTOR;
        if (PushBoll.isMoving == false)
        {


            if (Mathf.Abs(dx) > 0 &&
                rotationAngle + dx < MAX_ROTATION_ANGLE &&
                rotationAngle + dx > -MAX_ROTATION_ANGLE)
            {
                rotationAngle += dx;
                transform.RotateAround(         //������������� ��������
                    PushBoll.ballStartPosition ,  //�����                      ����������� ��� � ����
                    Vector3.up,                 // ��� (���������� ����� �����
                    dx);                        // ���� ��������������� ��������

                // playing sound
                if (!squik.isPlaying && !arrowHold)
                { 
                    squik.Play();
                    arrowHold = true;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    arrowHold = false;
                }
            }
        }
    }
}
