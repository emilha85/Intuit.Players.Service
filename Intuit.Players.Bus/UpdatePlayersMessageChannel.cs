using Intuit.Players.Models.Messages;
using System.Threading.Channels;

namespace Intuit.Players.Bus
{
    /// <summary>
    /// In memory service bus, to notify player updated data recevied 
    /// </summary>
    public class UpdatePlayersDataChannel : IPlayersChannel<PlayersDataMessage>
    {
        public UpdatePlayersDataChannel()
        {
            var channel = Channel.CreateUnbounded<PlayersDataMessage>();

            Writer = channel.Writer;
            Reader = channel.Reader;
        }

        public ChannelWriter<PlayersDataMessage> Writer { get; }

        public ChannelReader<PlayersDataMessage> Reader { get; }
    }
}
