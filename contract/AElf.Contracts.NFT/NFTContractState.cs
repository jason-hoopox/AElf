﻿using AElf.Sdk.CSharp.State;
using AElf.Types;

namespace AElf.Contracts.NFT
{
    public partial class NFTContractState : ContractState
    {
        public Int64State NftProtocolNumber { get; set; }
        public Int32State CurrentSymbolNumberLength { get; set; }
        public MappedState<long, bool> IsCreatedMap { get; set; }
        public MappedState<string, MinterList> MinterListMap { get; set; }
        public MappedState<Hash, NFTInfo> NftInfoMap { get; set; }

        /// <summary>
        /// Token Hash -> Owner Address -> Balance
        /// </summary>
        public MappedState<Hash, Address, long> BalanceMap { get; set; }

        public MappedState<string, NFTProtocolInfo> NftProtocolMap { get; set; }

        /// <summary>
        /// Token Hash -> Owner Address -> Spender Address -> Approved Amount
        /// Need to record approved by whom.
        /// </summary>
        public MappedState<Hash, Address, Address, long> ApprovedAmountMap { get; set; }
    }
}