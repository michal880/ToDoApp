using System.Text.Json;

namespace ToDoApp;

public class MessageEnvelope
{
    public string Type { get; set; }
    public string Data { get; set; }
}