namespace Romanesco.Common.Model.Interfaces
{
	public interface ILoadingStateReader
	{
		bool IsLoading { get; }
	}

	public interface ILoadingStateProvider : ILoadingStateReader
	{
		new bool IsLoading { get; set; }
	}
}
