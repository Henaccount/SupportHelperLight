#region Namespaces

using System;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

using System.Collections.Specialized;
using System.Collections.Generic;
using Autodesk.ProcessPower.ProjectManager;
using Autodesk.ProcessPower.PlantInstance;
using Autodesk.AutoCAD.Runtime;

#endregion
[assembly: CommandClass(typeof(SupportsUtils.SupportsActions))]


namespace SupportsUtils
{
    public class SupportsActions
    {
        public static int precision = 4;

        [CommandMethod("exposeParams", CommandFlags.UsePickSet)]
        public static void WriteChangeAblesFalse()
        {
            SupportsActions.WriteChangeAbles(false);
        }

        [CommandMethod("exposeParamsAll", CommandFlags.UsePickSet)]
        public static void WriteChangeAblesTrue()
        {
            SupportsActions.WriteChangeAbles(true);
        }

        [CommandMethod("exposeParamsHelp", CommandFlags.UsePickSet)]
        public static void ExposeParamsHelpExec()
        {
            SupportsActions.ExposeParamsHelp();
        }

        public static void ExposeParamsHelp() {
            Helper.Initialize();
            Helper.ed.WriteMessage("\n AUTODESK PROVIDES THIS PROGRAM \"AS IS\" AND WITH ALL FAULTS.");
            Helper.ed.WriteMessage("\n AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF");
            Helper.ed.WriteMessage("\n MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.");
            Helper.ed.WriteMessage("\n DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE");
            Helper.ed.WriteMessage("\n UNINTERRUPTED OR ERROR FREE.");
            Helper.ed.WriteMessage("\ncreate a string property called \"ParameterExtract\" on \"pipe run component\" level (or support level depending on the command)\n");
            Helper.ed.WriteMessage("\nput SupportsUtils.dll into C:\\Program Files\\Autodesk\\AutoCAD 20XX\\PLNT3D\n");
            Helper.ed.WriteMessage("\nload with “netload” or you place the following string in the\n");
            Helper.ed.WriteMessage("\nC:\\Program Files\\Autodesk\\AutoCAD 20XX\\Support\\xx-xx\acad20XXdoc.lsp\n");
            Helper.ed.WriteMessage("\nfor automatic loading (use slashes not backslashes):\n");
            Helper.ed.WriteMessage("\n(command \"_netload\" \"C:/Program Files/Autodesk/AutoCAD 2016/PLNT3D/supportUtils.dll\")\n");
            Helper.ed.WriteMessage("\nThe command is: \"exposeParams\" (just for Supports) or \"exposeParamsAll\" (for supports and pipeinlineassets): \n");
            Helper.ed.WriteMessage("\ncopies the design parameter to the Property field: ParameterExtract.\n");
            Helper.ed.WriteMessage("\nTipp: Use e.g. H=0,L=0 in the ParameterExtract field to just expose those two parameters, do it in the catalog already.\n");
            Helper.Terminate();
        }
        public static String getChangeAbles(String changeAbles, String partParams)
        {

            String updatedChangeAbles = "";
            IDictionary<string, double> changeAblesDict = Helper.ReadInDict(changeAbles, false);
            IDictionary<string, double> partParamsDict = Helper.ReadInDict(partParams, true);


            foreach (var item in partParamsDict)
            {
                if (changeAblesDict.ContainsKey(item.Key))
                {
                    updatedChangeAbles += item.Key + "=" + Converter.DistanceToString(item.Value, DistanceUnitFormat.Current, precision) + ",";
                }

            }

            return updatedChangeAbles.Substring(0, updatedChangeAbles.Length - 1);
        }

        public static void WriteChangeAbles(bool useConfig)
        {

            Helper.Initialize();

            string configstr = "ACPPSUPPORT";

            if (useConfig)
            {
                /*PromptResult pr = Helper.ed.GetString("\nconfiguration string: ");
                if (pr.Status != PromptStatus.OK)
                {
                    Helper.ed.WriteMessage("error reading configuration string\n");
                    return;
                }
                else
                {
                    configstr = pr.StringResult.Replace(" ", "");
                }*/
                configstr = "ACPPPIPE,ACPPSUPPORT,ACPPPIPEINLINEASSET";
            }


            try
            {
                if (!PnPProjectUtils.GetActiveDocumentType().Equals("Piping"))
                {
                    Helper.ed.WriteMessage("\n This tool works only on Plant 3D Piping drawings!");
                    return;
                }

                if (Autodesk.ProcessPower.AcPp3dObjectsUtils.ProjectUnits.CurrentLinearUnit == 1) //1=mm,2=in
                    precision = 1;
                else
                    precision = 4;

                Helper.ed.WriteMessage("\n You use this tool at own risk, for more details please type ExposeParamsHelp");


                using (Transaction tr = Helper.db.TransactionManager.StartTransaction())
                {

                    TypedValue[] filterlist = new TypedValue[1];

                    filterlist[0] = new TypedValue(0, configstr);

                    SelectionFilter filter = new SelectionFilter(filterlist);

                    PromptSelectionResult selRes = Helper.ed.SelectAll(filter);

                    if (selRes.Status != PromptStatus.OK)
                    { return; }

                    ObjectId[] objIdArray = selRes.Value.GetObjectIds();

                    foreach (ObjectId id in objIdArray)
                    {
                        try
                        {
                            StringCollection theKeys = new StringCollection();
                            StringCollection theValues = new StringCollection();
                            Entity ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                            PlantProject currentProj = PlantApplication.CurrentProject;

                            Autodesk.ProcessPower.ACPUtils.ParameterList plist = Autodesk.ProcessPower.PnP3dPipeSupport.SupportHelper.GetSupportParameters(id);
                            String partParams = plist.ToString();

                            String changeAbles = "";
                            theKeys.Add("ParameterExtract");
                            theValues = Helper.ActiveDataLinksManager.GetProperties(id, theKeys, true);
                            changeAbles = theValues[0];

                            String changeAblesUpdated = "";

                            if (changeAbles.Equals(""))
                                changeAbles = partParams;

                            changeAblesUpdated = getChangeAbles(changeAbles, partParams);


                            //write different in part property
                            theKeys = new StringCollection();
                            theValues = new StringCollection();

                            theKeys.Add("ParameterExtract");
                            theValues.Add(changeAblesUpdated);

                            Helper.ActiveDataLinksManager.SetProperties(id, theKeys, theValues);
                        }
                        catch (System.Exception e)
                        {
                            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                            Helper.ed.WriteMessage(trace.ToString());
                            Helper.ed.WriteMessage("\nLine: " + trace.GetFrame(0).GetFileLineNumber());
                            Helper.ed.WriteMessage("\nelement error: " + e.Message);
                        }


                    }

                    tr.Commit();
                }
                Helper.ed.WriteMessage("\n.");
                Helper.ed.WriteMessage("\n.");
                Helper.ed.WriteMessage("\n.");
                Helper.ed.WriteMessage("\n.");
                Helper.ed.WriteMessage("\nScript finished");
            }
            catch (System.Exception e)
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                Helper.ed.WriteMessage(trace.ToString());
                Helper.ed.WriteMessage("\nLine: " + trace.GetFrame(0).GetFileLineNumber());
                Helper.ed.WriteMessage("\nscript error: " + e.Message);
            }
            finally
            {
                Helper.Terminate();
            }
        }


    }




}

