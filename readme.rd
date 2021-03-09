<details>
  <summary>Inital setup of dev environment</summary>
  
  ## Setup dotnet
  1. Download + install netcore sdk (Version 3.1.3)  
     https://dotnet.microsoft.com/download
  2. Install ef extensions for `dotnet`cli  
     `dotnet tool install --global dotnet-ef`

  ## Setup node
  1. Download + install node version manager
     https://github.com/coreybutler/nvm-windows/releases/download/1.1.7/nvm-setup.zip
  2. Install + use node 12 LTS 
     ```powershell
     nvm install 12.18.2 64
     nvm use 12.18.2
     ```
</details>

<details>
	<summary>Usage of development environment</summary>

	## Mini Profiler for WebApi
	1. Can be access on this urls  
	https://localhost:5001/profiler/results-index
</details>

<details>
  <summary>Database import/reset + Entity Framework Migrations Usage</summary>
  
  ## How to reset the db completely
  1. Drop Db
  1. Import latest bacpac file (download from own-cloud) via SSMS
    - Make sure to change db name to match the one in Olma connection string
  1. Run execute ef migrations script  
    `dotnet ef database update --context OlmaDbContext --startup-project Dpl.B2b.Web.Api --project Dpl.B2b.Dal`
  1. Run dev seed  
    - rebuild cli project
    - afterwards run `dpl db reset olma`

  ## What todo after pulling a new version from the server
  1. Run Drop Olma Tables script (`Dpl.B2b.Dal\Seed\DropAllOlmaTables.sql`)
  1. Run execute migrations script  
    `dotnet ef database update --context OlmaDbContext --startup-project Dpl.B2b.Web.Api --project Dpl.B2b.Dal`
  1. Run dev seed  
    `dpl db reset olma`

  ## What todo after any changes was made to DB entities (Olma) incl indexes
  - Create new migration
     `dotnet ef migrations add <INSERT_NAME_FOR_MIGRATION> --context OlmaDbContext --startup-project Dpl.B2b.Web.Api --project Dpl.B2b.Dal --configuration Migration`
  - Test Migration
     `dotnet ef database update --context OlmaDbContext --startup-project Dpl.B2b.Web.Api --project Dpl.B2b.Dal`

  ## How to generate a SQL script for CI environments
  Run script generation command:  
  `dotnet ef migrations script --context OlmaDbContext --idempotent -o <PATH_TO_FILE>.sql --configuration Release --startup-project Dpl.B2b.Web.Api --project Dpl.B2b.Dal`
