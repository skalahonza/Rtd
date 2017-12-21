 using UnityEngine;
 using UnityEngine.UI;
 
 public class Counter : MonoBehaviour {
     int time,a;
     float x;
     public bool count;
     public string timeDisp;
     public GameObject text;
     public delegate void countDownFinished();
     countDownFinished func;
 
     void Start () {
         time = 4;
         count = true;
     }
     
    public void setDelegate(countDownFinished countDownFinished){
        func = countDownFinished;
    }

     // Update is called once per frame
     void FixedUpdate (){
         if (count){
             timeDisp = time.ToString ();
             text.GetComponent<Text> ().text = timeDisp;
             x += Time.deltaTime;
             a = (int)x;
             switch(a){
                 case 0: text.GetComponent<Text> ().text = "3"; break;
                 case 1: text.GetComponent<Text> ().text = "2"; break;
                 case 2: text.GetComponent<Text> ().text = "1"; break;
                 case 3: text.GetComponent<Text> ().text = "Go"; break;
                 case 4: text.GetComponent<Text> ().enabled = false;
                     count = false; func(); break;
             }
         }
     }
 }