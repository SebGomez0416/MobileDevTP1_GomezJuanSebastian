using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = new InputManager();

    private Dictionary<string, float> axisValues = new Dictionary<string, float>();

    public void SetAxis(string axis, float value)
    {
        if(!axisValues.ContainsKey(axis))
            axisValues.Add(axis,value);
        axisValues[axis] = value;
    }

    private float GetOrAddAxis(string axis)
    {
        if(!axisValues.ContainsKey(axis))
            axisValues.Add(axis,0f);
        return axisValues[axis];
    }

    public float GetAxis(string axis)
    {
#if UNITY_EDITOR
        return GetOrAddAxis(axis) + Input.GetAxis(axis);
#endif 
#if UNITY_ANDROID
        return GetOrAddAxis(axis); 
#endif 
#if UNITY_STANDALONE
        return GetOrAddAxis(axis);
#endif       
    }
    
    public bool GetButton(string button)
    {
        return Input.GetButton(button);
    }
}