</details>
<details>
  <summary>Authorization (.NET)</summary>

  ## Overview
  
  - Authorization is based on authorizaion resources types. 
  - Each resource type has specific actions. 
  - Authorization resource types are hierachial. The parent type in addition to its own actions also has the actions of all decendants.
  - A Permission is a combination of
    - user or group  
    - authorization resource type
    - authorization resource id
    - action
    - allow / deny
  - In .NET code actions are expressed via Requirements
  
  ### Structure of authorization resources  
  - Organization    
      - PostingAccount
      - Customer
        - Division

  ## .NET Infrastructure
  - .NET comes with an authorization infrastrcture that consists of
    - Requirements (maps to actions above)
    - Requirement handlers that evalute if a specific requirement is met
    - And a global authorization handler to trigger evaluation of requirements given
      - an instance of the actual resource
      - the current user
      - and a set of requirements to evaluate
  
  ## Securing a BusinessService / Controller

  ### Overview
  - Add new `Ress
  
  ### Preparation
  1. Identify which actions exist and on which authorization resources the belong to
      - Please consider if existing actions could be reused
      - Or if the desired result can be expressed by checking for multiple actions
      - Consider the hierachial nature of the authorization resource types
  2. Extend `AuthorizationResourceAction` enum
      - Please follow the existing placement of new entries (consider the authorization resource hierachy)
      - Please follow the existing numbering scheme for enum values
  3. Create requirement for action
      - Ensure returnig the newly created enum value for your action
      - Ensure returnig the correct `AuthorizationResourceType` value
  4. Register the requirement handle in the `ServicesExtension` file
      - Follow the other registrations for guidance
  5. Add Automapper mapping (in the authorization section) from current resource to authorization resource

  ## Secure Business Service
  1. Inject `IAuthorizationService` service into business service
  2. Add Security section in service in top of call  
  
  Sample below shows adding check for `CanRead` action to `VouchersService.Update`
  ```c#

  #region Security

  // get authorization resource object
  var resourceResponse = _olmaVoucherRepo.GetById<Olma.Voucher, DivisionPermissionResource>(id);
  if (resourceResponse.ResultType == ResultType.NotFound)
  {
      // return bad request as we do not wanne expose not found as auth hasn't been perfromed yet, alternetively we could return unauthorized
      return BadRequest<Contracts.Models.Voucher>();
  }

  // evaluate requiremnets against authorization resource
  var authorizationResult =
      await AuthService.AuthorizeAsync<DivisionPermissionResource, CanReadVoucherRequirement>(AuthData, resourceResponse.Data);

  // if authorization has failed
  if (!authorizationResult.Succeeded)
  {
    return Forbidden<Contracts.Models.Voucher>();
  }

  #endregion
  ```
</details>
<details>
  <summary>Sync Services</summary>

  ## Setup General
  1. Open the following Script in SSMS: /Dpl.B2b.Dal/Seed/SetupSyncServices.sql
  2. **IMPORTANT** make sure you have the Olma DB selected as active DB
  3. Execute the script
  
  This script ensures certain additional requirements are met when executing the sync services against the Olma DB directly. SHould there be any errors during execution please take a screenshot and share with Dominik and Dirk to identify the issue

  ## Setup Ltms Sync Service
  1. Go to the folder /LtmsSyncService
  1. **Copy** Lama2OlmaSync.exe.config.template and rename the copy to **Lama2OlmaSync.exe.config**  
     - **Please DO NOT just rename the file**. 
     - The template file is git but the config file is ignored. Renaming will cause the template file being deleted from git and the next developer will have problems getting the service setup.
  1. In the file Lama2OlmaSync.exe.config:
     - Change the `AccountingContext`connection string if neccessary (default is `Data Source=localhost\SQLEXPRESS` and `Initial Catalog=Olma`
     - Update the `PostingRequestSyncRequestQueueName` entry by replacing `<YOURLASTNAME>` with your last name  
      ```xml
      <add name="AccountingContext" connectionString="Data Source=localhost\\SQLEXPRESS;Initial Catalog=Olma;Integrated Security=true;" providerName="System.Data.SqlClient" />
      <add key="PostingRequestSyncRequestQueueName" value="postingrequestssyncrequest<YOURLASTNAME>" />
      ```

  ## Setup Lms Sync Service
  1. Go to folder /Dpl.B2b.Live2LMSService
  2. Copy file appsettings.Development.json.template and rename copy to **appsettings.Development.json**
  2. In the file appsettings.Development.json:
    - Update `LMSServerName` if neccessary (default is localhost\SQLEXPRESS)
    - Update `LMSDatabaseName` if neccessary (default is Olma)
    - Update LiveQueueName by replacing `<YOURLASTNAME>` with your last name  
      ```json
      "LMSServerName": "localhost\\SQLEXPRESS",
      "LMSDatabaseName": "Olma",
      "LiveQueueName": "ordersyncrequest<YOURLASTNAME>",
      ```


  ## Run Sync Services
  To run both sync services run the following bat file:
  - `start-sync.bat`
  
  To run only one of the services execute one of the following bat files
  - `start-lms-sync.bat`
  - or `start-lms-sync.bat`

  ## Stop Sync Services
  To stop the sync services press `CTRL+C`
</details>