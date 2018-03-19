using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CrazyMinnow.SALSA.Daz
{
    [CustomEditor(typeof(CM_DazSetup))]
    public class CM_DazSetupEditor : Editor
    {
        private CM_DazSetup dazSetup; // CM_DazSetup reference

        public void OnEnable()
        {
            // Get reference
            dazSetup = target as CM_DazSetup;

            // Run Setup
            dazSetup.Setup();

            // Remove setup component
            DestroyImmediate(dazSetup);
        }
    }
}