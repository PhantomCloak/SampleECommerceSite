#!/bin/bash
set -e

AKS_NAME="efurni-cluster"
SERVICE_PRINCIPAL=""
SERVICE_PRINCIPAL_SECRET=""
RESOURCE_GROUP_NAME="test-res"
KUBERNETES_VERSION="1.18.10"
NODE_POOL_NAME="default"
NODE_VM_SIZE="standard_a2_v2"
NODE_COUNT="2"
NODE_DISK_SIZE_IN_GB="30" #min 30 gig
LOCATION="germanywestcentral"
SSH_KEY_PATH="~/.ssh/id_rsa.pub "

az aks create -n $AKS_NAME \
--resource-group $RESOURCE_GROUP_NAME \
--location $LOCATION \
--kubernetes-version $KUBERNETES_VERSION \
--load-balancer-sku standard \
--nodepool-name $NODE_POOL_NAME \
--node-count $NODE_COUNT \
--node-vm-size $NODE_VM_SIZE \
--node-osdisk-size $NODE_DISK_SIZE_IN_GB \
--ssh-key-value $SSH_KEY_PATH \
--network-plugin kubenet \
--service-principal $SERVICE_PRINCIPAL \
--client-secret $SERVICE_PRINCIPAL_SECRET \
--output none

# get admin privledges
az aks get-credentials --name $AKS_NAME --resource-group $RESOURCE_GROUP_NAME --admin 

#create disk for pesistent volume
az disk create \
  --resource-group MC_test-res_efurni-cluster_germanywestcentral \
  --name efurniAKSDisk \
  --size-gb 20 \
  --sku Standard_LRS \
  --query id --output tsv

#show existing clusters
az aks list -o table 