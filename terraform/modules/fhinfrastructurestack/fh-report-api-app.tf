# Report API Application Group

# Create App Service for Report API
resource "azurerm_windows_web_app" "fh_report_api" {
  app_settings = {
    ApplicationInsightsAgent_EXTENSION_VERSION  = "~3"
    XDT_MicrosoftApplicationInsights_Mode       = "Recommended"
    ASPNETCORE_ENVIRONMENT                      = var.asp_netcore_environment
    WEBSITE_RUN_FROM_PACKAGE                    = "1"
    APPLICATIONINSIGHTS_CONNECTION_STRING       = azurerm_application_insights.app_insights.connection_string
    "AppConfiguration:KeyVaultIdentifier"       = "${var.prefix}-kv-fh-admin"
    "AppConfiguration:KeyVaultPrefix"           = "REPORT-API"
    "ConnectionStrings:ReportConnection"        = local.report_db_connection
  }
  name                                          = "${var.prefix}-as-fh-report-api"
  resource_group_name                           = local.resource_group_name
  location                                      = var.location
  service_plan_id                               = azurerm_service_plan.apps_plan.id
  client_affinity_enabled                       = false
  https_only                                    = true
  identity {
    type                                        = "SystemAssigned"
  }
  site_config {
    always_on                                   = true
    ftps_state                                  = "Disabled"
    health_check_path                           = "/api/health"
    health_check_eviction_time_in_min           = 5 # How long to be removed from LB if unhealthy
    http2_enabled                               = true
    application_stack {
      current_stack                               = var.current_stack
      dotnet_version                              = var.dotnet_version_general
    }
    ip_restriction {
      name       = "AllowAppAccess"
      priority   = 1
      action     = "Allow"
      ip_address = var.vnetint_address_space[0]
    }
    ip_restriction {
      name       = "DenyPublicAccess"
      priority   = 200
      action     = "Deny"
      ip_address = "0.0.0.0/0"
    }
  }
  tags = local.tags
  lifecycle {
    ignore_changes = [virtual_network_subnet_id, logs]
  }
}

# Swift Connection for Report API
resource "azurerm_app_service_virtual_network_swift_connection" "fh_report_api" {
  app_service_id = azurerm_windows_web_app.fh_report_api.id
  subnet_id      = azurerm_subnet.vnetint.id
}

# SQL Server Database for Report API
resource "azurerm_mssql_database" "report_serverless_db" {
    name                        = "${var.prefix}-fh-report-db"
    server_id                   = azurerm_mssql_server.sqlserver.id
    collation                   = "SQL_Latin1_General_CP1_CI_AS"

    max_size_gb                 = 1
    sku_name                    = "S0"
    zone_redundant              = false
    storage_account_type        = "Local"

    short_term_retention_policy {
        retention_days = "7"
        backup_interval_in_hours = "24"
    }

    long_term_retention_policy {
        monthly_retention = "P1M" # Retain for a month
    }

    threat_detection_policy {
        disabled_alerts      = []
        email_account_admins = "Disabled"
        email_addresses      = []
        retention_days       = 0
        state                = "Disabled"
    }

  tags = local.tags
}

# Private Endpoint 11: Report API
resource "azurerm_private_endpoint" "reportapi" {
  name                = "${var.prefix}-as-fh-report-api"
  location            = var.location
  resource_group_name = local.resource_group_name
  subnet_id           = azurerm_subnet.pvtendpoint.id
  custom_network_interface_name = "${var.prefix}-as-fh-report-api-nic"

  ip_configuration {
    name                       = "${var.prefix}-as-fh-report-api"
    private_ip_address         = var.private_endpoint_ip_address.report_api_ip
    subresource_name           = "sites" 
  }

 private_service_connection {
    name                           = "${var.prefix}-pvtendpt-report-api"
    private_connection_resource_id = azurerm_windows_web_app.fh_report_api.id
    is_manual_connection           = false
    subresource_names              = [ "sites" ]
  }

  private_dns_zone_group {
    name                 = azurerm_private_dns_zone.appservices.name
    private_dns_zone_ids = [ azurerm_private_dns_zone.appservices.id ]
  }
  tags = local.tags
}

# Private DNS Zone - SQL Server A Records
resource "azurerm_private_dns_a_record" "sql_report_api" {
  name                = "${var.prefix}-as-fh-report-api"
  zone_name           = azurerm_private_dns_zone.sqlserver.name
  resource_group_name = local.resource_group_name
  ttl                 = "10"
  records             = [var.private_endpoint_ip_address.report_api_ip]
  tags = local.tags
  lifecycle {
    ignore_changes = [tags]
  }
}

resource "azurerm_private_dns_a_record" "sql_report_api_scm" {
  name                = "${var.prefix}-as-fh-report-api.scm"
  zone_name           = azurerm_private_dns_zone.sqlserver.name
  resource_group_name = local.resource_group_name
  ttl                 = "10"
  records             = [var.private_endpoint_ip_address.report_api_ip]
  tags = local.tags
  lifecycle {
    ignore_changes = [tags]
  }
}