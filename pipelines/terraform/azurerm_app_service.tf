resource "azurerm_service_plan" "app_plan" {
  name = "${local.aplicacao}-${local.ambiente}${local.aliasAssinatura}-sp"
  location = local.localizacao
  resource_group_name = local.ambiente == "prod" ? data.azurerm_resource_group.rg[0].name : azurerm_resource_group.rg[0].name
  os_type = "Linux"
  sku_name = "B1"
  tags = local.tags

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_linux_web_app" "webapp_a" {
  name = "${local.aplicacao}-${local.ambiente}${local.aliasAssinatura}-wa"
  location = local.localizacao
  resource_group_name = local.ambiente == "prod" ? data.azurerm_resource_group.rg[0].name : azurerm_resource_group.rg[0].name
  service_plan_id = azurerm_service_plan.app_plan.id
  tags = local.tags
  https_only = true

  site_config {
    always_on = local.ambiente == "dev" ? false : true
    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "WEBSITE_RUN_FROM_PACKAGE" = "1",
    "APPINSIGHTS_INSTRUMENTATIONKEY"              = "${azurerm_application_insights.appinsights.instrumentation_key}",
    "APPLICATIONINSIGHTS__CONNECTIONSTRING"       = "${azurerm_application_insights.appinsights.connection_string}",
    "Serilog__WriteTo__0__Args__connectionString" = "${azurerm_application_insights.appinsights.connection_string}",
    "Serilog__MinimumLevel__Default"              = "Warning"
    "Logging__LogLevel__Default"                  = "Warning"

    "Cac__UrlBaseLoginService"                    = local.ambiente == "prod" ? "https://login.PocCliente.com.br/" : "https://login-${local.ambiente}.PocCliente.com.br/",

    "ExporSwagger" = local.ambiente == "prod" ? false : true
  }
}
