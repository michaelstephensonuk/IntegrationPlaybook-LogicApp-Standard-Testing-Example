﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace logicapp.testing.acceptancetests.Features.ShipInstruction.SpecFlow.Processor
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ShipInstructionProcessorFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = new string[] {
                "DEVOPS_WI:55"};
        
#line 1 "Processor.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/ShipInstruction/SpecFlow/Processor", "Ship Instruction Processor", @"	As a transport manager
    So that I can asssociate railcars to orders
    I want the transport system to be notified when an order is ready to be dispatched

<a href=""https://tfscsc.visualstudio.com/_git/IntegrationPlaybook?path=/vs-code-logicapp-testing/Documentation.ShipInstruction.md"">Click here for more info</a>", ProgrammingLanguage.CSharp, new string[] {
                        "DEVOPS_WI:55"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Ship Instruction Processor")))
            {
                global::logicapp.testing.acceptancetests.Features.ShipInstruction.SpecFlow.Processor.ShipInstructionProcessorFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Ship Instruction Processor Green Path")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Ship Instruction Processor")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DEVOPS_WI:55")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CleanTheSystem")]
        public virtual void ShipInstructionProcessorGreenPath()
        {
            string[] tagsOfScenario = new string[] {
                    "CleanTheSystem"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ship Instruction Processor Green Path", "This is the default use case where we get a valid event and then lookup data from" +
                    " the energy system\r\nand transform it to the destination flat file format and del" +
                    "iver it to the transport system.", tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 12
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table3.AddRow(new string[] {
                            "DeliveryId",
                            "{{Guid}}"});
                table3.AddRow(new string[] {
                            "DeliveryStatus",
                            "Scheduled"});
                table3.AddRow(new string[] {
                            "Commodity",
                            "Petrochemical"});
                table3.AddRow(new string[] {
                            "CargoId",
                            "28300"});
                table3.AddRow(new string[] {
                            "OrderPriority",
                            "1"});
                table3.AddRow(new string[] {
                            "BillOfLading",
                            ""});
#line 17
 testRunner.Given("I have a request to dispatch an order", ((string)(null)), table3, "Given ");
#line hidden
#line 25
 testRunner.And("the logic app test manager is setup", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 26
    testRunner.When("I send the message to the logic app", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 27
 testRunner.Then("the logic app will start running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 28
 testRunner.And("the logic app will receive the message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 29
 testRunner.And("the logic app will parse the request message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 30
    testRunner.And("the logic app will lookup data from the source system", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "Expected Notes"});
                table4.AddRow(new string[] {
                            "order_no"});
                table4.AddRow(new string[] {
                            "plant_id"});
                table4.AddRow(new string[] {
                            "customer_no"});
#line 31
    testRunner.And("the logic app will transform data to the destination format", ((string)(null)), table4, "And ");
#line hidden
#line 36
    testRunner.And("the logic app will convert the message to the flat file format", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
 testRunner.And("the logic app will send a reply", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 38
 testRunner.And("the logic app will complete successfully", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 39
 testRunner.And("the response from the logic app will be as expected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Ship Instruction Processor Not PetroChemical Event")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Ship Instruction Processor")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("DEVOPS_WI:55")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CleanTheSystem")]
        public virtual void ShipInstructionProcessorNotPetroChemicalEvent()
        {
            string[] tagsOfScenario = new string[] {
                    "CleanTheSystem"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ship Instruction Processor Not PetroChemical Event", "This is the case where the event is not related to the PetroChemical commodity so" +
                    " we will ignore the\r\nevent and not process it to other systems", tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 42
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "Key",
                            "Value"});
                table5.AddRow(new string[] {
                            "DeliveryId",
                            "{{Guid}}"});
                table5.AddRow(new string[] {
                            "DeliveryStatus",
                            "Scheduled"});
                table5.AddRow(new string[] {
                            "Commodity",
                            "Not-PetroChemical"});
                table5.AddRow(new string[] {
                            "CargoId",
                            "28300"});
                table5.AddRow(new string[] {
                            "OrderPriority",
                            "1"});
                table5.AddRow(new string[] {
                            "BillOfLading",
                            ""});
#line 47
 testRunner.Given("I have a request to dispatch an order", ((string)(null)), table5, "Given ");
#line hidden
#line 55
 testRunner.And("the logic app test manager is setup", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 56
    testRunner.When("I send the message to the logic app", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 57
 testRunner.Then("the logic app will start running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 58
 testRunner.And("the logic app will receive the message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 59
 testRunner.And("the logic app will parse the request message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 60
 testRunner.And("the logic app will identify the commodity is not Petrochemical", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 61
 testRunner.And("the logic app will terminate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion