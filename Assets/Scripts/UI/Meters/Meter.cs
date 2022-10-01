using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Every meter can have a 1-1 mapping with a 'channel' 
// that other code can use to easily find this meter
// eg. player health component can call 
// Meter.Get(ValueChannel.Health).value
// Instead of needing to establish and hold a reference
// Feel free to add/remove these as requires for a game
public enum ValueChannel {
    None, 
    Health,
    Stamina,
    Air,
    Boost,
    Cooldown,
    Progress,
    Speed,
    Fuel,
}

// Supported rounding types for meters
public enum RoundingMethod {
    None,
    NearestInt,
    NearestHalf,
    NearestQuarter,
}

public abstract class Meter : MonoBehaviour
{
    // If not None, channel that other scripts can use to easily get this meter
    public ValueChannel channel = ValueChannel.None;

    // Type of rounding this meter should employ 
    // (used for any meters that might want to move in clear increments instead of smoothly)
    public RoundingMethod roundMethod = RoundingMethod.None;

    // Changing these at runtime is only semi-supported
    // the new values won't take effect until the next time Value is changed
    // (or UpdateValue is called through some other means that shouldn't happen) 
    public float minVal;
    public float maxVal;

    // Protected values that inherited meter types should use to drive their values
    protected float curVal;
    protected float normalizedVal;

    private static Dictionary<ValueChannel, Meter> GlobalMetersDict;

    // Property to access and set a bounded version of the value
    // Pokes the overridden function in things that inherit from this to display the new value
    public float Value {
        get { return curVal; }
        set 
        { 
            curVal = Mathf.Clamp(value, minVal, maxVal); 
            switch (roundMethod)
            {  
                case RoundingMethod.NearestInt:
                    curVal = Mathf.Round(curVal);
                    break;
                case RoundingMethod.NearestHalf:
                    curVal = Mathf.Round(curVal * 2.0f) / 2.0f;
                    break;
                case RoundingMethod.NearestQuarter:
                    curVal = Mathf.Round(curVal * 4.0f) / 4.0f;
                    break;
                default:
                    break;
            }
            normalizedVal = Helpers.Remap(curVal, minVal, maxVal, 0, 1); 
            UpdateValue(); 
        }
    }

    // Get the meter corresponding to a channel
    // Getting a 'None' meter is invalid
    // There can only be one meter per channel
    public static Meter Get(ValueChannel channel)
    {
        if(channel == ValueChannel.None) 
        {
            Debug.LogError("Getting Meters on channel 'None' is not valid");
            return null;
        }

        Meter got;
        if(GlobalMetersDict.TryGetValue(channel, out got))
        {
            return got;
        }
        else
        {
            Debug.LogErrorFormat("Failed to get meter on channel {0}", channel);
            return null;
        }
    }

    private void Awake()
    {
        if(channel != ValueChannel.None) 
        {
            GlobalMetersDict.Add(channel, this);
        }
    }

    static Meter()
    {
        GlobalMetersDict = new Dictionary<ValueChannel, Meter>();
    }

    protected abstract void UpdateValue();
}
