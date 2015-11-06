using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockControl : MonoBehaviour {

    public GameObject[] block;
    public GameObject ground;
    private GameObject[,] stageblock;
    public int gamelevel = 1;

    public float width = 50;
    public float height = 50;
    public Vector2 startpos;
    public Vector2 mousepos;

    public int[,] block_color;
    public int[,] block_same;
    float width_index;
    float height_index;
    public int destroy_count = 0;

    public bool gameplay_enable = false;
    public bool level_complete = false;
    // Use this for initialization
    void Start () {
        //init();
    }
	
	// Update is called once per frame
	void Update () {
        if(destroy_count == 72)
        {
            level_complete = true;
            destroy_count = 0;
        }
        if (gameplay_enable)
        {
            mousepos = Input.mousePosition;
            width_index = (mousepos.x - startpos.x + width / 2) / width;
            height_index = (mousepos.y - startpos.y + height / 2) / height;
            // Debug.Log((int)width_index + "," + (int)height_index);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log((int)width_index + 1 + "," + ((int)height_index + 1));
                /////////////////destroy block//////////////////////
                detectAround((int)width_index, (int)height_index);
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (block_same[i, j] == 1)
                        {
                            Destroy(stageblock[i, j]);
                            block_color[i, j] = 10;
                            destroy_count++;
                        }
                    }
                }
                //////////////block_same init///////////////////////////
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        block_same[i, j] = 0;
                    }
                }
                ///////////////////////////// add force///////////////////////////
                if ((int)width_index <= 7 && (int)width_index >= 0 && (int)height_index >= 0 && (int)height_index <= 8)
                {
                    ////////////////////block add force //////////////////////////////////

                    for (int i = 0; i < 8; i++)
                    {

                        for (int j = 0; j < 9; j++)
                        {
                            if (stageblock[i, j] != null)
                            {
                                stageblock[i, j].GetComponent<setPosition>().addforce(0, -100);
                            }
                            else
                            {
                                Debug.Log(i + "," + j);
                            }
                        }
                    }


                }
            }



            //////////////////block position update///////////////////
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (stageblock[i, j] == null)
                    {
                        for (int k = j + 1; k < 9; k++)
                        {
                            if (stageblock[i, k] != null)
                            {
                                stageblock[i, j] = stageblock[i, k];
                                block_color[i, j] = block_color[i, k];
                                block_color[i, k] = 10;
                                stageblock[i, k] = null;
                                break;
                            }
                        }
                    }
                }
            }
            for (int i = 7; i >= 0; i--)
            {
                if (stageblock[i, 0] == null && stageblock[i, 1] == null && stageblock[i, 2] == null && stageblock[i, 3] == null && stageblock[i, 4] == null && stageblock[i, 5] == null && stageblock[i, 6] == null && stageblock[i, 7] == null && stageblock[i, 8] == null)
                {
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (stageblock[k - 1, j] != null)
                            {
                                stageblock[k - 1, j].GetComponent<setPosition>().addposition(width, 0);
                                stageblock[k, j] = stageblock[k - 1, j];
                                block_color[k, j] = block_color[k - 1, j];
                                stageblock[k - 1, j] = null;
                                block_color[k - 1, j] = 10;
                            }
                            else
                            {
                                stageblock[k, j] = null;
                                block_color[k, j] = 10;
                            }
                        }
                    }
                }

            }
            ////////////////////////////////////////////////////////

        }
    }

    public void init() {
        gameplay_enable = true;
        float screenwidth = Screen.width;
        float screenheight = Screen.height;
        float min = Mathf.Min(screenwidth, screenheight);
        width = min / 14;
        height = min / 14;
        startpos = new Vector2((screenwidth - width * 8) / 2, screenheight - height * 11);
        GameObject Ground = Instantiate(ground);
        Ground.GetComponent<setPosition>().setposition(Screen.width / 2, height * 1.5f, Screen.width, height * 2f);
        block_color = new int[8, 9];
        block_same = new int[8, 9];
        stageblock = new GameObject[8, 9];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                block_color[i, j] = 0;
                block_same[i, j] = 0;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int number = Random.Range(0, gamelevel + 1);
                GameObject newblockcanvas = Instantiate(block[number]);
                newblockcanvas.GetComponent<setPosition>().setposition(startpos.x + width * i, startpos.y + height * j, width - 0.1f, height - 0.1f);
                block_color[i, j] = number;
                stageblock[i, j] = newblockcanvas;
            }
        }
    }

    public void destroyAll()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if (stageblock[i, j] != null)
                {
                    Destroy(stageblock[i, j]);
                    block_color[i, j] = 10;
                }
            }
        }
    }
    void detectAround(int x_index, int y_index ) {
        if (x_index >= 0 && x_index < 8 && y_index >= 0 && y_index < 9)
        {
            if (block_color[x_index, y_index] != 10)
            {
                block_same[x_index, y_index] = 1;

                if (x_index - 1 >= 0 && (x_index - 1) < (int)width_index)
                {
                    if (block_color[x_index - 1, y_index] == block_color[x_index, y_index])
                    {
                        block_same[x_index - 1, y_index] = 1;
                        Debug.Log(x_index + "," + (y_index + 1));
                        detectAround(x_index - 1, y_index);
                    }
                }


                if (x_index + 1 < 8 && (x_index + 1) > (int)width_index)
                {
                    if (block_color[x_index + 1, y_index] == block_color[x_index, y_index])
                    {
                        block_same[x_index + 1, y_index] = 1;
                        Debug.Log(x_index + 2 + "," + (y_index + 1));
                        detectAround(x_index + 1, y_index);
                    }
                }

                if (y_index - 1 >= 0 && y_index - 1 < (int)height_index)
                {
                    if (block_color[x_index, y_index - 1] == block_color[x_index, y_index])
                    {
                        block_same[x_index, y_index - 1] = 1;
                        Debug.Log(x_index + 1 + "," + y_index);
                        detectAround(x_index, y_index - 1);
                    }
                }

                if (y_index + 1 < 9 && y_index + 1 > (int)height_index)
                {
                    if (block_color[x_index, y_index + 1] == block_color[x_index, y_index])
                    {
                        block_same[x_index, y_index + 1] = 1;
                        Debug.Log(x_index + 1 + "," + (y_index + 1));
                        detectAround(x_index, y_index + 1);
                    }
                }
            }
        }
    }

}
