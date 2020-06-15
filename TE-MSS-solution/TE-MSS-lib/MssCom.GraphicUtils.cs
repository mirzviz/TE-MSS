/*
 * Extensions for MSS COM API
 * gs-soft Visual Component Library
 * Copyright (c) 1986-2020 by gs-soft
 * All rights reserved.
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Interop.MssComServer;

namespace gssoft.Mss.Core.MssCom.GraphicUtils
{
    public static class GraphicExtensions
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        
        public static Bitmap CreateBitmapEx(IMssSymbolGraphicGS graphic, Color bkColor, int width, int height)
        {
            Bitmap result = null;
            IntPtr hbitmap;
            uint bkColorInt = (uint)ColorTranslator.ToOle(bkColor);
            if ((width <= 0) && (height <= 0))
                hbitmap = new IntPtr(graphic.CreateBitmap(bkColorInt));
            else
                hbitmap = new IntPtr(graphic.CreateBitmapEx(width, height, true, bkColorInt));
            if (hbitmap != null)
            {
                try
                {
                    result = Bitmap.FromHbitmap(hbitmap);
                    result.MakeTransparent(bkColor);
                }
                finally
                {
                    DeleteObject(hbitmap);
                }
            }
            return result;
        }

        public static Bitmap CreateBitmap(IMssSymbolGraphicGS graphic, Color bkColor)
        {
            return CreateBitmapEx(graphic, bkColor, 0, 0);
        }
        public static Bitmap CreateBitmap(IMssSymbolGraphicGS graphic, Size size)
        {
            return CreateBitmapEx(graphic, Color.Transparent, size.Width, size.Height);
        }

        public static MemoryStream CreateImageStream(IMssSymbolGraphicGS graphic, ImageFormat imageFormat, int width, int height)
        {
            Bitmap bitmap = CreateBitmapEx(graphic, Color.White, width, height);
            MemoryStream imageStream = new MemoryStream();
            bitmap.Save(imageStream, imageFormat);
            return imageStream;
        }

        public static MemoryStream CreateImageStream(IMssSymbolGraphicGS graphic, ImageFormat imageFormat)
        {
            return CreateImageStream(graphic, imageFormat, 0, 0);
        }

        public static Image CreateMetafile(IMssSymbolGraphicGS graphic, Size size)
        {
            IntPtr hMetafile = new IntPtr(graphic.CreateMetafileEx(true));
            Metafile metafile = new Metafile(hMetafile, true);
            try
            {
                Rectangle destRect = new Rectangle(0, 0, size.Width, size.Height);

                if (size.Width / size.Height > metafile.Width / metafile.Height)
                    destRect.Width = (destRect.Height * metafile.Width) / metafile.Height;
                else
                    destRect.Height = (destRect.Width * metafile.Height) / metafile.Width;

                Graphics screen = Graphics.FromHwnd(IntPtr.Zero);
                Metafile sizedMeta = new Metafile(screen.GetHdc(), EmfType.EmfPlusOnly);
                try
                {
                    Graphics graphics = Graphics.FromImage(sizedMeta);
                    graphics.DrawImage(metafile, destRect);
                    graphics.Dispose();
                }
                finally
                {
                    screen.ReleaseHdc();
                    screen.Dispose();
                }

                return sizedMeta;
            }
            finally
            {
                metafile.Dispose();
            }
        }
    }
}
