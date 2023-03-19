using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PDFSplitterConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter the path of the PDF file to split: ");
            string pdfFilePath = Console.ReadLine();

            if (!File.Exists(pdfFilePath))
            {
                Console.WriteLine("File not found. Please check the file path and try again. ");
                return;
            }
            
            Console.WriteLine("Enter the number of pages per output file: ");
            int.TryParse(Console.ReadLine(), out int pagesPerFile);

            if (pagesPerFile <= 0)
            {
                Console.WriteLine("Invalid number of pages. Please enter a positive integer value...");
                return;
            }

            try
            {
                SplitPdf(pdfFilePath, pagesPerFile);
                Console.WriteLine("PDF split complete...");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error has occured while splitting the PDF: {e.Message}");
            }
        }

        private static void SplitPdf(string pdfFilePath, int pagesPerFiles)
        {
            string outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads", "printed");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory((outputDirectory));
            }

            PdfDocument inputPdf = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.Import);
            int fileCounter = 1;

            for (int i = 0; i < inputPdf.PageCount; i += pagesPerFiles)
            {
                PdfDocument outputPdf = new PdfDocument();

                for (int j = i; j < i + pagesPerFiles && j < inputPdf.PageCount; j++)
                {
                    outputPdf.AddPage(inputPdf.Pages[j]);
                }

                string outputFileName = $"toPrint{fileCounter}.pdf";
                string outputFilePath = Path.Combine(outputDirectory, outputFileName);
                outputPdf.Save(outputFilePath);

                Console.WriteLine($"Created: {outputFilePath}");

                fileCounter++;
            }
        }
    }
}