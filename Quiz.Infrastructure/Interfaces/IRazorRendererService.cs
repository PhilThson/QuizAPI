using System;
namespace Quiz.Infrastructure.Interfaces
{
	public interface IRazorRendererService
	{
        string RenderPartialToString<TModel>(string partialName, TModel model);
    }
}