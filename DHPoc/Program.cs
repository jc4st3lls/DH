using System;
using Crypto;

namespace DHPoc
{
    class Program
    {
        private const string ARG_ALIAS = "-alias";
        private const string ARG_CREATEKEYS = "-createKeys";
        private const string ARG_CREATETEXT = "-createText";
        private const string ARG_READTEXT = "-readText";
        private const string ARG_PUB = "-pub";

        
        private static string alias =string.Empty;
        private static string action = string.Empty;
        private static string textfilename = string.Empty;
        private static string pubfilename = string.Empty;

        static void Main(string[] args)
        {
            

            Console.WriteLine("POC of Diffie-Hellman");
            Console.WriteLine("---------------------");


            if (args.Length < 3)
            {
                Usage();
            }
            else
            {
                bool IsOk = args[0].Equals(ARG_ALIAS) && !string.IsNullOrEmpty(args[1]);
                if (args.Length == 3)
                {
                    IsOk = IsOk && (args[2].Equals(ARG_CREATEKEYS));
                }
                else if (args.Length == 6)
                {
                    IsOk = IsOk
                        && (args[2].Equals(ARG_CREATETEXT)|| args[2].Equals(ARG_READTEXT))
                        && !string.IsNullOrEmpty(args[3])
                        && args[4].Equals(ARG_PUB)
                        && !string.IsNullOrEmpty(args[5]);
                }
                 


                if(!IsOk)
                {
                    Usage();
                }
                else
                {
                    alias = args[1];
                    action = args[2];

                    switch (action)
                    {
                        case ARG_CREATEKEYS:
                            {
                                CreateKeys(alias);
                                break;
                            }
                        case ARG_CREATETEXT:
                            {
                                textfilename= args[4];
                                pubfilename = args[6];
                                CreateTextFor(textfilename, pubfilename);
                                break;
                            }

                        case ARG_READTEXT:
                            {
                                
                                textfilename = args[4];
                                pubfilename = args[6];
                                ReadTextFrom(textfilename, pubfilename);
                                break;
                            }
                    }
                    Console.WriteLine($"DHPoc {ARG_ALIAS} {alias} {action} {textfilename}");
                }
                

                

                // -alias Bob -createKeys
                // -alias Bob -createTextFor Alice
                // -alias Alice -readTextFrom Bob

            }


            Console.ReadLine();
        }

        private static void ReadTextFrom(string textFileName, string pubFileName)
        {
            throw new NotImplementedException();
        }

        private static void CreateTextFor(string textFileName,string pubFileName)
        {
            throw new NotImplementedException();
        }

        private static void CreateKeys(string aliasName)
        {
            IDiffieHellman _dh = new DiffieHellman();

            var _keys = _dh.GenerateKeyPair();

            Console.WriteLine($"Private Key:\n {BitConverter.ToString(_keys.PrivateKey.Value)}");
            Console.WriteLine($"Public Key:\n {BitConverter.ToString(_keys.PublicKey.Value)}");

         
            WriteBinFile($"{aliasName}.private.key", _keys.PrivateKey.Value);
            Console.WriteLine($"Private Key File:\n {aliasName}.private.key");

            WriteTextFile($"{aliasName}.public.key", Convert.ToBase64String(_keys.PublicKey.Value));
            Console.WriteLine($"Public Key File:\n {aliasName}.public.key");
        }

        private static void WriteTextFile(string fileName, string content)
        {
            System.IO.File.WriteAllText(fileName, content);
        }

        private static void WriteBinFile(string fileName, byte[] content)
        {
            System.IO.File.WriteAllBytes(fileName, content);
        }

        static void Usage()
        {
            var msg = "Usage:\n";
            msg += "\tDHPoc -alias [alias] -createKeys\n";
            msg += "\tDHPoc -alias [alias] -createText [FileToCreate] -pub [PublicKeyFile]\n";
            msg += "\tDHPoc -alias [alias] -readText [FileToRead] -pub [PublicKeyFile]\n";
            msg += "\n\n\t\t\tBy @jc4st3lls";


            Console.WriteLine(msg);
        }
    }
}
