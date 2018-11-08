using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
using WindowsFormsApp1.sdk;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string appId;
        string sdkKey;
        IntPtr detectEngine;

        int code;

        AFD_FSDK_FACERES faceRes;
        public Form1()
        {
            InitializeComponent();

            appId = "AGvbFFH283KtytZJV8jvtsbWUcNj5g9rHRrErKBzjB7f";
            sdkKey = "GTjHQgie1rVfpkFMPSUYq1LqFyKbzJPKj3wXgWZLYixe";
            detectEngine = IntPtr.Zero;
            code = Init.ArcSoft_FIC_InitialEngine(appId, sdkKey, ref detectEngine);
        }

        private byte[] readBmp(Bitmap image, ref int width, ref int height, ref int pitch)
        {

            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
            IntPtr ptr = data.Scan0;
            //定义数组长度
            int soureBitArrayLength = data.Height * Math.Abs(data.Stride);

            byte[] sourceBitArray = new byte[soureBitArrayLength];

            //将bitmap中的内容拷贝到ptr_bgr数组中
            Marshal.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);

            width = data.Width;

            height = data.Height;

            pitch = Math.Abs(data.Stride);

            int line = width * 3;

            int bgr_len = line * height;

            byte[] destBitArray = new byte[bgr_len];

            for (int i = 0; i < height; ++i)
            {
                Array.Copy(sourceBitArray, i * pitch, destBitArray, i * line, line);
            }

            pitch = line;

            image.UnlockBits(data);

            return destBitArray;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int width = 0;

            int height = 0;

            int pitch = 0;
            OpenFileDialog op1 = new OpenFileDialog();
            if (op1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation=op1.FileName;
            }

            Bitmap bitmap = new Bitmap(pictureBox1.ImageLocation);


            // IntPtr imageDataPtr= bitmap.GetHbitmap();

            byte[] imageData = readBmp(bitmap, ref width, ref height, ref pitch);

            IntPtr imageDataPtr = Marshal.AllocHGlobal(imageData.Length);

            Marshal.Copy(imageData, 0, imageDataPtr, imageData.Length);

            ASVLOFFSCREEN offInput = new ASVLOFFSCREEN();

            offInput.u32PixelArrayFormat = 513;

            offInput.ppu8Plane = new IntPtr[4];

            offInput.ppu8Plane[0] = imageDataPtr;

            offInput.i32Width = width;

            offInput.i32Height = height;

            offInput.pi32Pitch = new int[4];

            offInput.pi32Pitch[0] = pitch;

            faceRes = new AFD_FSDK_FACERES();

            IntPtr offInputPtr = Marshal.AllocHGlobal(Marshal.SizeOf(offInput));

            Marshal.StructureToPtr(offInput, offInputPtr, false);

            IntPtr faceResPtr = Marshal.AllocHGlobal(Marshal.SizeOf(faceRes));
            code = Init.ArcSoft_FIC_FaceDataFeatureExtraction(detectEngine, 0, offInputPtr, ref faceResPtr);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int width = 0;

            int height = 0;

            int pitch = 0;
            OpenFileDialog op2 = new OpenFileDialog();
            if (op2.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = op2.FileName;
            }
            Image img2 = Image.FromFile(pictureBox2.ImageLocation);
            Bitmap bitmap = new Bitmap(pictureBox2.ImageLocation);


            // IntPtr imageDataPtr= bitmap.GetHbitmap();

            byte[] imageData = readBmp(bitmap, ref width, ref height, ref pitch);

            IntPtr imageDataPtr = Marshal.AllocHGlobal(imageData.Length);

            Marshal.Copy(imageData, 0, imageDataPtr, imageData.Length);

            ASVLOFFSCREEN offInput = new ASVLOFFSCREEN();

            offInput.u32PixelArrayFormat = 513;

            offInput.ppu8Plane = new IntPtr[4];

            offInput.ppu8Plane[0] = imageDataPtr;

            offInput.i32Width = width;

            offInput.i32Height = height;

            offInput.pi32Pitch = new int[4];

            offInput.pi32Pitch[0] = pitch;

            faceRes = new AFD_FSDK_FACERES();

            IntPtr offInputPtr = Marshal.AllocHGlobal(Marshal.SizeOf(offInput));

            Marshal.StructureToPtr(offInput, offInputPtr, false);

            code = Init.ArcSoft_FIC_IdCardDataFeatureExtraction(detectEngine, offInputPtr);
        }

        private void btn_compare_Click(object sender, EventArgs e)
        {
            float res=0f;
            int result = 0;
            int cc = Init.ArcSoft_FIC_FaceIdCardCompare(detectEngine, 0.8f, ref res, ref result);

            lb_SimilarScore.Text = (res * 100).ToString() + "%";
            lb_result.Text = result.ToString();
        }
    }
}
