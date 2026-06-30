using AtacadoCatalogToExcelApp.Models;
using ClosedXML.Excel;

namespace AtacadoCatalogToExcelApp.Services
{
    public class ExcelExportService
    {
        public static void Export(List<Produto> products, string fileName)
        {
            Console.WriteLine("Gerando Excel");
            var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            var filePath = Path.Combine(downloadsPath, fileName);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Produtos");

            worksheet.Cell(1, 1).Value = "Imagem";
            worksheet.Cell(1, 2).Value = "Nome";
            worksheet.Cell(1, 3).Value = "Preço";
            worksheet.Cell(1, 4).Value = "Categoria";

            const int IMAGE_WIDTH = 120;   // largura em pixels
            const int IMAGE_HEIGHT = 120;  // altura em pixels

            var row = 2;
            foreach (var produto in products)
            {
                worksheet.Cell(row, 2).Value = produto.Name;
                worksheet.Cell(row, 3).Value = produto.Price;
                worksheet.Cell(row, 4).Value = produto.Category;

                if (!string.IsNullOrWhiteSpace(produto.ImageUrl))
                {
                    try
                    {
                        var imageBytes = DownloadImage(produto.ImageUrl);

                        using var ms = new MemoryStream(imageBytes);
                        var picture = worksheet.AddPicture(ms)
                            .MoveTo(worksheet.Cell(row, 1))
                            .WithSize(IMAGE_WIDTH, IMAGE_HEIGHT);

                        // Ajusta linha e coluna para caber a imagem
                        worksheet.Row(row).Height = IMAGE_HEIGHT * 0.75;
                    }
                    catch
                    {
                        worksheet.Cell(row, 1).Value = "Erro ao carregar imagem";
                    }
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.RangeUsed().SetAutoFilter();
            worksheet.SheetView.FreezeRows(1);
            worksheet.Column(1).Width = IMAGE_WIDTH / 7.0;
            workbook.SaveAs(filePath);

            Console.WriteLine("Excel gerado com sucesso!");
        }

        private static byte[] DownloadImage(string url)
        {
            using var http = new HttpClient();
            return http.GetByteArrayAsync(url).Result;
        }
    }
}
