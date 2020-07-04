using Romanesco.Common.Model.Basics;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using System.IO;
using System.Reflection;
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
		public void 依存関係のあるアセンブリをロードできる2()
		{
			var loader = new MyAssemblyLoadContext("./Resources/Dependencies2.dll");
			var asm = loader.MyLoad(new AssemblyName("Dependencies2"));
			Assert.True(asm?.GetTypes().Length > 0);
		}

		[Fact]
		public void 依存関係のあるアセンブリをロードできる()
		{
			var editor = new ProjectSettingsEditor(new DataAssemblyRepository());
			editor.AssemblyPath.Value = Path.GetFullPath("./Resources/Dependencies2.dll");
			Assert.True(editor.ProjectTypeMenu.Value.Length > 0);
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
