using System.Collections.Generic;

namespace DotNetFrameworkVersionManager
{
    public class DefaultConfig : Config
    {
        public DefaultConfig()
        {
            ChangeDatas.Add(new ChangeData(
              "<TargetFrameworkVersion>",
              "</TargetFrameworkVersion>",
              0,
              new string[] {
                    "v2.0",
                    "v3.0", "v3.5",
                    "v4.0", "v4.5", "v4.5.1", "v4.5.2",
                    "v4.6", "v4.6.1", "v4.6.2",
                    "v4.7", "v4.7.1", "v4.7.2",
              })
            );

            ChangeDatas.Add(new ChangeData(
                "<OutputPath>",
                "</OutputPath>",
                0,
                new string[] {
                   "bin\\Release\\",
                   "bin\\Debug\\"
                })
            );

          
        }
    }

    public class Config
    {
        public readonly List<ChangeData> ChangeDatas = new List<ChangeData>();
    }

    public class ChangeData
    {
        public ChangeData(string start, string end, int selectedIndex, string[] items)
        {
            Start = start;
            End = end;
            SelectedIndex = selectedIndex;
            Items = items;
        }
        public string Start = "<TargetFrameworkVersion>";
        public string End = "</TargetFrameworkVersion>";
        public int SelectedIndex;
        public string[] Items = new string[1];
    }
}
