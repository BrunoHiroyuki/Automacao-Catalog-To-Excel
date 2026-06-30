# TowAtacado Catalog Scraper

Aplicação **.NET (C#)** para automatizar a coleta de produtos do catálogo online da Tow Atacado e gerar um **arquivo Excel** com as informações extraídas.

🔗 Site de origem: https://towaatacado.catalog.kyte.site/

---

## 🎯 Objetivo
- Acessar automaticamente o catálogo web
- Ler a lista de produtos disponíveis
- Extrair informações como:
  - Nome do produto
  - Preço
  - Categoria (se disponível)
  - Imagem (URL)
- Exportar os dados para um arquivo **.xlsx**

---

## 🛠️ Tecnologias sugeridas
- **.NET 8 / C#**
- **Microsoft Playwright** (automação web moderna e rápida)
- **ClosedXML** (geração de Excel sem dependência do Office)

---

## 📦 Dependências
```bash
dotnet add package Microsoft.Playwright
dotnet add package ClosedXML
```

Após instalar o Playwright:
```bash
playwright install
```

---

## 📁 Estrutura do projeto
```
TowAtacado.CatalogScraper
│
├── Models
│   └── Product.cs
│
├── Services
│   ├── ScraperService.cs
│   └── ExcelExportService.cs
│
├── Program.cs

```

---

## 🧩 Modelo de dados
```csharp
public class Product
{
    public string Name { get; set; }
    public string Price { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
}
```
