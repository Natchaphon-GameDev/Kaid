using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class CubeButton : MonoBehaviour {
 
    public UnityEvent CubeClickEvent; 
 
    private void OnMouseDown()
    {
        print("You clicked the cube!");
        CubeClickEvent.Invoke(); 
    }
}
