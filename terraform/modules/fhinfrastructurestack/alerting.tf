
locals {
  # All production alerts go into a separate silver monitor resource group otherwise add to the same resource group
  alert_resource_group_name = var.environment == "Prod" ? "${var.prefix}-silverMonitor" : local.resource_group_name
  
  gateway_details = {
    "referral-ui" = {
      gateway_id = azurerm_application_gateway.ref_ui_app_gateway.id
    },
    "sd-admin-ui" = {
      gateway_id = azurerm_application_gateway.sd_admin_ui_app_gateway.id
    },
    "sd-ui" = {
      gateway_id = azurerm_application_gateway.sd_ui_app_gateway.id
    }
  }
}

# Alert action group

resource "azurerm_monitor_action_group" "slack_channel_email_action_group" {
  name = "${var.prefix}-fh-ag-slack-channel-email"
  resource_group_name = local.alert_resource_group_name
  short_name = "fhalert"
  email_receiver {
    email_address = var.slack_channel_email
    name = "FamilyHub"
  }
  tags = local.tags
}

# App Gateway alerts

resource "azurerm_monitor_metric_alert" "app-gateway-failed-requests" {
  for_each = local.gateway_details
  name = "${var.prefix}-fh-appgw-${each.key}-failed-requests-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [each.value.gateway_id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  dynamic_criteria {
    aggregation = "Total"
    alert_sensitivity = "Medium"
    metric_name = "FailedRequests"
    metric_namespace = "Microsoft.Network/applicationGateways"
    operator = "GreaterThan"
  }
  tags = local.tags
}

resource "azurerm_monitor_metric_alert" "app-gateway-backend-connect-time-alert" {
  for_each = local.gateway_details
  name = "${var.prefix}-fh-appgw-${each.key}-backend-connect-time-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [each.value.gateway_id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  criteria {
    aggregation = "Average"
    metric_name = "BackendConnectTime"
    metric_namespace = "Microsoft.Network/applicationGateways"
    operator = "GreaterThan"
    threshold = 30
  }
  tags = local.tags
}

resource "azurerm_monitor_metric_alert" "app-gateway-backend-last-byte-response-time-alert" {
  for_each = local.gateway_details
  name = "${var.prefix}-fh-appgw-${each.key}-backend-last-byte-response-time-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [each.value.gateway_id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  dynamic_criteria {
    aggregation = "Average"
    alert_sensitivity = "Medium"
    metric_name = "BackendLastByteResponseTime"
    metric_namespace = "Microsoft.Network/applicationGateways"
    operator = "GreaterThan"
  }
  tags = local.tags
}

resource "azurerm_monitor_metric_alert" "app-gateway-backend-response-status-alert" {
  for_each = local.gateway_details
  name = "${var.prefix}-fh-appgw-${each.key}-backend-response-status-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [each.value.gateway_id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  dynamic_criteria {
    aggregation = "Total"
    alert_sensitivity = "Medium"
    metric_name = "BackendResponseStatus"
    metric_namespace = "Microsoft.Network/applicationGateways"
    operator = "GreaterThan"
  }
  tags = local.tags
}

resource "azurerm_monitor_metric_alert" "app-gateway-failed-requests-alert" {
  for_each = local.gateway_details
  name = "${var.prefix}-fh-appgw-${each.key}-failed-requests-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [each.value.gateway_id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  dynamic_criteria {
    aggregation = "Total"
    alert_sensitivity = "Medium"
    metric_name = "FailedRequests"
    metric_namespace = "Microsoft.Network/applicationGateways"
    operator = "GreaterThan"
  }
  tags = local.tags
}

resource "azurerm_monitor_metric_alert" "app-gateway-unhealthy-host-count-alert" {
  for_each = local.gateway_details
  name = "${var.prefix}-fh-appgw-${each.key}-unhealthy-host-count-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [each.value.gateway_id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  criteria {
    aggregation = "Average"
    metric_name = "UnhealthyHostCount"
    metric_namespace = "Microsoft.Network/applicationGateways"
    operator = "GreaterThan"
    threshold = 0
  }
  tags = local.tags
}

resource "azurerm_monitor_metric_alert" "app-gateway-storage-error-availability-alert" {
  name = "${var.prefix}-fh-appgw-saappgwerror-availability-alert"
  resource_group_name = local.alert_resource_group_name
  scopes = [azurerm_storage_account.storage_appgw_errorpage.id]
  window_size = "PT15M"
  frequency = "PT5M"
  action {
    action_group_id = azurerm_monitor_action_group.slack_channel_email_action_group.id
  }
  criteria {
    aggregation = "Average"
    metric_name = "Availability"
    metric_namespace = "Microsoft.Insights/metricAlerts"
    operator = "LessThan"
    threshold = 80
  }
  tags = local.tags
}