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
        ballStartPosition = Ball.transform.position;  // запоминаем стартовое положение шара
        score = 0;
        shots = 0;
        isMoving = false;
        collisionSound = GetComponent<AudioSource>();
        ForceLine = GameObject.Find("ForceLine");

        // сохраняем стартовые позиции кегель
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
            //await Task.Delay(500);            // дать время шару начать движение
            if (rb.velocity == Vector3.zero && dt > 1)  // признак остановки
            {
                isMoving = false;
                Arrow.SetActive(true);                        // включаем стрелку
                ForceLine.SetActive(true);
                Ball.transform.position = ballStartPosition;  // возвращаем шар на старт
                score = 0;

                //ball stopped - count hited kegels
                var kegels = GameObject.FindGameObjectsWithTag("Kegel");
                foreach (var kegel in kegels)
                {
                    //Debug.Log(kegel.name + "LOC Y = " + kegel.transform.localPosition.y +
                    //                      "  GL Y = " + kegel.transform.position.y);
                    //if ( !(kegel.transform.localPosition.y > 10f && kegel.transform.localPosition.y < 10.5f) )  // кегля упала
                    //{                               // у меня: кегля не упала - Y=10,40       упала - Y=11,22
                   /* Debug.Log(kegel.name + "Rot X = " + kegel.transform.localRotation.x +
                        "  Rot Z = " + kegel.transform.rotation.z);*/

                    if ( Mathf.Abs(kegel.transform.rotation.x) > 0.1 || Mathf.Abs(kegel.transform.rotation.z) > 0.1)
                    {
                        score++;
                        kegel.transform.position += (Vector3.down * 2);  // роняем в подполье :)
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

                    Arrow.SetActive(false);  // отключаем стрелку
                    ForceLine.SetActive(false);
                    isMoving = true;
                    dt = 0;

                    shots++;
                    titleShotNum.text = "Shots: " + shots;
                }
                else   //табом сбрасываем счетчики
                {
                    score = 0;
                    shots = 0;
                    titleShotNum.text = "Shots: " + shots;
                    titleScore.text = "Score: " + score;

                    {  // возвращаем кегли на старт
                        int index = 0;
                        var kegels = GameObject.FindGameObjectsWithTag("Kegel");
                        foreach (var kegel in kegels)
                        {
                            await Task.Delay(150);   // плавная поочередная расстановка
                                                     // скорость и вращение обнулим, иначе по инерции кегли сталкиваются с полом и падают
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
