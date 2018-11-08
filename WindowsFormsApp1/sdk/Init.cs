using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.sdk
{
    public class Init
    {
        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="sdkKey">sdkkey</param>
        /// <param name="Engine"></param>
        /// <returns></returns>
        [DllImport(@"sdk\libarcsoft_fsdk_fic.dll")]
        public static extern int ArcSoft_FIC_InitialEngine(string appId, string sdkKey, ref IntPtr Engine);

        /// <summary>
        /// 获取脸部特征
        /// </summary>
        /// <param name="pEngine">引擎 Handle</param>
        /// <param name="isVideo">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="faceData">人脸图像原始数据</param>
        /// <param name="pFaceRes">人脸属性 人脸数/人脸框</param>
        /// <returns></returns>
        [DllImport(@"sdk\libarcsoft_fsdk_fic.dll")]
        public static extern int ArcSoft_FIC_FaceDataFeatureExtraction(IntPtr pEngine, int isVideo, IntPtr faceData, ref IntPtr pFaceRes);

        /// <summary>
        /// 获取证件照脸部特征
        /// </summary>
        /// <param name="pEngine">引擎 Handle</param>
        /// <param name="faceData">人脸图像原始数据</param>
        /// <returns></returns>
        [DllImport(@"sdk\libarcsoft_fsdk_fic.dll")]
        public static extern int ArcSoft_FIC_IdCardDataFeatureExtraction(IntPtr pEngine, IntPtr faceData);

        /// <summary>
        /// 人证比对
        /// </summary>
        /// <param name="pEngine">引擎 Handle</param>
        /// <param name="threshold">比对阈值</param>
        /// <param name="SimilarScore">比对相似度</param>
        /// <param name="result">比对结果</param>
        /// <returns></returns>
        [DllImport(@"sdk\libarcsoft_fsdk_fic.dll")]
        public static extern int ArcSoft_FIC_FaceIdCardCompare(IntPtr pEngine, float threshold, ref float SimilarScore, ref int result);

        /// <summary>
        /// 销毁引擎
        /// </summary>
        /// <param name="pEngine">引擎 Handle</param>
        /// <returns></returns>
        [DllImport(@"sdk\libarcsoft_fsdk_fic.dll")]
        public static extern int ArcSoft_FIC_UninitialEngine(IntPtr pEngine);
    }
}
