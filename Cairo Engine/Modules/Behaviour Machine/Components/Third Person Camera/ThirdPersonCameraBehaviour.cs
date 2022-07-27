//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;
using CairoEngine;
using Cinemachine;

namespace CairoEngine.Behaviour
{
    [Serializable]
    public class ThirdPersonCameraBehaviour : BehaviourType<BehaviourTemplate_ThirdPersonCamera>
    {
        CinemachineFreeLook freeLook;
        GameObject freeLookObject;

        public void Init()
        {
            freeLookObject = new GameObject();

            if (template.preset != null)
                freeLook = (CinemachineFreeLook)template.preset.GetComponent<CinemachineFreeLook>().CopyComponent(freeLookObject);
            else
                freeLook = freeLookObject.AddComponent<CinemachineFreeLook>();

            if (rootTransform != root.transform)
            {
                freeLookObject.transform.parent = gameObject.transform;
            }
            else
            {
                BehaviourModule.AddBehaviourObject(freeLookObject);
            }

            freeLook.Follow = rootTransform;
            freeLook.LookAt = rootTransform;

            //Change settings depending on the active Input System
#if ENABLE_LEGACY_INPUT_MANAGER
            //TODO: Add Legacy Input Manager functionality
#endif
#if ENABLE_INPUT_SYSTEM
            freeLook.m_XAxis.m_InputAxisName = "";
            freeLook.m_YAxis.m_InputAxisName = "";
#endif
        }
    
        public void Update()
        {
            freeLook.m_XAxis.m_InputAxisValue = inputs["XAxis"];
            freeLook.m_YAxis.m_InputAxisValue = inputs["YAxis"];

            CinemachineBrain.SoloCamera = freeLook;

            GetEntity().cameraEuler.y = freeLook.m_XAxis.Value;
        }
    }
}
