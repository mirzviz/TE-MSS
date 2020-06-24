using System;
using System.Text;
using System.Threading.Tasks;
using TerraExplorerX;
using Interop.MssComServer;
using System.Windows.Forms;
using gssoft.Mss.Core.MssCom.GraphicUtils;
using System.Drawing;
using System.Runtime.Serialization.Formatters;

namespace TE_MSS
{
    public class TE_MSS_impl : ITE_MSS
    {
        private SGWorld71 m_Sgworld;

        private MssSymbolFormatGS symbolFormat;
        private MssSymbolProviderGS symbolProvider;
        private MssSymbolProviderServiceGS symbolProviderService;

        /* 
         * inspired by MSS DEMO DisplayMssSymbol(1)
         * 1. create SGWorld and MssSymbolProviderService
         * 2. license mss using exePath
         * 3. load mss library
         * 4. create symbolFormat
         */
        public void initializeTEMss()
        {
            try
            {
                //Terra Explorer 
                m_Sgworld = new SGWorld71();
                //GSS Symbol Provider
                symbolProviderService = new MssSymbolProviderServiceGS();   
                //get exe path for license
                string exePath = Application.ExecutablePath;
                /*string license = symbolProviderService.GetDefaultLicense(exePath);*/
                string license = "IIT_TESTKEY.2021.12.31_8A3EFBEA95A633A03F69840E0C0CC9FC023A9AEA";
                symbolProviderService.InitLicense(license);
                //for form applications:
                //symbolProviderService.OwnerHandle = Handle.ToInt32();                                     
                //sybol provider
                string libName = symbolProviderService.GetDefaultLibrary(exePath);
                symbolProvider = symbolProviderService.LoadLibrary(libName, 100);
                symbolProvider.DefaultRenderingOptions.RenderingCanvasType = TMssRenderingCanvasTypeGS.mssRenderingCanvasTypeDirect2dGS;
                symbolProvider.SupportedModifiersEx[TMssWorkModeGS.mssWorkModeExtendedGS].Modifiers_Base &= ~TMssModifiers_BaseGS.mssModifierN_HostileGS;
                //synbol format
                symbolFormat = symbolProviderService.CreateFormatObj();
                symbolFormat.SymbolSize = 60;
                symbolFormat.WorkMode = TMssWorkModeGS.mssWorkModeExtendedGS;

            }
            catch (Exception E)
            {
                Console.WriteLine("Error loading MSS Server: " + E.Message + "Error");
                Environment.Exit(1);
            }
        }

        public bool CreateNPointObject(string mssString, double xCoord, double yCoord)
        {
            if (mssString != null && mssString != String.Empty)
            {
                //create mss string object from mss xml string
                MssStringObjGS mssStringObj = symbolProviderService.CreateMssStringObjStr(mssString);
                //create NPointGraphicTemplate
                MssNPointGraphicTemplateGS mssNPointGraphicTemplateGS = symbolProvider.CreateNPointGraphic(mssStringObj, symbolFormat);
                
                //create MssSymbolGraphicGS
                TMssPointGS[] points = new TMssPointGS[] {
                                                             new TMssPointGS() { X = 0, Y = 0},
                                                             new TMssPointGS() { X = 100, Y =  0},
                                                              new TMssPointGS() { X = 100, Y = 100},
                                                              new TMssPointGS() { X = 0, Y = 100 }
                                                            };

                //MssSymbolGraphicGS mssSymbolGraphicGS = mssNPointGraphicTemplateGS.RenderImage(points, points.Length, 50);

                TMssRectGS visibleRect = new TMssRectGS();
                visibleRect.Bottom = 0;
                visibleRect.Top = 0;
                visibleRect.Left = 0;
                visibleRect.Right = 0;

                MssSymbolGraphicGS mssSymbolGraphicGS = mssNPointGraphicTemplateGS.RenderImageEx(points, points.Length, visibleRect, 50);

                if (mssSymbolGraphicGS != null)
                {
                    if (mssSymbolGraphicGS.IsValid)
                    {
                        //get Bitmap programmatically
                        //Bitmap symbolBmp = GraphicExtensions.CreateBitmap(symbolGraphic, Color.White);       
                        //exprot to file on hard drive
                        /*TMssModifiersGS assignedDisplayableModifiers = mssSymbolGraphicGS.AssignedDisplayableModifiers;*/
                        mssSymbolGraphicGS.ExportToFile(@"C:\mss_bitmaps\NPointSymbolEx.jpg");
                    }
                    else
                    {
                        Console.WriteLine(mssSymbolGraphicGS.ErrorMsg);
                        throw new Exception("error, read output");
                    }
                }

                //TODO: AltitudeTypeCode.ATC_ON_TERRAIN doesn't work 
                IPosition71 position = m_Sgworld.Creator.CreatePosition(xCoord, yCoord, 0, AltitudeTypeCode.ATC_ON_TERRAIN, 0, 0, 0, 5000);
                ITerrainImageLabel71 terrainImageLabel71 = m_Sgworld.Creator.CreateImageLabel(position, @"C:\mss_bitmaps\NPointSymbolEx.jpg", null, "", "mss symbol"); //add "mss symbol" under root in TE tree
                m_Sgworld.Navigate.FlyTo(position, ActionCode.AC_FLYTO);

                return true;
            }
            else
                return false;
        }

