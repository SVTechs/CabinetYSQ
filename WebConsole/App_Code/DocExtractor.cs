using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;

/// <summary>
/// DocExtractor 的摘要说明
/// </summary>
public class DocExtractor
{
    private static readonly Logger CsLogger = LogManager.GetCurrentClassLogger();

    public static string ExtractFile(string fileName, byte[] fileData)
    {
        return "";
        //StringBuilder contentBuilder = new StringBuilder();
        //if (fileName.ToUpper().EndsWith("DOCX"))
        //{
        //    XWPFDocument document;
        //    try
        //    {
        //        using (MemoryStream file = new MemoryStream(fileData))
        //        {
        //            document = new XWPFDocument(file);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //    foreach (XWPFParagraph paragraph in document.Paragraphs)
        //    {
        //        contentBuilder.AppendLine(paragraph.ParagraphText + "<br />");
        //    }
        //}
        //else
        //{
        //    HWPFDocument document;
        //    try
        //    {
        //        using (MemoryStream file = new MemoryStream(fileData))
        //        {
        //            document = new HWPFDocument(file);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //    contentBuilder.Append(document.Text.Replace("\r", "<br />"));
        //}

        //return TrimText(contentBuilder.ToString());
    }

    private static string TrimText(string origin)
    {
        origin = origin.Replace("\a", "").Replace("\b", "");
        origin = Regex.Replace(origin, "－\\u0013 [\\s\\da-zA-Z*\\\\]*－", "");
        origin = Regex.Replace(origin, "\\u0013 [\\S\\s]*", "");
        return origin;
    }

    public static string ConvertFile(string serverPath, string fileName, byte[] fileData, bool saveOnly)
    {
        Random rd = new Random();
        string storageName = rd.Next(1000000, 9999999) + "-" + fileName;
        if (!Directory.Exists(serverPath)) Directory.CreateDirectory(serverPath);
        FileStream fs = File.Open(serverPath + storageName, FileMode.Create, FileAccess.Write);
        fs.Write(fileData, 0, fileData.Length);
        fs.Close();

        if (!saveOnly)
        {
            try
            {
                string upperName = fileName.ToUpper();
                if (upperName.EndsWith("DOC"))
                {
                    FileConvert.DocToPdf(serverPath + storageName, serverPath + storageName + ".pdf");
                    return storageName + ".pdf";
                }

                if (upperName.EndsWith("DOCX"))
                {
                    FileConvert.DocToPdf(serverPath + storageName, serverPath + storageName + ".pdf");
                    return storageName + ".pdf";
                }

                if (upperName.EndsWith("XLS"))
                {
                    FileConvert.XlsToPdf(serverPath + storageName, serverPath + storageName + ".pdf");
                    return storageName + ".pdf";
                }

                if (upperName.EndsWith("XLSX"))
                {
                    FileConvert.XlsToPdf(serverPath + storageName, serverPath + storageName + ".pdf");
                    return storageName + ".pdf";
                }

                if (upperName.EndsWith("PDF"))
                {
                    return storageName;
                }

                CsLogger.Error("No Match");
            }
            catch (Exception e)
            {
                CsLogger.Error(e);
            }
        }

        return storageName;
    }

    private static void XlsToHtml(string serverPath, string storageName)
    {
        HSSFWorkbook workbook = ExcelToHtmlUtils.LoadXls(serverPath + storageName);
        ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();
        excelToHtmlConverter.OutputColumnHeaders = true;
        excelToHtmlConverter.OutputHiddenColumns = false;
        excelToHtmlConverter.OutputHiddenRows = false;
        excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = true;
        excelToHtmlConverter.OutputRowNumbers = true;
        excelToHtmlConverter.UseDivsToSpan = true;
        excelToHtmlConverter.ProcessWorkbook(workbook);
        var htmlFile = serverPath + storageName + ".html";
        excelToHtmlConverter.Document.Save(htmlFile);
    }

    private static void XlsxToHtml(string serverPath, string storageName)
    {
        XSSFWorkbook workbook = new XSSFWorkbook(serverPath + storageName);
        ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();
        excelToHtmlConverter.OutputColumnHeaders = true;
        excelToHtmlConverter.OutputHiddenColumns = false;
        excelToHtmlConverter.OutputHiddenRows = false;
        excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = true;
        excelToHtmlConverter.OutputRowNumbers = true;
        excelToHtmlConverter.UseDivsToSpan = true;
        excelToHtmlConverter.ProcessWorkbook(workbook);
        var htmlFile = serverPath + storageName + ".html";
        excelToHtmlConverter.Document.Save(htmlFile);
    }

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    public static void KillProcessByMainWindowHwnd(int hWnd)
    {
        uint processID;
        GetWindowThreadProcessId((IntPtr)hWnd, out processID);
        if (processID == 0)
            throw new ArgumentException("Process has not been found by the given main window handle.", "hWnd");
        Process.GetProcessById((int)processID).Kill();
    }
}