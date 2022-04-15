using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using FluentAssertions;
using NUnit.Framework;

namespace Repro1;

[TestFixture]
public class Tests
{
    [Test]
    public async Task Test1()
    {
        var arguments = new List<string>
        {
            "-threads", "1",
            "-nostdin",
            "-hide_banner",
            "-nostats",
            "-loglevel", "error",
            "-f", "lavfi",
            "-i", "testsrc=duration=1:size=1920x1080:rate=30",
            "-c:v", "libx264",
            "-pix_fmt", "yuv420p",
            "-strict", "-2",
            "-f", "mpegts",
            "pipe:1"
        };

        var sb = new StringBuilder();
        
        var result = await Cli.Wrap("ffmpeg")
            .WithArguments(arguments)
            .WithValidation(CommandResultValidation.None)
            .WithStandardOutputPipe(PipeTarget.Null)
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(sb))
            .ExecuteAsync();
        
        result.ExitCode.Should().Be(0, sb.ToString());
    }
}