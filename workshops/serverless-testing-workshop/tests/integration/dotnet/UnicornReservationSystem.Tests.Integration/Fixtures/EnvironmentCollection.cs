/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved
 *
 * SPDX-License-Identifier: MIT-0
 */

using Xunit;

namespace UnicornReservationSystem.Tests.Integration.Fixtures;

[CollectionDefinition("Environment")]
public class EnvironmentCollection : ICollectionFixture<EnvironmentFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}