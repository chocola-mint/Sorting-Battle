using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.Sound
{
    public class GameEventSoundBinder : MonoBehaviour
    {
        [SerializeField] private AudioClip gameOverJingle, gameOverMusic;
        [SerializeField] private GameEventDelegate gameEventDelegate;
        [SerializeField] private Jukebox jukebox;
        // Start is called before the first frame update
        void Start()
        {
            gameEventDelegate.onGameOverStart.AddListener(() => jukebox.PlayClip(gameOverJingle));
            gameEventDelegate.onGameOverEnd.AddListener(() => jukebox.PlayClip(gameOverMusic));
        }
    }
}
