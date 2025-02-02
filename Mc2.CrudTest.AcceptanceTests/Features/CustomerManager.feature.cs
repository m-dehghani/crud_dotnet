﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:1.0.0.0
//      Reqnroll Generator Version:1.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ReqnrollProject1.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "1.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class CustomerManagerFeature
    {
        
        private static Reqnroll.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private static string[] featureTags = ((string[])(null));
        
#line 1 "CustomerManager.feature"
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
        public static async System.Threading.Tasks.Task FeatureSetupAsync(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            Reqnroll.FeatureInfo featureInfo = new Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Customer Manager", "As a an operator I wish to be able to Create, Update, Delete customers and list a" +
                    "ll customers", ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Customer Manager")))
            {
                await global::ReqnrollProject1.Features.CustomerManagerFeature.FeatureSetupAsync(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("User can Create, Edit, Delete, and Read customer records")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Customer Manager")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("mytag")]
        public async System.Threading.Tasks.Task UserCanCreateEditDeleteAndReadCustomerRecords()
        {
            string[] tagsOfScenario = new string[] {
                    "mytag"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            Reqnroll.ScenarioInfo scenarioInfo = new Reqnroll.ScenarioInfo("User can Create, Edit, Delete, and Read customer records", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                Reqnroll.Table table1 = new Reqnroll.Table(new string[] {
                            "Code",
                            "Description"});
                table1.AddRow(new string[] {
                            "101",
                            "Invalid Email"});
                table1.AddRow(new string[] {
                            "102",
                            "Invalid PhoneNumber"});
                table1.AddRow(new string[] {
                            "103",
                            "Invalid BankAccountNumber"});
                table1.AddRow(new string[] {
                            "104",
                            "Invalid DateOfBirth"});
                table1.AddRow(new string[] {
                            "201",
                            "Duplicated Email Address"});
                table1.AddRow(new string[] {
                            "202",
                            "Duplicated Firstname, Lastname"});
#line 8
 await testRunner.GivenAsync("platform support following error codes", ((string)(null)), table1, "Given ");
#line hidden
#line 18
 await testRunner.GivenAsync("platform has \"0\" record of customers", ((string)(null)), ((Reqnroll.Table)(null)), "Given ");
#line hidden
                Reqnroll.Table table2 = new Reqnroll.Table(new string[] {
                            "Email",
                            "BankAccountNumber",
                            "Firstname",
                            "Lastname",
                            "DateOfBirth",
                            "Phonenumber"});
                table2.AddRow(new string[] {
                            "john.doe@email.com",
                            "NL91RABO0312345678",
                            "john",
                            "doe",
                            "19-JUN-1999",
                            "+989087645543"});
#line 20
 await testRunner.WhenAsync("When user send command to create new customer with following information", ((string)(null)), table2, "When ");
#line hidden
                Reqnroll.Table table3 = new Reqnroll.Table(new string[] {
                            "Email",
                            "BankAccountNumber",
                            "Firstname",
                            "Lastname",
                            "DateOfBirth",
                            "Phonenumber"});
                table3.AddRow(new string[] {
                            "john.doe@email.com",
                            "NL91RABO0312345678",
                            "john",
                            "doe",
                            "19-JUN-1999",
                            "+989087645543"});
#line 24
 await testRunner.ThenAsync("user can send query and receive \"1\" record of customer with following data", ((string)(null)), table3, "Then ");
#line hidden
                Reqnroll.Table table4 = new Reqnroll.Table(new string[] {
                            "Email",
                            "BankAccountNumber",
                            "Firstname",
                            "Lastname",
                            "DateOfBirth",
                            "Phonenumber"});
                table4.AddRow(new string[] {
                            "john.smith@email.com",
                            "NL91RABO0312345679",
                            "john",
                            "smith",
                            "19-JUN-1999",
                            "+989087645541"});
#line 28
 await testRunner.WhenAsync("user send command to update customer with email of \"john.doe@email.com\" and with " +
                        "below information", ((string)(null)), table4, "When ");
#line hidden
                Reqnroll.Table table5 = new Reqnroll.Table(new string[] {
                            "Code"});
                table5.AddRow(new string[] {
                            "202"});
                table5.AddRow(new string[] {
                            "102"});
#line 32
  await testRunner.ThenAsync("user should receive following error codes", ((string)(null)), table5, "Then ");
#line hidden
                Reqnroll.Table table6 = new Reqnroll.Table(new string[] {
                            "Email",
                            "BankAccountNumber",
                            "Firstname",
                            "Lastname",
                            "DateOfBirth",
                            "Phonenumber"});
                table6.AddRow(new string[] {
                            "john.smith@email.com",
                            "NL91RABO0312345679",
                            "john",
                            "smith",
                            "19-JUN-1999",
                            "+989087645541"});
#line 37
 await testRunner.ThenAsync("user can send query and receive \"1\" record of customer with following data", ((string)(null)), table6, "Then ");
#line hidden
                Reqnroll.Table table7 = new Reqnroll.Table(new string[] {
                            "Email",
                            "BankAccountNumber",
                            "Firstname",
                            "Lastname",
                            "DateOfBirth",
                            "Phonenumber"});
                table7.AddRow(new string[] {
                            "john.doe@email.com",
                            "NL91RABO0312345678",
                            "john",
                            "doe",
                            "19-JUN-1999",
                            "+989087645543"});
#line 41
  await testRunner.AndAsync("user can send query and receive \"0\" record of customer with following data", ((string)(null)), table7, "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
