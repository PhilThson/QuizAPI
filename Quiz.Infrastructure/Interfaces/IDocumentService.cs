using System;
namespace Quiz.Infrastructure.Interfaces
{
	public interface IDocumentService
	{
        byte[] GeneratePdfFromString();

        byte[] GeneratePdfFromRazorView<T>(string partialName, T viewModel);
    }
}

