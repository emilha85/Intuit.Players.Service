using System.Threading.Channels;

namespace Intuit.Players.Bus
{
    public interface IPlayersChannel<TMessage>
    {
        ChannelReader<TMessage> Reader { get; }

        ChannelWriter<TMessage> Writer { get; }
    }
}