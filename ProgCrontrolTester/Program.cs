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
            //using (Process myProcess = new Process())
            //{
            //    myProcess.StartInfo.FileName = "cmd.exe";
            //    myProcess.StartInfo.UseShellExecute = false;
            //    myProcess.StartInfo.RedirectStandardInput = true;
            //    myProcess.StartInfo.RedirectStandardOutput = true;

            //    myProcess.Start();
            //    StreamReader streamReader = myProcess.StandardOutput;
            //    StreamWriter myStreamWriter = myProcess.StandardInput;

            //    // Prompt the user for input text lines to sort.
            //    // Write each line to the StandardInput stream of
            //    // the sort command.
            //    String inputText = " ";
            //    progRunnign = true;
            //    void output()
            //    {
            //        while (true)
            //        {
            //            //string line = streamReader.ReadLine();
            //            //Console.WriteLine(line);
            //            char[] charArray = new char[10000];
            //            var readByteCount = streamReader.Read(charArray, 0, charArray.Length);
            //            if (readByteCount > 0)
            //            {
            //                //string line = streamReader.ReadLine();
            //                String data = new string(charArray, 0, readByteCount);
            //                string[] datas = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //                for (int i = 0; i < datas.Length; i++)
            //                {
            //                    if (i == datas.Length - 1)
            //                    {
            //                        Console.Write(datas[i]);
            //                    }
            //                    else
            //                    {
            //                        if (datas[i] != inputText)
            //                        {
            //                            Console.Write(datas[i].Replace("\r", "").Replace("\n", "") + "\r\n");
            //                        }
            //                    }
            //                    //Thread.Sleep(15);
            //                }

            //            }
            //            charArray = null;
            //            //Thread.Sleep(5);

            //        }
            //    }
            //    Task task = new Task(output);
            //    task.Start();

            //    do
            //    {
                    
            //        inputText = Console.ReadLine();
            //        //if (inputText.Length > 0)
            //        //{
            //        //    numLines++;
            //        myStreamWriter.WriteLine(inputText);
            //        myStreamWriter.Flush();
            //        //}
            //        if (inputText == "exit")
            //        {
            //            progRunnign = false;
            //        }

            //    } while (progRunnign);

                

            //    // End the input stream to the sort command.
            //    // When the stream closes, the sort command
            //    // writes the sorted text lines to the
            //    // console.
            //    myStreamWriter.Close();

            //    // Wait for the sort process to write the sorted text lines.
            //    myProcess.WaitForExit();
            //}
        



            Console.ReadKey();
        }
    }
}
