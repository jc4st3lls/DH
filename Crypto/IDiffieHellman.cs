using System;
namespace Crypto
{
    public interface IDiffieHellman
    {
        DHKeyPair GenerateKeyPair();
        DHDerivedKey GenrateDerivedKey(DHKeyPair from, PublicKey to);
    }
    public abstract class Key
    {
        public byte[] Value { get; protected set; }
    }

    public class DHDerivedKey : Key
    {
        public DHDerivedKey(byte[] value)
        {
            Value = value;
        }
    }

    public class PrivateKey : Key
    {
        public PrivateKey(byte[] value)
        {
            Value = value;
        }
    }


    public class PublicKey : Key
    {
        public PublicKey(byte[] value)
        {
            Value = value;
        }
    }
    

    public class DHKeyPair
    {
        public DHKeyPair(PrivateKey privateKey, PublicKey publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
        public PublicKey PublicKey { get; private set; }
        public PrivateKey PrivateKey { get; private set; }

    }

   
}
