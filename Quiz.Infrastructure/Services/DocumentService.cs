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
        private readonly IDataService _dataService;


        public DocumentService(
            IConverter converter,
            IRazorRendererService razorRendererService,
            IDataService dataService)
        {
            _converter = converter;
            _razorRendererService = razorRendererService;
            _dataService = dataService;
        }

        public byte[] GeneratePdfFromRazorView<T>(string partialName, T viewModel)
        {
            //var invoiceViewModel = GetInvoiceModel();
            //var partialName = "/Views/PdfTemplate/InvoiceDetails.cshtml";
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
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 10, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontSize = 8, Center = "PDF demo from JeminPro", Line = true },
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

