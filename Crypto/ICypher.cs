using System;
namespace Crypto
{
    public interface ICypher
    {
        byte[] Encrypt(string content);
        string Decrypt(byte[] content);
    }
}
