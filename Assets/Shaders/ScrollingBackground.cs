using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public int Speed;
    public Texture2D ScrollImage;
    
    private const string PROP_NAME_SCROLL_SPEED = "_WindowScrollSpeed";
    private int PROP_ID_SCROLL_SPEED;
    
    private const string PROP_NAME_MAIN_TEX = "_MainTex";
    private int PROP_ID_MAIN_TEX;
    
    private MaterialPropertyBlock PropertyBlock;
    
    private void Start()
    {
        PROP_ID_SCROLL_SPEED = Shader.PropertyToID(PROP_NAME_SCROLL_SPEED);
        PROP_ID_MAIN_TEX = Shader.PropertyToID(PROP_NAME_MAIN_TEX);
        
        PropertyBlock = new MaterialPropertyBlock();
        PropertyBlock.SetInt(PROP_ID_SCROLL_SPEED, Speed);
        PropertyBlock.SetTexture(PROP_ID_MAIN_TEX, ScrollImage);
        GetComponent<MeshRenderer>().SetPropertyBlock(PropertyBlock);
    }
}
