using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PushBoll : MonoBehaviour
{
    Rigidbody rb;
    Vector3 force;
    const float FORCEFACTOR = 5000;
    public static bool isMoving;
    public static Vector3 ballStartPosition;
    int score;
    public Text titleScore;
    int shots;
    public Text titleShotNum;
    float dt;

    private AudioSource collisionSound;

    List<Vector3> kegelPositionList;


    public GameObject Arrow;
    public GameObject ForceLine;
    public GameObject Ball;
    public GameObject Anchor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = new Vector3(0, 0, 0);
        ballStartPosition = Ball.transform.position;  // ���������� ��������� ��������� ����
        score = 0;
        shots = 0;
        isMoving = false;
        collisionSound = GetComponent<AudioSource>();
        ForceLine = GameObject.Find("ForceLine");

        // ��������� ��������� ������� ������
        kegelPositionList = new List<Vector3>();
        var kegels = GameObject.FindGameObjectsWithTag("Kegel");
        foreach (var kegel in kegels)
        {
            kegelPositionList.Add(kegel.transform.position);
        }
    }

    async void Update()
    {
        if (ButtonS.IsPaused) return;

        if (isMoving)
        {
            dt += Time.deltaTime;
            //await Task.Delay(500);            // ���� ����� ���� ������ ��������
            if (rb.velocity == Vector3.zero && dt > 1)  // ������� ���������
            {
                isMoving = false;
                Arrow.SetActive(true);                        // �������� �������
                ForceLine.SetActive(true);
                Ball.transform.position = ballStartPosition;  // ���������� ��� �� �����
                score = 0;

                //ball stopped - count hited kegels
                var kegels = GameObject.FindGameObjectsWithTag("Kegel");
                foreach (var kegel in kegels)
                {
                    //Debug.Log(kegel.name + "LOC Y = " + kegel.transform.localPosition.y +
                    //                      "  GL Y = " + kegel.transform.position.y);
                    //if ( !(kegel.transform.localPosition.y > 10f && kegel.transform.localPosition.y < 10.5f) )  // ����� �����
                    //{                               // � ����: ����� �� ����� - Y=10,40       ����� - Y=11,22
                   /* Debug.Log(kegel.name + "Rot X = " + kegel.transform.localRotation.x +
                        "  Rot Z = " + kegel.transform.rotation.z);*/

                    if ( Mathf.Abs(kegel.transform.rotation.x) > 0.1 || Mathf.Abs(kegel.transform.rotation.z) > 0.1)
                    {
                        score++;
                        kegel.transform.position += (Vector3.down * 2);  // ������ � �������� :)
                    }
                }
                titleScore.text = "Score: " + score;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (score < 10)
                {
                    force.x = ForceIndication.forceLevel
                        * Mathf.Sin(RotateArrow.rotationAngle * Mathf.PI / 180);
                    force.z = ForceIndication.forceLevel
                        * Mathf.Cos(RotateArrow.rotationAngle * Mathf.PI / 180);
                    rb.AddForce(force * FORCEFACTOR);

                    Arrow.SetActive(false);  // ��������� �������
                    ForceLine.SetActive(false);
                    isMoving = true;
                    dt = 0;

                    shots++;
                    titleShotNum.text = "Shots: " + shots;
                }
                else   //����� ���������� ��������
                {
                    score = 0;
                    shots = 0;
                    titleShotNum.text = "Shots: " + shots;
                    titleScore.text = "Score: " + score;

                    {  // ���������� ����� �� �����
                        int index = 0;
                        var kegels = GameObject.FindGameObjectsWithTag("Kegel");
                        foreach (var kegel in kegels)
                        {
                            await Task.Delay(150);   // ������� ����������� �����������
                                                     // �������� � �������� �������, ����� �� ������� ����� ������������ � ����� � ������
                            kegel.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            kegel.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            kegel.transform.rotation = Quaternion.Euler(0, 0, 0);
                            kegel.transform.position = kegelPositionList[index++];
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Kegel") /*&& !collisionSound.isPlaying*/ && rb.velocity.magnitude > 2)
        {
            Debug.Log(other.tag);
            collisionSound.Play();
        }
    }
}
