using Romanesco.Common.Model.Basics;

public interface IStorageCloneService
{
	ValueStorage Clone(ValueStorage source);
}