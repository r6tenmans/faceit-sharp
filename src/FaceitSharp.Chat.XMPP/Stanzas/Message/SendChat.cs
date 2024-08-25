
namespace FaceitSharp.Chat.XMPP.Stanzas;

public class SendChat : IStanzaRequest
{
    public required JID To { get; set; }

    public required JID From { get; set; }

    public required string Body { get; set; }

    public required string Id { get; set; }

    public string Type { get; set; } = "groupchat";

    public MessageMention[] Mentions { get; set; } = [];

    public string[] ImageIds { get; set; } = [];

    public void Expects(IResponseExpected response)
    {
        response.Where<Message>(Message => Message.Id == Id);
    }

    public XmlElement Serialize()
    {
        var children = new List<XmlElement>
        {
            X.C("body", null, [], Body)
        };

        foreach(var mention in Mentions)
        {
            var el = X.C("reference", Namespaces.REFERENCE,
                [
                    ("type", "mention"),
                    ("uri", mention.ResourceId),
                    ("begin", mention.MentionPositionStart.ToString()),
                    ("end", mention.MentionPositionEnd.ToString())
                ]);
            children.Add(el);
        }

        if (ImageIds.Length > 0)
        {
            var images = new List<XmlElement>();
            foreach (var imageId in ImageIds)
            {
                var el = X.C("img", null, [("id", imageId)]);
                images.Add(el);
            }

            var x = X.C("x", Namespaces.UPLOAD, [], images.ToArray());
            children.Add(x);
        }

        return X.C("message", Namespaces.CLIENT,
            [
                ("from", From.ToString()),
                ("to", To.ToString()),
                ("type", Type),
                ("id", Id)
            ],
            children.ToArray());
    }
}

public record class MessageMention(
    string ResourceId,
    int MentionPositionStart,
    int MentionPositionEnd);