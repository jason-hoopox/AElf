using System.Collections.Generic;
using AElf.Types;

namespace AElf.Contracts.NFT
{
    public partial class NFTContract
    {
        public override NFTInfo GetNFTInfo(GetNFTInfoInput input)
        {
            var tokenHash = CalculateTokenHash(input.Symbol, input.TokenId);
            return State.NftInfoMap[tokenHash];
        }

        public override NFTInfo GetNFTInfoByTokenHash(Hash input)
        {
            return State.NftInfoMap[input];
        }

        public override GetBalanceOutput GetBalance(GetBalanceInput input)
        {
            var tokenHash = CalculateTokenHash(input.Symbol, input.TokenId);
            return new GetBalanceOutput
            {
                Owner = input.Owner,
                Balance = State.BalanceMap[tokenHash][input.Owner],
                TokenHash = tokenHash
            };
        }

        public override GetBalanceOutput GetBalanceByTokenHash(GetBalanceByTokenHashInput input)
        {
            return new GetBalanceOutput
            {
                Owner = input.Owner,
                Balance = State.BalanceMap[input.TokenHash][input.Owner],
                TokenHash = input.TokenHash
            };
        }

        public override GetAllowanceOutput GetAllowance(GetAllowanceInput input)
        {
            var tokenHash = CalculateTokenHash(input.Symbol, input.TokenId);
            return new GetAllowanceOutput
            {
                Owner = input.Owner,
                Spender = input.Spender,
                TokenHash = tokenHash,
                Allowance = State.ApprovedAmountMap[tokenHash][input.Owner][input.Spender]
            };
        }

        public override GetAllowanceOutput GetAllowanceByTokenHash(GetAllowanceByTokenHashInput input)
        {
            return new GetAllowanceOutput
            {
                Owner = input.Owner,
                Spender = input.Spender,
                TokenHash = input.TokenHash,
                Allowance = State.ApprovedAmountMap[input.TokenHash][input.Owner][input.Spender]
            };
        }

        public override Hash CalculateTokenHash(CalculateTokenHashInput input)
        {
            return CalculateTokenHash(input.Symbol, input.TokenId);
        }

        private List<string> GetNftMetadataReservedKeys()
        {
            return new List<string>
            {
                NftTypeMetadataKey,
                NftBaseUriMetadataKey,
                AssembledNftsKey,
                AssembledFtsKey
            };
        }
    }
}