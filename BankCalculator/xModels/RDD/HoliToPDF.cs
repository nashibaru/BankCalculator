using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BankCalculator.xModels;

public class HoliToPDF
{
    public static void Generate(Applying apply)
    {
        PdfDocument document = new();
        PdfPage page = document.AddPage();

        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new("Arial", 20);
        XFont title = new("Arial", 30);

        double start = 100;
        int row = 25;
        double label = 37.5;

        gfx.DrawString("Wniosek - wakacje kredytowe", title, XBrushes.Black, new XPoint(100, start));
        start += 60;
        gfx.DrawString("Wnioskodawcy", font, XBrushes.Black, new XPoint(100, start));
        foreach (var i in apply.Pesels) 
        {
            start += row;
            gfx.DrawString(i, font, XBrushes.DarkGray, new XPoint(100, start));
        }
        start += label;
        gfx.DrawString("Kwota kredytu", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(apply.LendAmount.ToString() + " PLN", font, XBrushes.DarkGray, new XPoint(100, start));
        start += label;
        gfx.DrawString("Data uruchomienia", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(apply.StartDate.ToShortDateString(), font, XBrushes.DarkGray, new XPoint(100, start));
        start += label;
        gfx.DrawString("Ilość dzieci", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(apply.Children, font, XBrushes.DarkGray, new XPoint(100, start));
        start += label;
        if (apply.Children == "Mniej niż 3")
        {
            gfx.DrawString("Raty i dochody", font, XBrushes.Black, new XPoint(100, start));
            foreach (var i in apply.RddData)
            {
                start += row;
                gfx.DrawString("Rata | " + i.Invment.ToString() + " | Dochód | " + i.Gain.ToString(), font, XBrushes.DarkGray, new XPoint(100, start));
            }
            start += row;
            gfx.DrawString("Wskaźnik RDD | " + apply.RddAvg.ToString(), font, XBrushes.DarkGray, new XPoint(100, start));
        }
        start += label;
        gfx.DrawString(apply.Decision, font, XBrushes.Black, new XPoint(100, start));
        if (apply.Reason.Count > 0)
        {
            foreach (var i in apply.Reason)
            {
                start += row;
                gfx.DrawString(i, font, XBrushes.DarkGray, new XPoint(100, start));
            }
        }

        string filename = "Decyzja_" + apply.Pesels[0] + ".pdf";
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename);
        using (MemoryStream ms = new())
        {
            document.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);

            using (FileStream fs = new(path, FileMode.Create, FileAccess.Write))
            {
                ms.CopyTo(fs);
            }
        };
    }
}
