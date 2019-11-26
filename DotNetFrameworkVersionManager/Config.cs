using System;
using System.Collections.Generic;

namespace DotNetFrameworkVersionManager
{
    [Serializable]
    public class Config
    {
        public readonly List<ChangeData> ChangeDatas = new List<ChangeData>();

        /// 일반적인 MSBuild 프로젝트 속성
        /// https://docs.microsoft.com/ko-kr/visualstudio/msbuild/common-msbuild-project-properties?view=vs-2019
        public static Config DefaultConfig()
        {
            var config = new Config();
            config.ChangeDatas.Add(new ChangeData {
                Start = "<TargetFrameworkVersion>",
                End = "</TargetFrameworkVersion>",
                SelectedIndex = 0,
                Items = new string[] {
                    "v2.0",
                    "v3.0", "v3.5",
                    "v4.0", "v4.5", "v4.5.1", "v4.5.2",
                    "v4.6", "v4.6.1", "v4.6.2",
                    "v4.7", "v4.7.1", "v4.7.2",
                }
            });

            config.ChangeDatas.Add(new ChangeData {
                Start = "<FileAlignment>",
                End = "</FileAlignment>",
                SelectedIndex = 0,
                Items = new string[] {
                    "true",
                    "false"
                }
            });



            return config;
        }
    }

    [Serializable]
    public class ChangeData
    {
        public string Start = "<TargetFrameworkVersion>";
        public string End = "</TargetFrameworkVersion>";
        public int SelectedIndex;
        public string[] Items = new string[1];
    }
}
