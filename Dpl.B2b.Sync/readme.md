## Important notes for working with windows services
- The current working directory returned by calling GetCurrentDirectory for a Windows Service is the C:\WINDOWS\system32 folder.
    - Instead use `IHostEnvironment.ContentRootPath` or `ContentRootFileProvider` to locate an app's resources.


## Include runtimes (Self-contained deployment (SCD)
By including the runtime identifiers into the Property group of the project file the .netcore runtime and the apps dependencies will be deployed with the app.
```
  <PropertyGroup>
	...
    <RuntimeIdentifiers>win-x64;win-x86</RuntimeIdentifiers>
  </PropertyGroup>
```

## Adding additional worker / tasks
To add additinal workers use `AddHostedService`

```
public class Program
{
    ...

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
			...
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<NewServiceWorker>();
            })	
}
```

## Setting up the windows service

1. Create a dedicated user account
    ```
    New-LocalUser -Name {NAME}
    ```
1. Spefify Logon as service rights for newly created user  
As of now this cannot be done with built in PS commands.
(There is a PS module available here https://gallery.technet.microsoft.com/scriptcenter/Grant-Revoke-Query-user-26e259b0)
    1. Open the Local Security Policy editor by running secpol.msc.
    1. Expand the Local Policies node and select User Rights Assignment.
    1. Open the Log on as a service policy.
	  1. Select Add User or Group.
	  1. Provide the object name (user account) using either of the following approaches:
		    - Type the user account ({DOMAIN OR COMPUTER NAME\USER}) in the object name field and select OK to add the user to the policy.
		    - Select Advanced. Select Find Now. Select the user account from the list. Select OK. Select OK again to add the user to the policy.
	  1. Select OK or Apply to accept the changes.
1. Create the windows service
    ```
    $acl = Get-Acl "{EXE PATH}"
    $aclRuleArgs = {DOMAIN OR COMPUTER NAME\USER}, "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
    $accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
    $acl.SetAccessRule($accessRule)
    $acl | Set-Acl "{EXE PATH}"

    New-Service -Name {NAME} -BinaryPathName {EXE FILE PATH} -Credential {DOMAIN OR COMPUTER NAME\USER} -Description "{DESCRIPTION}" -DisplayName "{DISPLAY NAME}" -StartupType Automatic
    ```
    - {EXE PATH} – Path to the app's folder on the host (for example, d:\myservice). Don't include the app's executable in the path. A trailing slash isn't required.
    - {DOMAIN OR COMPUTER NAME\USER} – Service user account (for example, Contoso\ServiceUser).
    - {NAME} – Service name (for example, MyService).
    - {EXE FILE PATH} – The app's executable path (for example, d:\myservice\myservice.exe). Include the executable's file name with extension.
    - {DESCRIPTION} – Service description (for example, My sample service).
    - {DISPLAY NAME} – Service display name (for example, My Service).

## Remove service
```
Remove-Service -Name {NAME}
```

## References
- Host ASP.NET Core in a Windows Service
  https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-3.0&tabs=visual-studio
- Creating a Windows Service with .NET Core 3.0
  https://csharp.christiannagel.com/2019/10/15/windowsservice/