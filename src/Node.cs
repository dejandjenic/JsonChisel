namespace JsonChisel;

public class Node
{
    public string Name { get; set; }
    public List<Node> Children { get; set; }
    public static Node Parse(string[] data)
    {
        var root = new Node { Name = "root", Children = new List<Node>() };
        var nodeDict = new Dictionary<string, Node> { { "root", root } };

        foreach (var path in data)
        {
            var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            Node current = root;
            var fullpath = "";

            foreach (var segment in segments)
            {
                if(string.IsNullOrWhiteSpace(fullpath))
                    fullpath += segment;
                else
                {
                    fullpath += "."+segment;
                }
                
                if (!nodeDict.ContainsKey(fullpath))
                {
                    var newNode = new Node { Name = segment, Children = new List<Node>() };
                    nodeDict[fullpath] = newNode;
                    current.Children.Add(newNode);
                }
                current = nodeDict[fullpath];
            }
        }

        return new Node { Children = root.Children };
    }
}