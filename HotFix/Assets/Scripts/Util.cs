using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class Util
{
    public static string GetMd5(string filePath)
    {
        StringBuilder sb = new StringBuilder();
        filePath = filePath.Trim();
        FileStream fs = new FileStream(filePath,FileMode.Open);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] result = md5.ComputeHash(fs);

        for(int i = 0; i < result.Length; i++)
        {
            sb.Append(result[i].ToString("x2")); //x2表示输出按照16进制 并且2位对齐输出
        }
        return sb.ToString();
    }

}

