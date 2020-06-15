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
                m_Sgworld = new SGWorld71();    //Terra Explorer 

                symbolProviderService = new MssSymbolProviderServiceGS();   //GSS Symbol Provider

                string exePath = Application.ExecutablePath;
                string license = symbolProviderService.GetDefaultLicense(exePath);
                symbolProviderService.InitLicense(license);
                //symbolProviderService.OwnerHandle = Handle.ToInt32();     //works without handle                                  

                string libName = symbolProviderService.GetDefaultLibrary(exePath);
                symbolProvider = symbolProviderService.LoadLibrary(libName, 100);
                symbolProvider.DefaultRenderingOptions.RenderingCanvasType = TMssRenderingCanvasTypeGS.mssRenderingCanvasTypeDirect2dGS;
                symbolProvider.SupportedModifiersEx[TMssWorkModeGS.mssWorkModeExtendedGS].Modifiers_Base &= ~TMssModifiers_BaseGS.mssModifierN_HostileGS;

                symbolFormat = symbolProviderService.CreateFormatObj();
                symbolFormat.SymbolSize = 60;
                symbolFormat.WorkMode = TMssWorkModeGS.mssWorkModeExtendedGS;

            }
            catch (Exception E)
            {
                MessageBox.Show("Error loading MSS Server: " + E.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        public bool CreateMssObject(string mssString, double xCoord, double yCoord)
        {
            if (mssString != null && mssString != String.Empty)
            {
                MssStringObjGS mssStringObj = symbolProviderService.CreateMssStringObjStr(mssString);     //create mss string object from mss xml string
                IMssSymbolGraphicGS symbolGraphic = symbolProvider.CreateSymbolGraphic(mssStringObj, symbolFormat);    //create symbol graphic
                if ((symbolGraphic != null) && symbolGraphic.IsValid)
                {
                    //Bitmap symbolBmp = GraphicExtensions.CreateBitmap(symbolGraphic, Color.White);       //get Bitmap programmatically
                    symbolGraphic.ExportToFile(@"C:\mss_bitmaps\symbol.jpg");    //exprot to file on hard drive
                }

                IPosition71 position = m_Sgworld.Creator.CreatePosition(xCoord, yCoord, 0, AltitudeTypeCode.ATC_ON_TERRAIN, 0, 0, 0, 5000);    //AltitudeTypeCode.ATC_ON_TERRAIN doesn't work
                ITerrainImageLabel71 terrainImageLabel71 = m_Sgworld.Creator.CreateImageLabel(position, @"C:\mss_bitmaps\symbol.jpg", null, "", "mss symbol"); //add "mss symbol" under root in TE tree
                m_Sgworld.Navigate.FlyTo(position, ActionCode.AC_FLYTO);

                return true;
            }
            else
                return false;
        }
    }
}
