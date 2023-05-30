# SupportHelperLight


(Sample code, use at own risk):

you need to create a string property called "ParameterExtract" for your supports.

compile the code to get SupportsUtils.dll, put it into C:\Program Files\Autodesk\AutoCAD 20XX\PLNT3D

It has to be loaded with “netload”

The available commands from this code are: 
"exposeParams" : put supports design parameters in the "ParameterExtract" field


The script will loop through all supports in the drawing and copies the design parameter to the Property field: “ParameterExtract”.

Tipp: Use e.g. H=0,L=0 in the DesignParams field to just expose only those two parameters, do it in the catalog already.



This tool is example code to show you how you could work more efficiently. It is not supported by Autodesk and you use it at own risk.


