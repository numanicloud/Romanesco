using Romanesco.Annotations;
using Romanesco.Common.Model.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Romanesco.Sample
{
	public class Fuga
    {
        [EditorMember]
        public int Id { get; set; }
        [EditorMember]
        public float X { get; set; }
        [EditorMember]
        public float Y { get; set; }
        [EditorMember]
        public float Z { get; set; }
        [EditorMember]
        public List<int> IntList { get; set; } = new List<int>();

        public override string? ToString()
        {
            return $"Fuga:({X}, {Y}, {Z}, List={IntList.Count})";
        }
    }

    public class Hoge
    {
        [EditorMember(order: 0)]
        public int Integer { get; set; }
        [EditorMember(order: 1)]
        public bool Boolean { get; set; }
        [EditorMember(order: 2)]
        public string String { get; set; } = "";
        [EditorMember(order: 3)]
        public float Float { get; set; }
        [EditorMember(order: 4)]
        public byte Byte;
        [EditorMember(order: 5)]
        public short Short;
        [EditorMember(order: 6)]
        public long Long;
        [EditorMember(order: 7)]
        public double Double;
        public int Hidden;
        [EditorMember(order: 8)]
        public Fuga? Fuga { get; set; }
        [EditorMember(order: 9)]
        [EditorMaster("Fugas", "Id")]
        public List<Fuga> FugaList { get; set; } = new List<Fuga>();
        [EditorMember(order: 10)]
        [EditorChoiceOfMaster("Fugas")]
        public int FugaRef { get; set; }
        [EditorMember(order: 11)]
        public FooBar EnumValue { get; set; }

        public override string? ToString()
        {
            return $"Fuga x{FugaList.Count}";
        }
    }

    [EditorProject]
    public class Project
    {
        [EditorMember]
        public Hoge? Hoge { get; set; }
        [EditorMember]
        public Fuga? Fuga { get; set; }
    }

    public enum FooBar
    {
        Foo, Bar, Fizz, Buzz
    }

    [EditorProject]
    public class DependentSubject
    {
        [EditorMember]
        public Dependent? Dependency { get; set; }
    }

    public class Dependent
    {
        [EditorMember]
        [EditorChoiceOfMaster("Fugas")]
        public int Ref { get; set; }
    }

    [EditorProject]
    public class DependentTarget
    {
        [EditorMember]
        [EditorMaster("Fugas", "Id")]
        public List<Fuga> Fugas { get; set; } = new List<Fuga>();
    }

    public class SampleExporter : IProjectTypeExporter
    {
        public bool DoExportIntoSingleFile => false;

        public async Task ExportAsync(object rootInstance, string exportPath)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(rootInstance, Newtonsoft.Json.Formatting.Indented);

            using (var file = File.Create(Path.Combine(exportPath, "SampleMaster1.json")))
            {
                using (var writer = new StreamWriter(file))
                {
                    await writer.WriteAsync(json);
                }
            }

            using (var file = File.Create(Path.Combine(exportPath, "SampleMaster2.json")))
            {
                using (var writer = new StreamWriter(file))
                {
                    await writer.WriteAsync(json);
                }
            }
        }
    }
}
