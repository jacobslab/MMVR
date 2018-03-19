using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CrazyMinnow.SALSA;

namespace CrazyMinnow.SALSA.Daz
{
	/// <summary>
	/// This script acts as a proxy between SALSA with RandomEyes and Daz3D characters,
	/// and allows users to link SALSA with RandomEyes to Daz3D characters without any model
	/// modifications.
	/// 
	/// Good default inspector values
	/// Salsa3D
	/// 	Trigger values will depend on your recordings
	/// 	Blend Speed: 10
	/// 	Range of Motion: 75
	/// RandomEyes3D
	/// 	Range of Motion: 60
	/// </summary>
	/// 
	/// Crazy Minnow Studio, LLC
	/// CrazyMinnowStudio.com
	/// 
	/// NOTE:While every attempt has been made to ensure the safe content and operation of 
	/// these files, they are provided as-is, without warranty or guarantee of any kind. 
	/// By downloading and using these files you are accepting any and all risks associated 
	/// and release Crazy Minnow Studio, LLC of any and all liability.
	[AddComponentMenu("Crazy Minnow Studio/SALSA/Addons/Daz3D/CM_DazSync")]
	public class CM_DazSync : MonoBehaviour 
	{
		public Salsa3D salsa3D; // Salsa3D mouth component
		public RandomEyes3D randomEyes3D; // RandomEyes3D eye componet
		public SkinnedMeshRenderer skinnedMeshRenderer; // Daz character SkinnedMeshRenderer
		public string leftEyeName = "lEye"; // Used in search for left eye bone
		public GameObject leftEyeBone; // Left eye bone
		public string rightEyeName = "rEye"; // Used in search for right eye bone
		public GameObject rightEyeBone; // Right eye bone
        public string leftBlinkShapes = "EyesClosedL,BlinkLeft,Blink_l"; // Left blink shape search keywords
        public string rightBlinkShapes = "EyesClosedR,BlinkRight,Blink_r"; // Right blink shape search keywords
        public int leftBlinkIndex = -1; // Left blink shape index
        public int rightBlinkIndex = -1; // Right blink shape index
        public string leftBlinkShape = ""; // Left blink shape name
        public string rightBlinkShape = ""; // Right blink shape name
		public List<CM_ShapeGroup> saySmall = new List<CM_ShapeGroup>(); // saySmall shape group
		public List<CM_ShapeGroup> sayMedium = new List<CM_ShapeGroup>(); // sayMedium shape group
		public List<CM_ShapeGroup> sayLarge = new List<CM_ShapeGroup>(); // sayLarge shape group
		public string[] shapeNames; // Shape name string array for name picker popups
		public bool initialize = true; // Initialize once
        public enum DazType { Dragon, Emotiguy, Genesis_Genesis2, Genesis3, Genesis8 }; // Supported Daz base character types
        public DazType dazType = DazType.Genesis_Genesis2; // Default base type
        public DazType prevType; // Tracks previous base type to detect changes

		private Transform[] children; // For searching through child objects during initialization
		private float eyeSensativity = 500f; // Eye movement reduction from shape value to bone transform value
        private float blinkWeight; // Blink weight is applied to the body Blink_Left and Blink_Right BlendShapes
		private float vertical; // Vertical eye bone movement amount
		private float horizontal; // Horizontal eye bone movement amount
		private bool lockShapes; // Used to allow access to shape group shapes when SALSA is not talking

		/// <summary>
		/// Reset the component to default values
		/// </summary>
		void Reset()
		{
			initialize = true;            
			GetSalsa3D();
			GetRandomEyes3D();
			GetSmr();
			GetEyeBones();
            GetBlinkIndexes();
			if (saySmall == null) saySmall = new List<CM_ShapeGroup>();
			if (sayMedium == null) sayMedium = new List<CM_ShapeGroup>();
			if (sayLarge == null) sayLarge = new List<CM_ShapeGroup>();
			GetShapeNames();

            if (dazType == DazType.Dragon)
            {
                SetDragonSmall();
                SetDragonMedium();
                SetDragonLarge();
            }
            if (dazType == DazType.Emotiguy)
            {
                SetEmotiguySmall();
                SetEmotiguyMedium();
                SetEmotiguyLarge();
            }
            if (dazType == DazType.Genesis_Genesis2)
            {
                SetGenesis1_2Small();
                SetGenesis1_2Medium();
                SetGenesis1_2Large();
            }
            if (dazType == DazType.Genesis3)
            {
                SetGenesis3Small();
                SetGenesis3Medium();
                SetGenesis3Large();
            }
		}

