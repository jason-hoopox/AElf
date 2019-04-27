using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Contracts.CrossChain;
using Google.Protobuf;

namespace AElf.CrossChain
{
    public interface IBasicCrossChainDataProvider
    {
        // TODO: Temporarily changed to IMessage
        Task<IMessage> GetIndexedCrossChainBlockDataAsync(Hash currentBlockHash, long currentBlockHeight);

        // TODO: Temporarily changed to IMessage
        Task<IMessage> GetChainInitializationContextAsync(int chainId);
    }

    internal interface ICrossChainDataProvider : IBasicCrossChainDataProvider
    {
        Task<List<SideChainBlockData>> GetSideChainBlockDataAsync(Hash currentBlockHash, long currentBlockHeight);

        Task<bool> ValidateSideChainBlockDataAsync(List<SideChainBlockData> sideChainBlockData,
            Hash currentBlockHash, long currentBlockHeight);

        Task<List<ParentChainBlockData>> GetParentChainBlockDataAsync(Hash currentBlockHash, long currentBlockHeight);

        Task<bool> ValidateParentChainBlockDataAsync(List<ParentChainBlockData> parentChainBlockData,
            Hash currentBlockHash, long currentBlockHeight);

        void RegisterNewChain(int chainId);

        Task<CrossChainBlockData> GetCrossChainBlockDataForNextMiningAsync(Hash currentBlockHash,
            long currentBlockHeight);

        CrossChainBlockData GetUsedCrossChainBlockDataForLastMiningAsync(Hash blockHash, long previousBlockHeight);
    }
}