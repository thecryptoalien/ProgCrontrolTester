using ProgCrontrol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgCrontrolTester
{
    class Program
    {
        private static bool progRunnign;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting tester....");
            using (MainCtrl mainCtrl = new MainCtrl())
            {
                using (Process ctrlPipe = mainCtrl.RunProg(@"C:\Windows\System32\cmd.exe"))
                {
                    ctrlPipe.Start();
                    progRunnign = true;
                    StreamReader streamReader = ctrlPipe.StandardOutput;
                    StreamWriter myStreamWriter = ctrlPipe.StandardInput;
                    String inputText = " ";
                    Task outtask = new Task(dOutput);
                    outtask.Start();
                    Task intask = new Task(dInput);
                    intask.Start();
                    void dOutput()
                    {
                        while (true)
                        {
                            //string line = streamReader.ReadLine();
                            //Console.WriteLine(line);
                            char[] charArray = new char[10000];
                            var readByteCount = streamReader.Read(charArray, 0, charArray.Length);
                            if (readByteCount > 0)
                            {
                                //string line = streamReader.ReadLine();
                                String data = new string(charArray, 0, readByteCount);
                                string[] datas = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                for (int i = 0; i < datas.Length; i++)
                                {
                                    if (i == datas.Length - 1)
                                    {
                                        Console.Write(datas[i]);
                                    }
                                    else
                                    {
                                        if (datas[i] != inputText)
                                        {
                                            Console.Write(datas[i].Replace("\r", "").Replace("\n", "") + "\r\n");
                                        }
                                    }
                                    //Thread.Sleep(15);
                                }

                            }
                            charArray = null;
                            //Thread.Sleep(5);

                        }
                    }
                    void dInput()
                    {
                        do
                        {
                            inputText = Console.ReadLine();
                            
                            myStreamWriter.WriteLine(inputText);
                            myStreamWriter.Flush();
                            
                            if (inputText == "exit")
                            {
                                progRunnign = false;
                            }

                        } while (progRunnign);
                    }
                    intask.Wait();
                }
            }
            
            // wait
            Console.ReadKey();
        }
    }
}
