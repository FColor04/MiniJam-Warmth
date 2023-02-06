using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using MainGameFramework;
using Microsoft.Xna.Framework;
using RFMath = ReFactory.ExtraMathFunctions.ReFactoryExtraMathFunctions;

namespace AudioManagement
{

	public class SoundEmitterBase
	{
		private SoundEffectInstance sfxSound; // The Emitter Sound.

		public SoundEffectInstance SfxSound
		{
			get { return sfxSound; }
			set { sfxSound = value; }
		} // Public Get and Set for Sound Effect.

		private Vector2 position; // The Emitters Location.

		public Vector2 Position
		{
			get { return position; }
			set { position = value; } // Public Getter for location
		}


		public class ObjectSoundEmitter : SoundEmitterBase
		{
			private Vector2 playerPosition; // The Players current Location.
			public float[] radii = {5.0f, 25.5f}; // The Inner and Outer radius for sound emission
			private float emitterMaxVolume; // Maximum Volume of emitter.
			private float emitterMinVolume; // Minimum Volume of emitter.
			private float emitterVolumeSlide; // Volume Control Slider used to adjust the current Volume.

			public float EmitterMaxVolume
			{
				get { return emitterMaxVolume; }
			}

			public float EmitterMinVolume
			{
				get { return emitterMinVolume; }
			}

			public float EmitterVolumeSlide
			{
				get { return emitterVolumeSlide; }
				set { emitterVolumeSlide = value; }
			}

			public ObjectSoundEmitter(SoundEffectInstance sound, Vector2 emitterPos, float maxVolume, float minVolume)
			{
				this.sfxSound = sound;
				position = emitterPos;
				emitterMaxVolume = maxVolume;
				emitterMinVolume = minVolume;
			}

			public void Update(Vector2 playerPosition)
			{
				for (int i = radii.Length - 1; i >= 0; i--)
				{
					float distance = Vector2.Distance(position, playerPosition);
					if (distance <= radii[i])
					{
						float t = RFMath.InverseLerpClamped(radii[i], radii[0], distance);
						emitterVolumeSlide = MathHelper.Lerp(EmitterMinVolume, EmitterMaxVolume, t);
						break;
					}
				}
			}
		}

		public class PlayerSoundRadar
		{


			public PlayerSoundRadar()
			{

			}
		}

		public class PlayerSoundEmitter : SoundEmitterBase
		{
			private float radius; // Radius that the player emits sound to.
			private float noiseVal; // The amount of Noise that the player currently makes based on several factors.
			private float movementMultiplier; // A multiplier for the amount of additional Noise made when moving.
			private bool isMoving; // A boolean for whether the player is moving.

			public float Radius
			{
				get { return radius; }
				set { radius = value; }
			}

			public float Noise
			{
				get { return noiseVal; }
				set { noiseVal = value; }
			}

			public float MovementMultiplier
			{
				get { return movementMultiplier; }
				set { movementMultiplier = value; }
			}

			public bool IsMoving
			{
				get { return isMoving; }
				set { isMoving = value; }
			}

			public PlayerSoundEmitter(SoundEffectInstance sound, Vector2 playerPosition, float radius,
				float movementMultiplier, float noiseVal)
			{
				this.position = playerPosition;
				this.radius = radius;
				this.movementMultiplier = movementMultiplier;
				this.isMoving = false;
				this.noiseVal = noiseVal;
			}

			public void Update()
			{
				if (isMoving)
				{
					noiseVal *= movementMultiplier;
				}
				else
				{
					noiseVal = Noise;
				}
			}
		}
	}
}