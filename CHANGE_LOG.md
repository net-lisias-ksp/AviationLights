# Aviation Lights /L :: Change Log

* 2023-0130: 4.2.0.0 (LisiasT) for KSP >= 1.3.0
	+ Formal adoption of the thing. **#HURRAY!!**	+ Updating it to use `KSPe.Light`
	+ Automatically patches the lights to be used with `B9PartSwitch` if available.
	+ Allows customise the Lights's Max Range
		- Use with care, too big values can ruin your visuals!
* 2023-0129: 4.1.3.0 (MOARDdV) for KSP >= 1.12.0
	Removed the legacy (Aviation Lights 3.x) lights.
	Fix pt-br localization, also courtesy Lisias.
	+ Recompiled for KSP 1.12
	+ B9 patches may need updated. They are disabled by default.
	+ Placed up for adoption.
* 2021-1029: 4.1.1.4 (LisiasT) for KSP >= 1.3.0
	+ Updating KSPe to v2.4
	+ Properly patching the parts to be activated or deactivated depending on the KSP version you are using.
		- Old lights are available on the Part Palette only on KSP 1.3.1 and older, but they still exists for older craft compatibility.
		- The new light with variant are only present on KSP 1.4 and newer.
* 2021-0517: 4.1.1.3 (LisiasT) for KSP >= 1.3.0
	+ Reverting a bad decision.
* 2021-0413: 4.1.1.2 (LisiasT) for KSP >= 1.3.0
	+ Added KSPe facilities
	+ Making it compatible from KSP 1.3.0 to the latest.
	+ Merged updates from yamanaiyuki
		- Japanese translation
		- Adding stackable support on KSP 1.11
	+ Added PT-BR translation
	+ Added KIS support.
* 2021-0331: 4.1.1.1 (LisiasT) for KSP >= 1.3.0
	+ ***DITCHED*** due wrongly compilation against a beta release of KSPe.
* 2020-0705: 4.1.1 (MOARdV) for KSP 1.10.0
	+ For KSP 1.10.0 - 5 July 2020 - v4.1.1
			- Recompiled for KSP 1.10.0 (and finally updated the version file, alas).
			- Fixed bug that enabled the spotlight toggle on non-tweakable lights (such as the old aviation lights), as noted by GitHub user Promyclon.
* 2019-1016: 4.1.0 (MOARdV) for KSP 1.8.0
	+ For KSP 1.8.x
			- Recompiled for KSP 1.8.0
			- Redesigned the VAB PAW components.
* 2019-0706: 4.0.8 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - 1.7.x
			- Added zh-cn localization courtesy duck1998.
* 2019-0226: 4.0.7 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - 1.6.x
			- Added fr-fr localization courtesy don-vip.  Pull request #16.
			- Finally synchronized the DLL binary and the version file (forgot to update .version for 4.0.6).
* 2018-1221: 4.0.6 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - 1.6.x
			- Added es-es localization courtesy fitiales.  Pull request #15.
			- Scaled energy consumption based on light intensity.  Issue #13.
* 2018-1016: 4.0.5.1 (MOARdV) for KSP 1.5.0
	+ For KSP 1.5.0 and beyond
			- Recompiled against KSP 1.5.0.
			- Fixed: Typo in version file.
* 2018-0506: 4.0.5 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - KSP 1.5.0
			- Fixed: Aviation Lights illuminate the surface of the planet (Issue #10).
* 2018-0409: 4.0.4 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - 1.4.2
			- Fixed: Flashing lights blink rapidly after exiting warp.  Issue #8.
* 2018-0407: 4.0.3 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - 1.4.2
			- Correctly implement symmetry behavior for the new point/spot toggle (Issue #7).
* 2018-0407: 4.0.2 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - 1.4.2
			- Fix: Emissive layer not updating correctly in flight (Issue #6).
			- Tweakable lights configured as spot lights can switch between spot and point lighting modes in the editor (Issue #7).
* 2018-0403: 4.0.1 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - KSP 1.4.2
			- Added tags to the lights, including cck_lights for compatibility with the Community Category Kit.
* 2018-0331: 4.0.0 (MOARdV) for KSP 1.4.0
	+ For KSP 1.4.0 - KSP 1.4.2
	+ There are many changes in this version of Aviation Lights.  I recommend you spend a few minutes going through the [readme](https://github.com/MOARdV/AviationLights/blob/master/README.md) file.
			- Old Aviation Lights are now hidden in the editor.  They may be unhidden using a Module Manager patch.
			- New fully-configurable aviation light is available in the editor.  Light color and mode (navigation, beacon, strobe) are configurable using presets, housing and lens styles are configurable.  Using Advanced Tweakables allows more fine-tuning of the light.
* 2017-0526: 3.14 (MOARdV) for KSP 1.3.0
	+ For KSP 1.3.0 / 1.3.1
	+ Fixes NRE in VAB/SPH when trying to set flash mode to Flash, Double Flash, or Interval.
* 2017-0526: 3.13 (MOARdV) for KSP 1.3.0
	+ Recompiled for KSP 1.3.0
* 2016-1020: 3.12 (MOARdV) for KSP 1.2.
	+ For KSP 1.2.x
		- Re-enabled right-click menus in the VAB.
* 2016-1012: 3.11 (MOARdV) for KSP 1.2.0
	+ FIXED: Hang when hovering over aviation lights.
	+ FIXED: Flash interval backwards when toggling Flash mode when the light was on.
	+ CHANGED: Part names so they show up together (instead of "Red Beaconlight", it's now "Light, Beacon, Red").  This is _not_ a save-breaking change.
	+ CHANGED: Converted PNG textures to DDS, and resized them to 128 x 128 (256 x 256 is way too big for the part size - 128 x 128 is still probably bigger than it needs to be).
* 2016-1011: 3.10 (MOARdV) for KSP 1.2.0
	+ Aviation Lights recompiled for KSP 1.2.0
* 2016-0508: 3.9 (MOARdV) for KSP 1.1.
	+ KSP 1.1.0 - KSP 1.1.3
		- Updated to KSP 1.1.x
* 2016-0508: 3.8 (MOARdV) for KSP 1.0.5
	+ KSP 1.0.5
	+ This snapshot is to provide a reference state for the source code as of the AviationLights v3.8 release.  The download is not provided.
