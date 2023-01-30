using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Aviation Lights /L")]
[assembly: AssemblyDescription("Aviation Lights for Kerbal Space Program")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(AviationLights.LegalMamboJambo.Company)]
[assembly: AssemblyProduct(AviationLights.LegalMamboJambo.Product)]
[assembly: AssemblyCopyright(AviationLights.LegalMamboJambo.Copyright)]
[assembly: AssemblyTrademark(AviationLights.LegalMamboJambo.Trademark)]
[assembly: AssemblyCulture("")]

// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("166089c6-243e-4c70-b327-382045c081d9")]

[assembly: AssemblyVersion(AviationLights.Version.Number)]
[assembly: AssemblyFileVersion(AviationLights.Version.Number)]

[assembly: KSPAssembly("AviationLights", AviationLights.Version.major, AviationLights.Version.minor)]
[assembly: KSPAssemblyDependency("KSPe.Light.AviationLights", 2, 4)]
