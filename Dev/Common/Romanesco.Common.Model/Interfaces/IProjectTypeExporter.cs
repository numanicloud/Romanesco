using System.Threading.Tasks;

namespace Romanesco.Common.Model.Interfaces
{
    public interface IProjectTypeExporter
    {
        bool DoExportIntoSingleFile { get; }

        /// <summary>
        /// エディターで編集されたデータを出力します。
        /// </summary>
        /// <param name="rootInstance">エクスポートすべきデータ全体。</param>
        /// <param name="exportPath">単一ファイルへのエクスポートなら、エクスポート先のファイル名。そうでなければ、エクスポート先のディレクトリ名。</param>
        Task ExportAsync(object rootInstance, string exportPath);
    }
}
