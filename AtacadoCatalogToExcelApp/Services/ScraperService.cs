using AtacadoCatalogToExcelApp.Models;
using Microsoft.Playwright;
using System.Text.Json;

namespace AtacadoCatalogToExcelApp.Services
{
    public class ScraperService
    {
        private static readonly string Url = "https://towaatacado.catalog.kyte.site/";

        private static readonly string ComponenteLista = ".infinite-scroll-component";
        private static readonly string ComponenteCard = ".main_is-12__husl6";
        private static readonly string ComponenteTitulo = ".product_title__t7dLU";
        private static readonly string ComponentePreco = ".product_price__hgX1S";
        private static readonly string ComponenteCategoria = ".product_category__MfZs_";

        public async Task<List<Produto>> GetProductsAsync()
        {
            var listaProdutos = new List<Produto>();

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
            { 
                Headless = false 
            });


            Console.WriteLine($"Abrindo {Url}");
            var page = await browser.NewPageAsync();
            await page.GotoAsync(Url);
            Console.WriteLine($"{Url} aberta com sucesso");

            Console.WriteLine("Buscando lista");
            var container = await page.WaitForSelectorAsync(ComponenteLista) ?? throw new Exception("Container da lista não encontrada");
            Console.WriteLine("Lista encontrada");

            Console.WriteLine("Buscando produtos da lista");
            
            int qtd = 0;
            int tentativasSemCrescer = 0;
            const int maxTentativas = 3;
            var items = page.Locator(ComponenteCard);
            while (true)
            {
                int currentCount = await items.CountAsync();
                Console.WriteLine(currentCount);

                if (currentCount > qtd)
                {
                    qtd = currentCount;
                    tentativasSemCrescer = 0;
                }
                else
                {
                    tentativasSemCrescer++;
                    if (tentativasSemCrescer >= maxTentativas || qtd >=  100)
                        break;
                }

                await page.Mouse.WheelAsync(0, 1200);
                await page.WaitForTimeoutAsync(400);
            }

            Console.WriteLine($"{qtd} Produtos encontrados");

            var json = await page.EvaluateAsync<string>(@$"
                () => {{
                    const cards = document.querySelectorAll('{ComponenteCard}');

                    const data = Array.from(cards).map(card => ({{
                        Name: card.querySelector('{ComponenteTitulo}')?.innerText || '',
                        Price: card.querySelector('{ComponentePreco}')?.innerText || '',
                        Category: card.querySelector('{ComponenteCategoria}')?.innerText || '',
                        ImageUrl: card.querySelector('img')?.getAttribute('src') || ''
                    }}));

                    return JSON.stringify(data);
                }}
            ");

            listaProdutos.AddRange(JsonSerializer.Deserialize<List<Produto>>(json) ?? []);

            return listaProdutos;
        }
    }
}
