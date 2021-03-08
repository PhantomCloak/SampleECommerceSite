( exec ../AzureContainerRegistry/PushFrontendAcr.sh)

kubectl delete deployment/frontend-deployment
kubectl delete service/frontend-balancer

kubectl apply -f Deployments/frontend-deployment.yml
kubectl apply -f Services/frontend-balancer.yml
