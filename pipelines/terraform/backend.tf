terraform {
    backend "azurerm" {
        storage_account_name = "PocClienteterraformstate"
        container_name       = "servicosbase"
    }
}
