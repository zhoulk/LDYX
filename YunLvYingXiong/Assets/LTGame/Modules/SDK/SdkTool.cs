/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Sdk工具集实现
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System;

namespace LTGame.SDK
{
    public class SdkTool : ISdkTool
    {
        private AndroidJavaClass jc;
        private Action<PaymentResult> paymentResultCallback;

        public SdkTool()
        {
            App.On<string>(EventName.PaymentResult, OnPay, this);

            try
            {
                jc = new AndroidJavaClass("org.unity.UnityAPIBridge");
            }
            catch (Exception)
            {

            }
        }

        public SdkTool(AndroidJavaClass jc)
        {
            this.jc = jc;
        }

        public string GetDownloadQRCodePath()
        {
            if (jc == null)
                return "";
            else
                return jc.CallStatic<string>("getDownLoadHandQRPath", 188, 188, "QRcode.png");
        }

        public string GetLinkQRCodePath()
        {
            if (jc == null)
                return "";
            else
                return jc.CallStatic<string>("getQRcodePathByIP", 188, 188, "QRcode.png");
        }

        public void Pay(string payid)
        {
            jc?.CallStatic("payment", payid, "");
        }

        public void Pay(string payid, Action<PaymentResult> action)
        {
            paymentResultCallback = action;

            if (jc == null)
            {
                //如果未接sdk，则模拟一个支付成功的结果
                PaymentResult result = new PaymentResult();
                result.code = "100";
                result.payid = payid;
                result.text = "test data.";
                paymentResultCallback?.Invoke(result);
            }
            else
            {
                jc.CallStatic("payment", payid, "");
            }
        }

        //支付回调
        private void OnPay(string buf)
        {
            PaymentResult result = JsonUtility.FromJson<PaymentResult>(buf);
            paymentResultCallback?.Invoke(result);
        }
    }
}
