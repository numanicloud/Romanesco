
using System.Reflection;

namespace ConsoleApp1
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var assembly = Assembly.LoadFrom("D:\\Home\\MyDocuments\\Projects\\Repos\\CSharp\\Tools\\Solo\\Romanesco\\Dev\\bin\\Debug\\Plugins\\Builtin\\Romanesco.BuiltinPlugin.View.dll");
			var type = assembly.GetType("Romanesco.BuiltinPlugin.View.View.ListBlockView") ?? throw new Exception();
			var instance = Activator.CreateInstance(type) ?? throw new Exception();
			Console.WriteLine("Hello!");
		}
	}
}