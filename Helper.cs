//
//////////////////////////////////////////////////////////////////////////////
//
//  Copyright 2015 Autodesk, Inc.  All rights reserved.
//
//  Use of this software is subject to the terms of the Autodesk license 
//  agreement provided at the time of installation or download, or which 
//  otherwise accompanies this software in either electronic or hard copy form.   
//
//////////////////////////////////////////////////////////////////////////////
// if just one type of hose exists, shortdescription should be "HOSE"


using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;


using Autodesk.ProcessPower.DataLinks;
using Autodesk.ProcessPower.ProjectManager;
using Autodesk.ProcessPower.PlantInstance;
using Autodesk.AutoCAD.EditorInput;

using System;
using PlantApp = Autodesk.ProcessPower.PlantInstance.PlantApplication;

using System.Collections.Generic;
using Autodesk.ProcessPower.PnP3dObjects;
using Autodesk.ProcessPower.DataObjects;
using Autodesk.ProcessPower.PartsRepository.Specification;
using Autodesk.ProcessPower.P3dUI;
using Autodesk.ProcessPower.PartsRepository;
using System.IO;
using Autodesk.ProcessPower.PnP3dDataLinks;

namespace SupportsUtils
{
    /// <summary>
    /// Helper class including some static helper functions.
    /// </summary>
    /// 


    public class Helper
    {
        public static Project currentProject { get; set; }
        public static Document ActiveDocument { get; set; }
        public static DataLinksManager ActiveDataLinksManager { get; set; }
        public static DataLinksManager3d dlm3d { get; set; }
        public static Database db { get; set; }
        public static Editor ed { get; set; }
        public static PipingObjectAdder pipeObjAdder { get; set; }


        public static bool Initialize()
        {
            if (PlantApplication.CurrentProject == null)
                return false;


            currentProject = PlantApp.CurrentProject.ProjectParts["Piping"];
            ActiveDataLinksManager = currentProject.DataLinksManager;
            dlm3d = DataLinksManager3d.Get3dManager(ActiveDataLinksManager);
            pipeObjAdder = new PipingObjectAdder(dlm3d, db);
            ActiveDocument = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            db = ActiveDocument.Database;
            ed = ActiveDocument.Editor;
            return true;
        }

        public static void Terminate()
        {
            currentProject = null;
            ActiveDataLinksManager = null;
            ActiveDocument = null;
            db = null;
            ed = null;
        }

        public static PnPRow getPartSpecRow(PipePartSpecification pps, NominalDiameter nd, String shortdesc)
        {
            if (pps == null)
                return null;
            PnPTable table = pps.Database.Tables["EngineeringItems"];
            if (table != null)
            {
                String query = "\"NominalDiameter\"=" + nd.Value;
                //query += " and \"NominalUnit\"='" + part.PartSizeProperties.NominalDiameter.Units + "'";
                //query += " and \"PartCategory\"='" + part.PartSizeProperties.PropValue("PartCategory") + "'";
                //query += " and \"ShortDescription\"='" + shortdesc + "'";
                query += " and \"ItemCode\" = '" + shortdesc + "'";
                //query += " and \"ContentGeometryParamDefinition\" like '" + param + "'";
                //query += " and not \"ContentGeometryParamDefinition\" like '" + param + ",%'";

                try
                {
                    PnPRow[] r = table.Select(query);
                    if(!shortdesc.Equals(""))
                        Helper.ed.WriteMessage("\nfound in spec[itemcode]: " + r.Length.ToString());
                    if (r.Length > 0)
                    {

                        return r[0];
                    }

                }
                catch (Exception e)
                {
                    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                    ed.WriteMessage(trace.ToString());
                    ed.WriteMessage("Line: " + trace.GetFrame(0).GetFileLineNumber());
                    ed.WriteMessage("message: " + e.Message);
                }



                /*catch (Autodesk.ProcessPower.DataObjects.Expression.PnPExpressionException e)
                {
                    oEditor.WriteMessage(e.Message);
                }
                catch (System.Runtime.InteropServices.SEHException e)
                {
                    oEditor.WriteMessage(e.Message);
                }*/
            }
            return null;
        }

        public static PipePartSpecification getPipePartSpec(String alternatepath)
        {

            UISettings sett = new UISettings();
            String specName = sett.CurrentSpec;
            PipePartSpecification cachePPS = null;
            PlantProject currentProj = PlantApplication.CurrentProject;
            String pathSpec = currentProj.ProjectFolderPath + "\\Spec Sheets\\" + specName + ".pspx";

            if (!File.Exists(pathSpec)) pathSpec = alternatepath;

            ed.WriteMessage("Using spec file: " + pathSpec + "\n");

            try
            {
                PipePartSpecification pps = PipePartSpecification.OpenSpecification(pathSpec);
                return cachePPS = pps;
            }
            catch (System.Exception e)
            {
                ed.WriteMessage("Error on open " + pathSpec);
                return null;
            }

        }

        public static IDictionary<string, double> ReadInDict(string thestring, bool fillvalues)
        {
            string whole_file = thestring;

            IDictionary<string, double> dict = new Dictionary<string, double>();

            // Split into lines.
            string[] lines = whole_file.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;

            // Load the dictionary.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(new char[] { '=' });
                double theout = 0.0;
                if (fillvalues && Double.TryParse(line_r[1].Trim(), out theout))
                    dict[line_r[0]] = theout; // Convert.ToDouble(line_r[1]);
                else
                    dict[line_r[0]] = 0.0;
            }

            // Return the values.
            return dict;
        }

    }

    // Helper class to workaround a Hashtable issue: 
    // Can't change values in a foreach loop or enumerator
    class CBoolClass
    {
        public CBoolClass(bool val) { this.val = val; }
        public bool val;
        public override string ToString() { return (val.ToString()); }
    }
}


