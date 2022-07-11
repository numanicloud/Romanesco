using System;
using Romanesco.Common.Model.Basics;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model.Services;

public class StorageCloneService : IStorageCloneService
{
	private readonly IStateSerializer _serializer;
	private readonly IStateDeserializer _deserializer;

	public StorageCloneService(IStateSerializer serializer, IStateDeserializer deserializer)
	{
		_serializer = serializer;
		_deserializer = deserializer;
	}

	public ValueStorage Clone(ValueStorage source)
	{
		if (source.GetValue() is not {} value)
		{
			throw new ArgumentException();
		}

		var serialized = _serializer.Serialize(value);
		var deserialized = _deserializer.Deserialize(serialized, source.Type);
		return source.Clone(deserialized);
	}
}