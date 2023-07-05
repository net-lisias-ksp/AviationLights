/*
	This file is part of Aviation Lights /L
		© 2021-2023 LisiasT : http://lisias.net <support@lisias.net>
		© 2018-2023 MOARdv
		© 2012-2018 BigNose
		© 2012 RPGprayer

	Aviation Lights /L is licensed as follows:
		* CC-BY-NC-SA 4.0i : https://creativecommons.org/licenses/by-nc-sa/4.0/

	Aviation Lights /L is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

*/
using System;
using System.Collections.Generic;
using UnityEngine;

// Originally made by RPGprayer, edited by BigNose, Why485, GROOV3ST3R, JDP and J.Random
// Fixes for KSP 1.1, additional fixes, and refactoring by MOARdV.
// License: This file contains code from RPGprayers "Position/Navigation Lights". Used with permission.
namespace AviationLights
{
    public class ModuleNavLight : PartModule
    {
        public enum NavLightState
        {
            Off = 0,
            Flash = 1,
            DoubleFlash = 2,
            Interval = 3,
            On = 4 // Keep this as the last option for bounds checking sake.
        }

        public class TypePreset
        {
            public float flashOn;
            public float flashOff;
            public float interval;
            public float intensity;
            public float range;
            public string name;
        }

        [KSPField(isPersistant = true)]
        public int navLightSwitch = (int)NavLightState.Off;

        [KSPField(isPersistant = true)]
        public int toggleMode = (int)NavLightState.Flash;

        [KSPField]
        public string Resource = "ElectricCharge";
        private int resourceId;

        [KSPField]
        public float EnergyReq = 0.0f;
        public float actualEnergyReq = 0.0f;

        [KSPField]
        public float SpotAngle = 0.0f;

        [KSPField(isPersistant = true)]
        public float Interval = 1.0f;

        [KSPField(isPersistant = true)]
        public float FlashOn = 0.5f;

        [KSPField(isPersistant = true)]
        public float FlashOff = 1.5f;

        [KSPField(isPersistant = true)]
        public Vector3 Color = new Vector3(1.0f, 0.95f, 0.91f);
        private List<Vector3> presetColorValues;

