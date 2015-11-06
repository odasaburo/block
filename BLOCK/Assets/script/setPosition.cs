using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class setPosition : MonoBehaviour {
    public GameObject child;
    public float posx, posy;
    public Vector2 oldpos;
    public Sprite backimage1;
    public Sprite backimage2;
    // Use this for initialization
    void Start () {
        oldpos = child.GetComponent<RectTransform>().position;
        posx = 0;
        posy = 0;
    }
	
	// Update is called once per frame
	void Update () {
        /*if (child.GetComponent<RectTransform>().position.x < (oldpos.x + posx))
        {
            child.GetComponent<RectTransform>().position = new Vector2(oldpos.x + 1, oldpos.y);
        }
        if (child.GetComponent<RectTransform>().position.y > (oldpos.y - posy))
        {
            child.GetComponent<RectTransform>().position = new Vector2(oldpos.x, oldpos.y - 1);
        }*/
	}

    public void setposition(float x, float y, float width, float height) {
        child.transform.position = new Vector3(x, y, 0);
        child.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        child.GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    }

    public void addforce(int x, int y) {
        child.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y), 0);
    }

    public void addposition(float x, float y)
    {
        oldpos = child.GetComponent<RectTransform>().position;
        posx = x;
        posy = y;
        oldpos = child.GetComponent<RectTransform>().position;
        child.GetComponent<RectTransform>().position = new Vector2(child.GetComponent<RectTransform>().position.x +x, child.GetComponent<RectTransform>().position.y + y);
        //StartCoroutine(setpositiony());
       // StartCoroutine(setpositionx());
    }

    IEnumerator setpositiony()
    {
        Debug.Log(child.GetComponent<RectTransform>().position.y +"," + oldpos +","+posy);
        Debug.Log(child.GetComponent<RectTransform>().position.y > (oldpos.y + posy));
        while (child.GetComponent<RectTransform>().position.y > (oldpos.y + posy))
        {
            child.GetComponent<RectTransform>().position = new Vector2(oldpos.x, child.GetComponent<RectTransform>().position.y - 1);
            Debug.Log("bbbbb");
            yield return new WaitForSeconds(0.005f);
            
        }
    }

    IEnumerator setpositionx()
    {
        Debug.Log(child.GetComponent<RectTransform>().position.x + "," + oldpos + "," + posx);
        Debug.Log(child.GetComponent<RectTransform>().position.x > (oldpos.x + posx));
        while (child.GetComponent<RectTransform>().position.x > (oldpos.x + posx))
        {
            child.GetComponent<RectTransform>().position = new Vector2(child.GetComponent<RectTransform>().position.x + 1 , oldpos.y);
            Debug.Log("bbbbb");
            yield return new WaitForSeconds(0.005f);

        }
    }
}
