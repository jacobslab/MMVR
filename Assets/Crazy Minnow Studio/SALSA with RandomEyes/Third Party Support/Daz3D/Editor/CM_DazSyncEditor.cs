using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using CrazyMinnow.SALSA;

namespace CrazyMinnow.SALSA.Daz
{
	/// <summary>
	/// This is the custom inspector for CM_DazSync, a script that acts as a proxy between 
	/// SALSA with RandomEyes and Daz3D characters, and allows users to link SALSA with 
	/// RandomEyes to Daz3D characters without any model modifications.
	/// 
	/// Crazy Minnow Studio, LLC
	/// CrazyMinnowStudio.com
	/// 
	/// NOTE:While every attempt has been made to ensure the safe content and operation of 
	/// these files, they are provided as-is, without warranty or guarantee of any kind. 
	/// By downloading and using these files you are accepting any and all risks associated 
	/// and release Crazy Minnow Studio, LLC of any and all liability.
	[CustomEditor(typeof(CM_DazSync)), CanEditMultipleObjects]
	public class CM_DazSyncEditor : Editor 
	{
		private CM_DazSync dazSync; // CM_DazSync reference
		private bool dirtySmall; // SaySmall dirty inspector status
		private bool dirtyMedium; // SayMedum dirty inspector status
		private bool dirtyLarge; // SayLarge dirty inspector status

		private float width = 0f; // Inspector width
		private float addWidth = 10f; // Percentage
		private float deleteWidth = 10f; // Percentage
		private float shapeNameWidth = 60f; // Percentage
		private float percentageWidth = 30f; // Percentage

		public void OnEnable()
		{
			// Get reference
			dazSync = target as CM_DazSync;

			// Initialize
			if (dazSync.initialize)
			{                
				dazSync.GetSalsa3D();
				dazSync.GetRandomEyes3D();
				dazSync.GetSmr();
				dazSync.GetEyeBones();
                dazSync.GetBlinkIndexes();
				if (dazSync.saySmall == null) dazSync.saySmall = new List<CM_ShapeGroup>();
				if (dazSync.sayMedium == null) dazSync.sayMedium = new List<CM_ShapeGroup>();
				if (dazSync.sayLarge == null) dazSync.sayLarge = new List<CM_ShapeGroup>();
				dazSync.GetShapeNames();
                if (dazSync.dazType == CM_DazSync.DazType.Genesis_Genesis2)
                {
                    dazSync.SetGenesis1_2Small();
                    dazSync.SetGenesis1_2Medium();
                    dazSync.SetGenesis1_2Large();
                }
                if (dazSync.dazType == CM_DazSync.DazType.Genesis3)
                {
                    dazSync.SetGenesis3Small();
                    dazSync.SetGenesis3Medium();
                    dazSync.SetGenesis3Large();
                }
                dazSync.prevType = dazSync.dazType;
				dazSync.initialize = false;
			}
		}

