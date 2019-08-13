using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouseCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorTexture;
    private bool enableOnDisable = false;

    [SerializeField]
    private bool forceEnable = false;


    void Start()
    {        
        //Cursor.SetCursor (cursorTexture, new Vector2( centerX,centerY) ,CursorMode.ForceSoftware);
        Cursor.SetCursor(cursorTexture, new Vector2(0,0), CursorMode.ForceSoftware);
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        Cursor.visible = true;        
    }

    private void OnDisable()
    {
        Cursor.visible = false;
    }   
}
