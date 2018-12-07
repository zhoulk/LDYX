/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：由Sdk 提供的工具集 接口,具体包括支付，二维码扫码等功能
 * 
 * ------------------------------------------------------------------------------*/
using System;

namespace LTGame.SDK
{

    public interface ISdkTool
    {
        /// <summary>
        /// 获取提供下载功能的二维码图片路径
        /// </summary>
        /// <returns></returns>
        string GetDownloadQRCodePath();

        /// <summary>
        /// 获取提供扫码链接功能的二维码图片路径
        /// </summary>
        /// <returns></returns>
        string GetLinkQRCodePath();

        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="payid"> 计费id </param>
        [Obsolete("Pay is Obsolete,Use Pay(string payid, Action<PayResult> action)")]
        void Pay(string payid);

        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="payid">计费id </param>
        /// <param name="action"> 支付结果回调</param>
        void Pay(string payid, Action<PaymentResult> action);
    }

}