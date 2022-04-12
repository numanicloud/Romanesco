using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
	public class SubtypingStateContext
	{
		public SubtypingList List { get; }
		public IDataAssemblyRepository AsmRepo { get; }
		public IObjectInterpreter Interpreter { get; }

		public SubtypingStateContext(SubtypingList list, IDataAssemblyRepository asmRepo, IObjectInterpreter interpreter)
		{
			List = list;
			AsmRepo = asmRepo;
			Interpreter = interpreter;
		}
	}
}
