# Azure AD multi-application sample

## Summary
This sample shows how to use multiple AAD Applications, from either the same or different tenants, to authenticate users.

## Setup

Configure 2 AAD Applications in AAD and overwrite the configuration in appsettings.json.

Create a Web redirect flow for each:

 - One redirects to ``` https://localhost:7141/signin-oidc ```
 - The other redirects to ``` https://test.localtest.me:7141/signin-oidc ```

Create a secret in each application. Set these using
```
   dotnet user-secrets set AzureAD1:ClientSecret <secret-one>
   dotnet user-secrets set AzureAD2:ClientSecret <secret-two>
```

Each AAD Application will have a corresponding Enterprise Application. You can use this to limit the users for each.

## Testing
Run the application in two browser windows
Browse in one to ``` https://localhost:7141/ ```
Browse in the other to ``` https://test.localtest.me:7141/ ```

Depending on the hostname you will be challenged to authenticate against one of the applications but not the other.
