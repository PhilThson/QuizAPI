using System;
using DinkToPdf;
using DinkToPdf.Contracts;
using Quiz.Infrastructure.Interfaces;

namespace Quiz.Infrastructure.Services
{
	public class DocumentService : IDocumentService
	{
        private readonly IConverter _converter;
        private readonly IRazorRendererService _razorRendererService;


        public DocumentService(
            IConverter converter,
            IRazorRendererService razorRendererService)
        {
            _converter = converter;
            _razorRendererService = razorRendererService;
        }

        public byte[] GeneratePdfFromRazorView<T>(string partialName, T viewModel)
        {
            var htmlContent = _razorRendererService.RenderPartialToString(partialName, viewModel);

            return GeneratePdf(htmlContent);
        }

        public byte[] GeneratePdfFromString()
        {
            var htmlContent = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <style>
                p{{
                    width: 80%;
                }}
                </style>
            </head>
            <body>
                <h1>Some heading</h1>
                <p>Lorem ipsum ...</p>
            </body>
            </html>
            ";

            return GeneratePdf(htmlContent);
        }

        private byte[] GeneratePdf(string htmlContent)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 18, Bottom = 18 },
                //DocumentTitle =
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                //można dodać atrybut FontName = "Arial" dla ustawień nagłowka i stopki
                //HeaderSettings = { FontSize = 10, Right = "Strona [page] z [toPage]", Line = true },
                FooterSettings = { FontSize = 8, Center = "Placówka Oświatowa - Diagnoza", Line = true },
            };

            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            return _converter.Convert(htmlToPdfDocument);
        }
    }
}

