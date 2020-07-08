namespace Romanesco.Common.Model.Interfaces
{
	public interface IServiceLocator
	{
		T GetService<T>() where T : class;
		T? GetServiceOptional<T>() where T : class;
		T CreateInstance<T>(params object[] args);
	}
}
