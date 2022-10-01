using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/***************************************\
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
ADD FMOD STUDIO LISTENER TO MAIN CAMERA
/***************************************/

public class FmodReference : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string fmodEventName;

    FMOD.Studio.EventInstance fmodTimeline;
    public FMODUnity.StudioEventEmitter fmodEmitter;

    //Play a one shot event
    public void PlayOneShot (string eventName)
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventName);
    }

    //Set a global parameter, these are custom params tied to effects automations in FMOD
    public void SetGlobalParameter (string paramName, float paramValue)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(paramName, paramValue);
    }

    //Set emitter parameter, these are custom params tied to effects automations in FMOD
    public void SetEmitterParameter (string paramName, float paramvalue)
    {
        fmodEmitter.SetParameter(paramName, paramValue);
    }

    //Start emitter
    public void StartEmitter ()
    {
        fmodEmitter.Play();
    }

    //Stop emitter
    public void StopEmitter ()
    {
        fmodEmitter.Stop();
    }

    //Create and start a new instance of an FMOD event, which in this example is a timeline,
    //but it could be a normal event or any other fmod event instance
    public void CreateTimelineInstance (string eventName)
    {
        fmodTimeline = FMODUnity.RuntimeManager.CreateInstance(eventName);
        fmodTimeline.start();
    }

    //Stop an FMOD event (including timelines)
    //.stop() can only be called on an event instance
    public void StopEvent ()
    {
        fmodTimeline.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}