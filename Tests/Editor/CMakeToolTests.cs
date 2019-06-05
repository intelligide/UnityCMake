using System.IO;
using NUnit.Framework;
using UnityEngine;
using cmake;

namespace cmake.Tests
{
    [TestFixture]
    internal class CMakeToolTests
    {
        string ProjectPath;

        string SourcePath;

        string BuildPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            SourcePath = ProjectPath;
            BuildPath = Path.Combine(ProjectPath, "Library/cmake-build");
        }

        [Test]
        public void CanConfigure()
        {
            CMakeTool cmakeTool = new CMakeTool("cmake.exe");
            Assert.DoesNotThrow(() => cmakeTool.Configure());
        }

        [Test]
        public void CanBuild()
        {
            CMakeTool cmakeTool = new CMakeTool("cmake.exe");
            Assert.DoesNotThrow(() => cmakeTool.Build());
        }
    }
}