        /// <summary>
        /// Initial setup
        /// </summary>
		void Start()
		{
			// Initialize            
			GetSalsa3D();
			GetRandomEyes3D();
			GetSmr();
			GetEyeBones();
            GetBlinkIndexes();
			if (saySmall == null) saySmall = new List<CM_ShapeGroup>();
			if (sayMedium == null) sayMedium = new List<CM_ShapeGroup>();
			if (sayLarge == null) sayLarge = new List<CM_ShapeGroup>();
			GetShapeNames();
		}

        /// <summary>
        /// Perform the blendshape changes in LateUpdate for mechanim compatibility
        /// </summary>
		void LateUpdate() 
		{
			// Toggle shape lock to provide access to shape group shapes when SALSA is not talking
			if (salsa3D)
			{
				if (salsa3D.sayAmount.saySmall == 0f && salsa3D.sayAmount.sayMedium == 0f && salsa3D.sayAmount.sayLarge == 0f)
				{
					lockShapes = false;
				}
				else
				{
					lockShapes = true;
				}
			}

			if (salsa3D && skinnedMeshRenderer && lockShapes)
			{
				// Sync SALSA shapes
				for (int i=0; i<saySmall.Count; i++)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(
						saySmall[i].shapeIndex, ((saySmall[i].percentage/100)*salsa3D.sayAmount.saySmall));
				}
				for (int i=0; i<sayMedium.Count; i++)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(
						sayMedium[i].shapeIndex, ((sayMedium[i].percentage/100)*salsa3D.sayAmount.sayMedium));
				}			
				for (int i=0; i<sayLarge.Count; i++)
				{
					skinnedMeshRenderer.SetBlendShapeWeight(
						sayLarge[i].shapeIndex, ((sayLarge[i].percentage/100)*salsa3D.sayAmount.sayLarge));
				}
			}

