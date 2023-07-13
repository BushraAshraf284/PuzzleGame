using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameController : MonoBehaviourPun
{
    public static GameController gc;
    [Header("Game Info")]
    public float timer;
    public bool gameFinished;
    public bool Cooldown;

    [Header("Hud Info")]
    public TMP_Text Timer;
    public TMP_Text MyScoreText;
    public TMP_Text OtherScoreText;
    public GameObject EndGamePanel;
    public TMP_Text WinText;
   

    [Header("ButtonList")]
    public GameButton[] ButtonList;

    private int myScore;
    private int otherScore;
    
    PhotonView PV;
    GameObject clickedButton;
    GameButton currentButton;


    private void Awake()
    {
        if (gc == null)
        {
            gc = this;
        }
        else if (gc != this)
        {
            Destroy(gc.gameObject);
            gc = this;
        }
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(DecreaseTimer),0,1);      
    }

    public void ButtonPress()
    {
        clickedButton = EventSystem.current.currentSelectedGameObject;
        currentButton = clickedButton.GetComponent<GameButton>();
        
        Debug.Log("Clicked Button:"+ currentButton.id);
        switch(currentButton.id)
        {
            case 1:
            case 2:
                Debug.Log(currentButton.score);
                myScore += currentButton.score;
                PV.RPC(nameof(RPC_SendScore), RpcTarget.Others, myScore);
                break;
            case 3:
                PV.RPC(nameof(RPC_SendAttack), RpcTarget.Others);
                break;
            case 4:
                Cooldown = false;
                currentButton.StartCooldown();
                Invoke(nameof(EnableCoolDown), 10f);
                break;
        }
        if (Cooldown)
        {
            currentButton.StartCooldown();
        }
        /*  myScore += 10;
          Debug.Log("Button1 is pressed!");

          PV.RPC("RPC_SendScore", RpcTarget.Others, myScore);*/

    }

    /*public void ButtonPress2()
    {
        myScore += 20;
        Debug.Log("Button2 is pressed!");
        if (!ButtonList[1].noCooldown)
        {
            Debug.Log("Button2 has nocooldown: "+ ButtonList[1].noCooldown);
            ButtonList[1].GetComponent<Button>().interactable = false;
            ButtonList[1].startCooldown = true;
            ButtonList[1].disableTime = ButtonList[1].cooldownTime;
        }   

    }

    public void ButtonPress3()
    {
        Debug.Log("Button3 is pressed!");
        ButtonList[2].GetComponent<Button>().interactable = false;
        ButtonList[2].startCooldown = true;
        ButtonList[2].disableTime = ButtonList[2].cooldownTime;
    }

    public void ButtonPress4()
    {
        Debug.Log("Button4 is pressed!");
        ButtonList[3].GetComponent<Button>().interactable = false;
        ButtonList[3].startCooldown = true;
        ButtonList[3].disableTime = ButtonList[3].cooldownTime;
    }*/
    void DecreaseTimer()
    {
        if (timer > 0)
        {
            timer--;
            Timer.text = timer.ToString();
            OtherScoreText.text = otherScore.ToString();
            MyScoreText.text = myScore.ToString();
        }
        if (timer == 0)
        {
            gameFinished = true;//p1>p2 == win player
            EndGame();
            CancelInvoke(nameof(DecreaseTimer));            
        }
    }
    public void EndGame()
    {
        EndGamePanel.SetActive(true);
        Timer.text = "00";
        if(myScore>otherScore)
        {
            WinText.text = "You Won!";
        }
        else
        {
            WinText.text = "Other Player Won!";
        }
    }
    public void EnableCoolDown()
    {
        Debug.Log("Enable Cooldown is called");
        Cooldown = true;
    }

    [PunRPC]
    private void RPC_SendScore(int score)
    {
        otherScore = score;
    }

    [PunRPC]
    private void RPC_SendAttack()
    {
        Debug.Log("Recieved Attack");
        foreach (var btn in ButtonList)
        {
            Debug.Log("Button id:"+ btn.id);
            btn.DisableButton();
        }

    }

}

