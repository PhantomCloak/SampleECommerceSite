( exec ../AzureContainerRegistry/PushBackendAcr.sh)

kubectl delete deployment/backend-deployment
kubectl delete service/backend-cluster

kubectl apply -f Deployments/backend-deployment.yml
kubectl apply -f Services/backend-cluster.yml
