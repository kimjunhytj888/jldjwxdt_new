using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// 用户数据加密方法类Encrypt
/// </summary>
public class Encrypt 
{
    public Encrypt()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 数据加密
    /// </summary>
    /// <param name="pToEncrypt"></param>
    /// <returns></returns>
    public string JiaMi(string pToEncrypt)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();  ////把字符串放到byte数组中

        byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
        ////byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);

        des.Key = ASCIIEncoding.ASCII.GetBytes("wx_mobis");  ////建立加密对象的密钥和偏移量
        des.IV = ASCIIEncoding.ASCII.GetBytes("wx_mobis");   ////原文使用ASCIIEncoding.ASCII方法的GetBytes方法
        MemoryStream ms = new MemoryStream();     ////使得输入密码必须输入英文文本
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();

        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        ret.ToString();
        return ret.ToString();
    }

    /// <summary>
    /// 数据解密
    /// </summary>
    /// <param name="pToDecrypt"></param>
    /// <returns></returns>
    public string JieMi(string pToDecrypt)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
        for (int x = 0; x < pToDecrypt.Length / 2; x++)
        {
            int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
            inputByteArray[x] = (byte)i;
        }

        des.Key = ASCIIEncoding.ASCII.GetBytes("wx_mobis");////建立加密对象的密钥和偏移量，此值重要，不能修改
        des.IV = ASCIIEncoding.ASCII.GetBytes("wx_mobis");
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();

        StringBuilder ret = new StringBuilder();////建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象

        return System.Text.Encoding.Default.GetString(ms.ToArray());
    }
}
