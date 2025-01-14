# Aviation Lights /L

Aviation Lights for Kerbal Space Program.

Now under Lisias' Management!


## In a Hurry

* Documentation
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/211781-*/)
	+ [Project's README](https://github.com/net-lisias-ksp/AviationLights/blob/master/README.md)
	+ [Install Instructions](https://github.com/net-lisias-ksp/AviationLights/blob/master/INSTALL.md)
	+ [Change Log](https://github.com/net-lisias-ksp/AviationLights/blob/master/CHANGE_LOG.md)
	+ [Known Issues](https://github.com/net-lisias-ksp/AviationLights/blob/master/KNOWN_ISSUES.md)
* Official Distribution Sites:
	+ [CurseForge](https://www.curseforge.com/kerbal/ksp-mods/aviation-lights-l)
	+ [SpaceDock](https://spacedock.info/mod/3208/Aviation%20Lights)
	+ [Homepage](http://ksp.lisias.net/add-ons/TweakScale) on L Aerospace
	+ [Source and Binaries](https://github.com/net-lisias-ksp/AviationLights) on GitHub.
	+ [Latest Release](https://github.com/net-lisias-ksp/AviationLights/releases)
		- [Binaries](https://github.com/net-lisias-ksp/AviationLights/Archive)

## Description

There are two types of standard aviation lights:

**Navigation Lights** ("Position Lights") are a safety feature on every plane bigger than an ultra-light. They indicate the orientation of the plane so other aircraft know which direction the aircraft is going. For that purpose, the international standard is a red light on the left wingtip, a green one on the right, and a white nav light on the tail.

Example of the correct use in KSP:

![Aviation Lights](./Docs/imgs/6cz23stq.png)

But, of course, this is KSP. You can just put them on where it looks cool. :D

**Warning Lights** (both "Strobe" and "Beacon") are flashing lights to enhance visibility in bad weather and to warn air traffic and ground personnel.

Beacons are typically mounted on the tip of the vertical tail on smaller planes, or in the middle of the fuselage (top and/or bottom) on larger aircraft. They're red, bright and flashing to indicate that parts of the airplane (engines) are moving or the airplane itself is about to move.

Strobes are very bright, white, fast blinking lights which are mounted on the wingtips (next to the red/green navlights) and sometimes on the tail (next to the white navlight) on larger planes. They're so bright that they remain off until the pilot lines up on the runway, so the ground personnel won't be blinded if they stand right next to it.

In addition to the standard navigation and warning light colors, there are amber and blue preset options.

### Localization

Aviation Lights supports localization. Currently available:

* en-us
* es-es
* fr-fr
* ja
* pt-br
* zh-cn

### Legacy Aviation Lights

Found a nice craft file on [KerbalX](https://kerbalx.com) but it was made for KSP 1.3 and the old lights are not available anymore? We have you covered, Veteran! Aviation Lights /L has full support for legacy crafts made on the Old Days! The legacy aviation lights from Aviation Lights 3.x do not appear in the editor by default on KSP >= 1.4, but are available when running on KSP 1.3. 

If you do not wish to include the old parts in your installation, and you do not have any vessels currently using those lights, feel free to delete the
`AviationLights/Parts/lights` folder - unless you are running KSP 1.3.x .

On the other hand, if you **want** to use them even on modern KSPs, create a directory on `GameData` called `AviationLights_LEGACY` (or create a config file with a dummy MM patch defining the symbol `AviationLights_LEGACY` in a `:FOR`).

### B9 Part Switcher

Suport for B9 Part Switcher is now activated by default if `ModuleB9PartSwitch` is found.

### VAB/SPH CONFIGURATION

The basic light (Light, Aviation) is a configurable light, allowing one part to fill any of the preset aviation light roles. Using advanced tweakables, the color, range, and intensity may be customised even more. This light is found in the Utility menu by default (other mods may change this).

Aviation Lights may be configured in the editor (VAB and SPH). The basic part menu provides a part variant selector to choose which mode the light will use. The selected mode is displayed in the Toggle button next to the variant selector. If an aviation light is enabled in the editor, it will be enabled when the vessel spawns in the Flight scene.

A toggle button allows changes to be applied automatically to symmetry parts.

A Type Preset slider allows the light to be configured as a navigation light, strobe light, or a beacon light. Additional type presets may be added. See PRESETS below for information on how to configure custom types.

A Color Preset slider provides all of the standard Aviation Lights colors (white, red, green, blue, and amber). This allows a single part to function as any colored aviation light. Additional preset colors may be added. See PRESETS below for more information on how to configure custom colors.

### Advanced Tweakables

![Advanced Tweakables](./Docs/imgs/ZwnnqL.jpg)

When Advanced Tweakables are enabled, parts that support the Color Preset and Type Preset will also have sliders to customize the RGB colors of the light, as well as the intensity of the light and its range. The RGB colors reset if the Color Preset is changed. The intensity and range reset if the Type Preset is changed.

If the part is configured as a spot light (the SpotAngle config field is greater than 0), the editor will also allow toggling the light between a spot light or a point (omni-directional) light.

### FLASH MODES

In addition to the conventional "Light on" and "Light off" settings, Aviation Lights may be configured to flash using one of three patterns. The all-caps / all-lower below describes the pattern (ALL-CAPS = light is on, all-lower = light is off), with the name reporting which config value controls how long the light spends in that state.

* **Flash**: `FLASHON-flashoff` - In this mode, the light flashes on and off.  The time spent with the light on may be different than the time with the light off.
* **Double Flash**: `FLASHON-flashon-FLASHON-flashoff` - In this mode, the light flashes on and off.  The on time is a double flash - the light will turn on, turn off, and turn on again before turning off for a different period of time.  The `FlashOff` setting controls how long the light remains off after the double flash, while `FlashOn` controls how long the light
remains on as well as how long it switches off between the double flashes.
* **Interval**: `INTERVAL-interval` - In this mode, the light flashes evenly on and off. The amount of time spent on or off is controlled by the `Interval` setting in the config.

### PRESETS

Aviation Lights 4.0 and later supports *presets*. There are two categories of presets, *type* and *color*. The AL package includes its default presets in GameData/AviationLights/Plugins/AviationLightsPresets.cfg. This config file supports MM editing. Players and modders may also add their own custom presets in separate config files - AL will scan all applicable config nodes for color and type presets.

#### Type Presets

Type presets control the type of light that is configured. Each type defines the intensity of the light, the range of the light, and the intervals for the flash patterns. Type presets are searched for in config nodes named `AVIATION_LIGHTS_PRESET_TYPES`.

```
AVIATION_LIGHTS_PRESET_TYPES
{
	name = DefaultAviationLightsTypes
	
	Type
	{
		name = nav
		guiName = #AL_TypeNavigation

		flashOn = 0.5
		flashOff = 1.5
		interval = 1.0

		intensity = 0.5
		range = 10
	}
	
	... additional Type nodes
}
```

* **name** - The name of the preset type (making it easier to edit using MM).
* **guiName** - The name of the preset type that shows up in the Type Preset control. This field supports localization.
* **flashOn**, **flashOff**, **interval** - Timing values for the flash modes of the light, as described above in FLASH MODES.
* **intensity** - How bright the light is. Nav Lights, for instance, use 0.5. Strobe or beacon lights may use values of 1 or higher (with a maximum of 8).
* **range** - The range of the light.

#### Color Presets

Color presets are simply color options that may be selected in the VAB. Color presets are searched for in config nodes named `AVIATION_LIGHTS_PRESET_COLORS`.

```
AVIATION_LIGHTS_PRESET_COLORS
{
	name = DefaultAviationLightsColors
	
	Color
	{
		name = white
		guiName = #AL_ColorWhite
		value = 1.00, 0.95, 0.91
	}
	
	... additional Color nodes
}
```

* **name** - The name of the color (making it easier to edit using MM).
* **guiName** - The name of the color that shows up in the Color Preset control. This field supports localization.
* **value** - The normalised RGB values of the color, ranging from 0 to 1.

### MODULE CONFIGURATION

There are a number of fields in ModuleNavLight that allow creation of a custom of a part. Default values are shown below.

```
MODULE
{
   name = ModuleNavLight

   Color = 1.0, 0.95, 0.91

   Intensity = 0.5
   Range = 10.0

   Interval = 1.0
   FlashOn = 0.5
   FlashOff = 1.5

   Resource = ElectricCharge
   EnergyReq = 0.0

   LightOffset = 0.0, 0.0, 0.0
   LightRotation = 0, 0, 0
   SpotAngle = 0
   LensTransform = ""
   
   Tweakable = true
}
```

#### Light Color

The light color values default to a white navigation light.  The color fields allow a part creator to set up a light for a specific purpose by defining the color, intensity, and range of the light. When `Tweakable = true`, the color may be changed using the Color Preset control in the part menu, and the Intensity and Range may be changed with the Type Preset control.  In addition, the color, intensity, and range may be edited directly by enabling Advanced Tweakables.

* **Color**: The RGB color of the light. Valid values are from 0 to 1 for each channel.
* **Intensity**: The intensity of the light. Brighter lights should use larger values. Valid numbers range from 0 to 8. Nav lights use 0.5. Energy consumption
is affected by Intensity.
* **Range**: The range of the light, in meters.  This setting controls how far from the light any illumination is projected.  Objects outside this range are not illuminated by the light.

#### Flash Timing

The default flash timing values are for a navigation light.   Flash timing is one component of the Type Preset when `Tweakable` = `true`, which means that custom timings may be overridden in the Editor. If the custom timing does not also have a type preset defined for it, it will not be possible for a player to restore custom timing on a light without removing it and attaching a new one. All times are measured in seconds.

* **Interval**: How long the light stays on or stays off when it is in Interval mode.
* **FlashOn**: How long the light stays on in Flash mode or Double Flash mode, and how long it stays off between the two flashes in Double Flash mode.
* **FlashOff**: How long the light stays off in Flash mode or after the second flash in Double Flash mode.


#### Resources

The resource fields control the resource type and amount consumed per second. By default, the parts require `ElectricCharge`, but they do not consume energy.

EnergyReq is affected by the Intensity of the light. The EnergyReq listed in the part config is the amount of resources required for a light with an Intensity of 1.0. EnergyReq is scaled by the square of the Intensity, with a minimum scale of 0.25. For example, a Nav Light that has an EnergyReq of 0.020 and an Intensity of 0.5 will actually use 0.005 EC (= 0.020 x (0.5 x 0.5)). An Intensity of 2.0 will consume 4x the listed EnergyReq.

* **Resource**: The name (from the `RESOURCE_DEFINITION`) of the resource consumed when this light is on.
* **EnergyReq**: The amount of the resource consumed per second while switched on for a light of Intensity = 1.0. If this value is zero, the light does not consume any resources. Intensity modifies this value.

#### Advanced

These fields are advanced fields available for modders to create custom light models that integrate with Aviation Lights.

* **LightOffset**: The displacement from the root gameObject of the model where the light should be added, in meters.
* **LightRotation**: Rotates the light's game object around the X, Y, and Z axis. This field is only applicable for spot lights.
* **SpotAngle**: When SpotAngle is greater than zero, the light functions as a spot light instead of a point (omni-directional) light. SpotAngle is the width of the spotlight in degrees.
* **LensTransform**: A semi-colon (';') delimited list of transforms in the model that contain lens textures. AL will adjust the diffuse (\_Color) and emissive (\_EmissiveColor) tint on all of those transforms based on the current color of the light. If this field is omitted, lens colors will not change to match the light's color.
* **Tweakable**: A boolean that controls whether the part's color and type may be changed in the editor. With some custom lights, such as lights with pre-tinted lenses, allowing the colors and types to be changed in the Editor may result in poor-looking models in-flight.

There are persistent values not listed here. These values are used to keep track of internal state. They should not be added to a config file or edited in the `persistent.sfs` file.


## Installation

Detailed installation instructions are now on its own file (see the [In a Hurry](#in-a-hurry) section) and on the distribution file.

## License:

This work is licensed under the [CC BY-NC-SA 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/).

Please note the copyrights and trademarks in [NOTICE](./NOTICE).

### Additional Credits

From the original AviationLights [forum post](http://forum.kerbalspaceprogram.com/index.php?/topic/16801-105-aviation-lights-v38-16nov15/):

First things first: Big thanks for RPGprayer's "Position/Navigation Lights" AddOn, from which the Aviation Lights originated.

Additional credits go to Deadweasel, Why485, GROOV3ST3R, JDP and J.Random for their great help with this addon. Thanks to BigNose for keeping this mod going.

StoneBlue provided the Aviation Lights 4.0 configurable light.

MOARdV kept it ongoing for many years.


## UPSTREAM

* [MOARdv](https://forum.kerbalspaceprogram.com/index.php?/profile/60950-moardv/) PREVIOUS
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/173305-*/) 
	+ [Github](https://github.com/MOARdV/AviationLights)
* [BigNose](https://forum.kerbalspaceprogram.com/index.php?/profile/35713-bignose/)
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/16801-*/) 
	+ [Github](https://github.com/MOARdV/AviationLights)
* [RPGprayer](https://forum.kerbalspaceprogram.com/index.php?/profile/8534-rpgprayer/) ROOT
	+ [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/7950-*/)
	+ [SendSpace](https://www.sendspace.com/file/egsak6) (MIA)

### References

* [yamanaiyuki](https://github.com/yamanaiyuki) Parallel Fork
	+ [Github](https://github.com/yamanaiyuki/AviationLights)
