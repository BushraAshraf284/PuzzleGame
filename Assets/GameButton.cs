using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
   
    public float cooldownTime;
    public float AttackTime;
    public int score;
    public bool disable;
    public bool startCooldown;
    public bool coolDown;
    public int disableTime;
    public int id;
    private int totalTime;

    public TMP_Text buttonText;

 

    // Update is called once per frame
    /*  void FixedUpdate()
      {

         if(!GameController.gc.gameFinished)
         {      
          if (disableTime > 0)
          {
              // Debug.Log(id + ":" + disableTime);
              disableTime -= Time.deltaTime;                        
              buttonText.text = ((int)disableTime).ToString();
          }   
         }

      }*/
    public void StartCooldown()
    {
        Debug.Log(id + ": Button should get disabled");
        gameObject.GetComponent<Button>().interactable = false;
        //startCooldown = true;
        disableTime = (int)cooldownTime;
        totalTime = (int)cooldownTime;
        InvokeRepeating(nameof(DecreasedisableTime), 0, 1);
        //Invoke(nameof(EnableButton), cooldownTime);
    }

    public void EnableButton()
    {
        Debug.Log(id+": "+"Enable Button is called, Button:"+ id+" should be enabled");
        gameObject.GetComponent<Button>().interactable = true;       
        buttonText.text = id.ToString();
       /* startCooldown = false;
        disable = false;*/

    }

    public void DisableButton()
    {
        Debug.Log(id + ": Button should get disabled by disable button");
        gameObject.GetComponent<Button>().interactable = false;
        disableTime = (int)AttackTime;
        totalTime = (int)AttackTime;
        InvokeRepeating(nameof(DecreasedisableTime), 0, 1);
        //Invoke(nameof(EnableButton), AttackTime);
    }

    void DecreasedisableTime()
    {
        if (disableTime > 0)
        {
            disableTime--;
            Debug.Log((float)disableTime /totalTime);
            gameObject.transform.parent.GetComponent<Image>().fillAmount = ((float)disableTime / totalTime);
            buttonText.text = ((int)disableTime).ToString();
        }
        if (disableTime == 0)
        {
            gameObject.transform.parent.GetComponent<Image>().fillAmount = 1;
            EnableButton();
            CancelInvoke(nameof(DecreasedisableTime));
            
        }
    }
}
