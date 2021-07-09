using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("exposeParamsToSupportDetails, deleteSpecInfoFromSupports")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("exposeParamsToSupportDetails, deleteSpecInfoFromSupports")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("599cb18d-39a7-4181-aa02-480320946e8f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("16.21.0.10")]
[assembly: AssemblyFileVersion("16.21.0.10")]

//V0
//copied supportutils and deleted all but the param copy part and the spec erase
//v1
//for imperial projects the dimensions will be in feet, inch (precision = 4), other projects mm (precision = 1)
//current DWGUNITS are taken as flag for using inch or mm
//v2
//one command for info exposing and deleting spec field content:    exposeParamsToSupportDetails
//one command for deleting spec field content only:                 deleteSpecInfoFromSupports
//v3
//didn't jwork on 2019, had to move commands from extermanl file to the actual program
//v4
//special edition for a.j., applies to the AcPpDb3dPipeInlineAsset instead of Supports, will not delete spec entry
//v5
//will not delete spec entry, applies just for supports again, save designparams into the custom field "DesignParams"
//v6
//again for a.j., need to use ACPPPIPEINLINEASSET
//v7
//solving catch problems with parameters (no numbers, empty)
//v8 new command exposeParamsAll and exposeParams and exposeParamsHelp, not deleting the support spec information anymore, need to do this with ComponentDesignation=Parametric
//v9 PE_Design_Parameters instead of DesignParams on special request
//v10 PE_Design_Parameters changed to ParameterExtract