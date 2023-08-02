# Aviation Lights /L :: Changes

* 2023-0801: 4.2.1.1 (LisiasT) for KSP >= 1.3.0
	+ Desperate attempt to mitigate an issue (that I don't even now if it's really an issue) where AL could be inducing (or being induced) to screw up a Scene transitioning reported by [exospaceman](https://forum.kerbalspaceprogram.com/profile/224165-exospaceman/) on [Forum](https://forum.kerbalspaceprogram.com/topic/218634-need-a-help-on-a-ksp-bug-i-am-dealing-with/#comment-4306227).
	+ Related issues:
		- [#4](https://github.com/net-lisias-ksp/AviationLights/issues/4) Aviation Lights *may* be involved on a weird bug report on Forum
* 2023-0705: 4.2.1.0 (LisiasT) for KSP >= 1.3.0
	+ New Action to turn on whatever is the current mode at the moment
		- The Toggle Action was correctly labeled (and localized)
		- Less confusion on Editor when configuring Actions
	+ Enhanced API to make easier 3rd parties interactions
	+ The different modes toggles now really toggles the Light no matter what is the current mode.
* 2023-0627: 4.2.0.2 (LisiasT) for KSP >= 1.3.0
	+ A small glitch on handling Flashing Lights was detected and fixed.
	+ A stupid mistake on handling ModuleManager callbacks was detected and fixed.
* 2023-0130: 4.2.0.1 (LisiasT) for KSP >= 1.3.0
	+ Formal adoption of the thing. **#HURRAY!!**
	+ Updating it to use `KSPe.Light`
	+ Automatically patches the lights to be used with `B9PartSwitch` if available.
	+ Allows customise the Lights's Max Range
		- Use with care, too big values can ruin your visuals!
* 2023-0130: 4.2.0.0 (LisiasT) for KSP >= 1.3.0
	+ ***DITCHED*** as I had, **againg**, tested the thing in a dirty dev installment, and things gone south on a clean installation. 
