//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CairoEngine.CameraTools;

namespace CairoEngine
{
    /// <summary>
    /// Handles the Camera in the Game
    /// </summary>
    public class CameraModule
    {
        /// <summary>
        /// The current Camera's Transform
        /// </summary>
        private static Transform cameraTransform;
        /// <summary>
        /// The current Camera Target (The Object that the Camera should follow)
        /// </summary>
        private static Transform cameraTarget;
        /// <summary>
        /// The Current Camera Director
        /// </summary>
        private static CameraDirector current;
        /// <summary>
        /// All the Camera Directors currently in the Game.
        /// </summary>
        private static List<CameraDirector> directors = new List<CameraDirector>();
        /// <summary>
        /// All the Camera Director Templates in the Project
        /// </summary>
        private static List<DirectorTemplate> directorTemplates = new List<DirectorTemplate>();


        public static void Init()
        {
            //Load all the Director Templates in the Project
            directorTemplates.AddRange(Resources.LoadAll<DirectorTemplate>(""));
        }

        public static void Update()
        {
            if (current != null)
            {
                //Handle Camera Interpolation
                if (current.interpolate)
                {
                    if (current.lerpAlpha < 1.0)
                    {
                        current.cameraBase.Follow.position.Lerp(current.lastFollowTarget.position, current.lerpAlpha);
                        current.cameraBase.Follow.eulerAngles.Lerp(current.lastFollowTarget.eulerAngles, current.lerpAlpha);
                        current.cameraBase.LookAt.position.Lerp(current.lastAimTarget.position, current.lerpAlpha);
                        current.cameraBase.LookAt.eulerAngles.Lerp(current.lastAimTarget.eulerAngles, current.lerpAlpha);

                        //Lerp the Alpha to 1.0 
                        current.lerpAlpha = Mathf.Lerp(current.lerpAlpha, 1.0f, current.lerpRate);
                    }
                    //Complete Interpolation
                    else
                    {
                        current.interpolate = false;
                    }
                }
                //Update the LookAt and Follow transforms to the Target's Transforms
                else
                {
                    if (current.followTarget != null)
                    {
                        current.cameraBase.Follow.position = current.followTarget.position;
                        current.cameraBase.Follow.eulerAngles = current.followTarget.eulerAngles;
                    }
                    if(current.aimTarget != null)
                    {
                        current.cameraBase.LookAt.position = current.aimTarget.position;
                        current.cameraBase.LookAt.eulerAngles = current.aimTarget.eulerAngles;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the specified Director
        /// </summary>
        /// <param name="ID">Identifier.</param>
        /// <param name="enable">If set to <c>true</c> enable.</param>
        public static CameraDirector CreateDirector(string ID, bool enable = false, Transform followTarget = null, Transform aimTarget = null, Transform previousFollowTarget = null, Transform previousAimTarget = null)
        {
            DirectorTemplate template = GetDirectorTemplate(ID);

            if(template != null)
            {
                GameObject directorObject = new GameObject();
                CameraDirector director = (CameraDirector)directorObject.AddComponent(Type.GetType(template.directorClass));
                directorObject.name = template.directorClass;

                director.Init();

                Transform rootFollowTarget = new GameObject().transform;
                Transform rootAimTarget = new GameObject().transform;

                rootFollowTarget.transform.parent = director.transform;
                rootFollowTarget.name = "Follow";
                rootAimTarget.transform.parent = director.transform;
                rootAimTarget.name = "Aim";

                director.cameraBase.Follow = rootFollowTarget;
                director.cameraBase.LookAt = rootAimTarget;

                if (template.followtargetName != "")
                    rootFollowTarget = director.transform.Find(template.followtargetName);
                if (template.aimTargetName != "")
                    rootAimTarget = director.transform.Find(template.aimTargetName);

                if(aimTarget != null)
                    director.aimTarget = aimTarget;
                if (followTarget != null)
                    director.followTarget = followTarget;

                if (enable)
                {
                    if(current != null)
                    {
                        current.cameraBase.enabled = false;
                        current.enabled = false;
                    }

                    current = director;
                }
                else
                {
                    director.cameraBase.enabled = false;
                    director.enabled = false;
                }

                return director;
            }
            return null;
        }

        /// <summary>
        /// Sets the Camera's Follow Target
        /// </summary>
        /// <param name="target">Target.</param>
        public static void SetFollowTarget(Transform target)
        {
            current.followTarget = target;
        }

        /// <summary>
        /// Sets the Camera's Aim Target
        /// </summary>
        /// <param name="target">Target.</param>
        public static void SetAimTarget(Transform target)
        {
            current.aimTarget = target;
        }

        /// <summary>
        /// Changes the Current Camera Director to the specifed Director
        /// </summary>
        /// <param name="ID">Identifier.</param>
        /// <param name="interpolate">If set to <c>true</c> interpolate the Director's Target from the last Director's Target.</param>
        public static CameraDirector SetDirector(string ID, bool interpolate = false, float interpolationRate = 0.1f, Transform followTarget = null, Transform aimTarget = null)
        {
            CameraDirector director = GetDirector(ID);

            //Check if the Director has already been created, if it hasn't then attempt to create a new one
            if(director == null)
            {
                Debug.Log("Director "+ID+" wasn't previously created, an attempt has been made to Create a new one.");
                if (current != null)
                    director = CreateDirector(ID, true, followTarget: followTarget, aimTarget: aimTarget, previousAimTarget: current.cameraBase.LookAt, previousFollowTarget: current.cameraBase.Follow);
                else
                    director = CreateDirector(ID, true, followTarget: followTarget, aimTarget: aimTarget);
            }
            //if the Director exists, set it as the current Director. 
            if(director != null)
            {
                //If Interpolation is enabled, set the LookAt and Follow targets to the Target's of the previous Director
                if (interpolate)
                {
                    director.cameraBase.LookAt.position = current.cameraBase.LookAt.position;
                    director.cameraBase.LookAt.rotation = current.cameraBase.LookAt.rotation;
                    director.cameraBase.Follow.position = current.cameraBase.Follow.position;
                    director.cameraBase.Follow.rotation = current.cameraBase.Follow.rotation;

                    director.interpolate = true;
                    director.lerpRate = interpolationRate;

                    director.lastAimTarget = current.cameraBase.LookAt;
                    director.lastFollowTarget = current.cameraBase.Follow;
                }

                current = director;
            }
            else
            {
                Debug.LogWarning("Can't set to director "+ID+", because it doesn't exist and can't be created");
            }
            return current;
        }

        /// <summary>
        /// Gets the specified Camera Director
        /// </summary>
        /// <returns>The director.</returns>
        /// <param name="ID">Identifier.</param>
        private static CameraDirector GetDirector(string ID)
        {
            foreach(CameraDirector director in directors)
            {
                if(director.template.ID == ID)
                {
                    return director;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the specified Director Template
        /// </summary>
        /// <returns>The director template.</returns>
        /// <param name="ID">Identifier.</param>
        private static DirectorTemplate GetDirectorTemplate(string ID)
        {
            foreach(DirectorTemplate template in directorTemplates)
            {
                if (template.ID == ID)
                    return template;
            }

            return null;
        }
    }
}
