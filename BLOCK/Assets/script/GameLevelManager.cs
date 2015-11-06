using UnityEngine;
using System.Collections;

public class GameLevelManager : MonoBehaviour {
    public GameObject blockcontrol;
    public GameObject playmenu;
    public GameObject complete;
	// Use this for initialization
	void Start () {
        complete.SetActive(false);
        playmenu.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (blockcontrol.GetComponent<BlockControl>().level_complete)
        {
            blockcontrol.GetComponent<BlockControl>().level_complete = false;
            nextLevel();
        }
	}

    public void newGame() {
        playmenu.SetActive(false);
        blockcontrol.GetComponent<BlockControl>().gamelevel = 1;
        blockcontrol.GetComponent<BlockControl>().init();
    }

    public void nextLevel()
    {
        blockcontrol.GetComponent<BlockControl>().gamelevel ++;
        if (blockcontrol.GetComponent<BlockControl>().gamelevel <= 5)
        {
            blockcontrol.GetComponent<BlockControl>().init();
        }
        else
        {
            gameComplete();
        }
    }

    public void GameOver()
    {
        blockcontrol.GetComponent<BlockControl>().gamelevel = 1;
        blockcontrol.GetComponent<BlockControl>().destroyAll();
    }

    public void endGame()
    {
        blockcontrol.GetComponent<BlockControl>().gamelevel = 1;
        blockcontrol.GetComponent<BlockControl>().destroyAll();

    }
    
    public void gameComplete()
    {
        complete.SetActive(true);
    }

    public void RestartLevel()
    {

    }

    public void SoundSet(string onoff)
    {

    }

}
