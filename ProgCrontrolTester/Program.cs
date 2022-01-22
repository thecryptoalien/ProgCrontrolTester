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
            // using maincrontrol
            using (MainCtrl mainCtrl = new MainCtrl())
            {
                // using process from maincrontrol
                using (Process ctrlPipe = mainCtrl.RunProg(@"C:\Windows\System32\cmd.exe"))
                {
                    // start process
                    ctrlPipe.Start();
                    progRunnign = true;
                    StreamReader streamReader = ctrlPipe.StandardOutput;
                    StreamWriter myStreamWriter = ctrlPipe.StandardInput;
                    String inputText = " ";

                    // init and start tasks
                    Task outtask = new Task(dOutput);
                    outtask.Start();
                    Task intask = new Task(dInput);
                    intask.Start();

                    // internal output task
                    void dOutput()
                    {
                        // loop until forever
                        while (true)
                        {
                            // init buffer
                            char[] charArray = new char[10000];
                            var readByteCount = streamReader.Read(charArray, 0, charArray.Length);
                            if (readByteCount > 0)
                            {
                                // form string from buffer
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
                                }
                            }
                            // nullify buffer
                            charArray = null;
                        }
                    }

                    // internal input task
                    void dInput()
                    {
                        // loop untill quit
                        while (progRunnign)
                        {
                            // read in text
                            inputText = Console.ReadLine();
                            // write to stream
                            myStreamWriter.WriteLine(inputText);
                            myStreamWriter.Flush();
                            // check for exit
                            if (inputText == "exit")
                            {
                                progRunnign = false;
                            }
                        } 
                    }

                    // await standard input
                    intask.Wait();
                }
            }
            
            // wait
            Console.ReadKey();
        }
    }
}
