using System;
using System.Security.Cryptography;
namespace Crypto
{
    public class DiffieHellman:IDiffieHellman
    {

     

        public DHKeyPair GenerateKeyPair()
        {
            DHKeyPair ret = null;

            using (ECDiffieHellmanCng alias = new ECDiffieHellmanCng())
            {

                alias.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                alias.HashAlgorithm = CngAlgorithm.Sha256;

                ret = new DHKeyPair(new PrivateKey(alias.ExportECPrivateKey()),
                    new PublicKey(alias.PublicKey.ToByteArray()));

                

            }

            return ret;
        }

        public DHDerivedKey GenrateDerivedKey(DHKeyPair from, PublicKey to)
        {
            throw new NotImplementedException();
        }
    }
}
