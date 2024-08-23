using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RYCBEditorX.Utils;

public class CommentLoader
{
    public static async Task<List<Comment>> LoadCommentsAsync(string filePath)
    {
        var comments = new List<Comment>();

        // 读取 JSON 文件内容
        var jsonString = await File.ReadAllTextAsync(filePath);

        // 解析 JSON
        var jsonDocument = JsonDocument.Parse(jsonString);
        foreach (var element in jsonDocument.RootElement.EnumerateObject())
        {
            var userName = element.Name;
            var commentObject = element.Value;

            var comment = new Comment
            {
                User = userName,
                Uid = commentObject.GetProperty("Uid").GetString(),
                CommentText = commentObject.GetProperty("Comment").GetString(),
                Time = commentObject.GetProperty("Time").GetString(),
                Likes = commentObject.GetProperty("Likes").GetInt32()
            };

            comments.Add(comment);
        }

        return comments;
    }

}

