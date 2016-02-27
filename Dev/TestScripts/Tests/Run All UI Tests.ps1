﻿$TestSettingsFile = "$PSScriptRoot\LocalUITesting.testsettings"
$SolutionDir = (get-item $PSScriptRoot ).parent.parent.FullName
[system.io.file]::WriteAllText($TestSettingsFile,  @"
<?xml version=`"1.0`" encoding=`"UTF-8`"?>
<TestSettings name=`"UI Test`" id=`"6091E338-CE48-49F7-BC78-B459A768335A`" xmlns=`"http://microsoft.com/schemas/VisualStudio/TeamTest/2010`">
  <Description>These are default test settings for a local test run.</Description>
  <Deployment>
		<DeploymentItem filename=`"Dev2.Server\bin\Debug\`" outputDirectory=`"Server`" />
		<DeploymentItem filename=`"Dev2.Studio\bin\Debug\`" outputDirectory=`"Studio`" />
		<DeploymentItem filename=`"Warewolf.Studio.UISpecs\Properties\DefaultWorkspaceLayout.xml`" />
  </Deployment>
  <NamingScheme baseName=`"UI`" appendTimeStamp=`"false`" useDefault=`"false`" />
  <Scripts setupScript=`"TestScripts\Studio\Startup.bat`" cleanupScript=`"TestScripts\Studio\Cleanup.bat`" />
  <Execution>
    <Hosts skipUnhostableTests=`"false`" />
    <TestTypeSpecific>
      <UnitTestRunConfig testTypeId=`"13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b`">
        <AssemblyResolution>
          <TestDirectory useLoadContext=`"true`" />
        </AssemblyResolution>
      </UnitTestRunConfig>
      <WebTestRunConfiguration testTypeId=`"4e7599fa-5ecb-43e9-a887-cd63cf72d207`">
        <Browser name=`"Internet Explorer 9.0`" MaxConnections=`"6`">
          <Headers>
            <Header name=`"User-Agent`" value=`"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)`" />
            <Header name=`"Accept`" value=`"*/*`" />
            <Header name=`"Accept-Language`" value=`"{{`$IEAcceptLanguage}}`" />
            <Header name=`"Accept-Encoding`" value=`"GZIP`" />
          </Headers>
        </Browser>
      </WebTestRunConfiguration>
    </TestTypeSpecific>
    <AgentRule name=`"LocalMachineDefaultRole`">
    </AgentRule>
  </Execution>
</TestSettings>
"@)
$FullArgsList = "`"" + $SolutionDir + "\Warewolf.Studio.UISpecs\bin\Debug\Warewolf.Studio.UISpecs.dll`" /logger:trx /Settings:`"" + $TestSettingsFile + "`""
Write-Host $SolutionDir> `"$env:vs120comntools..\IDE\CommonExtensions\Microsoft\TestWindow\VSTest.console.exe`" $FullArgsList
Start-Process -FilePath "$env:vs120comntools..\IDE\CommonExtensions\Microsoft\TestWindow\VSTest.console.exe" -ArgumentList @($FullArgsList) -verb RunAs -WorkingDirectory $SolutionDir -Wait