        public bool CreateMssObject(string mssString, double xCoord, double yCoord)
        {
            if (mssString != null && mssString != String.Empty)
            {
                //create mss string object from mss xml string
                MssStringObjGS mssStringObj = symbolProviderService.CreateMssStringObjStr(mssString);
                //create symbol graphic
                IMssSymbolGraphicGS symbolGraphic = symbolProvider.CreateSymbolGraphic(mssStringObj, symbolFormat);
                MssNPointGraphicTemplateGS mssNPointGraphicTemplateGS = symbolProvider.CreateNPointGraphic(mssStringObj, symbolFormat);
                TMssPointGS[] points = new TMssPointGS[] { 
                                                            new TMssPointGS() { X = 80, Y = 90 },
                                                            new TMssPointGS() { X = 300, Y = 340 } 
                                                            };

                MssSymbolGraphicGS mssSymbolGraphicGS = mssNPointGraphicTemplateGS.RenderImage(points, 2, 1);
                if ((symbolGraphic != null) && symbolGraphic.IsValid)
                {
                    //get Bitmap programmatically
                    //Bitmap symbolBmp = GraphicExtensions.CreateBitmap(symbolGraphic, Color.White);       
                    //exprot to file on hard drive
                    symbolGraphic.ExportToFile(@"C:\mss_bitmaps\symbol.jpg");    
                }
                //TODO: AltitudeTypeCode.ATC_ON_TERRAIN doesn't work 
                IPosition71 position = m_Sgworld.Creator.CreatePosition(xCoord, yCoord, 0, AltitudeTypeCode.ATC_ON_TERRAIN, 0, 0, 0, 5000);    
                ITerrainImageLabel71 terrainImageLabel71 = m_Sgworld.Creator.CreateImageLabel(position, @"C:\mss_bitmaps\symbol.jpg", null, "", "mss symbol"); //add "mss symbol" under root in TE tree
                m_Sgworld.Navigate.FlyTo(position, ActionCode.AC_FLYTO);

                return true;
            }
            else
                return false;
        }

        public bool CreateMssObjectWithInsertPoint(string mssString, double xCoord, double yCoord)
        {
            if (mssString != null && mssString != String.Empty)
            {
                //create mss string object from mss xml string
                MssStringObjGS mssStringObj = symbolProviderService.CreateMssStringObjStr(mssString);
                //create symbol graphic
                IMssSymbolGraphicGS symbolGraphic = symbolProvider.CreateSymbolGraphic(mssStringObj, symbolFormat);
                if ((symbolGraphic != null) && symbolGraphic.IsValid)
                {
                    //get Bitmap programmatically
                    Bitmap symbolBmp = GraphicExtensions.CreateBitmap(symbolGraphic, Color.White);
                    using (Graphics graphics = Graphics.FromImage(symbolBmp))
                    {
                        DrawMarker(symbolGraphic.InsertPointOffsetX, symbolGraphic.InsertPointOffsetY, graphics);
                    }
                    symbolBmp.Save(@"C:\mss_bitmaps\symbol.jpg");
                }
                //TODO: AltitudeTypeCode.ATC_ON_TERRAIN doesn't work 
                IPosition71 position = m_Sgworld.Creator.CreatePosition(xCoord, yCoord, 0, AltitudeTypeCode.ATC_ON_TERRAIN, 0, 0, 0, 5000);
                ITerrainImageLabel71 terrainImageLabel71 = m_Sgworld.Creator.CreateImageLabel(position, @"C:\mss_bitmaps\symbol.jpg", null, "", "mss symbol"); //add "mss symbol" under root in TE tree
                m_Sgworld.Navigate.FlyTo(position, ActionCode.AC_FLYTO);

                return true;
            }
            else
                return false;
        }

        private static void DrawMarker(int markerPointX, int markerPointY, Graphics graphics)
        {
            const int insertPointMarkerHalfSize = 2;
            Rectangle markerRect = Rectangle.FromLTRB(
                markerPointX - insertPointMarkerHalfSize,
                markerPointY - insertPointMarkerHalfSize,
                markerPointX + insertPointMarkerHalfSize - 1,
                markerPointY + insertPointMarkerHalfSize - 1);

            using (Brush brush = new SolidBrush(Color.Red))
            {
                graphics.FillRectangle(brush, markerRect);
            }
            using (Pen pen = new Pen(Color.Maroon))
            {
                graphics.DrawRectangle(pen, markerRect);
            }
        }
    }
}
