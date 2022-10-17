using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.Utils.InteractWithCommandPrompt
{
    public class CommandPromptInteraction
    {
        private Process process { get; set; }

        private StreamWriter standardInputStreamWriter;

        private StringBuilder results = new StringBuilder();

        public Boolean markStdOrError { get; set; } = true;
        public Boolean insertTimeStamp { get; set; } = true;
        public Boolean insertNewLineAfter { get; set; } = true;
        public Boolean alsoOutputToConsole { get; set; } = true;

        public void newInstance()
        {
            process = new Process();

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd");

            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            process.StartInfo = processStartInfo;

            process.Start();

            standardInputStreamWriter = process.StandardInput;

            process.OutputDataReceived += DataReceivedHandler;
            process.ErrorDataReceived += ErrorReceivedHandler;

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            Console.WriteLine("Command Prompt launched!");
        }

        public void closeInstance()
        {
            Console.WriteLine("Closing Command Prompt...");
            process.Close();
        }

        private void DataReceivedHandler(object sendingProcess, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (markStdOrError)
            {
                results.Append("DATA|");
            }
            if (insertTimeStamp)
            {
                results.Append(DateTime.Now + "|");
            }

            results.Append(dataReceivedEventArgs.Data);

            if (insertNewLineAfter)
            {
                results.Append("|" + Environment.NewLine);
            }

            if (alsoOutputToConsole)
            {
                Console.WriteLine(results);
            }
        }

        private void ErrorReceivedHandler(object sendingProcess, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (markStdOrError)
            {
                results.Append("ERROR|");
            }
            if (insertTimeStamp)
            {
                results.Append(DateTime.Now + "|");
            }

            results.Append(dataReceivedEventArgs.Data);

            if (insertNewLineAfter)
            {
                results.Append("|" + Environment.NewLine);
            }
            if (alsoOutputToConsole)
            {
                Console.WriteLine(results);
            }
        }

        public void clearAndSendNewOrder(String argCommand, Boolean clearResults = true)
        {
            if (clearResults)
            {
                results.Clear();
            }

            standardInputStreamWriter.WriteLine(argCommand);
            standardInputStreamWriter.Flush();
        }

        public String getResultsAndClear(Boolean clearResults = true)
        {
            String temp = results.ToString();

            if (clearResults)
            {
                results.Clear();
            }

            return temp;
        }
    }
}