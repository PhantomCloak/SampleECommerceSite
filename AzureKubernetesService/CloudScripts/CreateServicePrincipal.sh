#!/bin/bash
set -e

SUBSCRIPTION_ID=""
RESOURCE_GROUP_NAME="test-res"
SERVICE_PRINCIPAL_NAME="kubesp"

SERVICE_PRINCIPAL_JSON=$(az ad sp create-for-rbac --skip-assignment --name $SERVICE_PRINCIPAL_NAME -o json)

SERVICE_PRINCIPAL=$(echo $SERVICE_PRINCIPAL_JSON | jq -r '.appId')
SERVICE_PRINCIPAL_SECRET=$(echo $SERVICE_PRINCIPAL_JSON | jq -r '.password')

az role assignment create --assignee $SERVICE_PRINCIPAL --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP_NAME" --role Contributor

echo "principal id: $SERVICE_PRINCIPAL"
echo "principal secret: $SERVICE_PRINCIPAL_SECRET"