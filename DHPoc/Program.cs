using System;
using System.Security.Cryptography;

using Crypto;

namespace DHPoc
{
    class Program
    {
        private const string ARG_ALIAS = "-alias";
        private const string ARG_CREATEKEYS = "-createKeys";
        private const string ARG_REMOVEKEYS = "-removeKeys";
        private const string ARG_CREATETEXT = "-encrypt";
        private const string ARG_READTEXT = "-decrypt";
        private const string ARG_PUB = "-pub";

        private const string extfile = ".dhpoc";




        static void Main(string[] args)
        {
            Console.WriteLine("DHPoc v0.1");
            Console.WriteLine("----------");


            if (args.Length < 3)
            {
                Usage();
            }
            else
            {
                bool IsOk = args[0].Equals(ARG_ALIAS) && !string.IsNullOrEmpty(args[1]);
                if (args.Length == 3)
                {
                    IsOk = IsOk && (args[2].Equals(ARG_CREATEKEYS)|| args[2].Equals(ARG_REMOVEKEYS));
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
                    string alias = args[1];
                    string action = args[2];

                    string textfilename=string.Empty;
                    string pubfilename=string.Empty;

                    if (args.Length == 6)
                    {
                        textfilename = args[3];
                        pubfilename = args[5];
                        if (!ExistFile(textfilename))
                        {
                            Console.WriteLine($"{textfilename} does not exist!");
                            IsOk = false;
                        }
                        else if (!ExistFile(pubfilename))
                        {
                            Console.WriteLine($"{pubfilename} does not exist!");
                            IsOk = false;
                        }
                    }
                    if (IsOk)
                    {
                        switch (action)
                        {
                            case ARG_CREATEKEYS:
                                {
                                    CreateKeys(alias);
                                    break;
                                }
                            case ARG_REMOVEKEYS:
                                {
                                    RemoveKeys(alias);
                                    break;
                                }
                            case ARG_CREATETEXT:
                                {

                                    CreateTextFor(alias, textfilename, pubfilename);
                                    break;
                                }

                            case ARG_READTEXT:
                                {

                                    ReadTextFrom(alias, textfilename, pubfilename);
                                    break;
                                }
                        }

                    }

                }
                


            }

            Console.WriteLine("Press key to quit.");
            Console.ReadLine();
        }

        private static void RemoveKeys(string aliasName)
        {
            System.IO.File.Delete($"{aliasName}.private.key");
            System.IO.File.Delete($"{aliasName}.public.key");

            Console.WriteLine($"Keys removed !!.");

        }

        private static bool ExistFile(string fileName)
        {
            return System.IO.File.Exists(fileName);
        }

        private static void ReadTextFrom(string aliasName,string textFileName, string pubFileName)
        {
            DHKeyPair _aliaskeys = RestoreKeys(aliasName);
            PublicKey _publickeyto = RestorePublicKey(pubFileName);

            IDiffieHellman _dh = new DiffieHellman();
            DHDerivedKey _derived = _dh.GenerateDerivedKey(_aliaskeys, _publickeyto);

            Console.WriteLine($"Derived Key:\n {BitConverter.ToString(_derived.Value)}");
            byte[] IV = System.Text.Encoding.UTF8.GetBytes(textFileName.Replace(extfile,string.Empty));
            ICypher cypher = new AesCypher(_derived.Value, IV);

            byte[] read=ReadBinFile(textFileName);

            string content=cypher.Decrypt(read);

            WriteTextFile(textFileName.Replace(extfile, string.Empty), content);

            Console.WriteLine($"{textFileName}{extfile} decrypt file created.");
        }

        private static void CreateTextFor(string aliasName,string textFileName,string pubFileName)
        {
            DHKeyPair _aliaskeys = RestoreKeys(aliasName);
            PublicKey _publickeyto = RestorePublicKey(pubFileName);

            IDiffieHellman _dh = new DiffieHellman();
            DHDerivedKey _derived = _dh.GenerateDerivedKey(_aliaskeys, _publickeyto);

            Console.WriteLine($"Derived Key:\n {BitConverter.ToString(_derived.Value)}");
            byte[] IV =System.Text.Encoding.UTF8.GetBytes(textFileName);
            ICypher cypher = new AesCypher(_derived.Value, IV);

            byte[] encrypted=cypher.Encrypt(ReadTextFile(textFileName));

            WriteBinFile($"{textFileName}{extfile}", encrypted);

            Console.WriteLine($"{textFileName}{extfile} encrypt file created.");
        }

        private static PublicKey RestorePublicKey(string pubFileName)
        {
            string _publictoS = ReadTextFile(pubFileName);
            byte[] _publicto = Convert.FromBase64String(_publictoS);

            PublicKey _publickeyto = new PublicKey(_publicto);

            return _publickeyto;
        }

        private static DHKeyPair RestoreKeys(string aliasName)
        {
            byte[] _aliasprivate = ReadBinFile($"{aliasName}.private.key");
            string _aliaspublicS = ReadTextFile($"{aliasName}.public.key");
            byte[] _aliaspublic = Convert.FromBase64String(_aliaspublicS);

            DHKeyPair _aliaskeys =
                new DHKeyPair(
                new PrivateKey(_aliasprivate),
                new PublicKey(_aliaspublic)
                );
            return _aliaskeys;
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
        private static string ReadTextFile(string fileName)
        {
            return System.IO.File.ReadAllText(fileName);
        }
        private static void WriteBinFile(string fileName, byte[] content)
        {
            System.IO.File.WriteAllBytes(fileName, content);
        }
        private static byte[] ReadBinFile(string fileName)
        {
            return System.IO.File.ReadAllBytes(fileName);
        }
        private static void Usage()
        {
            var msg = "Usage:\n";
            msg += "\tDHPoc -alias [alias] -createKeys\n";
            msg += "\tDHPoc -alias [alias] -encrypt [File] -pub [dstPublicKeyFile]\n";
            msg += "\tDHPoc -alias [alias] -decrypt [File] -pub [srcPublicKeyFile]\n";
            msg += "\tDHPoc -alias [alias] -removeKeys\n";
            msg += "\n\n\t\t\tBy @jc4st3lls";
            Console.WriteLine(msg);
        }
    }
}