		public override void OnInspectorGUI()
        {
            // Minus 45 width for the scroll bar
            width = Screen.width - 75f;

            // Set dirty flags
            dirtySmall = false;
            dirtyMedium = false;
            dirtyLarge = false;

            // Keep trying to get the shapeNames until I've got them
            if (dazSync.GetShapeNames() == 0) dazSync.GetShapeNames();

            // Make sure the CM_ShapeGroups are initialized
            if (dazSync.saySmall == null) dazSync.saySmall = new System.Collections.Generic.List<CM_ShapeGroup>();
            if (dazSync.sayMedium == null) dazSync.sayMedium = new System.Collections.Generic.List<CM_ShapeGroup>();
            if (dazSync.sayLarge == null) dazSync.sayLarge = new System.Collections.Generic.List<CM_ShapeGroup>();

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(width) });
            {
                dazSync.dazType = (CM_DazSync.DazType)EditorGUILayout.EnumPopup("Character Type", dazSync.dazType);

				if (dazSync.prevType != dazSync.dazType)
				{
					dazSync.SetCharacterType(dazSync.dazType);
                }

                GUILayout.Space(20);

                dazSync.salsa3D = EditorGUILayout.ObjectField(
                    "Salsa3D", dazSync.salsa3D, typeof(Salsa3D), true) as Salsa3D;
                dazSync.randomEyes3D = EditorGUILayout.ObjectField(
                    new GUIContent("RandomEyes3D", "The RandomEyes3D instance for controlling eye movement."),
                    dazSync.randomEyes3D, typeof(RandomEyes3D), true) as RandomEyes3D;
                dazSync.skinnedMeshRenderer = EditorGUILayout.ObjectField(
                    new GUIContent("SkinnedMeshRenderer", "The SkinnedMeshRenderer child object that contains all the BlendShapes"),
                    dazSync.skinnedMeshRenderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
                dazSync.leftEyeBone = EditorGUILayout.ObjectField(
                    "Left Eye Bone", dazSync.leftEyeBone, typeof(GameObject), true) as GameObject;
                dazSync.rightEyeBone = EditorGUILayout.ObjectField(
                    "Right Eye Bone", dazSync.rightEyeBone, typeof(GameObject), true) as GameObject;
                GUILayout.Space(10);
                dazSync.leftBlinkShapes = EditorGUILayout.TextField(
                    new GUIContent("Left Blink Shape Names", "Daz3D BlendShape names are not always " +
                        "consistant, this is a CSV list of common variations contained in blink shape names."),
                        dazSync.leftBlinkShapes);
                dazSync.rightBlinkShapes = EditorGUILayout.TextField(
                    new GUIContent("Right Blink Shape Names", "Daz3D BlendShape names are not always " +
                        "consistant, this is a CSV list of common variations contained in blink shape names."),
                        dazSync.rightBlinkShapes);
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(new GUIContent("Search",
                        "This will search the SkinnedMeshRenderer for BlendShapes that contain these names."));
                    if (GUILayout.Button("Search"))
                    {
                        dazSync.GetBlinkIndexes();
                    }
                }
                GUILayout.EndHorizontal();


                dazSync.leftBlinkShape = "";
                if (dazSync.leftBlinkIndex != -1) dazSync.leftBlinkShape =
                    dazSync.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(dazSync.leftBlinkIndex);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(new GUIContent("Left Blink", "-1 means no blink shape " +
                        "index was found, check the SkinnedMeshRenderer to find the left eye blink shape name"));
					GUILayout.Label(" (" + dazSync.leftBlinkIndex.ToString() + ") " + dazSync.leftBlinkShape);
                }
                GUILayout.EndHorizontal();

                dazSync.rightBlinkShape = "";
                if (dazSync.rightBlinkIndex != -1) dazSync.rightBlinkShape =
                    dazSync.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(dazSync.rightBlinkIndex);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(new GUIContent("Right Blink", "-1 means no blink shape " +
                        "index was found, check the SkinnedMeshRenderer to find the right eye blink shape name"));
					GUILayout.Label("(" + dazSync.rightBlinkIndex.ToString() + ") " + dazSync.rightBlinkShape);
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(20);

