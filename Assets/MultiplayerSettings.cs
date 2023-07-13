using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings ms;
    public bool delayStart;
    public int maxPlayers;
    public int MenuScene;
    public int GameScene;
    private void Start()
    {
        if (ms == null)
        {
            ms = this;
        }
        else if (ms != this)
        {
            Destroy(ms.gameObject);
            ms = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }
}
