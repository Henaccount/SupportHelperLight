# SupportHelperLight


Manual:

you need to create a string property called "ParameterExtract" for your pipe run components

compile the code to get SupportsUtils.dll, put it into C:\Program Files\Autodesk\AutoCAD 20XX\PLNT3D

It has to be loaded with “netload”

or you place the following string in the
C:\Program Files\Autodesk\AutoCAD 20XX\Support\de-de\acad20XXdoc.lsp (different path for different language installation)
for automatic loading (use slashes not backslashes):
(command "_netload" "C:/Program Files/Autodesk/AutoCAD 20XX/PLNT3D/supportUtils.dll")

integrate the command in your custom save button if you want:
https://knowledge.autodesk.com/community/article/166081

The available commands from this code are: 
"exposeParams" : put supports design parameters in the "ParameterExtract" field
"exposeParamsAll" : put supports and pipeinlineasset design parameters in the "ParameterExtract" field
"exposeParamsHelp" : showing help on the command line

If the dll doesn't load or giving errors, add the 2nd line "<load.." to acad.exe.config (AutoCAD installation folder):
   <runtime>        
               <generatePublisherEvidence enabled="false"/>   
               <loadFromRemoteSources enabled="true"/> 
   </runtime>


The script will loop through all supports in the drawing and copies the design parameter to the Property field: “ParameterExtract”.

Tipp: Use e.g. H=0,L=0 in the DesignParams field to just expose those two parameters, do it in the catalog already.


This has not been tested sufficiently, of course use at own risk!




This tool is example code to show you how you could work more efficiently. It is not supported by Autodesk and you use it at own risk:

// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.

// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF

// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.

// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE

// UNINTERRUPTED OR ERROR FREE.
