using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public double initspeed;
        public double distanceTravelled;
        public double g;
        public GameObject cart; 
        private double energy;
        public double v;
        public GameObject floor;
        public double height;

        public double audioPitch = 1;
        public double v_normalizer;
        AudioSource audioSource;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                energy = g * (cart.transform.position.y - floor.transform.position.y);
                audioSource = GetComponent<AudioSource>();
                audioSource.pitch = (float)audioPitch;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                height = cart.transform.position.y - floor.transform.position.y;
                v = initspeed + Math.Sqrt(2 * ((double)energy - g * (cart.transform.position.y - floor.transform.position.y)));
                // audioPitch = v / 2;

                distanceTravelled += v * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance((float)distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance((float)distanceTravelled, endOfPathInstruction);
                
                audioSource.pitch = (float)v / (float)v_normalizer;
            }

            if (distanceTravelled >= pathCreator.path.length)
            {
                distanceTravelled = 0.0f;
                audioSource.Stop();
                audioSource.Play();
            }
        }
    }
}