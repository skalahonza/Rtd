 using UnityEngine.UI;
 using UnityEngine;

 public class Counter : MonoBehaviour {
     int time, a;
     float x;
     public bool count;
     public string timeDisp;
     public GameObject text;
     public delegate void countDownFinished ();
     countDownFinished func;

     /// <summary>
     /// game initialization - specific data
     /// </summary>
     void Start () {
         time = 4;
         count = true;
     }

     /// <summary>
     /// Set delegate to be called when countdown reaches 0
     /// </summary>
     /// <param name="countDownFinished">
     /// delegate
     /// </param>
     public void setDelegate (countDownFinished countDownFinished) {
         func = countDownFinished;
     }

     /// <summary>
     /// run count down
     /// </summary>
     void FixedUpdate () {
         if (count) {
             timeDisp = time.ToString ();
             text.GetComponent<Text> ().text = timeDisp;
             x += Time.deltaTime;
             a = (int) x;
             switch (a) {
                 case 0:
                     text.GetComponent<Text> ().text = "3";
                     break;
                 case 1:
                     text.GetComponent<Text> ().text = "2";
                     break;
                 case 2:
                     text.GetComponent<Text> ().text = "1";
                     break;
                 case 3:
                     text.GetComponent<Text> ().text = "Go";
                     break;
                 case 4:
                     text.GetComponent<Text> ().enabled = false;
                     count = false;
                     func ();
                     break;
             }
         }
     }
 }