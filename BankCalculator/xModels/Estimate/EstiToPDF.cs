using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.ObjectModel;

namespace BankCalculator.xModels.Estimate;

public class EstiToPDF
{
    public static void Generate(ObservableCollection<EstimateElements> estimateElements, EstimateValueData evd, DateTime finish, params string[] values)
    {
        PdfDocument document = new();
        PdfPage page = document.AddPage();
        PdfPage page2 = document.AddPage();

        XGraphics gfx = XGraphics.FromPdfPage(page);
        XGraphics gfx2 = XGraphics.FromPdfPage(page2);
        XFont font = new("Arial", 15);
        XFont title = new("Arial", 20);
        XFont list = new("Arial", 10);

        double start = 100;
        double start2 = 100;
        int row = 25;
        double label = 37.5;

        gfx.DrawString("Dobranie kredytu - kosztorys budowy", title, XBrushes.Black, new XPoint(100, start));
        start += 60;
        gfx.DrawString("Wnioskodawca", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[0], font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Adres nieruchomości / nr działki", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[1], font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Planowany termin zakończenia inwestycji", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(finish.ToShortDateString(), font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Powierzchnia użytkowa domu (m2)", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[2] + " m2", font, XBrushes.DarkGray, new XPoint(100, start));

        start += label;
        gfx.DrawString("Wartość działki", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[3] + " PLN", font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Wartość nieruchomości po ukończeniu (wycena)", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[4] + " PLN", font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Dotychczasowa wartość kredytu", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[5] + " PLN", font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Wskaźnik kredyt-do-wyceny", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(Math.Round(evd.Ltv, 2).ToString() + " %", font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Dotychczasowa wartość wkładu własnego", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(values[6] + " PLN", font, XBrushes.DarkGray, new XPoint(100, start));
        start += row;
        gfx.DrawString("Dotychczasowe nakłady inwestycyjne", font, XBrushes.Black, new XPoint(100, start));
        start += row;
        gfx.DrawString(evd.AllValueStr + " PLN", font, XBrushes.DarkGray, new XPoint(100, start));


        gfx2.DrawString("Nazwa elementu | Grupa elementu | Koszt (PLN) | Zaawansowanie (PLN) | Środki własne (PLN)", list, XBrushes.Black, new XPoint(100, start2));
        foreach (var value in estimateElements)
        {
            start2 += row;
            gfx2.DrawString(value.ElementName + " | " + value.ElementGroup + " | " + value.Cost + " | " + value.Advance + " | " + value.Owned, list, XBrushes.DarkGray, new XPoint(100, start2));
        }
        start2 += label;

        gfx2.DrawString("Wartość elementów nieukończonych", font, XBrushes.Black, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString(evd.CostSumStr + " PLN", font, XBrushes.DarkGray, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString("Zaawansowanie elementów nieukończonych", font, XBrushes.Black, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString(evd.AdvanceSumStr + " PLN", font, XBrushes.DarkGray, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString("Wkład własny na elementy nieukończone", font, XBrushes.Black, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString(evd.OwnedSumStr + " PLN", font, XBrushes.DarkGray, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString("Maksymalna móżliwa wartość kredytu", font, XBrushes.Black, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString(evd.MaxLendStr + " PLN", font, XBrushes.DarkGray, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString("Kwota kredytu dofinansowującego", font, XBrushes.Black, new XPoint(100, start2));
        start2 += row;
        gfx2.DrawString(evd.ToLendStr + " PLN", font, XBrushes.DarkGray, new XPoint(100, start2));

        string filename = "DobranieKredytu_" + values[0] + ".pdf";
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
