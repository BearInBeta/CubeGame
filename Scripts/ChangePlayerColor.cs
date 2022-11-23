﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerColor : MonoBehaviour {
    [SerializeField] AudioClip[] notes;
    [SerializeField] PhysicMaterial bouncy;
    [SerializeField] PhysicMaterial Max;
    [SerializeField] Texture texture;
    Color[] colors = new Color[] {Color.white, Color.cyan, new Color(1, 69f / 255f, 0, 1), Color.yellow, Color.green, Color.red, new Color(.5f, .8f,1, 0.2f), Color.magenta };

    [SerializeField] BlockTypes.TYPES type;
    // Use this for initialization
    void Start () {
        changeType(type, false);
    }
    public BlockTypes.TYPES getType()
    {
     
        return type;
    }
    public Color getColor()
    {
        return colors[(int)type];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void changeType(BlockTypes.TYPES type, bool gradual)
    {
        this.type = type;
        if (gradual)
        {
            StartCoroutine(changeColor());
        }else
            ColorSet();
    }
    IEnumerator changeColor()
    {
        while (!GetComponent<Renderer>().material.GetColor("_Color").Equals(getColor()))
        {
            
            GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(GetComponent<Renderer>().material.GetColor("_Color"), getColor(), 0.01f));
            yield return new WaitForFixedUpdate();
        }
        ColorSet();
    }
    void ColorSet()
    {
        GetComponent<BoxCollider>().material = Max;
        GetComponent<Renderer>().material.SetColor("_Color", getColor());
        if (type == BlockTypes.TYPES.SHOOTABLE)
        {
            gameObject.tag = "Shootable";
        }
        GetComponent<AudioSource>().clip = notes[(int)type];
    }





  

}

public static class BlockTypes
{
    public enum TYPES:int // your custom enumeration
    {
        NORMAL = 0, DOUBLE_JUMP = 1, BOUNCE = 2, SPEED = 3, GRAVITY = 4, SHOOT = 5, SHOOTABLE = 6, ROTATE =7
    };
}