			// Sync Blink
			if (randomEyes3D)
			{
				blinkWeight = randomEyes3D.lookAmount.blink;

				// Apply blink action
				if (skinnedMeshRenderer)
				{
                    if (leftBlinkIndex != -1) skinnedMeshRenderer.SetBlendShapeWeight(leftBlinkIndex, blinkWeight);
                    if (rightBlinkIndex != -1) skinnedMeshRenderer.SetBlendShapeWeight(rightBlinkIndex, blinkWeight);
				}

				// Apply look amount to bone rotation
				if (leftEyeBone || rightEyeBone)
				{
					// Apply eye movement weight direction variables
					if (randomEyes3D.lookAmount.lookUp > 0) 
						vertical = -(randomEyes3D.lookAmount.lookUp / eyeSensativity) * randomEyes3D.rangeOfMotion;
					if (randomEyes3D.lookAmount.lookDown > 0) 
						vertical = (randomEyes3D.lookAmount.lookDown / eyeSensativity) * randomEyes3D.rangeOfMotion;
					if (randomEyes3D.lookAmount.lookLeft > 0) 
						horizontal = -(randomEyes3D.lookAmount.lookLeft / eyeSensativity) * randomEyes3D.rangeOfMotion;
					if (randomEyes3D.lookAmount.lookRight > 0) 
						horizontal = (randomEyes3D.lookAmount.lookRight / eyeSensativity) * randomEyes3D.rangeOfMotion;

					// Set eye bone rotations
					if (leftEyeBone) leftEyeBone.transform.localRotation = Quaternion.Euler(vertical, horizontal, 0);
					if (rightEyeBone) rightEyeBone.transform.localRotation = Quaternion.Euler(vertical, horizontal, 0);
				}
			}
		}

		/// <summary>
		/// Call this when initializing characters at runtime
		/// </summary>
		public void Initialize()
		{
			Reset();
		}

		/// <summary>
		/// Calling this method tries to find the SkinnedMeshRender again, if it's 
		/// not already linked, then remaps the small, medium, and large shape groups.
		/// </summary>
		/// <param name="characterType"></param>
		public void SetCharacterType(DazType characterType)
		{
			if (prevType != characterType)
			{
				dazType = characterType;
				GetSmr();
				GetBlinkIndexes();

				switch (dazType)
				{
					case DazType.Dragon:
						SetDragonSmall();
						SetDragonMedium();
						SetDragonLarge();
						break;
					case DazType.Emotiguy:
						SetEmotiguySmall();
						SetEmotiguyMedium();
						SetEmotiguyLarge();
						break;
					case DazType.Genesis_Genesis2:
						SetGenesis1_2Small();
						SetGenesis1_2Medium();
						SetGenesis1_2Large();
						break;
					case DazType.Genesis3:
						SetGenesis3Small();
						SetGenesis3Medium();
						SetGenesis3Large();
						break;
                    case DazType.Genesis8:
                        SetGenesis8Small();
                        SetGenesis8Medium();
                        SetGenesis8Large();
                        break;
				}
				prevType = dazType;
			}
		}

		/// <summary>
		/// Get the Salsa3D component
		/// </summary>
		public void GetSalsa3D()
		{
			if (!salsa3D) salsa3D = GetComponent<Salsa3D>();
		}

		/// <summary>
		/// Get the RandomEyes3D component
		/// </summary>
		public void GetRandomEyes3D()
		{
			//if (!randomEyes3D) randomEyes3D = GetComponent<RandomEyes3D>();

            RandomEyes3D[] randomEyes = GetComponents<RandomEyes3D>();
            if (randomEyes.Length > 1)
            {
                for (int i = 0; i < randomEyes.Length; i++)
                {
                    // Verify this instance ID does not match the reEyes instance ID
                    if (!randomEyes[i].useCustomShapesOnly)
                    {
                        // Set the reShapes instance
                        randomEyes3D = randomEyes[i];
                    }
                }
            }
		}

		/// <summary>
		/// Find the Body child object SkinnedMeshRenderer
		/// </summary>
		public void GetSmr()
		{
			if (!skinnedMeshRenderer) 
			{
				SkinnedMeshRenderer[] smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
				if (smr.Length > 0)
				{
					for (int i=0; i<smr.Length; i++)
					{
						if (smr[i].sharedMesh.blendShapeCount > 0 && smr[i].enabled)
						{
							if (dazType == DazType.Genesis_Genesis2 || dazType == DazType.Genesis3)
							{
								if (smr[i].gameObject.name.Contains("Genesis"))
								{
									skinnedMeshRenderer = smr[i];
								}
							}
							else
							{
								skinnedMeshRenderer = smr[i];
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Find left and right eye bones
		/// </summary>
		public void GetEyeBones()
		{
			Transform leftEyeTrans = ChildSearch(leftEyeName);
			if(leftEyeTrans) 
			{
				if (!leftEyeBone) leftEyeBone = leftEyeTrans.gameObject;
			}
			Transform rightEyeTrans = ChildSearch(rightEyeName);
			if (rightEyeTrans) 
			{
				if (!rightEyeBone) rightEyeBone = rightEyeTrans.gameObject;
			}
		}

        /// <summary>
        /// Get blink indexes for multiple Daz BlendShape name variations
        /// </summary>
        public void GetBlinkIndexes()
        {
            if (skinnedMeshRenderer)
            {
                int leftIndex = -1;
                int rightIndex = -1;

                leftBlinkShapes = leftBlinkShapes.Replace(" ", "");
                rightBlinkShapes = rightBlinkShapes.Replace(" ", "");
                string[] leftNames = leftBlinkShapes.Split(',');
                string[] rightNames = rightBlinkShapes.Split(',');

                // Loop through left blink BlendShape names
                for (int i = 0; i < leftNames.Length; i++)
                {
                    if (leftIndex == -1) leftIndex = ShapeSearch(skinnedMeshRenderer, leftNames[i]);
                    leftBlinkIndex = leftIndex;
                }

                // Loop through right blink BlendShape names
                for (int i = 0; i < rightNames.Length; i++)
                {
                    if (rightIndex == -1) rightIndex = ShapeSearch(skinnedMeshRenderer, rightNames[i]);
                    if (rightIndex != -1)
                    {
                        rightBlinkIndex = rightIndex;
                        break;
                    }
                }
            }
        }

		/// <summary>
        /// Find a child by name that ends with the search string. 
        /// This should compensates for BlendShape name prefixes variations.
		/// </summary>
		/// <param name="endsWith"></param>
		/// <returns></returns>
		public Transform ChildSearch(string endsWith)
		{
			Transform trans = null;

			children = transform.gameObject.GetComponentsInChildren<Transform>();

			for (int i=0; i<children.Length; i++)
			{
                if (children[i].name.EndsWith(endsWith)) trans = children[i];
			}

			return trans;
		}	

		/// <summary>
        /// Find a shape by name, that ends with the search string.
		/// </summary>
		/// <param name="skndMshRndr"></param>
		/// <param name="endsWith"></param>
		/// <returns></returns>
        public int ShapeSearch(SkinnedMeshRenderer skndMshRndr, string endsWith)
		{
			int index = -1;
			if (skndMshRndr)
			{
				for (int i=0; i<skndMshRndr.sharedMesh.blendShapeCount; i++)
				{
                    if (skndMshRndr.sharedMesh.GetBlendShapeName(i).EndsWith(endsWith))
					{
						index = i;
						break;
					}
				}
			}
			return index;
		}

		/// <summary>
		/// Populate the shapeName popup list
		/// </summary>
		public int GetShapeNames()
		{
			int nameCount = 0;

			if (skinnedMeshRenderer)
			{
				shapeNames = new string[skinnedMeshRenderer.sharedMesh.blendShapeCount];
				for (int i=0; i<skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
				{
					shapeNames[i] = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
					if (shapeNames[i] != "") nameCount++;
				}
			}

			return nameCount;
		}

        /// <summary>
        /// Set the Dragon saySmall shape group
        /// </summary>
        public void SetDragonSmall()
        {
            int index = -1;
            string name = "";

            saySmall = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "MouthSnarl");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 40f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthCH");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 30f));
            }
        }
        /// <summary>
        /// Set the Dragon sayMedium shape group
        /// </summary>
        public void SetDragonMedium()
        {
            int index = -1; ;
            string name = "";

            sayMedium = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "MouthSmileOpen");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 35f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "LipUpCurl");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 22.4f));
            }
        }
        /// <summary>
        /// Set the Dragon sayLarge shape group
        /// </summary>
        public void SetDragonLarge()
        {
            int index = -1; ;
            string name = "";

            sayLarge = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "MouthSmileOpen");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 83.2f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthFrown");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 80.4f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthCH");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 44.7f));
            }
        }

        /// <summary>
        /// Set the Emotiguy saySmall shape group
        /// </summary>
        public void SetEmotiguySmall()
        {
            int index = -1;
            string name = "";

            saySmall = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "PuckerLipsWide");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 10f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthO");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 60f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "StretchLips");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 40f));
            }

        }
        /// <summary>
        /// Set the Emotiguy sayMedium shape group
        /// </summary>
        public void SetEmotiguyMedium()
        {
            int index = -1; ;
            string name = "";

            sayMedium = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "MouthTH");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 70f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthSpeak");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 100f));
            }
        }
        /// <summary>
        /// Set the Emotiguy sayLarge shape group
        /// </summary>
        public void SetEmotiguyLarge()
        {
            int index = -1; ;
            string name = "";

            sayLarge = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "PuckerLipsOO");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 40f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthYell");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 100f));
            }
        }

		/// <summary>
		/// Set the Genesis 1 / Genesis 2 saySmall shape group
		/// </summary>
		public void SetGenesis1_2Small()
		{
			int index = -1;
			string name = "";

			saySmall = new List<CM_ShapeGroup>();

			index = ShapeSearch(skinnedMeshRenderer, "VSMW");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				saySmall.Add(new CM_ShapeGroup(index, name, 10f));
			}

			index = ShapeSearch(skinnedMeshRenderer, "VSMUW");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				saySmall.Add(new CM_ShapeGroup(index, name, 30f));
			}

			index = ShapeSearch(skinnedMeshRenderer, "VSMK");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				saySmall.Add(new CM_ShapeGroup(index, name, 50f));
			}
		}
		/// <summary>
        /// Set the Genesis 1 / Genesis 2 sayMedium shape group
		/// </summary>
        public void SetGenesis1_2Medium()
		{
			int index = -1;;
			string name = "";

			sayMedium = new List<CM_ShapeGroup>();

			index = ShapeSearch(skinnedMeshRenderer, "VSMAA");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				sayMedium.Add(new CM_ShapeGroup(index, name, 100f));
			}

			index = ShapeSearch(skinnedMeshRenderer, "MouthSmileOpen");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				sayMedium.Add(new CM_ShapeGroup(index, name, 20f));
			}

			index = ShapeSearch(skinnedMeshRenderer, "MouthSmile");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				sayMedium.Add(new CM_ShapeGroup(index, name, 40f));
			}

            index = ShapeSearch(skinnedMeshRenderer, "LipTopUpR");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 40f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "LipTopUpL");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 40f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "LipBottomDownR");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 60f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "LipBottomDownL");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 60f));
            }
		}
		/// <summary>
        /// Set the Genesis 1 / Genesis 2 sayLarge shape group
		/// </summary>
        public void SetGenesis1_2Large()
		{
			int index = -1;;
			string name = "";

			sayLarge = new List<CM_ShapeGroup>();

			index = ShapeSearch(skinnedMeshRenderer, "VSMTH");
			if (index != -1)
			{
				name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
				sayLarge.Add(new CM_ShapeGroup(index, name, 60f));
			}

            index = ShapeSearch(skinnedMeshRenderer, "MouthOpenWide");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 40f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "MouthOpen");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 60f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "TongueBendTip");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 20f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "Tongue Curl");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 20f));
            }
		}
        
        /// <summary>
        /// Set the Genesis 3 saySmall shape group
        /// </summary>
        public void SetGenesis3Small()
        {
            int index = -1;
            string name = "";

            saySmall = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvW");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 30f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvUW");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 30f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvK");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 20f));
            }
        }
        /// <summary>
        /// Set the Genesis 3 sayMedium shape group
        /// </summary>
        public void SetGenesis3Medium()
        {
            int index = -1; ;
            string name = "";

            sayMedium = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvIY");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 60f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvAA");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 50f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLMouthSmileOpen");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 20f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLMouthSmile");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 40f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLLipTopUp-DownR");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 20f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLLipTopUp-DownL");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 20f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLLipBottomUp-DownR");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 20f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLLipBottomUp-DownL");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 20f));
            }
        }
        /// <summary>
        /// Set the Genesis 3 sayLarge shape group
        /// </summary>
        public void SetGenesis3Large()
        {
            int index = -1; ;
            string name = "";

            sayLarge = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvTH");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 100f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvOW");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 60f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvEE");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 50f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLLipTopUp-DownR");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 35f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLLipTopUp-DownL Curl");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 35f));
            }
        }

        /// <summary>
        /// Set the Genesis 8 saySmall shape group
        /// </summary>
        public void SetGenesis8Small()
        {
            int index = -1;
            string name = "";

            saySmall = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvTH");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                saySmall.Add(new CM_ShapeGroup(index, name, 100f));
            }
        }
        /// <summary>
        /// Set the Genesis 8 sayMedium shape group
        /// </summary>
        public void SetGenesis8Medium()
        {
            int index = -1; ;
            string name = "";

            sayMedium = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvEE");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayMedium.Add(new CM_ShapeGroup(index, name, 100f));
            }
        }
        /// <summary>
        /// Set the Genesis 8 sayLarge shape group
        /// </summary>
        public void SetGenesis8Large()
        {
            int index = -1; ;
            string name = "";

            sayLarge = new List<CM_ShapeGroup>();

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLvOW");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 100f));
            }

            index = ShapeSearch(skinnedMeshRenderer, "eCTRLMouthOpen");
            if (index != -1)
            {
                name = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(index);
                sayLarge.Add(new CM_ShapeGroup(index, name, 30f));
            }
        }
    }

	/// <summary>
	/// Shape index and percentage class for SALSA/Daz shape groups
	/// </summary>
	[System.Serializable]
	public class CM_ShapeGroup
	{
		public int shapeIndex;
		public string shapeName;
		public float percentage;

		public CM_ShapeGroup()
		{
			this.shapeIndex = 0;
			this.shapeName = "";
			this.percentage = 100f;
		}

		public CM_ShapeGroup(int shapeIndex, string shapeName, float percentage)
		{
			this.shapeIndex = shapeIndex;
			this.shapeName = shapeName;
			this.percentage = percentage;
		}
	}
}
