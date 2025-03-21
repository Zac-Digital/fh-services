# Azure Virtual Network

This page describes the Family Hubs virtual network.

| CIDR          | Range              | Description                          |
|---------------|--------------------|--------------------------------------|
| 10.0.3.0/24   | 10.0.3.0 - 10.0.3.255 | Virtual network range             |
| 10.0.3.0/26   | 10.0.3.0 - 10.0.3.63  | Subnet for app gateway            |
| 10.0.3.80/28  | 10.0.3.80 - 10.0.3.95 | Integration subnet                |
| 10.0.3.96/28  | 10.0.3.96 - 10.0.3.111| Subnet for private endpoint for apps |
| 10.0.3.112/28 | 10.0.3.112 - 10.0.3.127 | Subnet for SQL Server          |
| 10.0.3.128/28 | 10.0.3.128 - 10.0.3.143 | *Subnet for idam maintenance   |

*Could this be incorporated into the private endpoints and that range expanded?

## Current VNET Diagram
Below shows our current VNET setup at a high level. The VNET is assigned an IP range using a CIDR of 256 IPs. There are some subnets - one for the gateway, one for the app and one for the SQL databases. There is a swift network connection that joins the app into the network.

![VNET Diagram](../img/Azure%20Virtual%20Network%20Image%20Oct%207.png)

## Notes
Diagram created with vnet.drawio - draw.io (diagrams.net)  
File: vnet.drawio