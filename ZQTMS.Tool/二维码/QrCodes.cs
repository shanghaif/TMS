//using Gma.QrCodeNet.Encoding;
//using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;

namespace ZQTMS.Tool
{
     public static class QrCodes
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="qrCodeString"></param>
        public static void qrCodeToConsole(string qrCodeString)
        {

            //QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            //QrCode qrCode = qrEncoder.Encode(qrCodeString);
            //输出二维码到控制台
            //for (int j = 0; j < qrCode.Matrix.Width; j++)
            //{
            //    for (int i = 0; i < qrCode.Matrix.Width; i++)
            //    {

            //        char charToPrint = qrCode.Matrix[i, j] ? '　' : '█';
            //        Console.Write(charToPrint);
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine(@"Press any key to quit.");
            //const int moduleSizeInPixels = 5;
            //GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(moduleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            //Panel panel = new Panel();
            //Point padding = new Point(10, 16);
            //DrawingSize qrCodeSize = renderer.SizeCalculator.GetSize(qrCode.Matrix.Width);
            //panel.AutoSize = false;
            //panel.Size = new Size(qrCodeSize.CodeWidth, qrCodeSize.CodeWidth) + new Size(2 * padding.X, 2 * padding.Y);
            //using (Graphics graphics = panel.CreateGraphics())
            //{
            //    renderer.Draw(graphics, qrCode.Matrix, padding);
            //}

        }

        /// <summary>
        /// 保存二维码
        /// </summary>
        /// <param name="qrCodeString"></param>
        /// <param name="imageFileName"></param>
        /// <param name="ZoomMulti"></param>
        public static void qrCodeToImageFile(string qrCodeString, string imageFileName, int ZoomMulti)
        {
            //Bitmap bmp = GenerateQRCode(qrCodeString, Color.Black, Color.White, ZoomMulti);
            //bmp.Save(imageFileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        public static Bitmap GenerateQRCode(string text, Color DarkColor, Color LightColor, int ZoomMulti)
        {
        //    QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);

        //    QrCode Code = qrEncoder.Encode(text);

        //    Bitmap TempBMP = new Bitmap(Code.Matrix.Width * ZoomMulti, Code.Matrix.Height * ZoomMulti);
        //    for (int X = 0; X <= Code.Matrix.Width * ZoomMulti - 1; X++)
        //    {
        //        for (int Y = 0; Y <= Code.Matrix.Height * ZoomMulti - 1; Y++)
        //        {
        //            if (Code.Matrix.InternalArray[X / ZoomMulti, Y / ZoomMulti])
        //                TempBMP.SetPixel(X, Y, DarkColor);
        //            else
        //                TempBMP.SetPixel(X, Y, LightColor);
        //        }
        //    }
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 0;//  ZB 20200316
            Bitmap bt = qrCodeEncoder.Encode(text, Encoding.UTF8);
            return bt;
        }
    }

}
