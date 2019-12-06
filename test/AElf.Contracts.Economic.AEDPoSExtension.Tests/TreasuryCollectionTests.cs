using System.Linq;
using System.Threading.Tasks;
using AElf.Contracts.MultiToken;
using AElf.Contracts.TestKet.AEDPoSExtension;
using AElf.Types;
using Volo.Abp.Threading;
using Xunit;

namespace AElf.Contracts.Economic.AEDPoSExtension.Tests
{
    public partial class EconomicTests : EconomicTestBase
    {
        [Fact]
        public async Task TreasuryCollection_FirstTerm_Test()
        {
            var distributedAmount = await TreasuryDistribution_FirstTerm_Test();

            // First 7 core data centers can profit from backup subsidy
            var firstSevenCoreDataCenters = MissionedECKeyPairs.CoreDataCenterKeyPairs.Take(7).ToList();
            var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                {
                    Owner = Address.FromPublicKey(k.PublicKey),
                    Symbol = EconomicTestConstants.TokenSymbol
                })).Balance);
            await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.BackupSubsidy].SchemeId);
            await CheckBalancesAsync(firstSevenCoreDataCenters,
                distributedAmount / 5 / 7 , balancesBefore);
        }

        [Fact(Skip = "Save time.")]
        public async Task TreasuryCollection_SecondTerm_Test()
        {
            var distributedAmountOfFirstTerm = await TreasuryDistribution_FirstTerm_Test();
            var distributionInformationOfSecondTerm = await TreasuryDistribution_SecondTerm_Test();

            // First 7 core data centers can profit from backup subsidy of term 1 and term 2.
            var firstSevenCoreDataCenters = MissionedECKeyPairs.CoreDataCenterKeyPairs.Take(7).ToList();
            {
                var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.BackupSubsidy].SchemeId);
                var subsidyInFirstTerm = distributedAmountOfFirstTerm / 5 / 7;
                var subsidyInformation = distributionInformationOfSecondTerm[SchemeType.BackupSubsidy];
                var subsidyInSecondTerm = subsidyInformation.Amount / subsidyInformation.TotalShares;
                await CheckBalancesAsync(firstSevenCoreDataCenters,
                    subsidyInFirstTerm + subsidyInSecondTerm - EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // First 7 core data centers can profit from miner basic reward because they acted as miners during second term.
            {
                var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.MinerBasicReward].SchemeId);
                var basicRewardInSecondTerm =
                    distributionInformationOfSecondTerm[SchemeType.MinerBasicReward].Amount / 12;
                await CheckBalancesAsync(firstSevenCoreDataCenters,
                    basicRewardInSecondTerm - EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // First 7 core data centers can profit from votes weight reward.
            {
                var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.VotesWeightReward].SchemeId);
                var votesWeightReward = distributionInformationOfSecondTerm[SchemeType.VotesWeightReward].Amount / 7;
                await CheckBalancesAsync(firstSevenCoreDataCenters,
                    votesWeightReward - EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // First 10 voters can profit from citizen welfare.
            var firstTenVoters = MissionedECKeyPairs.CitizenKeyPairs.Take(10).ToList();
            {
                var balancesBefore = firstTenVoters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstTenVoters, _schemes[SchemeType.CitizenWelfare].SchemeId);
                var citizenWelfare = distributionInformationOfSecondTerm[SchemeType.CitizenWelfare].Amount / 10;
                await CheckBalancesAsync(firstTenVoters,
                    citizenWelfare - EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }
        }

        [Fact(Skip = "Save time.")]
        public async Task TreasuryCollection_ThirdTerm_Test()
        {
            var distributedAmountOfFirstTerm = await TreasuryDistribution_FirstTerm_Test();
            var distributionInformationOfSecondTerm = await TreasuryDistribution_SecondTerm_Test();
            var distributionInformationOfThirdTerm = await TreasuryDistribution_ThirdTerm_Test();

            var subsidyInformationOfSecondTerm = distributionInformationOfSecondTerm[SchemeType.BackupSubsidy];
            var subsidyInformationOfThirdTerm = distributionInformationOfThirdTerm[SchemeType.BackupSubsidy];

            // First 7 core data centers can profit from backup subsidy of term 1, term 2 and term 3.
            var firstSevenCoreDataCenters = MissionedECKeyPairs.CoreDataCenterKeyPairs.Take(7).ToList();
            {
                var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.BackupSubsidy].SchemeId);
                var subsidyInFirstTerm = distributedAmountOfFirstTerm / 5 / 7;
                var subsidyInSecondTerm =
                    subsidyInformationOfSecondTerm.Amount / subsidyInformationOfSecondTerm.TotalShares;
                var subsidyInThirdTerm =
                    subsidyInformationOfThirdTerm.Amount / subsidyInformationOfThirdTerm.TotalShares;
                await CheckBalancesAsync(firstSevenCoreDataCenters,
                    subsidyInFirstTerm + subsidyInSecondTerm + subsidyInThirdTerm -
                    EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // Last 12 core data centers can profit from backup subsidy of term 2 and term 3.
            var lastFourCoreDataCenters = MissionedECKeyPairs.CoreDataCenterKeyPairs.Skip(7).Take(12).ToList();
            {
                var balancesBefore = lastFourCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(lastFourCoreDataCenters, _schemes[SchemeType.BackupSubsidy].SchemeId);
                var subsidyInSecondTerm =
                    subsidyInformationOfSecondTerm.Amount / subsidyInformationOfSecondTerm.TotalShares;
                var subsidyInThirdTerm =
                    subsidyInformationOfThirdTerm.Amount / subsidyInformationOfThirdTerm.TotalShares;
                await CheckBalancesAsync(lastFourCoreDataCenters,
                    subsidyInSecondTerm + subsidyInThirdTerm -
                    EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // First 7 core data centers can profit from miner basic reward of term 2 and term 3.
            {
                var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.MinerBasicReward].SchemeId);
                var basicRewardInSecondTerm =
                    distributionInformationOfSecondTerm[SchemeType.MinerBasicReward].Amount / 12;
                var basicRewardInThirdTerm =
                    distributionInformationOfThirdTerm[SchemeType.MinerBasicReward].Amount / 17;
                await CheckBalancesAsync(firstSevenCoreDataCenters,
                    basicRewardInSecondTerm + basicRewardInThirdTerm -
                    EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // Last 10 core data centers can profit from miner basic reward of term 3.
            {
                var balancesBefore = lastFourCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(lastFourCoreDataCenters, _schemes[SchemeType.MinerBasicReward].SchemeId);
                var basicRewardInThirdTerm =
                    distributionInformationOfThirdTerm[SchemeType.MinerBasicReward].Amount / 17;
                await CheckBalancesAsync(lastFourCoreDataCenters,
                    basicRewardInThirdTerm - EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // First 7 core data centers can profit from votes weight reward.
            {
                var balancesBefore = firstSevenCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(firstSevenCoreDataCenters, _schemes[SchemeType.VotesWeightReward].SchemeId);
                var votesWeightRewardInSecondTerm =
                    distributionInformationOfSecondTerm[SchemeType.VotesWeightReward].Amount / 7; // amount / 14 * 2
                var votesWeightRewardInThirdTerm =
                    distributionInformationOfThirdTerm[SchemeType.VotesWeightReward].Amount / 12; // amount / (7 + 17) * 2
                await CheckBalancesAsync(firstSevenCoreDataCenters,
                    votesWeightRewardInSecondTerm + votesWeightRewardInThirdTerm -
                    EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // Last 10 core data centers can also profit from votes weight reward. (But less.)
            {
                var balancesBefore = lastFourCoreDataCenters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                await ClaimProfits(lastFourCoreDataCenters, _schemes[SchemeType.VotesWeightReward].SchemeId);
                var votesWeightRewardInThirdTerm =
                    distributionInformationOfThirdTerm[SchemeType.VotesWeightReward].Amount / 24; // amount / (7 + 17)
                await CheckBalancesAsync(lastFourCoreDataCenters,
                    votesWeightRewardInThirdTerm - EconomicTestConstants.TransactionFeeOfClaimProfit,
                    balancesBefore);
            }

            // First 10 voters can profit from citizen welfare.
            var firstTenVoters = MissionedECKeyPairs.CitizenKeyPairs.Take(10).ToList();
            {
                var balancesBefore = firstTenVoters.ToDictionary(k => k, k =>
                    AsyncHelper.RunSync(() => TokenStub.GetBalance.CallAsync(new GetBalanceInput
                    {
                        Owner = Address.FromPublicKey(k.PublicKey),
                        Symbol = EconomicTestConstants.TokenSymbol
                    })).Balance);
                // We limited profiting, thus ClaimProfits need to be called 4 times to profit all.
                await ClaimProfits(firstTenVoters, _schemes[SchemeType.CitizenWelfare].SchemeId);
                await ClaimProfits(firstTenVoters, _schemes[SchemeType.CitizenWelfare].SchemeId);
                await ClaimProfits(firstTenVoters, _schemes[SchemeType.CitizenWelfare].SchemeId);
                await ClaimProfits(firstTenVoters, _schemes[SchemeType.CitizenWelfare].SchemeId);
                var citizenWelfareInSecondTerm =
                    distributionInformationOfSecondTerm[SchemeType.CitizenWelfare].Amount / 10;
                var citizenWelfareInThirdTerm =
                    distributionInformationOfThirdTerm[SchemeType.CitizenWelfare].Amount / 10;
                await CheckBalancesAsync(firstTenVoters,
                    citizenWelfareInSecondTerm + citizenWelfareInThirdTerm -
                    EconomicTestConstants.TransactionFeeOfClaimProfit * 4,
                    balancesBefore);
            }
        }
    }
}