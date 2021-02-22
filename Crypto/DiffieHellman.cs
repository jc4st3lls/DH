using System;
using System.Security.Cryptography;
namespace Crypto
{
    public class DiffieHellman:IDiffieHellman
    {

     

        public DHKeyPair GenerateKeyPair()
        {
            DHKeyPair ret = null;


            using (ECDiffieHellman alias = ECDiffieHellman.Create())
            {
               

                PrivateKey _privKey = new PrivateKey(alias.ExportECPrivateKey());
                PublicKey _pubKey = new PublicKey(alias.ExportSubjectPublicKeyInfo());

                ret = new DHKeyPair(_privKey,_pubKey);


            }



                //using (ECDiffieHellmanCng alias = new ECDiffieHellmanCng())
                //{

                //    alias.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                //    alias.HashAlgorithm = CngAlgorithm.Sha256;

                //    ret = new DHKeyPair(new PrivateKey(alias.ExportECPrivateKey()),
                //        new PublicKey(alias.PublicKey.ToByteArray()));



                //}

            return ret;
        }

        public DHDerivedKey GenerateDerivedKey(DHKeyPair from, PublicKey to)
        {
            DHDerivedKey ret = null;

            using (ECDiffieHellman alias = ECDiffieHellman.Create())
            {
                

                ReadOnlySpan<byte> spriv= new ReadOnlySpan<byte>(from.PrivateKey.Value);
                
                
                alias.ImportECPrivateKey(spriv, out int bytesRead);
                ECDiffieHellmanPublicKey topub;

                using (ECDiffieHellman _tmp = ECDiffieHellman.Create())
                {
                    ReadOnlySpan<byte> topubread = new ReadOnlySpan<byte>(to.Value);
                    _tmp.ImportSubjectPublicKeyInfo(topubread, out bytesRead);

                    topub = _tmp.PublicKey;
                }
              
                byte[] derivedKey = alias.DeriveKeyMaterial(topub);
                ret = new DHDerivedKey(derivedKey);

            }


                //using (ECDiffieHellmanCng alias = new ECDiffieHellmanCng())
                //{
                //    ReadOnlySpan<byte> source = new ReadOnlySpan<byte>(from.PrivateKey.Value);
                //    int butesRead = 0;
                //    alias.ImportECPrivateKey(source, out butesRead);


                //    alias.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                //    alias.HashAlgorithm = CngAlgorithm.Sha256;


                //    CngKey k = CngKey.Import(to.Value, CngKeyBlobFormat.EccPublicBlob);

                //    byte[] derivedKey = alias.DeriveKeyMaterial(k);

                //    ret = new DHDerivedKey(derivedKey);



                //}

                return ret;
        }
    }
}
