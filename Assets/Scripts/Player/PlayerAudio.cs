using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerAudio {
    // Variables declaration
    [SerializeField] private AudioClip[] footStepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
	[SerializeField] private float walkingVolume;
	[SerializeField] private float regularVolume;
	// End of variables declaration

	public void PlayFootStepAudio(CharacterController player, AudioSource audio, bool isWalking) {
        if (!player.isGrounded) return;

		AdjustAudio(audio, isWalking);

        int n = Random.Range(1, footStepSounds.Length);
        audio.clip = footStepSounds[n];
        audio.PlayOneShot(audio.clip);

        footStepSounds[n] = footStepSounds[0];
        footStepSounds[0] = audio.clip;
    }

    public void PlayJumpAudio(AudioSource audio) {
		AdjustAudio(audio);

        audio.clip = jumpSound;
        audio.Play();
    }

    public void PlayLandAudio(AudioSource audio, float nxtStp, float stpCycle) {
		AdjustAudio(audio);

		audio.clip = landSound;
        audio.Play();
        nxtStp = stpCycle + 0.5f;
    }

	private void AdjustAudio(AudioSource audio, bool iswalking = false) {
		float volume = iswalking ? walkingVolume : regularVolume;
		volume = Mathf.Clamp(volume, 0, 1);
		audio.volume = volume;
	}

}