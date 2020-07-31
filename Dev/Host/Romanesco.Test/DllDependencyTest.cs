using Romanesco.Model.ProjectComponents;
using System.IO;
using System.Reflection;
using Romanesco.Model;
using Xunit;

namespace Romanesco.Test
{
	public class DllDependencyTest
	{
		[Fact]
		public void メタデータをロードする手段がある()
		{
			var fullPath = Path.GetFullPath("./Resources/Dependencies2.dll");
			var resolver = new PathAssemblyResolver(new[] { fullPath, typeof(object).Assembly.Location });
			var loader = new MetadataLoadContext(resolver, "Dependencies2");
			var assembly = loader.LoadFromAssemblyPath(fullPath);
			var types = assembly.GetTypes();
			Assert.True(types.Length > 0);
		}

		[Fact]
		public void 依存関係のあるアセンブリをロードできる()
		{
			var editor = new ProjectSettingsEditor(new DataAssemblyRepository());
			editor.AssemblyPath.Value = Path.GetFullPath("./Resources/Dependencies2.dll");
			Assert.NotNull(editor.Assembly);
		}

		[Fact]
		public void メタデータと実体を呼び分けながらロードする()
		{
			var repo = new DataAssemblyRepository();
			var asm = repo.LoadAssemblyFromPath("./Resources/Dependencies2.dll");

			Assert.NotNull(asm);

			var types = asm!.GetTypes();
			Assert.True(types.Length > 0);
		}
	}
}
