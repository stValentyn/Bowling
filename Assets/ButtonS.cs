using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;

public class ButtonS : MonoBehaviour
{
    public GameObject MainWindowImage;
    public static bool IsPaused;

    // Start is called before the first frame update
    void Start()
    {
       /* IsPaused = true;
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameData));
        if(File.Exists("gamedata.xml"))
        {
            using (StreamReader reader = new StreamReader("gamedata.xml")) ;
            Debug.Log(GameData.BestScore )
        }
        else
        {
            using(StreamWriter writer = new StreamWriter("gamedata.xml"))
            {
                xmlSerializer.Serialize(writer, new GameData { BestScore = 10})
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = true;
        }

        MainWindowImage.SetActive(IsPaused);

    }

    public void OnStartClick() 
    {
        //Debug.Log("Button Start click");
        IsPaused = false;
    }
    public void OnCloseClick()
    {
        Application.Quit();
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public class GameData
    {
        public int BestScore { get; set; }
    }
}

/*public static class AppHelper
{
#if UNITY_WEBPLAYER
     public static string webplayerQuitURL = "http://google.com";
#endif
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}*/