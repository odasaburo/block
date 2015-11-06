using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class setPosition : MonoBehaviour {
    public GameObject child;
    public float posx, posy;
    public Vector2 oldpos;
	// Use this for initialization
	void Start () {
        oldpos = child.GetComponent<RectTransform>().position;
        posx = 0;
        posy = 0;
    }
	
	// Update is called once per frame
	void Update () {
        ///child.GetComponent<RectTransform>().position = Vector2.Lerp(oldpos, new Vector2(oldpos.x + posx, oldpos.y + posy), Time.deltaTime);
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
        posx = x;
        posy = y;
        oldpos = child.GetComponent<RectTransform>().position;
        child.GetComponent<RectTransform>().position = new Vector2(child.GetComponent<RectTransform>().position.x +x, child.GetComponent<RectTransform>().position.y + y);
    }
}
