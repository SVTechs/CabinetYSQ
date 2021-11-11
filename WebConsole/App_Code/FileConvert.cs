using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspose.Cells;
using Aspose.Words;
using SaveFormat = Aspose.Words.SaveFormat;

/// <summary>
/// FileConvert 的摘要说明
/// </summary>
public class FileConvert
{
    public static bool DocToPdf(string originPath, string outputPath)
    {
        try
        {
            Document doc = new Document(originPath);
            doc.Save(outputPath, SaveFormat.Pdf);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool XlsToPdf(string originPath, string outputPath)
    {
        try
        {
            Workbook doc = new Workbook(originPath);
            doc.Save(outputPath, Aspose.Cells.SaveFormat.Pdf);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}