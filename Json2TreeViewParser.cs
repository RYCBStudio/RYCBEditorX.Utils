using Newtonsoft.Json.Linq;
using System.Windows.Controls;

namespace RYCBEditorX.Utils;
public class Json2TreeViewParser
{
    public static void LoadJsonIntoTreeView(TreeView treeView, string jsonFilePath)
    {
        // 读取 JSON 文件
        var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
        var jsonObject = JToken.Parse(jsonContent);

        // 清空现有节点
        treeView.Items.Clear();

        // 创建根节点并添加到 TreeView
        var rootItem = new TreeViewItem { Header = "Root File" };
        treeView.Items.Add(rootItem);

        // 将 JSON 数据添加到 TreeView
        AddNode(rootItem, jsonObject);
    }

    private static void AddNode(TreeViewItem parentItem, JToken token)
    {
        if (token.Type == JTokenType.Object)
        {
            foreach (var property in token.Children<JProperty>())
            {
                var propertyItem = new TreeViewItem
                {
                    Header = $"{property.Name}",
                    Tag = property.Value
                };
                parentItem.Items.Add(propertyItem);

                AddNode(propertyItem, property.Value);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            var index = 0;
            foreach (var item in token.Children())
            {
                var arrayItem = new TreeViewItem
                {
                    Header = $"Item {index++}",
                    Tag = item
                };
                parentItem.Items.Add(arrayItem);

                AddNode(arrayItem, item);
            }
        }
        else
        {
            parentItem.Header = token.ToString();
        }
    }
}