            if (dazSync.skinnedMeshRenderer)
            {
                EditorGUILayout.LabelField("SALSA shape groups");
                GUILayout.Space(10);

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("SaySmall Shapes");
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width((addWidth / 100) * width) }))
                    {
                        dazSync.saySmall.Add(new CM_ShapeGroup());
                        dazSync.initialize = false;
                    }
                    EditorGUILayout.EndHorizontal();
                    if (dazSync.saySmall.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(
                            new GUIContent("Delete", "Remove shape"),
                            GUILayout.Width((deleteWidth / 100) * width));
                        EditorGUILayout.LabelField(
                            new GUIContent("ShapeName", "BlendShape - (shapeIndex)"),
                            GUILayout.Width((shapeNameWidth / 100) * width));
                        EditorGUILayout.LabelField(
                            new GUIContent("Percentage", "The percentage of total range of motion for this shape"),
                            GUILayout.Width((percentageWidth / 100) * width));
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < dazSync.saySmall.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(
                                new GUIContent("X", "Remove this shape from the list (index:" + dazSync.saySmall[i].shapeIndex + ")"),
                                GUILayout.Width((deleteWidth / 100) * width)))
                            {
                                dazSync.saySmall.RemoveAt(i);
                                dirtySmall = true;
                                break;
                            }
                            if (!dirtySmall)
                            {
							    if (dazSync.saySmall != null && dazSync.shapeNames != null)
							    {
								    dazSync.saySmall[i].shapeIndex = EditorGUILayout.Popup(
									    dazSync.saySmall[i].shapeIndex, dazSync.shapeNames,
									    GUILayout.Width((shapeNameWidth / 100) * width));
								    dazSync.saySmall[i].shapeName =
									    dazSync.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(dazSync.saySmall[i].shapeIndex);
								    dazSync.saySmall[i].percentage = EditorGUILayout.Slider(
									    dazSync.saySmall[i].percentage, 0f, 100f,
									    GUILayout.Width((percentageWidth / 100) * width));
								    dazSync.initialize = false;
							    }
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("SayMedium Shapes");
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width((addWidth / 100) * width) }))
                    {
                        dazSync.sayMedium.Add(new CM_ShapeGroup());
                        dazSync.initialize = false;
                    }
                    EditorGUILayout.EndHorizontal();
                    if (dazSync.sayMedium.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(
                            new GUIContent("Delete", "Remove shape"),
                            GUILayout.Width((deleteWidth / 100) * width));
                        EditorGUILayout.LabelField(
                            new GUIContent("ShapeName", "BlendShape - (shapeIndex)"),
                            GUILayout.Width((shapeNameWidth / 100) * width));
                        EditorGUILayout.LabelField(
                            new GUIContent("Percentage", "The percentage of total range of motion for this shape"),
                            GUILayout.Width((percentageWidth / 100) * width));
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < dazSync.sayMedium.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(
                                new GUIContent("X", "Remove this shape from the list (index:" + dazSync.sayMedium[i].shapeIndex + ")"),
                                GUILayout.Width((deleteWidth / 100) * width)))
                            {
                                dazSync.sayMedium.RemoveAt(i);
                                dirtyMedium = true;
                                break;
                            }
                            if (!dirtyMedium)
                            {
							    if (dazSync.sayMedium != null && dazSync.shapeNames != null)
							    {
								    dazSync.sayMedium[i].shapeIndex = EditorGUILayout.Popup(
									    dazSync.sayMedium[i].shapeIndex, dazSync.shapeNames,
									    GUILayout.Width((shapeNameWidth / 100) * width));
								    dazSync.sayMedium[i].shapeName =
									    dazSync.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(dazSync.sayMedium[i].shapeIndex);
								    dazSync.sayMedium[i].percentage = EditorGUILayout.Slider(
									    dazSync.sayMedium[i].percentage, 0f, 100f,
									    GUILayout.Width((percentageWidth / 100) * width));
								    dazSync.initialize = false;
							    }
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("SayLarge Shapes");
                    if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width((addWidth / 100) * width) }))
                    {
                        dazSync.sayLarge.Add(new CM_ShapeGroup());
                        dazSync.initialize = false;
                    }
                    EditorGUILayout.EndHorizontal();
                    if (dazSync.sayLarge.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(
                            new GUIContent("Delete", "Remove shape"),
                            GUILayout.Width((deleteWidth / 100) * width));
                        EditorGUILayout.LabelField(
                            new GUIContent("ShapeName", "BlendShape - (shapeIndex)"),
                            GUILayout.Width((shapeNameWidth / 100) * width));
                        EditorGUILayout.LabelField(
                            new GUIContent("Percentage", "The percentage of total range of motion for this shape"),
                            GUILayout.Width((percentageWidth / 100) * width));
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < dazSync.sayLarge.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(
                                new GUIContent("X", "Remove this shape from the list (index:" + dazSync.sayLarge[i].shapeIndex + ")"),
                                GUILayout.Width((deleteWidth / 100) * width)))
                            {
                                dazSync.sayLarge.RemoveAt(i);
                                dirtyLarge = true;
                                break;
                            }
                            if (!dirtyLarge)
                            {
							    if (dazSync.sayLarge != null && dazSync.shapeNames != null)
							    {
								    dazSync.sayLarge[i].shapeIndex = EditorGUILayout.Popup(
									    dazSync.sayLarge[i].shapeIndex, dazSync.shapeNames,
									    GUILayout.Width((shapeNameWidth / 100) * width));
								    dazSync.sayLarge[i].shapeName = dazSync.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(dazSync.sayLarge[i].shapeIndex);
								    dazSync.sayLarge[i].percentage = EditorGUILayout.Slider(
									    dazSync.sayLarge[i].percentage, 0f, 100f,
									    GUILayout.Width((percentageWidth / 100) * width));
								    dazSync.initialize = false;
							    }
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
	}
}
