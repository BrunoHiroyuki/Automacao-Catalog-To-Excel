using AtacadoCatalogToExcelApp.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AtacadoCatalogToExcelApp;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            Console.WriteLine("Realizando chamada dos serviços");
            var scraper = new ScraperService();
            var exporter = new ExcelExportService();
            Console.WriteLine("Chamada realizada com sucesso");

            var products = await scraper.GetProductsAsync();
            ExcelExportService.Export(products, "towaatacado-produtos.xlsx");

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro: {ex.Message}");
            return -1;
        }
    }
}
