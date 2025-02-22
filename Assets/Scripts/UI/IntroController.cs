using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public ObjectEventSO loadMenuEvent;

    private void Awake() {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.stopped += OnDirectorStopped;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && playableDirector.state == PlayState.Playing) {
            playableDirector.Stop();
        }

    }
    private void OnDirectorStopped(PlayableDirector director)
    {
        loadMenuEvent.RaiseEvent(null,this);
    }
}
