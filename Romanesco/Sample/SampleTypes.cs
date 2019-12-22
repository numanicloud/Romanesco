using Romanesco.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

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
        public string String { get; set; }
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
        public Fuga Fuga { get; set; }
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

    public class Project
    {
        [EditorMember]
        public Hoge Hoge { get; set; }
    }

    public enum FooBar
    {
        Foo, Bar, Fizz, Buzz
    }
}
