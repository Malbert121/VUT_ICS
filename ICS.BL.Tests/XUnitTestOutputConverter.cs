using System.Text;
using Xunit.Abstractions;

namespace ICS.BL.Tests
{
    internal class XUnitTestOutputConverter(ITestOutputHelper output) : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string? message) => output.WriteLine(message);

        public override void WriteLine(string format, params object?[] args) => output.WriteLine(format, args);
    }
}