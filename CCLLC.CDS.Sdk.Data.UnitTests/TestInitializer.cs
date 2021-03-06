﻿using DLaB.Xrm.Test;
using DLaB.Xrm.Client;
using Microsoft.Xrm.Sdk;


namespace CCLLC.CDS.Sdk.Data.UnitTest
{
    using Builders;
    using Proxy;

    /// <summary>
    /// Class to Initalize all TestSettings used by the Framework
    /// </summary>
    public class TestInitializer
    {
        /// <summary>
        /// Initializes the test settings.
        /// </summary>
        public static void InitializeTestSettings()
        {
            if (!TestSettings.AssumptionXmlPath.IsConfigured)
            {
              //TestSettings.AssumptionXmlPath.Configure(new PatherFinderProjectOfType(typeof(MsTestProvider), "Assumptions\\Entity Xml"));
            }
            if (!TestSettings.UserTestConfigPath.IsConfigured)
            {
                TestSettings.UserTestConfigPath.Configure(new PathFinder("UnitTestSettings.user.config"));
            }
            if (!TestSettings.EntityBuilder.IsConfigured)
            {
                TestSettings.EntityBuilder.ConfigureDerivedAssembly<EntityBuilder<Entity>>();
            }
            if (!TestSettings.EarlyBound.IsConfigured)
            {
                TestSettings.EarlyBound.ConfigureDerivedAssembly<TestServiceContext>();
                CrmServiceUtility.GetEarlyBoundProxyAssembly(TestSettings.EarlyBound.Assembly);
            }
            if (!TestSettings.TestFrameworkProvider.IsConfigured)
            {
                TestSettings.TestFrameworkProvider.Configure(new MsTestProvider());
            }
        }
    }
}
