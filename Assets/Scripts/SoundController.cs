using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioClip jumpSound;
    public AudioClip dropBoxSound;
    public AudioClip pickupBoxSound;
    public AudioClip scoreSound;
    public AudioClip errorSound;
    public AudioClip hitSound;
    
    private float scoreResetTime = 2f;
    private int scoreStreak = 1;
    private float scoreCurrentTime = 0f;

    public AudioSource audioSource;
    public AudioSource scoreAudioSource;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnEnable() {
        EventManager.StartListening("Player Hit", playHitSound);
        EventManager.StartListening("Score", playScoreSound);
        EventManager.StartListening("Error", playErrorSound);
        EventManager.StartListening("Pick Up", playPickupSound);
        EventManager.StartListening("Drop", playDropSound);
        EventManager.StartListening("Jump", playJumpSound);
        
    }

    public void OnDisable()
    {
        EventManager.StopListening("Player Hit", playHitSound);
        EventManager.StopListening("Score", playScoreSound);
        EventManager.StopListening("Error", playErrorSound);
        EventManager.StopListening("Pick Up", playPickupSound);
        EventManager.StopListening("Drop", playDropSound);
        EventManager.StopListening("Jump", playJumpSound);
    }

    private void playJumpSound() {
        audioSource.PlayOneShot(jumpSound);
    }

    private void playDropSound() {
        audioSource.PlayOneShot(dropBoxSound);
    }

    private void playPickupSound() {
        audioSource.PlayOneShot(pickupBoxSound);
    }

    private void playErrorSound() {
        audioSource.PlayOneShot(errorSound);
    }

    private void playHitSound() {
        audioSource.PlayOneShot(hitSound);
    }

    private void playScoreSound() {
        audioSource.clip = scoreSound;
        if (scoreResetTime > Time.time - scoreCurrentTime) {
            scoreAudioSource.pitch += 0.3f;
        } else {
            scoreAudioSource.pitch = 1;
        }
        scoreCurrentTime = Time.time;

        scoreAudioSource.PlayOneShot(scoreSound);
    }

}