        [KSPField(guiActiveEditor = true, guiName = "#AL_Symmetry")]
        [UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
        public bool applySymmetry = true;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#AL_TypePreset")]
        [UI_ChooseOption(affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public int typePreset = 0;
        private List<TypePreset> presetTypes;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#AL_ColorPreset")]
        [UI_ChooseOption(affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public int colorPreset = 0;

        [KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001402", advancedTweakable = true)]
        [UI_FloatRange(stepIncrement = 0.05f, maxValue = 1.0f, minValue = 0.0f, affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public float lightR;

        [KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001403", advancedTweakable = true)]
        [UI_FloatRange(stepIncrement = 0.05f, maxValue = 1.0f, minValue = 0.0f, affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public float lightG;

        [KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001404", advancedTweakable = true)]
        [UI_FloatRange(stepIncrement = 0.05f, maxValue = 1.0f, minValue = 0.0f, affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public float lightB;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#AL_LightIntensity", advancedTweakable = true)]
        [UI_FloatRange(minValue = 0.0f, stepIncrement = 0.25f, maxValue = 8.0f, affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public float Intensity = 0.5f;

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#AL_LightRange", advancedTweakable = true)]
        [UI_FloatRange(minValue = 1.0f, stepIncrement = 1.0f, maxValue = 150.0f, affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public float Range = 10.0f;

        [KSPField(isPersistant = true, guiName = "#AL_LightType", advancedTweakable = true)]
        [UI_Toggle(disabledText = "#AL_LightTypePoint", enabledText = "#AL_LightTypeSpot", affectSymCounterparts = UI_Scene.None, suppressEditorShipModified = true)]
        public bool spotLight = true;

        [KSPField]
        public Vector3 LightOffset = Vector3.zero;

        [KSPField]
        public Vector3 LightRotation = Vector3.zero;

        [KSPField]
        public string LensTransform = string.Empty;
        private Material[] lensMaterial = new Material[0];
        private readonly int colorProperty = Shader.PropertyToID("_Color");
        private readonly int emissiveColorProperty = Shader.PropertyToID("_EmissiveColor");

        [KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#AL_LightMode")]
        [UI_ChooseOption(affectSymCounterparts = UI_Scene.None, scene = UI_Scene.Editor, suppressEditorShipModified = true)]
        public int toggleModeSelector = (int)NavLightState.Off;

        // Controls whether in-editor tweakable configurations are permitted.  We don't
        // really want the old parts to use the tweaks, since it'll look odd.
        [KSPField]
        public bool Tweakable = true;

        private int flashCounter = 0;

        private float nextInterval = 0.0f;
        private float elapsedTime = 0.0f;

        private GameObject lightOffsetParent;
        private Light mainLight;
        private BaseEvent toggleBaseEvent;
        private BaseAction toggleBaseAction;

        private delegate void UpdateDelegate();
        private UpdateDelegate CurrentUpdate;

        /// <summary>
        /// Initialize game object / light, set up mode status and flasher controls.
        /// </summary>
        public void Start()
        {
			if (HighLogic.LoadedSceneIsEditor)      this.CurrentUpdate = this.UpdateOnEditor;
			else if (HighLogic.LoadedSceneIsFlight) this.CurrentUpdate = this.UpdateOnFlight;
			else                                    this.CurrentUpdate = this.UpdateDummy;

			{
				BaseField field = Fields["Range"];
				UI_FloatRange range = (UI_FloatRange)field.uiControlEditor;
				range.maxValue = Globals.RangeMaxValueInMeters;
			}

            if (!string.IsNullOrEmpty(LensTransform))
            {
                string[] lensNames = LensTransform.Split(';');
                List<Material> materials = new List<Material>();
                MeshRenderer[] mrs = gameObject.transform.GetComponentsInChildren<MeshRenderer>(true);

                for (int i = 0; i < lensNames.Length; ++i)
                {
                    string lensName = lensNames[i].Trim();
                    MeshRenderer lens = Array.Find<MeshRenderer>(mrs, x => x.name == lensName);
                    // Do I really need to test lens.material?
                    if (lens != null && lens.material != null)
                    {
                        materials.Add(lens.material);
                    }
                }

                lensMaterial = materials.ToArray();
            }

            // Sanity checks:
            if (navLightSwitch < (int)NavLightState.Off || navLightSwitch > (int)NavLightState.On)
            {
                navLightSwitch = (int)NavLightState.Off;
            }

            if (toggleMode <= (int)NavLightState.Off || toggleMode > (int)NavLightState.On)
            {
                toggleMode = (int)NavLightState.Flash;
            }
            toggleModeSelector = toggleMode - 1;

            toggleBaseEvent = Events["ToggleEvent"];
            toggleBaseAction = Actions["CurrentModeToggle"];

            try
            {
                resourceId = PartResourceLibrary.Instance.resourceDefinitions[Resource].id;
            }
            catch
            {
                resourceId = PartResourceLibrary.ElectricityHashcode;
            }

            Intensity = Mathf.Clamp(Intensity, 0.0f, 8.0f);
            actualEnergyReq = EnergyReq * Mathf.Max(0.25f, Intensity * Intensity);
            FlashOn = Mathf.Max(FlashOn, 0.01f);
            FlashOff = Mathf.Max(FlashOff, 0.01f);
            Interval = Mathf.Max(Interval, 0.01f);
            Range = Mathf.Max(Range, 0.0f);

            Color.x = Mathf.Clamp01(Color.x);
            Color.y = Mathf.Clamp01(Color.y);
            Color.z = Mathf.Clamp01(Color.z);

            Color newColor = new Color(Color.x, Color.y, Color.z);
            if (lensMaterial.Length > 0)
            {
                for (int i = 0; i < lensMaterial.Length; ++i)
                {
                    lensMaterial[i].SetColor(colorProperty, newColor);
                    lensMaterial[i].SetColor(emissiveColorProperty, newColor);
                }
            }

            SpotAngle = Mathf.Clamp(SpotAngle, 0.0f, 179.0f);

            // Initialize the sliders for advanced tweakables.
            lightR = Color.x;
            lightG = Color.y;
            lightB = Color.z;

            // Parent for main illumination light, used to move it away from the root game object.
            lightOffsetParent = new GameObject("AL_light");
            lightOffsetParent.transform.position = base.gameObject.transform.position;
            lightOffsetParent.transform.rotation = base.gameObject.transform.rotation;
            lightOffsetParent.transform.parent = base.gameObject.transform;
            lightOffsetParent.transform.Translate(LightOffset);
            // Swing the light around now that it's been translated.
            lightOffsetParent.transform.rotation = base.gameObject.transform.rotation * Quaternion.Euler(LightRotation);

            // Main Illumination light
            mainLight = lightOffsetParent.gameObject.AddComponent<Light>();
            mainLight.color = newColor;
            mainLight.intensity = 0.0f;
            mainLight.range = Range;
            if (SpotAngle > 0.0f)
            {
                mainLight.type = (spotLight) ? LightType.Spot : LightType.Point;
                mainLight.spotAngle = SpotAngle;
            }
            else
            {
                mainLight.type = LightType.Point;
                mainLight.spotAngle = 0.0f;
            }
            // Remove layer 10 from the cullingMask - it's the layer KSP uses to draw planets while in
            // orbit, and longer distances on the lights will illuminate the surface.
            mainLight.cullingMask = (mainLight.cullingMask & ~(1 << 10));

            UpdateMode();

            if (HighLogic.LoadedSceneIsEditor)
            {
                SetupChooser();
            }
        }

        /// <summary>
        /// For the editor, load the color presets so the player can adjust colors in the VAB.
        /// </summary>
        private void SetupChooser()
        {
            BaseField chooseField = Fields["toggleModeSelector"];
            {
                UI_ChooseOption chooseOption = (UI_ChooseOption)chooseField.uiControlEditor;
                chooseOption.options = new string[] { "#AL_ModeFlash", "#AL_ModeDoubleFlash", "#AL_ModeInterval", "#autoLOC_6001074" };
                chooseOption.onFieldChanged = FlashModeChanged;
            }

            if (Tweakable)
            {
                ConfigNode[] colorPresetNodes = GameDatabase.Instance.GetConfigNodes("AVIATION_LIGHTS_PRESET_COLORS");
                List<string> colorNames = new List<string>();
                presetColorValues = new List<Vector3>();
                for (int presetNode = 0; presetNode < colorPresetNodes.Length; ++presetNode)
                {
                    ConfigNode[] colors = colorPresetNodes[presetNode].GetNodes("Color");
                    for (int colorIndex = 0; colorIndex < colors.Length; ++colorIndex)
                    {
                        string guiName = string.Empty;
                        Vector3 value = new Vector3(0.0f, 0.0f, 0.0f);
                        if (colors[colorIndex].TryGetValue("guiName", ref guiName) && colors[colorIndex].TryGetValue("value", ref value))
                        {
                            if (colorNames.Contains(guiName) == false)
                            {
                                colorNames.Add(guiName);
                                value.x = Mathf.Clamp01(value.x);
                                value.y = Mathf.Clamp01(value.y);
                                value.z = Mathf.Clamp01(value.z);
                                presetColorValues.Add(value);
                            }
                        }
                    }
                }

                chooseField = Fields["colorPreset"];
                if (colorNames.Count > 0)
                {
                    UI_ChooseOption chooseOption = (UI_ChooseOption)chooseField.uiControlEditor;
                    chooseOption.options = colorNames.ToArray();
                    chooseOption.onFieldChanged = ColorPresetChanged;
                }
                else
                {
                    // No colors?  No preset slider.
                    chooseField.guiActiveEditor = false;
                }

                ConfigNode[] typePresetNodes = GameDatabase.Instance.GetConfigNodes("AVIATION_LIGHTS_PRESET_TYPES");
                List<string> presetNames = new List<string>();
                presetTypes = new List<TypePreset>();
                for (int presetNode = 0; presetNode < typePresetNodes.Length; ++presetNode)
                {
                    ConfigNode[] types = typePresetNodes[presetNode].GetNodes("Type");
                    for (int typeIndex = 0; typeIndex < types.Length; ++typeIndex)
                    {
                        string guiName = string.Empty;
                        string name = string.Empty;
                        float flashOn = 0.0f, flashOff = 0.0f, interval = 0.0f, intensity = 0.0f, range = 0.0f;
                        if (types[typeIndex].TryGetValue("guiName", ref guiName) &&
                            types[typeIndex].TryGetValue("flashOn", ref flashOn) &&
                            types[typeIndex].TryGetValue("flashOff", ref flashOff) &&
                            types[typeIndex].TryGetValue("interval", ref interval) &&
                            types[typeIndex].TryGetValue("intensity", ref intensity) &&
                            types[typeIndex].TryGetValue("range", ref range) &&
                            types[typeIndex].TryGetValue("name", ref name)
                        )
                        {
                            if (!presetNames.Contains(guiName))
                            {
                                presetNames.Add(guiName);

                                TypePreset type = new TypePreset();
                                type.flashOn = Mathf.Max(flashOn, 0.0f);
                                type.flashOff = Mathf.Max(flashOff, 0.0f);
                                type.interval = Mathf.Max(interval, 0.0f);
                                type.intensity = Mathf.Clamp(intensity, 0.0f, 8.0f);
                                type.range = Mathf.Max(range, 0.0f);
                                type.name = name;

                                presetTypes.Add(type);
                            }
                        }
                    }
                }

                chooseField = Fields["typePreset"];
                if (presetNames.Count > 0)
                {
                    UI_ChooseOption chooseOption = (UI_ChooseOption)chooseField.uiControlEditor;
                    chooseOption.options = presetNames.ToArray();
                    chooseOption.onFieldChanged = TypePresetChanged;
                    this.typePreset = this.typePreset % presetNames.Count;
                }
                else
                {
                    // No types?  No preset slider.
                    chooseField.guiActiveEditor = false;
                    this.typePreset = 0;
                }

                chooseField = Fields["Intensity"];
                UI_FloatRange floatRange = (UI_FloatRange)chooseField.uiControlEditor;
                floatRange.onFieldChanged = ValueChanged;

                chooseField = Fields["Range"];
                floatRange = (UI_FloatRange)chooseField.uiControlEditor;
                floatRange.onFieldChanged = ValueChanged;

                chooseField = Fields["lightR"];
                floatRange = (UI_FloatRange)chooseField.uiControlEditor;
                floatRange.onFieldChanged = ValueChanged;

                chooseField = Fields["lightG"];
                floatRange = (UI_FloatRange)chooseField.uiControlEditor;
                floatRange.onFieldChanged = ValueChanged;

                chooseField = Fields["lightB"];
                floatRange = (UI_FloatRange)chooseField.uiControlEditor;
                floatRange.onFieldChanged = ValueChanged;

                chooseField = Fields["spotLight"];
                chooseField.guiActiveEditor = (SpotAngle > 0.0f);
                UI_Toggle toggle = (UI_Toggle)chooseField.uiControlEditor;
                toggle.onFieldChanged = ValueChanged;
            }
            else
            {
                // The module is configured as non-Tweakable.  Remove the config options from the editor.
                Fields["colorPreset"].guiActiveEditor = false;
                Fields["typePreset"].guiActiveEditor = false;
                Fields["Intensity"].guiActiveEditor = false;
                Fields["Range"].guiActiveEditor = false;
                Fields["lightR"].guiActiveEditor = false;
                Fields["lightG"].guiActiveEditor = false;
                Fields["lightB"].guiActiveEditor = false;
                Fields["spotLight"].guiActiveEditor = false;
            }
        }

        /// <summary>
        /// Callback to handle slider values changing, allowing for symmetry updates
        /// </summary>
        /// <param name="field">The field that's changing.</param>
        /// <param name="oldFieldValueObj">The old value (unused).</param>
        private void ValueChanged(BaseField field, object oldFieldValueObj)
        {
            if (applySymmetry)
            {
                if (field.name == "spotLight")
                {
                    bool newValue = field.GetValue<bool>(field.host);

                    foreach (Part p in part.symmetryCounterparts)
                    {
                        ModuleNavLight ml = p.FindModuleImplementing<ModuleNavLight>();
                        if (ml != null) // shouldn't ever be null?
                        {
                            ml.spotLight = newValue;
                        }
                    }
                }
                else
                {
                    float newValue = field.GetValue<float>(field.host);

                    Action<ModuleNavLight, float> paramUpdate = null;
                    switch (field.name)
                    {
                        case "Intensity":
                            paramUpdate = delegate(ModuleNavLight lt, float val) { lt.Intensity = val; };
                            break;
                        case "Range":
                            paramUpdate = delegate(ModuleNavLight lt, float val) { lt.Range = val; };
                            break;
                        case "lightR":
                            paramUpdate = delegate(ModuleNavLight lt, float val) { lt.lightR = val; };
                            break;
                        case "lightG":
                            paramUpdate = delegate(ModuleNavLight lt, float val) { lt.lightG = val; };
                            break;
                        case "lightB":
                            paramUpdate = delegate(ModuleNavLight lt, float val) { lt.lightB = val; };
                            break;
                    }

                    foreach (Part p in part.symmetryCounterparts)
                    {
                        ModuleNavLight ml = p.FindModuleImplementing<ModuleNavLight>();
                        if (ml != null) // shouldn't ever be null?
                        {
                            paramUpdate(ml, newValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the selected in-flight mode based on the chooser.
        /// </summary>
        /// <param name="field">Field that changed (unused).</param>
        /// <param name="oldFieldValueObj">Previous value (unused).</param>
        private void FlashModeChanged(BaseField field, object oldFieldValueObj)
        {
            toggleMode = toggleModeSelector + 1;
            if (navLightSwitch != (int)NavLightState.Off)
            {
                navLightSwitch = toggleMode;
            }

            UpdateMode();

            if (applySymmetry)
            {
                UpdateSymmetry();
            }
        }

        /// <summary>
        /// Callback to manage changes to the type preset slider.
        /// </summary>
        /// <param name="field">Field that changed (unused).</param>
        /// <param name="oldFieldValueObj">Previous value (unused).</param>
        private void TypePresetChanged(BaseField field, object oldFieldValueObj)
        {
            TypePreset newtype = presetTypes[typePreset];

            this.FlashOn = newtype.flashOn;
            this.FlashOff = newtype.flashOff;
            this.Interval = newtype.interval;
            this.Intensity = newtype.intensity;
            this.Range = newtype.range;
            this.actualEnergyReq = EnergyReq * Mathf.Max(0.25f, Intensity * Intensity);

            if (applySymmetry)
            {
                foreach (Part p in part.symmetryCounterparts)
                {
                    ModuleNavLight ml = p.FindModuleImplementing<ModuleNavLight>();
                    if (ml != null) // shouldn't ever be null?
                    {
                        ml.FlashOn = this.FlashOn;
                        ml.FlashOff = this.FlashOff;
                        ml.Interval = this.Interval;
                        ml.Intensity = this.Intensity;
                        ml.Range = this.Range;
                        ml.typePreset = this.typePreset;
                        ml.colorPreset = this.colorPreset;
                    }
                }
            }
        }

        /// <summary>
        /// Callback to manage changes to the preset colors slider.
        /// </summary>
        /// <param name="field">Field that changed (unused).</param>
        /// <param name="oldFieldValueObj">Previous value (unused).</param>
        private void ColorPresetChanged(BaseField field, object oldFieldValueObj)
        {
            Color = presetColorValues[colorPreset];
            this.lightR = Color.x;
            this.lightG = Color.y;
            this.lightB = Color.z;

            if (applySymmetry)
            {
                foreach (Part p in part.symmetryCounterparts)
                {
                    ModuleNavLight ml = p.FindModuleImplementing<ModuleNavLight>();
                    if (ml != null) // shouldn't ever be null?
                    {
                        ml.Color = Color;
                        ml.lightR = lightR;
                        ml.lightG = lightG;
                        ml.lightB = lightB;
                        ml.colorPreset = this.colorPreset;
                    }
                }
            }
        }

        /// <summary>
        /// Check to update lights.
        /// </summary>
        public void Update() => this.CurrentUpdate();

		private void UpdateDummy() { }
		private void UpdateOnFlight()
		{
			if (navLightSwitch != (int)NavLightState.Off && actualEnergyReq > 0.0f && TimeWarp.deltaTime > 0.0f)
			{
				if (vessel.RequestResource(part, resourceId, actualEnergyReq * TimeWarp.deltaTime, true) < actualEnergyReq * TimeWarp.deltaTime * 0.5f)
				{
					UpdateLights(false);
					return;
				}
				else if (mainLight.intensity == 0.0f && navLightSwitch == (int)NavLightState.On)
				{
					// All other nav lights will reset below, but the On case is different.
					UpdateLights(true);
				}
			}

			elapsedTime += TimeWarp.deltaTime;

			switch ((NavLightState)navLightSwitch)
			{
				case NavLightState.Off:
					this.UpdateLights(false);
					break;

				case NavLightState.On:
					this.UpdateLights(true);
					break;

				case NavLightState.Flash:
					// Lights are in 'Flash' mode
					if (elapsedTime >= nextInterval)
					{
						elapsedTime = elapsedTime % nextInterval;

						flashCounter = (flashCounter + 1) & 1;

						nextInterval = ((flashCounter & 1) == 1) ? FlashOn : FlashOff;

						UpdateLights((flashCounter & 1) == 1);
					}
					break;

				case NavLightState.DoubleFlash:
					// Lights are in 'Double Flash' mode
					if (elapsedTime >= nextInterval)
					{
						elapsedTime = elapsedTime % nextInterval;

						flashCounter = (flashCounter + 1) & 3;

						nextInterval = (flashCounter > 0) ? FlashOn : FlashOff;

						UpdateLights((flashCounter & 1) == 1);
					}
					break;

				case NavLightState.Interval:
					// Lights are in 'Interval' mode
					if (elapsedTime >= Interval)
					{
						elapsedTime = elapsedTime % Interval;

						flashCounter = (flashCounter + 1) & 1;
						UpdateLights((flashCounter & 1) == 1);
					}
					break;
			}
		}
		private void UpdateOnEditor()
		{
			// Account for any tweakable tweaks.
			Color.x = lightR;
			Color.y = lightG;
			Color.z = lightB;

			elapsedTime += Time.deltaTime;

			bool lightsOn = false;
			switch ((NavLightState)navLightSwitch)
			{
				case NavLightState.On:
					flashCounter = 1;
					break;
				case NavLightState.Off:
					flashCounter = 0;
					break;

				case NavLightState.Flash:
					// Lights are in 'Flash' mode
					if (elapsedTime >= nextInterval)
					{
						elapsedTime = elapsedTime % nextInterval;

						flashCounter = (flashCounter + 1) & 1;

						nextInterval = ((flashCounter & 1) == 1) ? FlashOn : FlashOff;
					}
					break;

				case NavLightState.DoubleFlash:
					// Lights are in 'Double Flash' mode
					if (elapsedTime >= nextInterval)
					{
						elapsedTime = elapsedTime % nextInterval;

						flashCounter = (flashCounter + 1) & 3;

						nextInterval = (flashCounter > 0) ? FlashOn : FlashOff;
					}
					break;

				case NavLightState.Interval:
					// Lights are in 'Interval' mode
					if (elapsedTime >= Interval)
					{
						elapsedTime = elapsedTime % Interval;

						flashCounter = (flashCounter + 1) & 1;
					}
					break;
			}

			// Or it with lightsOn in case we're in On mode.
			lightsOn = ((flashCounter & 1) == 1);


			Color newColor = new Color(Color.x, Color.y, Color.z);
			if (lensMaterial.Length > 0)
			{
				Color newEmissiveColor = (lightsOn) ? newColor : XKCDColors.Black;
				for (int i = 0;i < lensMaterial.Length;++i)
				{
					lensMaterial[i].SetColor(colorProperty, newColor);
					lensMaterial[i].SetColor(emissiveColorProperty, newEmissiveColor);
				}
			}

			mainLight.intensity = (lightsOn) ? Intensity : 0.0f;
			mainLight.range = Range;
			mainLight.color = newColor;

			if (SpotAngle > 0.0f)
			{
				mainLight.type = (spotLight) ? LightType.Spot : LightType.Point;
			}
		}

        /// <summary>
        /// Provide the VAB module display name.
        /// </summary>
        /// <returns></returns>
        public override string GetModuleDisplayName()
        {
            return "#AL_ModuleDisplayName";
        }

        /// <summary>
        /// Provide VAB info.
        /// </summary>
        /// <returns></returns>
        public override string GetInfo()
        {
            string resourceUiName = string.Empty;
            PartResourceDefinition def;
            if (!string.IsNullOrEmpty(Resource))
            {
                try
                {
                    def = PartResourceLibrary.Instance.resourceDefinitions[Resource];
                    resourceId = def.id;
                }
                catch (Exception)
                {
                    resourceId = PartResourceLibrary.ElectricityHashcode;
                    def = PartResourceLibrary.Instance.resourceDefinitions[resourceId];
                }
            }
            else
            {
                resourceId = PartResourceLibrary.ElectricityHashcode;
                def = PartResourceLibrary.Instance.resourceDefinitions[resourceId];
            }

            resourceUiName = def.displayName;

            if (EnergyReq > 0.0f)
            {
                return KSP.Localization.Localizer.Format("#autoLOC_244201", resourceUiName, (EnergyReq * 60.0f).ToString("0.0"));
            }
            else
            {
                return KSP.Localization.Localizer.Format("#AL_NoEnergy", resourceUiName);
            }
        }

        /// <summary>
        /// Toggle the light on or off.
        /// </summary>
        /// <param name="lightsOn"></param>
        private void UpdateLights(bool lightsOn)
        {
            mainLight.intensity = (lightsOn) ? Intensity : 0.0f;

            if (lensMaterial.Length > 0)
            {
                Color newColor = (lightsOn) ? new Color(Color.x, Color.y, Color.z) : XKCDColors.Black;

                for (int i = 0; i < lensMaterial.Length; ++i)
                {
                    lensMaterial[i].SetColor(emissiveColorProperty, newColor);
                }
            }
        }

        /// <summary>
        /// Update settings based on navLightSwitch changing, reset counters, etc.
        /// </summary>
        private void UpdateMode()
        {
            elapsedTime = 0.0f;
            flashCounter = 0;

            switch (navLightSwitch)
            {
                case (int)NavLightState.Off:
                default:
                    navLightSwitch = (int)NavLightState.Off; // Trap invalid values.
                    UpdateLights(false);
                    break;
                case (int)NavLightState.Flash:
                    nextInterval = FlashOff;
                    UpdateLights(false);
                    break;
                case (int)NavLightState.DoubleFlash:
                    nextInterval = FlashOff;
                    UpdateLights(false);
                    break;
                case (int)NavLightState.Interval:
                    UpdateLights(false);
                    break;
                case (int)NavLightState.On:
                    UpdateLights(true);
                    break;
            }

            switch (toggleMode)
            {
                case (int)NavLightState.Off:
                case (int)NavLightState.Flash:
                default:
                    // For the toggle mode, always force the default to ModeFlash.  Toggle mode should never be set to Off.
                    toggleBaseEvent.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#AL_ToggleFlash" ;
                    toggleBaseAction.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#AL_ToggleFlash" ;
                    toggleMode = (int)NavLightState.Flash;
                    break;
                case (int)NavLightState.DoubleFlash:
                    toggleBaseEvent.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#AL_ToggleDoubleFlash" ;
                    toggleBaseAction.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#AL_ToggleDoubleFlash" ;
                    break;
                case (int)NavLightState.Interval:
                    toggleBaseEvent.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#AL_ToggleInterval" ;
                    toggleBaseAction.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#AL_ToggleInterval" ;
                    break;
                case (int)NavLightState.On:
                    toggleBaseEvent.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#autoLOC_6001405" ;
                    toggleBaseAction.guiName = HighLogic.LoadedSceneIsEditor ? "#AL_ToggleSelectedMode" : "#autoLOC_6001405" ;
                    break;
            }
            toggleModeSelector = toggleMode - 1;
        }

        private void UpdateSymmetry()
        {
            if (HighLogic.LoadedSceneIsEditor && applySymmetry)
            {
                foreach (Part p in part.symmetryCounterparts)
                {
                    ModuleNavLight ml = p.FindModuleImplementing<ModuleNavLight>();
                    if (ml != null) // shouldn't ever be null?
                    {
                        ml.navLightSwitch = this.navLightSwitch;
                        ml.toggleMode = this.toggleMode;
                        ml.UpdateMode();
                    }
                }
            }
        }

        //--- "Toggle" action group actions ----------------------------------

        [KSPAction("#AL_ToggleSelectedMode", KSPActionGroup.Light)]
        public void CurrentModeToggle(KSPActionParam param) => this.ToggleCurrentMode();

        [KSPAction("#autoLOC_6001405", KSPActionGroup.None)]
        public void LightToggle(KSPActionParam param) => this.ToggleLightOn();

        [KSPAction("#AL_ToggleFlash", KSPActionGroup.None)]
        public void FlashToggle(KSPActionParam param) => this.ToggleLightFlash();

        [KSPAction("#AL_ToggleDoubleFlash", KSPActionGroup.None)]
        public void DoubleFlashToggle(KSPActionParam param) => this.ToggleLightDoubleFlash();

        [KSPAction("#AL_ToggleInterval", KSPActionGroup.None)]
        public void IntervalToggle(KSPActionParam param) => this.ToggleLightInterval();

        //--- "Set" action group actions -------------------------------------

        [KSPAction("#AL_SetSelectedMode", KSPActionGroup.None)]
        public void CurrentModeOnAction(KSPActionParam param) => this.SetCurrentModeOn();

        [KSPAction("#autoLOC_6001406", KSPActionGroup.None)]
        public void LightOnAction(KSPActionParam param) => this.SetLightOn();

        [KSPAction("#AL_SetFlash", KSPActionGroup.None)]
        public void LightFlashAction(KSPActionParam param) => this.SetLightFlashOn();

        [KSPAction("#AL_SetDoubleFlash", KSPActionGroup.None)]
        public void LightDoubleFlashAction(KSPActionParam param) => this.SetLightDoubleFlashOn();

        [KSPAction("#AL_SetInterval", KSPActionGroup.None)]
        public void LightIntervalAction(KSPActionParam param) => this.SetLightIntervalOn();

        [KSPAction("#autoLOC_6001407", KSPActionGroup.None)]
        public void LightOffAction(KSPActionParam param) => this.SetLightOff();

        //--- Part context menu events ---------------------------------------

        [KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#AL_ToggleSelectedMode")]
        public void ToggleEvent() => this.ToggleCurrentMode();

        //--- Public Interface -----------------------------------------------

        public bool IsLightTurnedOn => NavLightState.Off != (NavLightState)this.navLightSwitch;

        public void SetLightOff()
        {
            Log.dbg("SetLightOff for {0}", this.InstanceID);
            navLightSwitch = (int)NavLightState.Off;
            this.UpdateMe();
        }

        public void SetCurrentModeOn()
        {
            Log.dbg("SetCurrentModeOn for {0}", this.InstanceID);
            navLightSwitch = toggleMode;
            this.UpdateMe();
        }

        public void ToggleCurrentMode()
        {
            Log.dbg("ToggleCurrentMode for {0}", this.InstanceID);
            navLightSwitch = this.IsLightTurnedOn ? (int)NavLightState.Off : toggleMode;
            this.UpdateMe();
        }

        public void SetLightOn()
        {
            Log.dbg("SetLightOn for {0}", this.InstanceID);
            toggleMode = navLightSwitch = (int)NavLightState.On;
            this.UpdateMe();
        }

        public void ToggleLightOn()
        {
            Log.dbg("ToggleLightOn for {0}", this.InstanceID);
            navLightSwitch = this.IsLightTurnedOn ? (int)NavLightState.Off : (int)NavLightState.On;
            toggleMode = (int)NavLightState.On;
            this.UpdateMe();
        }

        public void SetLightFlashOn()
        {
            Log.dbg("SetLightFlashOn for {0}", this.InstanceID);
            toggleMode = navLightSwitch = (int)NavLightState.Flash;
            this.UpdateMe();
        }

        public void ToggleLightFlash()
        {
            Log.dbg("ToggleLightFlash for {0}", this.InstanceID);
            navLightSwitch = this.IsLightTurnedOn ? (int)NavLightState.Off : (int)NavLightState.Flash;
            toggleMode = (int)NavLightState.Flash;
            this.UpdateMe();
        }

        public void SetLightDoubleFlashOn()
        {
            Log.dbg("SetLightDoubleFlashOn for {0}", this.InstanceID);
            toggleMode = navLightSwitch = (int)NavLightState.DoubleFlash;
            this.UpdateMe();
        }

        public void ToggleLightDoubleFlash()
        {
            Log.dbg("ToggleLightDoubleFlash for {0}", this.InstanceID);
            navLightSwitch = this.IsLightTurnedOn ? (int)NavLightState.Off : (int)NavLightState.DoubleFlash;
            toggleMode = (int)NavLightState.DoubleFlash;
            this.UpdateMe();
        }

        public void SetLightIntervalOn()
        {
            Log.dbg("SetLightIntervalOn for {0}", this.InstanceID);
            toggleMode = navLightSwitch = (int)NavLightState.Interval;
            this.UpdateMe();
        }

        public void ToggleLightInterval()
        {
            Log.dbg("ToggleLightInterval for {0}", this.InstanceID);
            navLightSwitch = this.IsLightTurnedOn ? (int)NavLightState.Off : (int)NavLightState.Interval;
            toggleMode = (int)NavLightState.Interval;
            this.UpdateMe();
        }

        private void UpdateMe()
        {
            UpdateMode();
            UpdateSymmetry();
        }

		// This was borking on OnDestroy, so I decided to cache the information and save a NRE there.
		private string _InstanceID = null;
		public string InstanceID => this._InstanceID ?? (this._InstanceID = string.Format("{0}:{1}:{2:X}", this.vessel?.vesselName, this?.part?.name, this.part?.GetInstanceID()));
    }
}
