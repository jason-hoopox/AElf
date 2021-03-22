﻿using AElf.Sdk.CSharp.State;
using Google.Protobuf.WellKnownTypes;

namespace AElf.Contracts.Configuration
{
    public partial class ConfigurationState : ContractState
    {
        public SingletonState<AuthorityInfo> ConfigurationController { get; set; }
        public SingletonState<AuthorityInfo> MethodFeeController { get; set; }
        public MappedState<string, BytesValue> Configurations { get; set; }
    }
}