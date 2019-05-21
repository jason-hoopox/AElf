/*using System.Threading.Tasks;
using AElf.Contracts.MultiToken.Messages;
using AElf.Contracts.TestBase;
using AElf.Kernel;
using AElf.Kernel.Token;
using AElf.Types;
using Google.Protobuf;

namespace AElf.Contracts.MultiToken
{
    public static class ContractTesterExtensions
    {
        public static Address GetTokenContractAddress(
            this ContractTester<MultiTokenContractTestAElfModule> contractTester)
        {
            return contractTester.GetContractAddress(TokenSmartContractAddressNameProvider.Name);
        }

        public static async Task TransferTokenAsync(this ContractTester<MultiTokenContractTestAElfModule> starter, Address to,
            long amount)
        {
            await starter.ExecuteTokenContractMethodWithMiningAsync(nameof(TokenContractContainer.TokenContractStub.Transfer), new TransferInput
            {
                Symbol = "TELF",
                Amount = amount,
                To = to
            });
        }

        public static async Task<TransactionResult> ExecuteTokenContractMethodWithMiningAsync(
            this ContractTester<MultiTokenContractTestAElfModule> contractTester, string methodName, IMessage input)
        {
            return await contractTester.ExecuteContractWithMiningAsync(contractTester.GetTokenContractAddress(),
                methodName, input);
        }

        public static async Task<long> GetBalanceAsync(this ContractTester<MultiTokenContractTestAElfModule> contractTester,
            Address targetAddress)
        {
            var balanceOutput =GetBalanceOutput.Parser.ParseFrom(
                await contractTester.CallContractMethodAsync(contractTester.GetTokenContractAddress(),
                nameof(TokenContractContainer.TokenContractStub.GetBalance), new GetBalanceInput
                {
                    Owner = targetAddress,
                    Symbol = "TELF"
                }));
            return balanceOutput.Balance;
        }

        public static async Task<TransactionResult> Lock(this ContractTester<MultiTokenContractTestAElfModule> contractTester, long amount,
            Hash lockId, Address lockToAddress = null)
        {
            if (lockToAddress == null)
            {
                lockToAddress = contractTester.GetConsensusContractAddress();
            }
            
            return await contractTester.ExecuteContractWithMiningAsync(contractTester.GetTokenContractAddress(),
                nameof(TokenContractContainer.TokenContractStub.Lock),
                new LockInput
                {
                    From = contractTester.GetCallOwnerAddress(),
                    To = lockToAddress,
                    Amount = amount,
                    Symbol = "TELF",
                    LockId = lockId,
                    Usage = "Testing."
                });
        }

        public static async Task<TransactionResult> Unlock(this ContractTester<MultiTokenContractTestAElfModule> contractTester, long amount,
            Hash lockId)
        {
            return await contractTester.ExecuteContractWithMiningAsync(contractTester.GetTokenContractAddress(),
                nameof(TokenContractContainer.TokenContractStub.Unlock),
                new UnlockInput
                {
                    From = contractTester.GetCallOwnerAddress(),
                    To = contractTester.GetConsensusContractAddress(),
                    Amount = amount,
                    Symbol = "TELF",
                    LockId = lockId,
                    Usage = "Testing."
                });
        }
    }
}*/