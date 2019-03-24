using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerAudio {
    // Variables declaration
    [SerializeField] private AudioClip[] footStepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    // End of variables declaration

    public void PlayFootStepAudio(CharacterController player, AudioSource audio) {
        if (!player.isGrounded) return;

        int n = Random.Range(1, footStepSounds.Length);
        audio.clip = footStepSounds[n];
        audio.PlayOneShot(audio.clip);

        footStepSounds[n] = footStepSounds[0];
        footStepSounds[0] = audio.clip;
    }

    public void PlayJumpAudio(AudioSource audio) {
        audio.clip = jumpSound;
        audio.Play();
    }

    public void PlayLandAudio(AudioSource audio, float nxtStp, float stpCycle) {
        audio.clip = landSound;
        audio.Play();
        nxtStp = stpCycle + 0.5f;
    }

}