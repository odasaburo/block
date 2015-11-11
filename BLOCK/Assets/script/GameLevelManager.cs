using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour {
    public GameObject blockcontrol;
    public GameObject playmenu;
    public GameObject complete;
    public GameObject[] gamebackground;
    public GameObject life;
    public GameObject FBobject;

    public Text gameaction_text;
    public Image audiobutton;
    public Sprite audioON;
    public Sprite audioOFF;
    public Text lives_text;
    public Text score_text;
    public int lives = 5;
    public int score = 0;
	// Use this for initialization
	void Start () {
        complete.SetActive(false);
        playmenu.SetActive(true);
        lives = 5;
        score = 0;
        life.SetActive(false);
        AudioListener.volume = 1;
        audiobutton.sprite = audioON;
        //FBobject.GetComponent<FBHolder>().FBlogin();
    }
	
	// Update is called once per frame
	void Update () {
        lives_text.text = lives.ToString();
        score_text.text = score.ToString();
        if (blockcontrol.GetComponent<BlockControl>().level_complete)
        {
            blockcontrol.GetComponent<BlockControl>().level_complete = false;
            nextLevel();
        }
        if(lives == 0)
        {
            GameOver();
        }
	}

    public void newGame() {
        complete.SetActive(false);
        playmenu.SetActive(false);
        life.SetActive(true);
        score = 0;
        lives = 5;
        blockcontrol.GetComponent<BlockControl>().gamelevel = 1;
        blockcontrol.GetComponent<BlockControl>().init();
    }

    public void nextLevel()
    {
        blockcontrol.GetComponent<BlockControl>().gamelevel ++;
        if (blockcontrol.GetComponent<BlockControl>().gamelevel <= 5)
        {
            blockcontrol.GetComponent<BlockControl>().init();
            setbackgroundImage(blockcontrol.GetComponent<BlockControl>().gamelevel);
        }
        else
        {
            gameComplete();
            life.SetActive(false);
        }
    }

    public void GameOver()
    {
        blockcontrol.GetComponent<BlockControl>().gamelevel = 1;
        blockcontrol.GetComponent<BlockControl>().destroyAll();
        complete.SetActive(true);
    }

    public void MainMenu()
    {
        lives = 5;
        blockcontrol.GetComponent<BlockControl>().init();
        blockcontrol.GetComponent<BlockControl>().gamelevel = 1;
        blockcontrol.GetComponent<BlockControl>().destroyAll();
        playmenu.SetActive(true);
        complete.SetActive(false);
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

    public void audioSwitch()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            audiobutton.sprite = audioOFF;
        }
        else if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            audiobutton.sprite = audioON;
        }
    }

    public void setbackgroundImage(int level)
    {
        for(int i = 0; i < 3; i++)
        {
            gamebackground[i].SetActive(false);
        }
        int index = level % 3;
        gamebackground[index].SetActive(true);
    }

    public void setactionstring(string text)
    {
        gameaction_text.text = text;
    }
}
