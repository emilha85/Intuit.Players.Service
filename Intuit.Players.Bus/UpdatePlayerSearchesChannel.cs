using Intuit.Players.Models.Messages;
using System.Threading.Channels;

namespace Intuit.Players.Bus
{
    /// <summary>
    /// In memory service bus, to notify player searched
    /// </summary>
    public class UpdatePlayerSearchesChannel : IPlayersChannel<UpdatePlayerSearchedMessage>
    {
        public UpdatePlayerSearchesChannel()
        {
            var channel = Channel.CreateUnbounded<UpdatePlayerSearchedMessage>();

            Writer = channel.Writer;
            Reader = channel.Reader;
        }

        public ChannelWriter<UpdatePlayerSearchedMessage> Writer { get; }

        public ChannelReader<UpdatePlayerSearchedMessage> Reader { get; }
    }
}
