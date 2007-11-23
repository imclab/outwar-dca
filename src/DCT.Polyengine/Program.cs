using System;
using System.IO;
using System.Text;

namespace DCT.Polyengine
{
    class Program
    {
        private static Random mRandom;
        
        static void Main(string[] args)
        {
            mRandom = new Random();
            if(args.Length == 1 && args[0] == "-h")
            {
                Console.WriteLine("Usage: DCT.Polyengine outputfile key=value [...]");
                return;
            }
            else if (args.Length > 1)
            {
                Generate(args);
                return;
            }

            Console.Write("> ");
            string input;
            while ((input = Console.ReadLine()) != string.Empty && input != "exit")
            {
                string[] ins = input.Split(' ');
                if (ins.Length == 2)
                {
                    Console.WriteLine(DCT.Security.Crypt.BinToHex(DCT.Security.Crypt.Get(ins[0], ins[1], false)));
                }
                else if (ins[0] == "gen" && ins.Length > 2)
                {
                    string[] tmp = new string[ins.Length - 1];
                    for (int i = 1; i < ins.Length; i++)
                    {
                        tmp[i - 1] = ins[i];
                    }
                    Main(tmp);
                }
                else
                {
                    Console.WriteLine("Bad input");
                }
                Console.Write("\n> ");
            }
        }

        private static void Generate(string[] args)
        {
            Console.WriteLine("Polyengine Started");
            Console.WriteLine("Ready to compute {0} crypted strings...", args.Length - 1);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Generated by Typpo's Polyengine on " + DateTime.Now);
            sb.AppendLine();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine();
            sb.AppendLine("namespace DCT.Polyengine.Generated");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class StringLibrary");
            sb.AppendLine("\t{");

            for (int i = 1; i < args.Length; i++)
            {
                int idx = args[i].IndexOf("=");
                string key = args[i].Substring(0, idx);
                string val = args[i].Substring(idx + 1, args[i].Length - idx -1);

                Console.WriteLine("Generating method {0}()...", key);
                sb.AppendLine("\t\tpublic static string " + key + "()");
                sb.AppendLine("\t\t{");
                
                AppendMethodBody(sb, val);
                sb.AppendLine("\t\t}");   // method
                sb.AppendLine();
            }

            Console.WriteLine("Generating decryption method...");
            AppendDecryptMethod(sb);
            sb.AppendLine("\t}"); // class
            sb.Append("}"); // namespace

            
            Console.WriteLine("Writing to file {0}...", args[0]);
            TextWriter t = new StreamWriter(args[0]);
            try
            {
                t.Write(sb.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                t.Flush();
                t.Close();
            }
            
            Console.WriteLine("Done");
        }

        private static void  AppendMethodBody(StringBuilder sb, string s)
        {
            sb.AppendLine("\t\t\tStack<int> stack = new Stack<int>(" + s.Length + ");");
            sb.AppendLine("\t\t\tint bCrypt = 0;");
            sb.AppendLine();

            int crypt = 0;
            int tempVal = 0;

            for (int i = s.Length - 1; i > -1; i--)
            {
                int val = Convert.ToChar(s[i]);
                int op = mRandom.Next(0, 3);

                switch (op)
                {
                    //SUB
                    case 0:
                        tempVal = (crypt - val);
                        break;
                    //ADD
                    case 1:
                        tempVal = (val - crypt);
                        break;
                    //XOR
                    case 2:
                        tempVal = (crypt ^ val);
                        break;
                }
                tempVal = tempVal & 255;
                crypt = val;

                sb.AppendLine("\t\t\tbCrypt = StackDecrypt(bCrypt, " + op + ", 0x" + tempVal.ToString("X") + ");");
                sb.AppendLine("\t\t\tstack.Push(bCrypt);");
            }

            sb.AppendLine();
            sb.AppendLine("\t\t\tStringBuilder sb = new StringBuilder();");
            sb.AppendLine("\t\t\twhile (stack.Count > 0)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tsb.Append((char)stack.Pop());");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn sb.ToString();");
        }

        private static void AppendDecryptMethod(StringBuilder sb)
        {
            sb.AppendLine("\t\tprivate static int StackDecrypt(int bCrypt, int iOpCode, int iSalt)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tswitch (iOpCode)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tcase 0:");
            sb.AppendLine("\t\t\t\t\tbCrypt = bCrypt - iSalt;");
            sb.AppendLine("\t\t\t\t\tbreak;");
            sb.AppendLine("\t\t\t\tcase 1:");
            sb.AppendLine("\t\t\t\t\tbCrypt = bCrypt + iSalt;");
            sb.AppendLine("\t\t\t\t\tbreak;");
            sb.AppendLine("\t\t\t\tcase 2:");
            sb.AppendLine("\t\t\t\t\tbCrypt = bCrypt ^ iSalt;");
            sb.AppendLine("\t\t\t\t\tbreak;");
            sb.AppendLine("\t\t\t}"); // switch (iOpCode)
            sb.AppendLine("\t\t\tbCrypt = bCrypt & 255;");
            sb.AppendLine("\t\t\treturn bCrypt;");
            sb.AppendLine("\t\t}"); // StackDecrypt()
        }
    }
}
