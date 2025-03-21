# EntraId Login

We have enabled EntraId login to the SQL Server along with the standard sys admin account. The sys admin account can still be used and the connection details can be found on the parent page.

The EntraId has to have a single user or group as admin. We have added the delivery user group, which is what we use to manage team member access on Azure.

The web apps also connect using a managed identity. They are treated as an external provider and connect using the name of the web app. A separate script is run to give each web app, reader and writer permissions. The connection string then uses a specialised format that contains Authentication=Active Directory Default instead of the usual username and password details. Under the hood, this will get a access token and present it to SQL Server, who will authenticate and determine the permissions of that user.

Depending on your preferred tooling and platform, you can connect using Entra. For Windows users, SQL Server Management Studio (SSMS) works by connecting using the authentication flow Microsoft Entra Password:

![](../img/EntraId%20Login%20Image.png)

It appears that Azure Data Studio (ADS) will not handshake with users with BYOD and thus you cannot login with EntraId. You can still continue to login with the sys admin account as before and will have the same permissions. Alternatively, you can sign in via Azure:

![](../img/EntraId%20Login%20Image%20(1).png)

It is possible that other cross platform tools, such as JetBrains DataGrip will also support the same EntraId login as SSMS - as long as you can sign in with your email and password. If it tries to browser authenticate then it is likely you will experience the same issues as ADS above.